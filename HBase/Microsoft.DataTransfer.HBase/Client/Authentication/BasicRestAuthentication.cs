using System;
using System.Net;
using System.Text;

namespace Microsoft.DataTransfer.HBase.Client.Authentication
{
    sealed class BasicRestAuthentication : IRestAuthentication
    {
        private string username;
        private string password;

        public BasicRestAuthentication(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public void Apply(WebRequest request)
        {
            request.Headers.Set(HttpRequestHeader.Authorization,
                "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password)));
        }
    }
}
