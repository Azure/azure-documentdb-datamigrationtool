using System.Net;

namespace Microsoft.DataTransfer.HBase.Client.Authentication
{
    sealed class NoRestAuthentication : IRestAuthentication
    {
        public static readonly NoRestAuthentication Instance = new NoRestAuthentication();

        public void Apply(WebRequest request) { }
    }
}
