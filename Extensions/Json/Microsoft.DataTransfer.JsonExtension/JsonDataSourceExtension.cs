using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.DataTransfer.JsonExtension.Settings;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class JsonDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "JSON";
        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<JsonSourceSettings>();
            settings.Validate();

            if (settings.FilePath != null)
            {
                await using var file = File.OpenRead(settings.FilePath);
                var list = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object?>>>(file, cancellationToken: cancellationToken);

                if (list != null)
                {
                    foreach (var listItem in list)
                    {
                        yield return new JsonDictionaryDataItem(listItem);
                    }
                }
            }
        }
    }
}