
namespace Microsoft.DataTransfer.DocumentDb.Client
{
    interface IDocumentDbConnectionSettings
    {
        string AccountEndpoint { get; }
        string AccountKey { get; }
        string Database { get; }
    }
}
