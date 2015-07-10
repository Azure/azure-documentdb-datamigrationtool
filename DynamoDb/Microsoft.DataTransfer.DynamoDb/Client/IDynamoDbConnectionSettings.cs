
namespace Microsoft.DataTransfer.DynamoDb.Client
{
    interface IDynamoDbConnectionSettings
    {
        string ServiceUrl { get; }
        string AccessKey { get; }
        string SecretKey { get; }
    }
}
