using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DynamoDb.Source;
using Microsoft.DataTransfer.DynamoDb.Wpf.Shared;

namespace Microsoft.DataTransfer.DynamoDb.Wpf.Source
{
    sealed class DynamoDbSourceAdapterConfiguration : DynamoDbAdapterConfiguration, IDynamoDbSourceAdapterConfiguration
    {
        public static readonly string RequestPropertyName =
            ObjectExtensions.MemberName<IDynamoDbSourceAdapterConfiguration>(c => c.Request);

        public static readonly string RequestFilePropertyName =
            ObjectExtensions.MemberName<IDynamoDbSourceAdapterConfiguration>(c => c.RequestFile);

        private bool useRequestFile;
        private string request;
        private string requestFile;

        public bool UseRequestFile
        {
            get { return useRequestFile; }
            set
            {
                SetProperty(ref useRequestFile, value);
                ValidateRequest();
            }
        }

        public string Request
        {
            get { return useRequestFile ? null : request; }
            set
            {
                SetProperty(ref request, value);
                ValidateRequest();
            }
        }

        public string RequestFile
        {
            get { return useRequestFile ? requestFile : null; }
            set
            {
                SetProperty(ref requestFile, value);
                ValidateRequest();
            }
        }

        private void ValidateRequest()
        {
            SetErrors(RequestPropertyName, !useRequestFile ? ValidateNonEmptyString(request) : null);
            SetErrors(RequestFilePropertyName, useRequestFile ? ValidateNonEmptyString(requestFile) : null);
        }
    }
}
