using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Contains dynamic resources for data transfer infrastructure configuration.
    /// </summary>
    public sealed class DynamicConfigurationResources : DynamicResourcesBase
    {
        /// <summary>
        /// Gets the description for data transfer error details.
        /// </summary>
        public static string Errors_Details
        {
            get { return Format(ConfigurationResources.Errors_DetailsFormat, InfrastructureDefaults.Current.ErrorDetails,
                    String.Join(", ", Enum.GetNames(typeof(ErrorDetails)))); }
        }

        private DynamicConfigurationResources() { }
    }
}
