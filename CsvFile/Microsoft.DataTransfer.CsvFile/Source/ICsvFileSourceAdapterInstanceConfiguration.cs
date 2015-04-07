
namespace Microsoft.DataTransfer.CsvFile.Source
{
    interface ICsvFileSourceAdapterInstanceConfiguration
    {
        string FileName { get; }
        string NestingSeparator { get; }
    }
}
