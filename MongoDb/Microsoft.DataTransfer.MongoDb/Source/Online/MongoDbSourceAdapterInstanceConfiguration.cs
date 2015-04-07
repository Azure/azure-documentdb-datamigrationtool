
namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    sealed class MongoDbSourceAdapterInstanceConfiguration : IMongoDbSourceAdapterInstanceConfiguration
    {
        public string ConnectionString { get; set; }
        public string Collection { get; set; }
        public string Query { get; set; }
        public string Projection  { get; set; }
    }
}
