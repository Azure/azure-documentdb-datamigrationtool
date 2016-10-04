using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Files.Source;
using Microsoft.DataTransfer.CsvFile.Reader;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Source;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.Source
{
    sealed class CsvFileSourceAdapter : IDataSourceAdapter
    {
        private readonly ISourceStreamProvider sourceStreamProvider;
        private readonly ICsvFileSourceAdapterInstanceConfiguration configuration;

        private CsvReader reader;
        private IReadOnlyList<string> header;

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
                if (reader == null)
                {
                    reader = new CsvReader(
                        new StreamReader(await sourceStreamProvider.CreateStream(cancellation)),
                        new CsvReaderConfiguration
                        {
                            TrimQuoted = configuration.TrimQuoted,
                            IgnoreUnquotedNulls = configuration.NoUnquotedNulls,
                            ParserCulture = configuration.UseRegionalSettings ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture
                        });
                    header = ReadHeaderRow();
                }

                if (header == null)
                    return null;

                return await Task.Factory.StartNew<IDataItem>(ReadNext);
            }
            finally
            {
                readOutput.DataItemId = String.Format(CultureInfo.InvariantCulture,
                    Resources.DataItemIdFormat, sourceStreamProvider.Id, reader == null ? 0 : reader.Row);
            }
        }

        private IDataItem ReadNext()
        {
            var values = reader.Read();

            if (values == null)
                return null;

            if (values.Count != header.Count)
                throw Errors.InvalidNumberOfColumns(values.Count, header.Count);

            var dataItem = NestedDataItem.Create(configuration.NestingSeparator);

            for (var index = 0; index < header.Count; ++index)
                dataItem.AddProperty(header[index], values[index]);

            return dataItem;
        }

        private IReadOnlyList<string> ReadHeaderRow()
        {
            var headerRow = reader.Read();
            if (headerRow == null || !headerRow.Any())
                return null;

            return headerRow.Select(i => i == null ? String.Empty : i.ToString()).ToList();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "reader",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            TrashCan.Throw(ref reader);
        }
    }
}
