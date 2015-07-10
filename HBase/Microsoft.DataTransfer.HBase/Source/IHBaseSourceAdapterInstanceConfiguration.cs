
namespace Microsoft.DataTransfer.HBase.Source
{
    interface IHBaseSourceAdapterInstanceConfiguration
    {
        string TableName { get; }
        string Filter { get; }
        bool ExcludeId { get; }
        int BatchSize { get; }
    }
}
