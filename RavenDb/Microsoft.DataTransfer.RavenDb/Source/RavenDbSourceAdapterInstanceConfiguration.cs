
namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapterInstanceConfiguration : IRavenDbSourceAdapterInstanceConfiguration
    {
        public string Index { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public bool ExcludeIdField { get; set; }
    }
}
