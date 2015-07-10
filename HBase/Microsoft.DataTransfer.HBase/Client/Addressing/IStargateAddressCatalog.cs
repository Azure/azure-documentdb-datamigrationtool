
namespace Microsoft.DataTransfer.HBase.Client.Addressing
{
    interface IStargateAddressCatalog
    {
        string ClusterVersion();
        string CreateScanner(string tableName);
        string Scanner(string tableName, string scannerId);
    }
}
