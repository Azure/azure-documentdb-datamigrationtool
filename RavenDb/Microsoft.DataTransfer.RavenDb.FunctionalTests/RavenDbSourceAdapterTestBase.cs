using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.RavenDb.FunctionalTests
{
    [TestClass]
    public abstract class RavenDbSourceAdapterTestBase : DataTransferAdapterTestBase
    {
        protected async Task<List<IDataItem>> ReadData(IRavenDbSourceAdapterConfiguration configuration)
        {
            using (var adapter = await new RavenDbSourceAdapterFactory()
                .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
            {
                return await ReadDataAsync(adapter);
            }
        }
    }
}
