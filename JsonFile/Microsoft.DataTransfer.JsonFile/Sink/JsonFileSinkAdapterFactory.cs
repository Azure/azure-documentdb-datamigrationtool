using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.JsonFile.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.JsonFile.Sink
{
    /// <summary>
    /// Provides data sink adapters capable of writing data to JSON file.
    /// </summary>
    public sealed class JsonFileSinkAdapterFactory : IDataSinkAdapterFactory<IJsonFileSinkAdapterConfiguration>
    {
        /// <summary>
        /// Gets the description of the data adapter.
        /// </summary>
        public string Description
        {
            get { return Resources.SinkDescription; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDataSinkAdapter" /> with the provided configuration.
        /// </summary>
        /// <param name="configuration">Data sink adapter configuration.</param>
        /// <param name="context">Data transfer operation context.</param>
        /// <returns>Task that represents asynchronous create operation.</returns>
        public Task<IDataSinkAdapter> CreateAsync(IJsonFileSinkAdapterConfiguration configuration, IDataTransferContext context)
        {
            return Task.Factory.StartNew(() => Create(configuration));
        }

        private static IDataSinkAdapter Create(IJsonFileSinkAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            // Ensure output folder exists
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configuration.File));
            }
            catch { }

            return new JsonFileSinkAdapter(
                File.Open(configuration.File, configuration.Overwrite ? FileMode.Create : FileMode.CreateNew),
                JsonSerializersFactory.Create(configuration.Prettify));
        }
    }
}
