
namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapterInstanceConfiguration : IRavenDbSourceAdapterInstanceConfiguration
    {
        public string ConnectionString { get; set; }
        public string Collection { get; set; }
    }
}
