using Microsoft.DataTransfer.DocumentDb.Sink.Parallel;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink.Parallel
{
    sealed class DocumentDbParallelSinkAdapterConfiguration : DocumentDbSinkAdapterConfiguration, IDocumentDbParallelSinkAdapterConfiguration
    {
        public static readonly string ParallelRequestsPropertyName =
            ObjectExtensions.MemberName<IDocumentDbParallelSinkAdapterConfiguration>(c => c.ParallelRequests);

        private int? parallelRequests;

        public int? ParallelRequests
        {
            get { return parallelRequests; }
            set { SetProperty(ref parallelRequests, value, ValidatePositiveInteger); }
        }

        public DocumentDbParallelSinkAdapterConfiguration()
        {
            ParallelRequests = Defaults.Current.ParallelSinkNumberOfParallelRequests;
        }
    }
}
