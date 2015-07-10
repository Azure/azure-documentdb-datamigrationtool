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

        public async Task<StreamReader> CreateReader(CancellationToken cancellation)
        {
            var request = HttpWebRequest.CreateHttp(Id);

            try
            {
                return new ResponseStreamReader(await request.GetResponseAsync());
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
