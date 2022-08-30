using Microsoft.DataTransfer.Interfaces;
using MongoDB.Bson;

namespace Microsoft.DataTransfer.MongoExtension;
public class MongoDataItem : IDataItem
{
    private readonly BsonDocument record;

    public MongoDataItem(BsonDocument record)
    {
        this.record = record;
    }

    public IEnumerable<string> GetFieldNames()
    {
        return record.Names;
    }

    public object? GetValue(string fieldName)
    {
        var value = record.GetValue(fieldName);
        return BsonTypeMapper.MapToDotNetValue(value);
    }
}
