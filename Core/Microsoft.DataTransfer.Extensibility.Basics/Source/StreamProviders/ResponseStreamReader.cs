using Microsoft.DataTransfer.Basics;
using System.IO;
using System.Net;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders
{
    sealed class ResponseStreamReader : StreamReader
    {
        private WebResponse response;

        public ResponseStreamReader(WebResponse response)
            : base(GetResponseStream(response))
        {
            this.response = response;
        }

        private static Stream GetResponseStream(WebResponse response)
        {
            Guard.NotNull("response", response);
            return response.GetResponseStream();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
                return;

            TrashCan.Throw(ref response, r => r.Close());
        }
    }
}
