using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics.Collections;
using Microsoft.DataTransfer.HBase.Client;
using Microsoft.DataTransfer.HBase.Client.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Source
{
    sealed class HBaseSourceAdapter : IDataSourceAdapter
    {
        private const string RowIdFieldName = "RowId";

        private IStargateClient client;
        private IHBaseSourceAdapterInstanceConfiguration configuration;

        private IAsyncEnumerator<HBaseRow> rowsCursor;

        public HBaseSourceAdapter(IStargateClient client, IHBaseSourceAdapterInstanceConfiguration configuration)
        {
            Guard.NotNull("client", client);
            Guard.NotNull("configuration", configuration);

            this.client = client;
            this.configuration = configuration;
        }

        public async Task Initialize(CancellationToken cancellation)
        {
            rowsCursor = await client.ScanAsync(configuration.TableName, configuration.Filter, configuration.BatchSize, cancellation);
        }

        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (rowsCursor == null)
            {
                throw Errors.SourceIsNotInitialized();
            }

            if (!await rowsCursor.MoveNextAsync(cancellation))
                return null;

            var currentRow = rowsCursor.Current;

            readOutput.DataItemId = currentRow.Key;

            var cells = new Dictionary<string, HBaseCell>(currentRow.Cells.Count + 1);

            if (!configuration.ExcludeId)
            {
                cells[RowIdFieldName] = new HBaseCell { ColumnName = RowIdFieldName, Value = currentRow.Key };
            }

            foreach (var cell in rowsCursor.Current.Cells)
            {
                HBaseCell existingCell;
                if (!cells.TryGetValue(cell.ColumnName, out existingCell) ||
                    cell.Timestamp > existingCell.Timestamp)
                {
                    cells[cell.ColumnName] = cell;
                }
            }

            return new HBaseCellsDataItem(cells);
        }

        public void Dispose()
        {
            TrashCan.Throw(ref rowsCursor);
        }
    }
}
