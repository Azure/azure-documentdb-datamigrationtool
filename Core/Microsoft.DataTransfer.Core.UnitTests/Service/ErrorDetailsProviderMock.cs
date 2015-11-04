using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.Core.UnitTests.Service
{
    sealed class ErrorDetailsProviderMock : IErrorDetailsProvider
    {
        public static readonly ErrorDetailsProviderMock Instance = new ErrorDetailsProviderMock();

        public string Get(Exception error)
        {
            return error.ToString();
        }

        public string GetCritical(Exception error)
        {
            return error.ToString();
        }
    }
}
