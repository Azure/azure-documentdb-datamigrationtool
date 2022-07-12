using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition.Hosting;

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

foreach(var extension in extensions)
{
    Console.WriteLine(extension.DisplayName);
    extension.ReadAsSource();
    extension.WriteAsSink();
}

Console.WriteLine("Done");

