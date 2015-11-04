using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.ServiceModel.Errors
{
    /// <summary>
    /// Configuration for detailed data transfer errors information.
    /// </summary>
    public interface IErrorDetailsConfiguration
    {
        /// <summary>
        /// Gets the data transfer error details configuration.
        /// </summary>
        [Display(ResourceType = typeof(DynamicConfigurationResources), Description = "Errors_Details")]
        ErrorDetails? ErrorDetails { get; }
    }
}
