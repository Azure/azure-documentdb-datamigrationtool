using Microsoft.DataTransfer.Basics.Files.Shared;
using System.IO;
using System.Net;

namespace Microsoft.DataTransfer.Basics.Files.Source.WebFile
{
    sealed class WebResponseStream : WrapperStream
    {
        private WebResponse response;

        public WebResponseStream(WebResponse response)
            : base(GetResponseStream(response))
        {
            this.response = response;
        }

        private static Stream GetResponseStream(WebResponse response)
        {
            Guard.NotNull("response", response);
            return response.GetResponseStream();
        }

        public override void Close()
        {
            base.Close();
            response.Close();
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
