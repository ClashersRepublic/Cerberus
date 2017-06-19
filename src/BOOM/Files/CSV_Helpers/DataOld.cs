using System;
using System.Collections.Generic;
using CRepublic.Boom.Files.CSV_Reader_Old;

namespace CRepublic.Boom.Files.CSV_Helpers
{
    using Newtonsoft.Json;
    using System.Reflection;

    internal class DataOld
    {
        internal DataTable DataTable;
        internal Row Row;

        internal readonly int ID;

        internal int Type => this.DataTable.Index;

        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="Row">The row.</param>
        /// <param name="DataTable">The data table.</param>
        internal DataOld(Row Row, DataTable DataTable)
        {
            this.Row = Row;
            this.DataTable = DataTable;
            this.ID = DataTable.Datas.Count + 1000000 * DataTable.Index;
        }

        internal static void LoadData(DataOld Data, Type Type, Row Row)
        {
            foreach (PropertyInfo Property in Type.GetProperties())
            {
                if (Property.PropertyType.IsGenericType)
                {
                    Type ListType = typeof(List<>);
                    Type[] Generic = Property.PropertyType.GetGenericArguments();
                    Type ConcreteType = ListType.MakeGenericType(Generic);
                    object NewList = Activator.CreateInstance(ConcreteType);
                    MethodInfo Add = ConcreteType.GetMethod("Add");
                    string IndexerName =
                    ((DefaultMemberAttribute)
                        NewList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                    PropertyInfo IndexProperty = NewList.GetType().GetProperty(IndexerName);

                    for (int i = Row.Offset; i < Row.Offset + Row.GetArraySize(Property.Name); i++)
                    {
                        string Value = Row.GetValue(Property.Name, i - Row.Offset);

                        if (Value == string.Empty && i != Row.Offset)
                        {
                            Value = IndexProperty.GetValue(NewList, new object[]
                            {
                                i - Row.Offset - 1
                            }).ToString();
                        }

                        if (string.IsNullOrEmpty(Value))
                        {
                            object Object = Generic[0].IsValueType ? Activator.CreateInstance(Generic[0]) : string.Empty;

                            Add.Invoke(NewList, new[]
                            {
                                Object
                            });
                        }
                        else
                        {
                            Add.Invoke(NewList, new[]
                            {
                                Convert.ChangeType(Value, Generic[0])
                            });
                        }
                    }

                    Property.SetValue(Data, NewList);
                }
                else
                {
                    
                    //Console.WriteLine(Row.GetValue(Property.Name, 0) + ": Type " + Property.PropertyType);

                    //Console.WriteLine(Data + ": Type " + Property.PropertyType);

                    Property.SetValue(Data, Row.GetValue(Property.Name, 0) == string.Empty ?  null : Convert.ChangeType(Row.GetValue(Property.Name, 0), Property.PropertyType), null);
                }
            }
        }

        internal int GetID()
        {
            return GlobalID.GetID(this.ID);
        }

        internal int GetGlobalID()
        {
            return this.ID;
        }
    }
}