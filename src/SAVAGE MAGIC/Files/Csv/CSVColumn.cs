using System.Collections.Generic;

namespace Magic.Files.CSV
{
    internal class CsvColumn
    {
        readonly List<string> _values;

        public CsvColumn()
        {
            _values = new List<string>();
        }

        public static int GetArraySize(int currentOffset, int nextOffset) => nextOffset - currentOffset;

        public void Add(string value)
        {
            _values.Add(value);
        }

        public string Get(int row) => _values[row];

        public int GetSize() => _values.Count;
    }
}
