using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.Core.ErrorsImplementation
{
    sealed class ErrorDetailsProvider : IErrorDetailsProvider
    {
        private readonly IErrorDetailsConfiguration configuration;

        public ErrorDetailsProvider(IErrorDetailsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Get(Exception error)
        {
            if (error == null)
                return String.Empty;

            return GetDetailsConfiguration() == ErrorDetails.All ? error.ToString() : error.Message;
        }

        public string GetCritical(Exception error)
        {
            if (error == null)
                return String.Empty;

            return GetDetailsConfiguration() == ErrorDetails.None ? error.Message : error.ToString();
        }

        private ErrorDetails GetDetailsConfiguration()
        {
            return configuration.ErrorDetails ?? InfrastructureDefaults.Current.ErrorDetails;
        }
    }
}
