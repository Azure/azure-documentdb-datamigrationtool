using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.DataTransfer.JsonExtension.Settings;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class JsonDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "JSON";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<JsonSinkSettings>();
            settings.Validate();

            if (settings.FilePath != null)
            {
                Console.WriteLine($"Writing to file '{settings.FilePath}'");
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
                Console.WriteLine($"Completed writing data to file '{settings.FilePath}'");
            }
        }
    }
}