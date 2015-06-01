
namespace Microsoft.DataTransfer.DocumentDb.Source
{
    interface IDocumentDbSourceAdapterInstanceConfiguration
    {
        string Collection { get; }
        string Query { get; }
    }
}
