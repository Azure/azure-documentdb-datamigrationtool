using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.MongoDb.Source
{
    sealed class BsonDocumentDataItem : IDataItem
    {
        private static readonly DateTime EpochStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private BsonDocument data;

        public BsonDocumentDataItem(BsonDocument data)
        {
            Guard.NotNull("data", data);
            this.data = data;
        }

        public IEnumerable<string> GetFieldNames()
        {
            return data.Names;
        }

        public object GetValue(string fieldName)
        {
            Guard.NotNull("fieldName", fieldName);
            return ConvertBsonValue(data[fieldName]);
        }

        private static object ConvertBsonValue(BsonValue value)
        {
            /*
             * Possible return types for this object: 
             * 1. BsonDocumentDataItem (for BsonDocument and JavascriptWithScope)
             * 2. String (for string, id, javascript, symbol)
             * 3. Guid
             * 4. Numeric types: double, float, int, long
             * 5. DateTime
             * 6. null (timestamp, maxkey, minkey, undefined, BsonNull)
             * 7. Byte array (for BsonBinaryData)
             * 8. IEnumerable (for BsonArray)
             */

            if (value.IsObjectId)
                return value.AsObjectId.ToString();

            if (value.IsGuid)
                return value.AsGuid;

            if (value.IsBsonJavaScriptWithScope)
                return new BsonDocumentDataItem(new BsonDocument
                    {
                        { "code", value.AsBsonJavaScriptWithScope.Code },
                        { "scope", value.AsBsonJavaScriptWithScope.Scope }
                    });

            if (value.IsBsonJavaScript)
                return value.AsBsonJavaScript.Code;

            if (value.IsBsonTimestamp)
                return EpochStartTime
                    .AddSeconds(value.AsBsonTimestamp.Timestamp)
                    .AddTicks(Math.Min(value.AsBsonTimestamp.Increment, TimeSpan.TicksPerSecond - 1));

            if (value.IsBsonMaxKey || value.IsBsonMinKey || value.IsBsonUndefined || value.IsBsonNull)
                return null;

            if (value.IsBsonBinaryData)
                return value.AsBsonBinaryData.Bytes;

            if (value.IsBsonRegularExpression)
                return value.AsBsonRegularExpression.ToString();

            if (value.IsBsonSymbol)
                return value.AsBsonSymbol.ToString();

            if (value.IsBsonArray)
                return value.AsBsonArray.Select(v => ConvertBsonValue(v));

            if (value.IsBsonDocument)
                return new BsonDocumentDataItem(value.AsBsonDocument);

            return BsonTypeMapper.MapToDotNetValue(value);
        }
    }
}
