# Creating Extensions for the Data Transfer tool

1. Add a new folder in the Extensions folder with the name of your extension.
1. Create the extension project and an accompanying test project.
    - The naming convention for extension projects is `Microsoft.DataTransfer.<Name>Extension`.
    - Extension projects should use .NET 6 framework and **Console Application** type. A Program.cs file must be included in order to build the console project.
1. Add the new projects to the `CosmosDbDataMigrationTool` solution.
1. In order to facilitate local debugging the extension build output along with any dependencies needs to be copied into the `Core\Microsoft.DataTransfer.Core\bin\Debug\net6.0\Extensions` folder. To set up the project to automatically copy add the following changes.
    - Add a Publish Profile to Folder named `LocalDebugFolder` with a Target Location of `..\..\..\Core\Microsoft.DataTransfer.Core\bin\Debug\net6.0\Extensions`
    - To publish every time the project builds, edit the .csproj file to add a new post-build step:

    ```xml
    <Target Name="PublishDebug" AfterTargets="Build" Condition=" '$(Configuration)' == 'Debug' ">
		<Exec Command="dotnet publish --no-build -p:PublishProfile=LocalDebugFolder" />
	</Target>
    ``` 
1. Add references to the `System.ComponentModel.Composition` NuGet package and the `Microsoft.DataTransfer.Interfaces` project.
1. Extensions can implement either `IDataSourceExtension` to read data or `IDataSinkExtension` to write data. Classes implementing these interfaces should include a class level `System.ComponentModel.Composition.ExportAttribute` with the implemented interface type as a parameter. This will allow the plugin to get picked up by the main application.
1. Settings needed by the extension can be retrieved from any standard .NET configuration source in the main application by using the `IConfiguration` instance passed into the `ReadAsync` and `WriteAsync` methods. Settings under `SourceSettings`/`SinkSettings` will be included as well as any settings included in JSON files specified by the `SourceSettingsPath`/`SinkSettingsPath`.
1. Implement your extension to read and/or write using the generic `IDataItem` interface which exposes object properties as a list key-value pairs. Depending on the specific structure of the data storage type being implemented, you can choose to support nested objects and arrays or only flat top-level properties.