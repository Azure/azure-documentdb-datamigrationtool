using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Threading;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.JsonFile.Sink
{
    sealed class JsonFileSinkAdapter : IDataSinkAdapter
    {
        private JsonSerializer serializer;

        private FileStream fileStream;
        private StreamWriter streamWriter;
        private JsonTextWriter jsonWriter;

        public int MaxDegreeOfParallelism { get { return 1; } }

        public JsonFileSinkAdapter(FileStream fileStream, JsonSerializer serializer)
        {
            Guard.NotNull("fileStream", fileStream);
            Guard.NotNull("serializer", serializer);

            this.fileStream = fileStream;
            this.serializer = serializer;

            streamWriter = new StreamWriter(fileStream);
            jsonWriter = new JsonTextWriter(streamWriter);
            jsonWriter.WriteStartArray();
        }

        public async Task WriteAsync(IDataItem dataItem, CancellationToken cancellation)
        {
            serializer.Serialize(jsonWriter, dataItem);
            await streamWriter.FlushAsync();
        }

        public Task CompleteAsync(CancellationToken cancellation)
        {
            // Do not close an array here
            // This call just indicates that there will be no more items, but write tasks might not be completed yet
            return TaskHelper.NoOp;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "jsonWriter",
            Justification = "Disposed through TrashCan helper")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "streamWriter",
            Justification = "Disposed through TrashCan helper")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "fileStream",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref jsonWriter, w => 
                {
                    w.WriteEndArray();
                    w.Close();
                });
            TrashCan.Throw(ref streamWriter, w => w.Close());
            TrashCan.Throw(ref fileStream, s => s.Close());
        }
    }
}
