
namespace Microsoft.DataTransfer.HBase.Client.Entities
{
    interface IScannerReference
    {
        string TableName { get; }
        string ScannerId { get; }
    }
}
