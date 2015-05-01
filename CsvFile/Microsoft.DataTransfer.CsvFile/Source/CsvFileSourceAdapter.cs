using CsvHelper;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using Microsoft.DataTransfer.Extensibility.Basics.Source.StreamProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.Source
{
    sealed class CsvFileSourceAdapter : IDataSourceAdapter
    {
        private readonly ISourceStreamProvider sourceStreamProvider;
        private readonly ICsvFileSourceAdapterInstanceConfiguration configuration;

        private StreamReader file;
        private CsvReader csvReader;

        private int rowNumber;

        public CsvFileSourceAdapter(ISourceStreamProvider sourceStreamProvider, ICsvFileSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("sourceStreamProvider", sourceStreamProvider);
            Guard.NotNull("configuration", configuration);

            this.sourceStreamProvider = sourceStreamProvider;
            this.configuration = configuration;
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            try
            {
                if (file == null)
                {
                    file = await sourceStreamProvider.CreateReader();
                    csvReader = new CsvReader(file);
                }

                return await Task.Factory.StartNew(() =>
                {
                    if (!csvReader.Read())
                        return null;

                    // Unfortunatelly there is no support for mapping to a Dictionary yet.
                    // Use dynamic workaround https://github.com/JoshClose/CsvHelper/issues/187
                    var record = csvReader.GetRecord<dynamic>() as IDictionary<string, object>;

                    return NestedDataItem.Create(record, configuration.NestingSeparator);
                });
            }
            finally
            {
                if (csvReader != null)
                    // If it fails on the first read - it will throw an exception from Row property
                    try { rowNumber = csvReader.Row; }
                    catch { }

                readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                    Resources.DataItemIdFormat, sourceStreamProvider.Id, rowNumber);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "csvReader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref csvReader);
            TrashCan.Throw(ref file, f => f.Close());
        }
    }
}
