namespace CRepublic.Royale.Files.CSV_Reader
{
    using System;

    [Serializable]
    public class CsvException : Exception
    {
        public CsvException() : base()
        {
            // Space
        }

        public CsvException(string message) : base(message)
        {
            // Space
        }
    }
}
