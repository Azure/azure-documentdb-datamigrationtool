
namespace Microsoft.DataTransfer.HBase.Source
{
    sealed class HBaseSourceAdapterInstanceConfiguration : IHBaseSourceAdapterInstanceConfiguration
    {
        public string TableName { get; set; }
        public string Filter { get; set; }
        public bool ExcludeId { get; set; }
        public int BatchSize { get; set; }
    }
}
