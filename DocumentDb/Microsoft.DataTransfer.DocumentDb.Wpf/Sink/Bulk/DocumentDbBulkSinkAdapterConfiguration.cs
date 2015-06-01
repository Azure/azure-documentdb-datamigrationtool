using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Sink.Bulk;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Bulk
{
    sealed class DocumentDbBulkSinkAdapterConfiguration : DocumentDbSinkAdapterConfiguration, IDocumentDbBulkSinkAdapterConfiguration
    {
        public static readonly string StoredProcFilePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.StoredProcFile);

        public static readonly string BatchSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.BatchSize);

        public static readonly string MaxScriptSizePropertyName =
            ObjectExtensions.MemberName<IDocumentDbBulkSinkAdapterConfiguration>(c => c.MaxScriptSize);

        private string storedProcFile;
        private int? batchSize;
        private int? maxScriptSize;

        public string StoredProcFile
        {
            get { return storedProcFile; }
            set { SetProperty(ref storedProcFile, value); }
        }

        public int? BatchSize
        {
            get { return batchSize; }
            set { SetProperty(ref batchSize, value, ValidatePositiveInteger); }
        }

        public int? MaxScriptSize
        {
            get { return maxScriptSize; }
            set { SetProperty(ref maxScriptSize, value, ValidatePositiveInteger); }
        }

        public DocumentDbBulkSinkAdapterConfiguration()
        {
            BatchSize = Defaults.Current.BulkSinkBatchSize;
            MaxScriptSize = Defaults.Current.BulkSinkMaxScriptSize;
        }
    }
}
