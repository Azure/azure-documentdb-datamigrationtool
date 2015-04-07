using Microsoft.DataTransfer.MongoDb.Shared;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    interface IMongoDbSourceAdapterInstanceConfiguration : IMongoDbAdapterConfiguration
    {
        string Query { get; }
        string Projection { get; }
    }
}
