using Microsoft.DataTransfer.DocumentDb.Source;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.DocumentDb.FunctionalTests
{
    [TestClass]
    public class DocumentDbSourceAdapterFactoryTests : DocumentDbAdapterTestBase
    {
        private const string CollectionName = "TestCollection";

        [TestMethod, Timeout(300000)]
        public async Task CreateAsync_QueryAndQueryFileBothSet_ArgumentExceptionThrown()
        {
            const string Query = "SELECT * FROM Collection";

            string queryFileName = null;

            try
            {
                queryFileName = CreateQueryFile(Query);

                var configuration =
                Mocks
                    .Of<IDocumentDbSourceAdapterConfiguration>(c =>
                        c.ConnectionString == ConnectionString &&
                        c.Collection == CollectionName &&
                        c.Query == Query &&
                        c.QueryFile == queryFileName)
                    .First();

                try
                {
                    using (var adapter = await new DocumentDbSourceAdapterFactory()
                        .CreateAsync(configuration, DataTransferContextMock.Instance, CancellationToken.None))
                    {
                        Assert.Fail(TestResources.AmbiguousQueryDidNotFail);
                    }
                }
                catch (ArgumentException)
                {
                    return;
                }
            }
            finally
            {
                if (!String.IsNullOrEmpty(queryFileName) && File.Exists(queryFileName))
                {
                    File.Delete(queryFileName);
                }
            }
        }

        private string CreateQueryFile(string query)
        {
            var queryFileName = Path.GetTempFileName();
            File.WriteAllText(queryFileName, query);
            return queryFileName;
        }
    }
}
