using Microsoft.DataTransfer.ServiceModel.Errors;

namespace Microsoft.DataTransfer.Core.ErrorsImplementation
{
    sealed class ErrorDetailsProviderFactory : IErrorDetailsProviderFactory
    {
        public IErrorDetailsProvider Create(IErrorDetailsConfiguration configuration)
        {
            return new ErrorDetailsProvider(configuration);
        }
    }
}
