using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Files.Source.WebFile
{
    sealed class WebFileSourceStreamProvider : ISourceStreamProvider
    {
        public string Id { get; private set; }

        public WebFileSourceStreamProvider(string url)
        {
            Id = url;
        }

        public async Task<Stream> CreateStream(CancellationToken cancellation)
        {
            var request = WebRequest.CreateHttp(Id);

            try
            {
                return new WebResponseStream(await request.GetResponseAsync());
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                    webException.Response.Close();

                throw;
            }
        }
    }
}
