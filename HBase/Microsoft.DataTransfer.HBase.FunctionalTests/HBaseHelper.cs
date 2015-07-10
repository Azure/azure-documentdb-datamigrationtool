using Microsoft.DataTransfer.HBase.Client;
using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.DataTransfer.HBase.FunctionalTests
{
    sealed class HBaseHelper
    {
        public const string RowIdPropertyName = "RowId";

        public static void CreateSampleTable(string connectionString, string tableName, IEnumerable<IReadOnlyDictionary<string, object>> documents)
        {
            var client = CreateClient(connectionString);

            client.CreateTable(new TableSchema()
            {
                name = tableName,
                columns = { new ColumnSchema() { name = "data" } }
            });

            foreach (var document in documents)
            {
                var row = new CellSet.Row
                {
                    key = Encoding.UTF8.GetBytes(document[RowIdPropertyName].ToString())
                };

                foreach (var property in document)
                {
                    if (property.Key == RowIdPropertyName)
                        continue;

                    row.values.Add(
                        new Cell {
                            column = Encoding.UTF8.GetBytes("data:" + property.Key),
                            data = property.Value == null ? null
                                : Encoding.UTF8.GetBytes(property.Value.ToString())
                        });
                }

                client.StoreCells(tableName, new CellSet { rows = { row } });
            }
        }

        public static void DeleteTable(string connectionString, string tableName)
        {
            CreateClient(connectionString).DeleteTable(tableName);
        }

        private static IHBaseClient CreateClient(string connectionString)
        {
            var connectionSettings = StargateConnectionStringBuilder.Parse(connectionString);

            return new HBaseClient(new ClusterCredentials(new Uri(connectionSettings.ServiceURL),
                connectionSettings.Username, connectionSettings.Password));
        }
    }
}
