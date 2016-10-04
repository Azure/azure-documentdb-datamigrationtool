using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.Extensibility;
using MongoDB.Bson;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.MongoDb.Source.Mongoexport
{
    sealed class MongoexportFileSourceAdapter : IDataSourceAdapter
    {
        private ISourceStreamProvider sourceStreamProvider;
        private StreamReader file;
        private int lineNumber;

        public MongoexportFileSourceAdapter(ISourceStreamProvider sourceStreamProvider)
        {
            this.sourceStreamProvider = sourceStreamProvider;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (file == null)
            {
                file = new StreamReader(await sourceStreamProvider.CreateStream(cancellation));
            }

            readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                Resources.DataItemIdFormat, sourceStreamProvider.Id, ++lineNumber);

            string jsonData = null;
            while (!file.EndOfStream &&
                String.IsNullOrEmpty(jsonData = await file.ReadLineAsync())) ;

            if (file.EndOfStream && String.IsNullOrEmpty(jsonData))
                return null;

            try
            {
                return new BsonDocumentDataItem(BsonDocument.Parse(jsonData));
            }
            catch (Exception parseError)
            {
                throw new NonFatalReadException(parseError.Message, parseError.InnerException);
            }
        }

        public void Dispose()
        {
            TrashCan.Throw(ref file);
        }
    }
}
