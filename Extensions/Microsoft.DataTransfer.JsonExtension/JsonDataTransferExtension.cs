using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataTransferExtension))]
    public class JsonDataTransferExtension : IDataTransferExtension
    {
        private string? _filepath;
        public string DisplayName => "JSON";
        public async IAsyncEnumerable<IDataItem> ReadAsSourceAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var file = File.OpenRead(_filepath);
            var list = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object?>>>(file, cancellationToken: cancellationToken);

            foreach (var listItem in list)
            {
                yield return new JsonDictionaryDataItem(listItem);
            }
        }

        public Task WriteAsSinkAsync(IAsyncEnumerable<IDataItem> dataItems, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Configure(IConfiguration configuration)
        {
            _filepath = configuration.GetValue<string>("FileSource");

            return Task.CompletedTask;
        }
    }
}