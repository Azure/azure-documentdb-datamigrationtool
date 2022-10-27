using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.DataTransfer.JsonExtension.Settings;
using Microsoft.Extensions.Logging;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class JsonDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "JSON";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, IDataSourceExtension dataSource, ILogger logger, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<JsonSinkSettings>();
            settings.Validate();

            if (settings.FilePath != null)
            {
                logger.LogInformation("Writing to file '{FilePath}'", settings.FilePath);
                await using var stream = File.Create(settings.FilePath);
                await using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                {
                    Indented = settings.Indented
                });
                writer.WriteStartArray();

                await foreach (var item in dataItems.WithCancellation(cancellationToken))
                {
                    DataItemJsonConverter.WriteDataItem(writer, item, settings.IncludeNullFields);
                }

                writer.WriteEndArray();
                logger.LogInformation("Completed writing data to file '{FilePath}'", settings.FilePath);
            }
        }
    }
}