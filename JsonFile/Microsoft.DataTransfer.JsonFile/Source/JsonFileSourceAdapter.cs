using Microsoft.DataTransfer.Basics;
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
        private JsonSerializer serializer;
        private string fileName;
        private StreamReader file;
        private JsonTextReader jsonReader;

        public JsonFileSourceAdapter(string fileName, JsonSerializer serializer)
        {
            Guard.NotEmpty("fileName", fileName);
            Guard.NotNull("serializer", serializer);

            this.fileName = fileName;
            this.serializer = serializer;
        }

        public Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            return Task.Factory.StartNew<IDataItem>(ReadNext, readOutput);
        }

        private IDataItem ReadNext(object taskState)
        {
            var readOutput = (ReadOutputByRef)taskState;

            try
            {
                if (file == null)
                {
                    file = File.OpenText(fileName);
                    jsonReader = new JsonTextReader(file) { SupportMultipleContent = true };
                }

                while (jsonReader.Read() && jsonReader.TokenType != JsonToken.StartObject) ;

                if (jsonReader.TokenType != JsonToken.StartObject)
                    return null;

                return serializer.Deserialize<IDataItem>(jsonReader);
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
                    Resources.DataItemIdFormat, fileName, lineNumber, linePosition);
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
