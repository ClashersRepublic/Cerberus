namespace CRepublic.Boom.Files.CSV_Reader_Old
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Column
    {
        internal readonly List<string> Values;

        internal Column()
        {
            this.Values = new List<string>();
        }

        internal static int GetArraySize(int _Offset, int _NOffset)
        {
            return _NOffset - _Offset;
        }

        internal void Add(string _Value)
        {
            if (_Value == null)
            {
                this.Values.Add(this.Values.Count > 0 ? this.Values.Last() : string.Empty);
            }
            else
            {
                this.Values.Add(_Value);
            }
        }

        internal string Get(int _Row)
        {
            return this.Values[_Row];
        }

        internal int GetSize()
        {
            return this.Values.Count;
        }
    }
}