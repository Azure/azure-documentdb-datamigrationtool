using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
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

        public Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}
