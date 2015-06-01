using Microsoft.DataTransfer.CsvFile.Source;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.CsvFile.FunctionalTests
{
    static class CsvFileHelper
    {
        public static async Task<List<IDataItem>> ReadCsv(ICsvFileSourceAdapterConfiguration configuration)
        {
            var records = new List<IDataItem>();

            using (var source = await new CsvFileSourceAdapterFactory().CreateAsync(configuration, DataTransferContextMock.Instance))
            {
                IDataItem record = null;
                var readOutput = new ReadOutputByRef();
                while (true)
                {
                    try
                    {
                        record = await source.ReadNextAsync(readOutput, CancellationToken.None);
                    }
                    catch (NonFatalReadException)
                    {
                        continue;
                    }

                    if (record == null)
                        break;

                    records.Add(record);

                    Assert.IsNotNull(readOutput.DataItemId, CommonTestResources.MissingDataItemId);
                    readOutput.Wipe();
                }
            }

            return records;
        }
    }
}
