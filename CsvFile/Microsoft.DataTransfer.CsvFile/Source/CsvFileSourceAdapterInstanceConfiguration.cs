
namespace Microsoft.DataTransfer.CsvFile.Source
{
    sealed class CsvFileSourceAdapterInstanceConfiguration : ICsvFileSourceAdapterInstanceConfiguration
    {
        public string FileName { get; set; }
        public string NestingSeparator { get; set; }
    }
}
