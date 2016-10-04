using System.Globalization;

namespace Microsoft.DataTransfer.CsvFile.Reader
{
    sealed class CsvReaderConfiguration
    {
        public bool TrimQuoted { get; set; }
        public bool IgnoreUnquotedNulls { get; set; }
        public CultureInfo ParserCulture { get; set; }
    }
}
