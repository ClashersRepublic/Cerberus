namespace Magic.Files.CSV
{
    internal class CsvRow
    {
        public CsvRow(CsvTable table)
        {
            _table = table;
            _rowStart = table.GetColumnRowCount();

            _table.AddRow(this);
        }

        private readonly CsvTable _table;
        private readonly int _rowStart;

        public int GetArraySize(string name)
        {
            var columnIndex = _table.GetColumnIndexByName(name);
            var result = 0;
            if (columnIndex != -1)
                result = _table.GetArraySizeAt(this, columnIndex);
            return result;
        }

        public string GetName() => _table.GetValueAt(0, _rowStart);

        public int GetRowOffset() => _rowStart;

        public string GetValue(string name, int level) => _table.GetValue(name, level + _rowStart);
    }
}

