using Microsoft.DataTransfer.DocumentDb.Shared;

namespace Microsoft.DataTransfer.DocumentDb.Source
{
    interface IDocumentDbSourceAdapterInstanceConfiguration : IDocumentDbAdapterInstanceConfiguration
    {
        string Query { get; }
    }
}
