using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.DataTransfer.JsonExtension.Settings;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.DataTransfer.JsonExtension
{
    [Export(typeof(IDataSourceExtension))]
    public class JsonDataSourceExtension : IDataSourceExtension
    {
        public string DisplayName => "JSON";
        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, ILogger logger, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var settings = config.Get<JsonSourceSettings>();
            settings.Validate();

            if (settings.FilePath != null)
            {
                if (File.Exists(settings.FilePath))
                {
                    logger.LogInformation("Reading file '{FilePath}'", settings.FilePath);
                    var list = await ReadFileAsync(cancellationToken, settings.FilePath, logger);

                    if (list != null)
                    {
                        foreach (var listItem in list)
                        {
                            yield return new JsonDictionaryDataItem(listItem);
                        }
                    }
                }
                else if (Directory.Exists(settings.FilePath))
                {
                    string[] files = Directory.GetFiles(settings.FilePath, "*.json", SearchOption.AllDirectories);
                    logger.LogInformation("Reading {FileCount} files from '{Folder}'", files.Length, settings.FilePath);
                    foreach (string filePath in files.OrderBy(f => f))
                    {
                        logger.LogInformation("Reading file '{FilePath}'", filePath);
                        var list = await ReadFileAsync(cancellationToken, filePath, logger);

                        if (list != null)
                        {
                            foreach (var listItem in list)
                            {
                                yield return new JsonDictionaryDataItem(listItem);
                            }
                        }
                    }
                }
                logger.LogInformation("Completed reading '{FilePath}'", settings.FilePath);
            }
        }

        private static async Task<List<Dictionary<string, object?>>?> ReadFileAsync(CancellationToken cancellationToken, string filePath, ILogger logger)
        {
            var file = await File.ReadAllTextAsync(filePath, cancellationToken);
            try
            {
                using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(file));
                return await JsonSerializer.DeserializeAsync<List<Dictionary<string, object?>>>(stream, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                // list failed
            }

            var list = new List<Dictionary<string, object?>>();
            try
            {
                using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(file));
                var item = await JsonSerializer.DeserializeAsync<Dictionary<string, object?>>(stream, cancellationToken: cancellationToken);
                if (item != null)
                {
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                // single item failed
            }

            if (!list.Any())
            {
                logger.LogWarning("No records read from '{FilePath}'", filePath);
            }

            return list;
        }
    }
}