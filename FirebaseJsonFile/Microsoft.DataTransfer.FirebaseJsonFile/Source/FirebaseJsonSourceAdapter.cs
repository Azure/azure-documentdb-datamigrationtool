using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.JsonNet.Serialization;
using Newtonsoft.Json;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Source
{
    sealed class FirebaseJsonFileSourceAdapter: IDataSourceAdapter
    {
        private readonly ISourceStreamProvider sourceStreamProvider;
        private readonly string node;
        private readonly string idField;
        private readonly string collectionField;
        private readonly JsonSerializer serializer;

        private StreamReader streamReader;
        private JsonTextReader jsonReader;

        public FirebaseJsonFileSourceAdapter(ISourceStreamProvider sourceStreamProvider, string node, string idField, string collectionField, JsonSerializer serializer)
        {
            Guard.NotNull("sourceStreamProvider", sourceStreamProvider);
            Guard.NotNull("serializer", serializer);

            this.sourceStreamProvider = sourceStreamProvider;
            this.node = node;
            this.idField = idField;
            this.collectionField = collectionField;
            this.serializer = serializer;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            try
            {
                // Create the JSON text reader if necessary
                if (streamReader == null)
                {
                    streamReader = new StreamReader(await sourceStreamProvider.CreateStream(cancellation));
                    jsonReader = new JsonTextReader(streamReader) { SupportMultipleContent = true };
                }

                // Read until a DataItem
                while (await jsonReader.ReadAsync() && !ReaderAtNextItem(jsonReader)) ;

                if (jsonReader.TokenType != JsonToken.PropertyName)
                    return null;

                // Firebase item's IDs are stored as property names and the 
                // items themselves are stored as property values
                var id = jsonReader.Value.ToString();
                await jsonReader.ReadAsync();
                var dataItem = serializer.Deserialize<IDataItem>(jsonReader) as JObjectDataItem;

                // If the user requested ID or Collection name to be stored as 
                // fields (for use by the DT target), add/update those values
                if (!string.IsNullOrEmpty(idField))
                {
                    dataItem.SetValue(idField, id);
                }
                if (!string.IsNullOrEmpty(collectionField))
                {
                    var collectionName = GetCollectionName(jsonReader);
                    if (!string.IsNullOrEmpty(collectionName))
                    {
                        dataItem.SetValue(collectionField, collectionName);
                    }
                }

                return dataItem;
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

        // By convention, top-level firebase nodes are logical buckets  
        // similar to CosmosDB collections
        private string GetCollectionName(JsonTextReader jsonReader) => jsonReader.Path
            .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();

        private bool ReaderAtNextItem(JsonTextReader jsonReader)
        {
            // Firebase items begin with property names
            // They are stored as properties with the ID as the 
            // property name and the item as the property value
            if (jsonReader.TokenType != JsonToken.PropertyName)
            {
                return false;
            }

            // If inferring collection name from top-level node, then we need 
            // to read items at depth 2
            // (top-level nodes are at depth 1)
            if (!string.IsNullOrEmpty(collectionField) && jsonReader.Depth < 2)
            {
                return false;
            }

            // If reading items from a particular top-level node, make sure 
            // that the path begins with that node and that the reader is 
            // deep enough to read the top-level node's children
            if (!string.IsNullOrEmpty(node) && (jsonReader.Depth < 2 || !jsonReader.Path.StartsWith(node, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "jsonReader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref jsonReader, r => r.Close());
            TrashCan.Throw(ref streamReader, f => f.Close());
        }
    }
}
