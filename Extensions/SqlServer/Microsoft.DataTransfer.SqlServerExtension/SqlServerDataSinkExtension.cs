using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.SqlServerExtension
{
    [Export(typeof(IDataSinkExtension))]
    public class SqlServerDataSinkExtension : IDataSinkExtension
    {
        public string DisplayName => "SqlServer";

        public async Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            var settings = config.Get<SqlServerSinkSettings>();
            settings.Validate();

            string tableName = settings.TableName!;

            await using var connection = new SqlConnection(settings.ConnectionString);
            await connection.OpenAsync(cancellationToken);
            await using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.KeepIdentity, transaction);
                    bulkCopy.DestinationTableName = tableName;

                    var dataColumns = new Dictionary<ColumnMapping, DataColumn>();
                    foreach (ColumnMapping columnMapping in settings.ColumnMappings)
                    {
                        DataColumn dbColumn = new DataColumn(columnMapping.ColumnName);
                        dataColumns.Add(columnMapping, dbColumn);
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dbColumn.ColumnName, dbColumn.ColumnName));
                    }

                    var batches = dataItems.Buffer(settings.BatchSize);
                    await foreach (var batch in batches.WithCancellation(cancellationToken))
                    {
                        var dataTable = new DataTable();
                        dataTable.Columns.AddRange(dataColumns.Values.ToArray());
                        foreach (var item in batch)
                        {
                            var fieldNames = item.GetFieldNames().ToList();
                            DataRow row = dataTable.NewRow();
                            foreach (var columnMapping in dataColumns)
                            {
                                DataColumn column = columnMapping.Value;
                                ColumnMapping mapping = columnMapping.Key;

                                string? fieldName = mapping.GetFieldName();
                                if (fieldName != null)
                                {
                                    object? value = null;
                                    var sourceField = fieldNames.FirstOrDefault(n => n.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
                                    if (sourceField != null)
                                    {
                                        value = item.GetValue(sourceField);
                                    }
                                    
                                    if (value != null || mapping.AllowNull)
                                    {
                                        row[column.ColumnName] = value;
                                    }
                                    else
                                    {
                                        row[column.ColumnName] = mapping.DefaultValue;
                                    }
                                }
                            }
                            dataTable.Rows.Add(row);
                        }
                        await bulkCopy.WriteToServerAsync(dataTable, cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying data to table {tableName}: {ex.Message}");
                    await transaction.RollbackAsync(cancellationToken);
                }
            }

            await connection.CloseAsync();
        }
    }
}