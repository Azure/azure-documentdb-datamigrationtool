using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.DataTransfer.DynamoDb.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DynamoDb.FunctionalTests
{
    static class DynamoDbHelper
    {
        public static async Task CreateSampleTableAsync(string connectionString, string tableName, IEnumerable<IReadOnlyDictionary<string, object>> items)
        {
            const int BatchSize = 25;

            using (var client = CreateClient(connectionString))
            {
                var tableResponse = await client.CreateTableAsync(new CreateTableRequest(
                    tableName, 
                    new List<KeySchemaElement>
                    {
                        new KeySchemaElement("HashKey", KeyType.HASH),
                        new KeySchemaElement("RangeKey", KeyType.RANGE),
                    },
                    new List<AttributeDefinition>
                    {
                        new AttributeDefinition("HashKey", ScalarAttributeType.S),
                        new AttributeDefinition("RangeKey", ScalarAttributeType.N)
                    },
                    new ProvisionedThroughput(1, 1)));

                Assert.AreEqual(HttpStatusCode.OK, tableResponse.HttpStatusCode, TestResources.FailedToCreateTable);

                var itemsBatch = new List<WriteRequest>(BatchSize);
                foreach (var item in items)
                {
                    itemsBatch.Add(new WriteRequest(new PutRequest(AsAttributes(item))));

                    if (itemsBatch.Count >= BatchSize)
                    {
                        await client.BatchWriteItemAsync(new BatchWriteItemRequest(
                            new Dictionary<string, List<WriteRequest>> { { tableName, itemsBatch } }));
                        itemsBatch.Clear();
                    }
                }

                if (itemsBatch.Count > 0)
                    await client.BatchWriteItemAsync(new BatchWriteItemRequest(
                        new Dictionary<string, List<WriteRequest>> { { tableName, itemsBatch } }));
            }
        }

        public static async Task DeleteTableAsync(string connectionString, string tableName)
        {
            using (var client = CreateClient(connectionString))
            {
                await client.DeleteTableAsync(new DeleteTableRequest(tableName));
            }
        }

        private static Dictionary<string, AttributeValue> AsAttributes(IReadOnlyDictionary<string, object> item)
        {
            return item.ToDictionary(i => i.Key, i => AsAttribute(i.Value));
        }

        private static AttributeValue AsAttribute(object value)
        {
            if (value == null)
                return new AttributeValue { NULL = true };

            if (value is int || value is long || value is float || value is double)
                return new AttributeValue { N = value.ToString() };

            if (value is string)
                return new AttributeValue { S = (string)value };

            if (value is bool)
                return new AttributeValue { IsBOOLSet = true, BOOL = (bool)value };

            if (value is IReadOnlyDictionary<string, object>)
                return new AttributeValue { IsMSet = true,  M = AsAttributes((IReadOnlyDictionary<string, object>)value) };

            if (value is byte[])
                return new AttributeValue { B = new MemoryStream((byte[])value) };

            if (value is IEnumerable)
                return new AttributeValue { IsLSet = true,  L = ((IEnumerable)value).OfType<object>().Select(v => AsAttribute(v)).ToList() };

            return new AttributeValue { S = value.ToString() };
        }

        private static IAmazonDynamoDB CreateClient(string connectionString)
        {
            return AmazonDynamoDbFactory.Create(connectionString);
        }
    }
}
