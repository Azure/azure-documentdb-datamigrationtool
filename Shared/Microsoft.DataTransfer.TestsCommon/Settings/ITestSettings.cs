
namespace Microsoft.DataTransfer.TestsCommon.Settings
{
    public interface ITestSettings
    {
        string SqlConnectionString { get; }
        string MongoConnectionString { get; }
        string DocumentDbConnectionString(string databaseName);
    }
}
