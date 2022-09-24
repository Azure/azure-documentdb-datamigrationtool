# Developer Guidance

- [Developer Guidance](#developer-guidance)
  - [Software prerequisites](#software-prerequisites)
  - [Clone the source code repository](#clone-the-source-code-repository)
  - [Build the solution](#build-the-solution)
  - [Tutorial: JSON to Cosmos DB migration](#tutorial-json-to-cosmos-db-migration)
    - [Task 1: Provision a sample database and container using the Azure Cosmos DB Emulator as the destination(sink)](#task-1-provision-a-sample-database-and-container-using-the-azure-cosmos-db-emulator-as-the-destinationsink)
    - [Task 2: Prepare JSON source documents](#task-2-prepare-json-source-documents)
    - [Task 3: Setup the data migration configuration](#task-3-setup-the-data-migration-configuration)
  - [Creating Extensions](#creating-extensions)

## Software prerequisites

1. [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
2. [.NET 6.0 SDK](https://github.com/Azure/azure-documentdb-datamigrationtool)
3. [Git client](https://github.com/git-guides/install-git)
4. [Azure Cosmos DB Emulator](https://learn.microsoft.com/azure/cosmos-db/local-emulator) or Azure Cosmos DB resource.

## Clone the source code repository

1. From a command prompt, execute the following command in an empty working folder that will house the source code.

```shell
git clone --branch feature/begin-core https://github.com/Azure/azure-documentdb-datamigrationtool.git
```

## Build the solution

1. Using Visual Studio 2022, open `CosmosDbDataMigrationTool.sln`.

2. Build the project using the keyboard shortcut <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> (<kbd>Cmd</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> on a Mac). This will build all current extension projects as well as the command-line **Core** application. The extension projects build assemblies get written to the **Extensions** folder of the **Core** application build. This way all extension options are available when the application is run.

## Tutorial: JSON to Cosmos DB migration

This tutorial outlines how to use the Cosmos DB Data Migration tool to move JSON data to Azure Cosmos DB. This tutorial uses the Azure Cosmos DB Emulator.

### Task 1: Provision a sample database and container using the Azure Cosmos DB Emulator as the destination(sink)

1. Launch the Azure Cosmos DB emulator application and open <https://localhost:8081/_explorer/index.html> in a browser.

2. Select the **Explorer** option from the left menu. Then choose the **New Database** link found beneath the **Common Tasks** heading.

    ![The Azure Cosmos DB emulator screen displays with Explorer selected from the left menu and the New Database link highlighted beneath the Common Tasks heading.](media/cosmosemulator_createdatabase.png "Create new database")

3. On the **New Database** blade, enter `datamigration` in the **Database id** field, then select **OK**.

    ![The New Database blade displays with datamigration entered in the Database id field and the OK button is highlighted.](media/cosmosemulator_newdatabaseform.png "New Database blade")

4. If the **datamigration** database doesn't appear in the list of databases, select the **Refresh** icon.

    ![The datamigration database displays with the refresh button highlighted.](media/cosmosemulator_datamigrationdb.png "Azure Cosmos DB Emulator database listing")

5. Expand the ellipsis menu next to the **datamigration** database and select **New Container**.

   ![The ellipsis menu of the datamigration database is expanded with the New Container item highlighted.](media/cosmosdbemulator_newcontainermenu.png "New Container")

6. On the **New Container** blade, enter `btcdata` in the **Container id** field, and `/id` in the **Partition key** field.  Select the **OK** button.

    ![The New Container blade displays with btcdata entered in the Container id field and /id entered in the Partition key field. The OK button is highlighted.](media/cosmosdbemulator_newcontainerform.png "New Container blade")

    >**Note**: When using the Cosmos DB Data Migration tool, the container doesn't have to previously exist, it will be created automatically using /id as the partition key. If a different partition key is desired, create the container beforehand as done in this tutorial.

### Task 2: Prepare JSON source documents

1. Locate the **docs/resources/sample-data.zip** file. Extract the files to any desired folder. These files serve as the JSON data that is migrated to Cosmos DB. This path is now referred to as **Source Path**.

### Task 3: Setup the data migration configuration

1. Each extension contains a README document that outlines configuration for the data migration. In this case, locate the configuration for [JSON (Source)](https://github.com/Azure/azure-documentdb-datamigrationtool/tree/feature/begin-core/Extensions/Json/Microsoft.DataTransfer.JsonExtension) and [Cosmos DB (Sink)](https://github.com/Azure/azure-documentdb-datamigrationtool/tree/feature/begin-core/Extensions/Cosmos/Microsoft.DataTransfer.CosmosExtension).

2. In the Visual Studio Solution Explorer, expand the **Microsoft.Data.Transfer.Core** project, and open **appsettings.json**. This file provides an example outline of the settings file structure. Using the documentation linked above, configure the **JsonSourceSettings** and **CosmosSinkSettings** sections. Ensure the **FilePath** setting is the location where the sample data is extracted. The ConnectionString setting can be found on the Cosmos DB Emulator **Quickstart** screen as the **Primary Connection String**. Save the file.

    ```json
    {
        "JSONSourceSettings": {
            "FilePath": "C:\\btcdata"
        },
        "CosmosSinkSettings": {
            "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDj...",
            "Database": "datamigration",
            "Container": "btcdata",
            "IncludeMetadataFields": false
        }
    }
    ```

    ![The Cosmos DB Emulator Quickstart screen displays with the Primary Connection String value highlighted.](media/cosmosemulator_primaryconnectionstring.png "Cosmos DB Emulator Primary Connection String")

3. Ensure the **Microsoft.DataTransfer.Core** project is set as the startup project then press <kbd>F5</kbd> to run the application.

4. The application will load the provided extensions, then prompt for a **Source**. Select **JSON**. When prompted to load settings from a file enter `n` and press <kbd>Enter</kbd>. The application provides the ability to configure extensions in a separate file versus having them located in the **appsettings.json** file. When a separate file is not specified, the settings are read from **appsettings.json**. When prompted to provide the Configuration section name, press <kbd>Enter</kbd> to accept the default: **JSONSourceSettings**.

    ![The Data Migration application displays with a menu prompt for Source.](media/app_source_prompt.png "Select Source")

5. The application will then prompt for a **Sink**, choose **Cosmos**. When prompted to load settings from a file enter `n` and press <kbd>Enter</kbd>. The application provides the ability to configure extensions in a separate file versus having them located in the **appsettings.json** file. When a separate file is not specified, the settings are read from **appsettings.json**. When prompted to provide the Configuration section name, press <kbd>Enter</kbd> to accept the default: **CosmosSinkSettings**.

    ![The Data migration application displays with a menu prompt for Sink.](media/app_sink_prompt.png "Select Sink")

6. The application then performs the data migration.

## Creating Extensions

1. Add a new folder in the Extensions folder with the name of your extension.

2. Create the extension project and an accompanying test project.
    - The naming convention for extension projects is `Microsoft.DataTransfer.<Name>Extension`.
    - Extension projects should use .NET 6 framework and **Console Application** type. A Program.cs file must be included in order to build the console project. A Console Application Project is required to have the build include NuGet referenced packages.
  
3. Add the new projects to the `CosmosDbDataMigrationTool` solution.

4. In order to facilitate local debugging the extension build output along with any dependencies needs to be copied into the `Core\Microsoft.DataTransfer.Core\bin\Debug\net6.0\Extensions` folder. To set up the project to automatically copy add the following changes.
    - Add a Publish Profile to Folder named `LocalDebugFolder` with a Target Location of `..\..\..\Core\Microsoft.DataTransfer.Core\bin\Debug\net6.0\Extensions`
    - To publish every time the project builds, edit the .csproj file to add a new post-build step:

    ```xml
    <Target Name="PublishDebug" AfterTargets="Build" Condition=" '$(Configuration)' == 'Debug' ">
       <Exec Command="dotnet publish --no-build -p:PublishProfile=LocalDebugFolder" />
    </Target>
    ```

5. Add references to the `System.ComponentModel.Composition` NuGet package and the `Microsoft.DataTransfer.Interfaces` project.

6. Extensions can implement either `IDataSourceExtension` to read data or `IDataSinkExtension` to write data. Classes implementing these interfaces should include a class level `System.ComponentModel.Composition.ExportAttribute` with the implemented interface type as a parameter. This will allow the plugin to get picked up by the main application.

7. Settings needed by the extension can be retrieved from any standard .NET configuration source in the main application by using the `IConfiguration` instance passed into the `ReadAsync` and `WriteAsync` methods. Settings under `SourceSettings`/`SinkSettings` will be included as well as any settings included in JSON files specified by the `SourceSettingsPath`/`SinkSettingsPath`.

8. Implement your extension to read and/or write using the generic `IDataItem` interface which exposes object properties as a list key-value pairs. Depending on the specific structure of the data storage type being implemented, you can choose to support nested objects and arrays or only flat top-level properties.
