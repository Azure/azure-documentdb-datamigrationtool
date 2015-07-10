
namespace Microsoft.DataTransfer.TestsCommon.Settings
{
    public interface ITestSettings
    {
        string SqlConnectionString { get; }
        string MongoConnectionString { get; }
        string AzureStorageConnectionString { get; }
        string DynamoDbConnectionString { get; }
        string HBaseConnectionString { get; }
        string DocumentDbConnectionString(string databaseName);
        string RavenDbConnectionString(string databaseName);
    }
}
