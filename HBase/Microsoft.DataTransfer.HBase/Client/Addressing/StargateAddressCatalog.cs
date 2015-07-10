using System;
using System.Globalization;

namespace Microsoft.DataTransfer.HBase.Client.Addressing
{
    sealed class StargateAddressCatalog : IStargateAddressCatalog
    {
        public string ClusterVersion()
        {
            return "version/cluster";
        }

        public string CreateScanner(string tableName)
        {
            return Format("{0}/scanner", Uri.EscapeDataString(tableName));
        }

        public string Scanner(string tableName, string scannerId)
        {
            return Format("{0}/scanner/{1}", Uri.EscapeDataString(tableName), Uri.EscapeDataString(scannerId));
        }

        private static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
