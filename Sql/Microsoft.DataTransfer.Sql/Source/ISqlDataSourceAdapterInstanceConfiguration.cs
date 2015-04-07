using Microsoft.DataTransfer.Sql.Shared;

namespace Microsoft.DataTransfer.Sql.Source
{
    interface ISqlDataSourceAdapterInstanceConfiguration : ISqlDataAdapterConfiguration
    {
        string Query { get; }
        string NestingSeparator { get; }
    }
}
