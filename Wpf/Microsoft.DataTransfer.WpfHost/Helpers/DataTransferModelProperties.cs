using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel;

namespace Microsoft.DataTransfer.WpfHost.Helpers
{
    static class DataTransferModelProperties
    {
        public static readonly string InfrastructureConfiguration =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.InfrastructureConfiguration);

        public static readonly string SourceAdapterName = 
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.SourceAdapterName);

        public static readonly string SourceConfiguration =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.SourceConfiguration);

        public static readonly string SinkAdapterName =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.SinkAdapterName);

        public static readonly string SinkConfiguration =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.SinkConfiguration);

        public static readonly string HasImportStarted =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.HasImportStarted);

        public static readonly string ImportCancellation =
            ObjectExtensions.MemberName<IDataTransferModel>(m => m.ImportCancellation);
    }
}
