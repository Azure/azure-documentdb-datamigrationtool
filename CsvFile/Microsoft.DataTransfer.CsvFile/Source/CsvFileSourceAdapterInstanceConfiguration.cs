
namespace Microsoft.DataTransfer.CsvFile.Source
{
    sealed class CsvFileSourceAdapterInstanceConfiguration : ICsvFileSourceAdapterInstanceConfiguration
    {
        public string NestingSeparator { get; set; }
        public bool TrimQuoted { get; set; }
        public bool NoUnquotedNulls { get; set; }
        public bool UseRegionalSettings { get; set; }
    }
}
