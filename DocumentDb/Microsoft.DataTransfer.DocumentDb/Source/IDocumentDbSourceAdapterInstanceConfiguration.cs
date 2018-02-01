
namespace Microsoft.DataTransfer.DocumentDb.Source
{
    interface IDocumentDbSourceAdapterInstanceConfiguration
    {
        string Database { get; }
        string Collection { get; }
        string Query { get; }
    }
}
