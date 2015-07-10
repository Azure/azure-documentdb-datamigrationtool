using System.Net;

namespace Microsoft.DataTransfer.HBase.Client.Authentication
{
    interface IRestAuthentication
    {
        void Apply(WebRequest request);
    }
}
