using Microsoft.DataTransfer.Sql.Source;
using Microsoft.DataTransfer.TestsCommon.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.FunctionalTests
{
    [TestClass]
    public class SqlDataSourceAdapterFactoryTests : SqlTestsBase
    {
        [TestMethod]
        public async Task CreateAsync_QueryAndQueryFileBothSet_ArgumentExceptionThrown()
        {
            var tableName = CreateTableName();
            string queryFileName = null;

            try
            {
                queryFileName = CreateQueryFile(tableName);

                var configuration =
                    Mocks
                        .Of<ISqlDataSourceAdapterConfiguration>()
                        .Where(c =>
                            c.ConnectionString == ConnectionString &&
                            c.Query == String.Format(TestResources.SimpleSelectQueryFormat, tableName) &&
                            c.QueryFile == queryFileName)
                        .First();

                try
                {
                    using (var adapter = await new SqlDataSourceAdapterFactory()
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
    }
}
