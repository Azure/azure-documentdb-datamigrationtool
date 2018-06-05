using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Source
{
    public sealed class FirebaseJsonFileSourceAdapterFactory : DataAdapterFactoryBase, IDataSourceAdapterFactory<IFirebaseJsonFileSourceAdapterConfiguration>
    {
        private readonly JsonSerializer serializer = CreateJsonSerializer();

        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description => Resources.SourceDescription;

        /// <summary>
        /// Creates a new instance of <see cref="IDataSourceAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data source adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSourceAdapter> CreateAsync(IFirebaseJsonFileSourceAdapterConfiguration configuration, IDataTransferContext context, CancellationToken cancellation)
        {
            return Task.Factory.StartNew(() => Create(configuration), cancellation);
        }

        private IDataSourceAdapter Create(IFirebaseJsonFileSourceAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            return new AggregateDataSourceAdapter(
                configuration.Files
                    .SelectMany(f => SourceStreamProvidersFactory
                        .Create(f, configuration.Decompress)
                        .Select(p => new FirebaseJsonFileSourceAdapter(p, 
                            configuration.Node, 
                            configuration.IdField, 
                            configuration.CollectionField, 
                            serializer))));
        }

        private static JsonSerializer CreateJsonSerializer() => 
            JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                Converters =
                {
                    DataItemJsonConverter.Instance,
                    GeoJsonConverter.Instance
                }
            });
    }
}
