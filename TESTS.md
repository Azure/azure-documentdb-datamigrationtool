# Testing

This project includes a suite of unit and functional tests for each source and sink. These tests can be ran in the latest version of [Microsoft Visual Studio][visual-studio].

## Unit Tests

The project ships with a suite of unit tests that can be ran immediately on a clean fork of this repository. Running the unit tests do not require any additional configuration.

## Functional Tests

When you run all tests, in Visual Studio you may notice that most of the functional tests are marked as **inconclusive** with a status message. The functional tests require you to configure a **.runsettings** file in each project to run the tests in an end-to-end environment.

Each functional test project has a **.runsettings** in the root. The settings file includes an empty value for the corresponding data platform's connection string. If this value is empty, the test runner will automatically mark any functional test that requires a live data platform as **inconclusive**. If you provide a connection string, the test runner will use that connection string to connect to the live data platform.

> 💡 The **.runsettings** file includes tips and examples on how to configure each data platform.

### Azure DocumentDb (Azure Cosmos DB SQL API)

Use the [Azure Cosmos DB emulator][azure-cosmos-db-emulator] to run the functional tests for the DocumentDb source and sinks. The emulator can be installed on your local machine and ran like any other desktop application.

After the emulator is running, update the **Microsoft.DataTransfer.DocumentDb.FunctionalTests/.runsettings** file with the following connection string:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <TestRunParameters>
        <Parameter name="DocumentDbConnectionString" value="AccountEndpoint=https://localhost:8081;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;" />
    </TestRunParameters>
</RunSettings>
```

> 💡 Azure Cosmos DB has a default static value for the account endpoint and key when using the emulator. [Read more about the default credentials][azure-cosmos-db-emulator-credentials].

> 💡 Make sure you include a semi-colon at the end of your connection string.

### MongoDb

Run functional tests for Mongo with the **[mongo][mongo-docker]** container image from Docker Hub. To start a new mongo instance in a Docker installation:

```bash
docker pull mongo

docker run --detach --publish 27017:27017 mongo
```

Then, update the **Microsoft.DataTransfer.MongoDb.FunctionalTests/.runsettings** file with the following connection string:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <TestRunParameters>
        <Parameter name="MongoConnectionString" value="mongodb://localhost:27017" />
    </TestRunParameters>
</RunSettings>
```

> 💡 Make sure you do not include a trailing slash with your connection string.

### Azure Storage

Run functional tests for Azure Storage with the [Azurite][azurite] emulator. The emulator is available on npm as the [azurite][azurite-npm] package, and is available to install globally:

```bash
npm install --global azurite
```

Once installed, the emulator includes a special ``azurite-table`` command to only start the table service using default settings and a port of ``10002``:

```bash
azurite-table --tablePort 10002
```

Then, update the **Microsoft.DataTransfer.AzureTable.FunctionalTests/.runsettings** file with the following connection string:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <TestRunParameters>
        <Parameter name="AzureStorageConnectionString" value="UseDevelopmentStorage=true;" />
    </TestRunParameters>
</RunSettings>
```

> 💡 The ``UseDevelopmentStorage=true;`` shorthand connection string is a special value for Azure Storage that maps to ``DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;``.

### Sql

Run functional tests for Sql using [Sql Server Express LocalDb][sql-express-localdb]. This lightweight version of the database is included, by default, with many version of Visual Studio.

> 💡 The database is typically already running on Windows machines with Visual Studio installed.

Update the **Microsoft.DataTransfer.AzureTable.FunctionalTests/.runsettings** file with the following connection string:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <TestRunParameters>
        <Parameter name="SqlConnectionString" value="Server=(localdb)\MSSQLLocalDB;Integrated Security=true" />
    </TestRunParameters>
</RunSettings>
```

> 💡 The ``Server=(localdb)\MSSQLLocalDB`` server endpoint connects the test runner to the currently running instance of SQL Server Express LocalDb regardless of version.

### DynamoDB

Run functional tests for DynamoDB with the **[amazon/dynamodb-local][dynamodb-docker]** container image from Docker Hub. To start a new instance in a Docker installation:

```bash
docker pull amazon/dynamodb-local

docker run --detach --publish 8000:8000 amazon/dynamodb-local
```

Then, update the **Microsoft.DataTransfer.DynamoDb.FunctionalTests/.runsettings** file with the following connection string:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <TestRunParameters>
        <Parameter name="DynamoDbConnectionString" value="ServiceURL=http://localhost:8000;AccessKey=anykeyvalue;SecretKey=anysecretvalue" />
    </TestRunParameters>
</RunSettings>
```

> 💡 For the local installation of DynamoDB, you can use any value for the **accesskey** and **secretkey** parameters.

### ~~Apache HBase~~

### ~~RavenDb~~

[azure-cosmos-db-emulator]: https://docs.microsoft.com/azure/cosmos-db/local-emulator
[azure-cosmos-db-emulator-credentials]: https://docs.microsoft.com/azure/cosmos-db/local-emulator#authenticate-requests
[azurite]: https://docs.microsoft.com/azure/storage/common/storage-use-azurite
[azurite-credentials]: https://docs.microsoft.com/azure/storage/common/storage-use-azurite?#well-known-storage-account-and-key
[azurite-npm]: https://www.npmjs.com/package/azurite
[dynamodb-docker]: https://hub.docker.com/r/amazon/dynamodb-local
[mongo-docker]: https://hub.docker.com/_/mongo
[sql-express-localdb]: https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb
[visual-studio]: https://visualstudio.microsoft.com/
