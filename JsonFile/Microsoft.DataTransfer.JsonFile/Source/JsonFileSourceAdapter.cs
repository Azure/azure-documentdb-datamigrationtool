using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.JsonFile.Source
{
    sealed class JsonFileSourceAdapter : IDataSourceAdapter
    {
        private ISourceStreamProvider sourceStreamProvider;
        private JsonSerializer serializer;

        private StreamReader file;
        private JsonTextReader jsonReader;

        public JsonFileSourceAdapter(ISourceStreamProvider sourceStreamProvider, JsonSerializer serializer)
        {
            Guard.NotNull("sourceStreamProvider", sourceStreamProvider);
            Guard.NotNull("serializer", serializer);

            this.sourceStreamProvider = sourceStreamProvider;
            this.serializer = serializer;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            try
            {
                if (file == null)
                {
                    file = new StreamReader(await sourceStreamProvider.CreateStream(cancellation));
                    jsonReader = new JsonTextReader(file) { SupportMultipleContent = true };
                }

                return await Task.Factory.StartNew(() =>
                {
                    while (jsonReader.Read() && jsonReader.TokenType != JsonToken.StartObject) ;

                    if (jsonReader.TokenType != JsonToken.StartObject)
                        return null;

                    return serializer.Deserialize<IDataItem>(jsonReader);
                });
            }
            finally
            {
                int lineNumber = 0, linePosition = 0;
                if (jsonReader != null)
                {
                    lineNumber = jsonReader.LineNumber;
                    linePosition = jsonReader.LinePosition;
                }

                readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                    Resources.DataItemIdFormat, sourceStreamProvider.Id, lineNumber, linePosition);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "jsonReader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref jsonReader, r => r.Close());
            TrashCan.Throw(ref file, f => f.Close());
        }
    }
}
