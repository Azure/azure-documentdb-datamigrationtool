using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Microsoft.DataTransfer.Sql.FunctionalTests
{
    [DeploymentItem("SqlServerTypes", "SqlServerTypes")]
    public class SqlTestsBase : DataTransferTestBase
    {
        protected string ConnectionString { get { return Settings.SqlConnectionString; } }

        protected static string CreateTableName()
        {
            return String.Format("TestTable_{0:N}", Guid.NewGuid());
        }

        protected static void CreateTable(SqlConnection connection, string tableName, IDictionary<string, string> columnMappings)
        {
            var commandText = String.Format(TestResources.CreateTableCommandFormat,
                    tableName, String.Join(",", columnMappings.Select(c => String.Format("{0} {1}", AsIdentifier(c.Key), c.Value))));

            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        protected static void DropTable(SqlConnection connection, string tableName)
        {
            var commandText = String.Format(TestResources.DropTableCommandFormat, tableName);

            using (var command = new SqlCommand(commandText, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        protected static void AddRows(SqlConnection connection, string tableName, IDictionary<string, object>[] rows, bool useRawValues)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;

                foreach (var row in rows)
                {
                    command.CommandText = String.Format(TestResources.InsertRowsCommandFormat,
                        tableName,
                        String.Join(",", row.Keys.Select(k => AsIdentifier(k))),
                        String.Join(",", useRawValues ? (IEnumerable<object>)row.Values : row.Values.Select(v => AsValue(v))));

                    command.ExecuteNonQuery();
                }
            }
        }

        protected static DateTime GetSampleDateTime()
        {
            // Round up to seconds to conform to SQL precision
            return new DateTime(DateTime.Now.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
        }

        protected static void VerifyDataItem(IReadOnlyDictionary<string, object> expected, IDataItem actual)
        {
            var actualColumns = actual.GetFieldNames().ToList();

            foreach (var key in expected.Keys)
            {
                Assert.IsTrue(actualColumns.Contains(key), TestResources.PropertyMissingFormat, key);
                Assert.AreEqual(expected[key], actual.GetValue(key), TestResources.InvalidPropertyValueFormat, key);
            }
        }

        protected static string CreateQueryFile(string tableName)
        {
            var queryFileName = Path.GetTempFileName();

            File.WriteAllText(queryFileName,
                String.Format(TestResources.SimpleSelectQueryFormat, tableName));

            return queryFileName;
        }

        private static string AsIdentifier(string name)
        {
            return "[" + name + "]";
        }

        private static string AsValue(object value)
        {
            return "'" + value.ToString() + "'";
        }
    }
}
