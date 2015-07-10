using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.JsonFile.Sink
{
    sealed class JsonFileSinkAdapter : IDataSinkAdapter
    {
        private JsonSerializer serializer;

        private StreamWriter streamWriter;
        private JsonTextWriter jsonWriter;

        public int MaxDegreeOfParallelism { get { return 1; } }

        public JsonFileSinkAdapter(StreamWriter streamWriter, JsonSerializer serializer)
        {
            Guard.NotNull("streamWriter", streamWriter);
            Guard.NotNull("serializer", serializer);

            this.streamWriter = streamWriter;
            this.serializer = serializer;

            jsonWriter = new JsonTextWriter(streamWriter);
            jsonWriter.WriteStartArray();
        }

        public Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            serializer.Serialize(jsonWriter, dataItem);
            return Task.FromResult<object>(null);
        }

        public Task CompleteAsync(CancellationToken cancellation)
        {
            // Do not close an array here
            // This call just indicates that there will be no more items, but write tasks might not be completed yet
            return TaskHelper.NoOp;
        }

        public void Dispose()
        {
            /*
             * Don't do this in try-catch as we want to get a BlobAlreadyExists exception from
             * BLOB storage, that only happens on commit (dispose of the stream).
            */ 
            jsonWriter.WriteEndArray();
            ((IDisposable)jsonWriter).Dispose();
            streamWriter.Dispose();
        }
    }
}
