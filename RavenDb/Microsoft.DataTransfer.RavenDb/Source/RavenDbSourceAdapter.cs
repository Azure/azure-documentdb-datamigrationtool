using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.RavenDb.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Raven.Client;
using Raven.Json.Linq;

namespace Microsoft.DataTransfer.RavenDb.Source
{
    sealed class RavenDbSourceAdapter : RavenDbDataAdapterBase<IRavenDbSourceAdapterInstanceConfiguration>
    {
        private IEnumerator<RavenJObject> data;
        private IDocumentStore documentStore;

        private int rowNumber;

        public RavenDbSourceAdapter(IRavenDbSourceAdapterInstanceConfiguration configuration)
            : base(configuration)
        {
        }

        public override Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            return Task.Factory.StartNew<IDataItem>(ReadNext, readOutput);
        }

        private IDataItem ReadNext(object taskState)
        {
            var readOutput = (ReadOutputByRef)taskState;

            try
            {
                if (data == null)
                {
                    data = Connection.DatabaseCommands.StreamDocs(startsWith: Configuration.Collection + "/");
                }

                if (!data.MoveNext())
                    return null;

                var document = data.Current;

                var converter = new ExpandoObjectConverter();
                var message = JsonConvert.DeserializeObject<ExpandoObject>(document.ToString(), converter);

                var record = message.ToDictionary(x => x.Key, x => x.Value);
                IDictionary<string, object> metadata = ((ExpandoObject) record["@metadata"]);
                record["Id"] = metadata["@id"];

                return new DictionaryDataItem(record);
            }
            finally
            {
                readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                    Resources.DataItemIdFormat, Configuration.Collection, rowNumber);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "csvReader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref documentStore, r => r.Dispose());
        }
    }
}
