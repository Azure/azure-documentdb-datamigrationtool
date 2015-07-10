using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.HBase.Client.Addressing;
using Microsoft.DataTransfer.HBase.Client.Authentication;
using Microsoft.DataTransfer.HBase.Client.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client
{
    sealed class StargateClient : RestClientBase, IStargateClient, IStargateScanClient
    {
        private static readonly Regex ScannerUrlRegex = new Regex(
            "/(?<tableName>[^/]*)/scanner/(?<scannerId>[^/]*)/?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly IStargateAddressCatalog catalog = new StargateAddressCatalog();

        public StargateClient(string serviceUrl, IRestAuthentication authentication)
            : base(serviceUrl, authentication) { }

        public Task<string> GetClusterVersionAsync(CancellationToken cancellation)
        {
            return ExecuteRequestAsync<string>(
                WebRequestMethods.Http.Get, catalog.ClusterVersion(),
                null, HandleJsonResponse<string>, cancellation);
        }

        public Task<IAsyncEnumerator<HBaseRow>> ScanAsync(string tableName, string filter, int batchSize, CancellationToken cancellation)
        {
            return ExecuteRequestAsync<IAsyncEnumerator<HBaseRow>>(
                WebRequestMethods.Http.Post, catalog.CreateScanner(tableName),
                new { filter = filter, batch = batchSize }, HandleCreateScannerResponse, cancellation);
        }

        private IAsyncEnumerator<HBaseRow> HandleCreateScannerResponse(HttpWebResponse response)
        {
            Guard.NotNull("response", response);

            if (response.StatusCode != HttpStatusCode.Created)
                throw Errors.FailedToCreateScanner(response.StatusCode);

            var scannerUrl = response.Headers[HttpResponseHeader.Location];
            if (String.IsNullOrEmpty(scannerUrl))
                throw Errors.ScannerLocationMissing();

            var scannerDefinitionMatch = ScannerUrlRegex.Match(scannerUrl);
            if (!scannerDefinitionMatch.Success)
                throw Errors.InvalidScannerLocation();

            return new AsyncTableScanner(this,
                scannerDefinitionMatch.Groups["tableName"].Value,
                scannerDefinitionMatch.Groups["scannerId"].Value);
        }

        public async Task<IReadOnlyList<HBaseRow>> ScanNextChunkAsync(IScannerReference scanner, CancellationToken cancellation)
        {
            var response = await ExecuteRequestAsync<ScanResponseSurrogate>(
                WebRequestMethods.Http.Get, catalog.Scanner(scanner.TableName, scanner.ScannerId),
                null, HandleJsonResponse<ScanResponseSurrogate>, cancellation);
            return response == null ? null : response.Row;
        }

        public Task CloseScannerAsync(IScannerReference scanner, CancellationToken cancellation)
        {
            return ExecuteRequestAsync<object>(
                "DELETE", catalog.Scanner(scanner.TableName, scanner.ScannerId),
                null, HandleOkResponse, cancellation);
        }
    }
}
