namespace BL.Servers.BB.Files.CSV_Reader_Old
{
    internal class Row
    {
        internal readonly int RowStart;
        internal readonly Table Table;

        internal string Name => this.Table.GetValueAt(0, this.RowStart);

        internal int Offset => this.RowStart;

        internal Row(Table Table)
        {
            this.Table = Table;
            this.RowStart = this.Table.GetColumnRowCount();

            this.Table.AddRow(this);
        }

        internal int GetArraySize(string Name)
        {
            var Index = this.Table.GetColumnIndexByName(Name);
            return Index != -1 ? this.Table.GetArraySizeAt(this, Index) : 0;
        }

        internal string GetValue(string Name, int Level)
        {
            return this.Table.GetValue(Name, Level + this.RowStart);
        }
    }
}