using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.HBase.Client.Authentication;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client
{
    abstract class RestClientBase
    {
        private const string JsonMimeType = "application/json";

        private Uri serviceUrl;
        private IRestAuthentication authentication;
        private JsonSerializer serializer;

        public RestClientBase(string serviceUrl, IRestAuthentication authentication)
        {
            Guard.NotNull("authentication", authentication);

            this.serviceUrl = new Uri(EnsureTrailingSlash(serviceUrl));
            this.authentication = authentication;

            serializer = JsonSerializer.CreateDefault();
        }

        private static string EnsureTrailingSlash(string url)
        {
            return url.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? url : url + "/";
        }

        protected async Task<TResult> ExecuteRequestAsync<TResult>(string method, string relativePath, object data,
            Func<HttpWebResponse, TResult> responseHandler, CancellationToken cancellation)
        {
            Guard.NotEmpty("method", method);
            Guard.NotNull("responseHandler", responseHandler);

            var request = HttpWebRequest.CreateHttp(new Uri(serviceUrl, relativePath));

            request.Method = method;
            request.Accept = JsonMimeType;

            authentication.Apply(request);

            await SetRequestContent(request, data, cancellation);

            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    return responseHandler((HttpWebResponse)response);
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    webException.Response.Dispose();
                }

                throw;
            }
        }

        private async Task SetRequestContent(HttpWebRequest request, object data, CancellationToken cancellation)
        {
            Guard.NotNull("request", request);

            if (data == null)
                return;

            request.ContentType = JsonMimeType;

            using (var jsonStream = new MemoryStream())
            {
                using (var jsonWriter = new StreamWriter(jsonStream))
                {
                    serializer.Serialize(jsonWriter, data);
                    jsonWriter.Flush();

                    request.ContentLength = jsonStream.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        jsonStream.Seek(0, SeekOrigin.Begin);
                        await jsonStream.CopyToAsync(requestStream, 1024, cancellation);
                        await requestStream.FlushAsync(cancellation);
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times",
            Justification = "As per .NET framework design guidelines Dispose method should be re-entrant")]
        protected TResult HandleJsonResponse<TResult>(HttpWebResponse response)
        {
            Guard.NotNull("response", response);

            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<TResult>(jsonReader);
            }
        }

        protected object HandleOkResponse(HttpWebResponse response)
        {
            Guard.NotNull("response", response);

            if (response.StatusCode != HttpStatusCode.OK)
                throw Errors.InvalidServerResponse(response.StatusCode);

            return null;
        }
    }
}
