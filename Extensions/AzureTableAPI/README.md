# AzureTableAPI Extension

This extension is built to facilitate both a data Source and Sink for the migration tool to be able to read and write to Azure Table API. This covers both the Azure Storage Table API and Azure Cosmos DB Table API, since they both adhere to the same API spec.

## Configuration Settings

The AzureTableAPI has a couple required and optional settings for configuring the connection to the Table API you are connecting to. This applies to both Azure Storage Table API and Azure Cosmos DB Table API.

The following are the required settings that must be defined for using either the data Source or Sink:

- `ConnectionString` - This defines the Table API Connection String used for connecting to and authenticating with the Table API service. For example, this will contain the name of the Azure Storage Account and the Account Key for connecting to the Table API on the service. This is required.
- `Table` - This defines the name of the Table to connect to on the Table API service. Such as the name of the Azure Storage Table. This is required.

There are also a couple optional settings that can be configured on the AzureTableAPI to help with mapping data between the Source and Sink:

- `RowKeyFieldName` - This is used to specify a different field name when mapping data to / from the `RowKey` field of Azure Table API. Optional.
- `PartitionKeyFieldName` - This is used to specify a different field name when mapping data to / from the `PartitionKey` field of Azure Table API. Optional.

In the Azure Table API, the `RowKey` and `PartitionKey` are required fields on the entities storage in a Table. When performing mapping of data between Azure Table API and Cosmos DB (or some other data store), you may be required to use a different field name in the other data store than these names as required by the Azure Table API. The `RowKeyFieldName` and `PartitionKeyFieldName` enables these fields to be mapped to / from a custom field name that matches your data requirements. If these settings are not specified, then these fields will not be renamed in the data mapping and will remain as they are in the Azure Table API.

### Additional Source Settings

The AzureTableAPI Source extension has an additional setting that can be configured for helping with querying data.

The following setting is supported for the Source:

- `QueryFilter` - This enables you to specify an OData filter to be applied to the data being retrieved by the AzureTableAPI Source. This is used in cases where only a subset of data from the source Table is needed in the migration. Example usage to query a subset of entities from the source table: `PartitionKey eq 'foo'`.

## Example Source and Sink Settings Usage

The following are a couple example `settings.json` files for configuring the AzureTableAPI Source and Sink extensions.

**AzureTableAPISourceSettings.json**

```json
{
  "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<storage-account-name>;AccountKey=<key>;EndpointSuffix=core.windows.net",
  "Table": "SourceTable1",
  "PartitionKeyFieldName": "State",
  "RowKeyFieldName": "id",
  "QueryFilter": "PartitionKey eq 'WI'"
}
```

**AzureTableAPISinkSettings.json**

```json
{
  "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<storage-account-name>;AccountKey=<key>;EndpointSuffix=core.windows.net",
  "Table": "DestinationTable1",
  "PartitionKeyFieldName": "State",
  "RowKeyFieldName":  "id"
}
```
