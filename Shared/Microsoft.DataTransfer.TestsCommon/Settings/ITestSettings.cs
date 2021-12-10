
namespace Microsoft.DataTransfer.TestsCommon.Settings
{
    public interface ITestSettings
    {
        string SqlConnectionString { get; }
        string MongoConnectionString { get; }
        string AzureStorageConnectionString { get; }
        string DynamoDbConnectionString { get; }
        string HBaseConnectionString { get; }
        string DocumentDbConnectionString { get; }
        string RavenDbConnectionString(string databaseName);
    }
}
