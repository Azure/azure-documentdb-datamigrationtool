
namespace Microsoft.DataTransfer.ServiceModel.Errors
{
    /// <summary>
    /// Provides <see cref="IErrorDetailsProvider" /> instances to extract error details.
    /// </summary>
    public interface IErrorDetailsProviderFactory
    {
        /// <summary>
        /// Creates a new instance of data transfer error details provider.
        /// </summary>
        /// <param name="configuration">Error details configuration.</param>
        /// <returns>An instance of <see cref="IErrorDetailsProvider" /> that can be used to get error details.</returns>
        IErrorDetailsProvider Create(IErrorDetailsConfiguration configuration);
    }
}
