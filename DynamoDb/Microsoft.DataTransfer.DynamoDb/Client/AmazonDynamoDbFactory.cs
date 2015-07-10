using Amazon.DynamoDBv2;
using Amazon.Runtime;
using System;

namespace Microsoft.DataTransfer.DynamoDb.Client
{
    static class AmazonDynamoDbFactory
    {
        public static IAmazonDynamoDB Create(string connectionString)
        {
            var connectionSettings = ParseConnectionString(connectionString);

            return new AmazonDynamoDBClient(
                new BasicAWSCredentials(connectionSettings.AccessKey, connectionSettings.SecretKey),
                new AmazonDynamoDBConfig { ServiceURL = connectionSettings.ServiceUrl });
        }

        private static IDynamoDbConnectionSettings ParseConnectionString(string connectionString)
        {
            var connectionSettings = DynamoDbConnectionStringBuilder.Parse(connectionString);

            if (String.IsNullOrEmpty(connectionSettings.ServiceUrl))
                throw Errors.ServiceUrlMissing();

            if (String.IsNullOrEmpty(connectionSettings.AccessKey))
                throw Errors.AccessKeyMissing();

            if (String.IsNullOrEmpty(connectionSettings.SecretKey))
                throw Errors.SecretKeyMissing();

            return connectionSettings;
        }
    }
}
