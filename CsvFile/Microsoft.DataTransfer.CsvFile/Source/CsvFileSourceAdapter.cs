using CsvHelper;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
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
        private readonly ICsvFileSourceAdapterInstanceConfiguration configuration;

        private StreamReader file;
        private CsvReader csvReader;

        private int rowNumber;

        public CsvFileSourceAdapter(ICsvFileSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);
            this.configuration = configuration;
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
                    file = File.OpenText(configuration.FileName);
                    csvReader = new CsvReader(file);
                }

                if (!csvReader.Read())
                    return null;

                // Unfortunatelly there is no support for mapping to a Dictionary yet.
                // Use dynamic workaround https://github.com/JoshClose/CsvHelper/issues/187
                var record = csvReader.GetRecord<dynamic>() as IDictionary<string, object>;

                return NestedDataItem.Create(record, configuration.NestingSeparator);
            }
            finally
            {
                if (csvReader != null)
                    // If it fails on the first read - it will throw an exception from Row property
                    try { rowNumber = csvReader.Row; } catch { }

                readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                    Resources.DataItemIdFormat, configuration.FileName, rowNumber);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "csvReader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref csvReader, r => r.Dispose());
            TrashCan.Throw(ref file, f => f.Close());
        }
    }
}
