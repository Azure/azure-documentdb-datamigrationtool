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
                    var list = await ReadFileAsync(settings.FilePath, logger, cancellationToken);

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
                        var list = await ReadFileAsync(filePath, logger, cancellationToken);

                        if (list != null)
                        {
                            foreach (var listItem in list)
                            {
                                yield return new JsonDictionaryDataItem(listItem);
                            }
                        }
                    }
                }
                else if (Uri.IsWellFormedUriString(settings.FilePath, UriKind.RelativeOrAbsolute))
                {
                    logger.LogInformation("Reading from URI '{FilePath}'", settings.FilePath);

                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(settings.FilePath, cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError("Failed to read {FilePath}. Response was: {ResponseCode} {ResponseMessage}", settings.FilePath, response.StatusCode, response.ReasonPhrase);
                        yield break;
                    }

                    var json = await response.Content.ReadAsStringAsync(cancellationToken);

                    var list = await ReadJsonItemsAsync(json, logger, cancellationToken);

                    if (list != null)
                    {
                        foreach (var listItem in list)
                        {
                            yield return new JsonDictionaryDataItem(listItem);
                        }
                    }
                }
                else
                {
                    logger.LogWarning("No content was found at configured path '{FilePath}'", settings.FilePath);
                    yield break;
                }

                logger.LogInformation("Completed reading '{FilePath}'", settings.FilePath);
            }
        }

        private static async Task<List<Dictionary<string, object?>>?> ReadFileAsync(string filePath, ILogger logger, CancellationToken cancellationToken)
        {
            var jsonText = await File.ReadAllTextAsync(filePath, cancellationToken);
            return await ReadJsonItemsAsync(jsonText, logger, cancellationToken);
        }

        private static async Task<List<Dictionary<string, object?>>?> ReadJsonItemsAsync(string jsonText, ILogger logger, CancellationToken cancellationToken)
        {
            try
            {
                using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
                return await JsonSerializer.DeserializeAsync<List<Dictionary<string, object?>>>(stream, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                // list failed
            }

            var list = new List<Dictionary<string, object?>>();
            try
            {
                using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
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
                logger.LogWarning("No records read from '{Content}'", jsonText);
            }

            return list;
        }
    }
}