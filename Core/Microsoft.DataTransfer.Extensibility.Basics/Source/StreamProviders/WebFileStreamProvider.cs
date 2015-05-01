using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders
{
    sealed class WebFileStreamProvider : ISourceStreamProvider
    {
        public string Id { get; private set; }

        public WebFileStreamProvider(string url)
        {
            Id = url;
        }

        public async Task<StreamReader> CreateReader()
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
