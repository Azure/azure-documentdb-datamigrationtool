using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.DataTransfer.JsonExtension.Settings;
using Microsoft.Extensions.Configuration;
using System;

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
                if (File.Exists(settings.FilePath))
                {
                    Console.WriteLine($"Reading file '{settings.FilePath}'");
                    var list = await ReadFileAsync(cancellationToken, settings.FilePath);

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
                    Console.WriteLine($"Reading {files.Length} files from '{settings.FilePath}'");
                    foreach (string filePath in files.OrderBy(f => f))
                    {
                        Console.WriteLine($"Reading file '{filePath}'");
                        var list = await ReadFileAsync(cancellationToken, filePath);

                        if (list != null)
                        {
                            foreach (var listItem in list)
                            {
                                yield return new JsonDictionaryDataItem(listItem);
                            }
                        }
                    }
                }
                Console.WriteLine($"Completed reading '{settings.FilePath}'");
            }
        }

        private static async Task<List<Dictionary<string, object?>>?> ReadFileAsync(CancellationToken cancellationToken, string filePath)
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
                Console.WriteLine($"No records read from '{filePath}'");
            }

            return list;
        }
    }
}