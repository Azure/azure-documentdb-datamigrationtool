# SQL Server Extension

The SQL Server data transfer extension provides source and sink capabilities for reading from and writing to table data in SQL Server. Hierarchical data is not fully supported but the Sink extension will write a nested object from a hierarchical source (i.e. JSON, Cosmos) into a SQL column as a JSON string.

## Settings

Source and sink settings both require a `ConnectionString` parameter.

Source settings also require a `QueryText` parameter to define the data to select from SQL. This can combine data from multiple tables, views, etc. but should produce a single result set.

### Source

```json
{
    "ConnectionString": "",
    "QueryText": ""
}
```

Sink settings require a `TableName` to define where to insert data and an array of `ColumnMappings`. Only fields listed in `ColumnMappings` will be imported. Each element in `ColumnMappings` requires a `ColumnName` specifying the target SQL column along with situation specific fields:
- `SourceFieldName`: This should be set in cases where the source data uses a different name than the SQL column. Column name to source field mapping defaults to using the `ColumnName` for both sides and is case-insensitive so it is not necessary to specify this parameter for mappings like `"id"` -> `"Id"`.
- `AllowNull`: Depending on the table schema you may need to force values to be set for columns when no value is present in the source. If this is set to `false` this column will use the `DefaultValue` for any records missing a source value. Defaults to `true`.
- `DefaultValue`: Value to be used in place of missing or null source fields. This parameter is ignored unless `AllowNull` is set to `false` for this column.

Sink settings also include an optional `BatchSize` parameter to specify the count of records to accumulate before bulk inserting, default value is 1000.

### Sink

```json
{
    "ConnectionString": "",
    "TableName": "MyStagingTable",
    "ColumnMappings": [
        {
            "ColumnName": "Id"
        },
        {
            "ColumnName": "Name"
        },
        {
            "ColumnName": "Description"
        },
        {
            "ColumnName": "Count",
            "SourceFieldName": "number"
        },
        {
            "ColumnName": "IsSet",
            "AllowNull": false,
            "DefaultValue": false
        }
    ],
    "BatchSize": 1000
}
```