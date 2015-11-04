using Microsoft.DataTransfer.Basics.Extensions;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    static class SharedDocumentDbAdapterConfigurationProperties
    {
        public static readonly string ConnectionString =
            ObjectExtensions.MemberName<ISharedDocumentDbAdapterConfiguration>(c => c.ConnectionString);

        public static readonly string ConnectionMode =
            ObjectExtensions.MemberName<ISharedDocumentDbAdapterConfiguration>(c => c.ConnectionMode);

        public static readonly string Retries =
            ObjectExtensions.MemberName<ISharedDocumentDbAdapterConfiguration>(c => c.Retries);

        public static readonly string RetryInterval =
            ObjectExtensions.MemberName<ISharedDocumentDbAdapterConfiguration>(c => c.RetryInterval);
    }
}
