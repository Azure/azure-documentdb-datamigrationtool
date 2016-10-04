
namespace Microsoft.DataTransfer.CsvFile.Source
{
    interface ICsvFileSourceAdapterInstanceConfiguration
    {
        string NestingSeparator { get; }
        bool TrimQuoted { get; }
        bool NoUnquotedNulls { get; }
        bool UseRegionalSettings { get; }
    }
}
