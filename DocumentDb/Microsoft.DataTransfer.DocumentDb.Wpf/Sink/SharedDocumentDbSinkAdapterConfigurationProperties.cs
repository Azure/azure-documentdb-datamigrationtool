using Microsoft.DataTransfer.Basics.Extensions;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    static class SharedDocumentDbSinkAdapterConfigurationProperties
    {
        public static readonly string CollectionThroughput =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.CollectionThroughput);

        public static readonly string UseIndexingPolicyFile =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.UseIndexingPolicyFile);

        public static readonly string IndexingPolicy =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.IndexingPolicy);

        public static readonly string IndexingPolicyFile =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.IndexingPolicyFile);

        public static readonly string IdField =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.IdField);

        public static readonly string DisableIdGeneration =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.DisableIdGeneration);

        public static readonly string UpdateExisting =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.UpdateExisting);

        public static readonly string Dates =
            ObjectExtensions.MemberName<ISharedDocumentDbSinkAdapterConfiguration>(c => c.Dates);
    }
}
