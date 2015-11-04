using Microsoft.DataTransfer.ServiceModel.Errors;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// Contains default configuration for data transfer infrastructure.
    /// </summary>
    public interface IInfrastructureDefaults
    {
        /// <summary>
        /// Gets the default data transfer error details configuration.
        /// </summary>
        ErrorDetails ErrorDetails { get; }
    }
}
