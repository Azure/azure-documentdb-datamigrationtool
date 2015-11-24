using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;

namespace Microsoft.DataTransfer.WpfHost.Helpers
{
    static class InfrastructureConfigurationProperties
    {
        public static readonly string ErrorLog =
            ObjectExtensions.MemberName<IInfrastructureConfiguration>(m => m.ErrorLog);

        public static readonly string OverwriteErrorLog =
            ObjectExtensions.MemberName<IInfrastructureConfiguration>(m => m.OverwriteErrorLog);

        public static readonly string ErrorDetails =
            ObjectExtensions.MemberName<IInfrastructureConfiguration>(m => m.ErrorDetails);

        public static readonly string ProgressUpdateInterval =
            ObjectExtensions.MemberName<IInfrastructureConfiguration>(m => m.ProgressUpdateInterval);
    }
}
