using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg => cfg.AddUserSecrets<Program>())
    .Build();

IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();

CompositionContainer _container;

var catalog = new AggregateCatalog();
if (!Directory.Exists("Extensions"))
{
    Directory.CreateDirectory("Extensions");
}    
catalog.Catalogs.Add(new DirectoryCatalog("Extensions"));    
_container = new CompositionContainer(catalog);

var extensions = new List<IDataTransferExtension>();

foreach(var exportedExtension in _container.GetExports<IDataTransferExtension>())
{
    extensions.Add(exportedExtension.Value);
}

Console.WriteLine("Extensions Loaded");
for (var index = 0; index < extensions.Count; index++)
{
    var extension = extensions[index];
    Console.WriteLine($"{index + 1}:{extension.DisplayName}");
}

Console.WriteLine("Select Input");

string? selection = "";
int input;
while (!int.TryParse(selection, out input) || input > extensions.Count){
    selection = Console.ReadLine();
}
var source = extensions[input - 1];

Console.WriteLine("Select Output");

selection = "";
int output;
while (!int.TryParse(selection, out output) || output > extensions.Count)
{
    selection = Console.ReadLine();
}
var sink = extensions[output - 1];

await source.Configure(configuration);
await sink.Configure(configuration);
var data = source.ReadAsSourceAsync();
await sink.WriteAsSinkAsync(data);

Console.WriteLine("Done");

await host.RunAsync();