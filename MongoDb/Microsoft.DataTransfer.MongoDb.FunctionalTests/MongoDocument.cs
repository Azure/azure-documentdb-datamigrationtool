using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.MongoDb.FunctionalTests
{
    sealed class MongoDocument
    {
        public ObjectId Id { get; private set; }

        [BsonExtraElements]
        public IDictionary<string, object> Properties { get; private set; }

        public MongoDocument(IDictionary<string, object> properties)
        {
            Id = ObjectId.GenerateNewId();
            Properties = properties;
        }
    }
}
