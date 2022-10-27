using Microsoft.DataTransfer.Interfaces;
using Microsoft.DataTransfer.MongoExtension.Settings;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.ComponentModel.Composition;
using Microsoft.Extensions.Logging;

namespace Microsoft.DataTransfer.MongoExtension;
[Export(typeof(IDataSinkExtension))]
public class MongoDataSinkExtension : IDataSinkExtension
{
    public string DisplayName => "Mongo";

    public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, IDataSourceExtension dataSource, ILogger logger, CancellationToken cancellationToken = default)
    {
        var settings = config.Get<MongoSinkSettings>();
        settings.Validate();

        if (!string.IsNullOrEmpty(settings.ConnectionString) && !string.IsNullOrEmpty(settings.DatabaseName) && !string.IsNullOrEmpty(settings.Collection))
        {
            var context = new Context(settings.ConnectionString, settings.DatabaseName);
            var repo = context.GetRepository<BsonDocument>(settings.Collection);

            var batchSize = settings.BatchSize ?? 1000;

            var objects = new List<BsonDocument>();
            await foreach (var item in dataItems)
            {
                var dict = item.GetFieldNames().ToDictionary(key => key, key => item.GetValue(key));
                objects.Add(new BsonDocument(dict));

                if (objects.Count == batchSize)
                {
                    await repo.AddRange(objects);
                    objects.Clear();
                }
            }

            if (objects.Any())
            {
                await repo.AddRange(objects);
            }
        }
    }
}
