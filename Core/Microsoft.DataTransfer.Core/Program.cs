using System.ComponentModel.Composition.Hosting;
using System.IO;
using Microsoft.DataTransfer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.DataTransfer.Core;

class Program
{
    public static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddUserSecrets<Program>();
            })
            .Build();

        IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();

        var options = configuration.Get<DataTransferOptions>();

        var hostingProcess = host.RunAsync();

        var catalog = new AggregateCatalog();
        if (!Directory.Exists("Extensions"))
        {
            Directory.CreateDirectory("Extensions");
        }
        catalog.Catalogs.Add(new DirectoryCatalog("Extensions", "*Extension.dll"));
        var container = new CompositionContainer(catalog);

        var sources = LoadExtensions<IDataSourceExtension>(container);
        var sinks = LoadExtensions<IDataSinkExtension>(container);

        Console.WriteLine($"{sources.Count + sinks.Count} Extensions Loaded");

        var source = GetExtensionSelection(options.Source, sources, "Source");
        var sourceConfig = BuildSettingsConfiguration(configuration, options.SourceSettingsPath, $"{source.DisplayName}SourceSettings");

        var sink = GetExtensionSelection(options.Sink, sinks, "Sink");
        var sinkConfig = BuildSettingsConfiguration(configuration, options.SinkSettingsPath, $"{sink.DisplayName}SinkSettings");

        var data = source.ReadAsync(sourceConfig);
        await sink.WriteAsync(data, sinkConfig);

        Console.WriteLine("Done");

        Console.WriteLine("Enter to Quit...");
        Console.ReadLine();

        await host.StopAsync();
        await hostingProcess;
    }

    private static List<T> LoadExtensions<T>(CompositionContainer container)
        where T : class, IDataTransferExtension
    {
        var sources = new List<T>();

        foreach (var exportedExtension in container.GetExports<T>())
        {
            sources.Add(exportedExtension.Value);
        }

        return sources;
    }

    private static T GetExtensionSelection<T>(string? selectionName, List<T> extensions, string inputPrompt) 
        where T : class, IDataTransferExtension
    {
        if (!string.IsNullOrWhiteSpace(selectionName))
        {
            var extension = extensions.FirstOrDefault(s => selectionName.Equals(s.DisplayName, StringComparison.OrdinalIgnoreCase));
            if (extension != null)
            {
                Console.WriteLine($"Using {extension.DisplayName} {inputPrompt}");
                return extension;
            }
        }

        Console.WriteLine($"Select {inputPrompt}");
        for (var index = 0; index < extensions.Count; index++)
        {
            var extension = extensions[index];
            Console.WriteLine($"{index + 1}:{extension.DisplayName}");
        }

        string? selection = "";
        int input;
        while (!int.TryParse(selection, out input) || input > extensions.Count)
        {
            selection = Console.ReadLine();
        }

        return extensions[input - 1];
    }

    private static IConfiguration BuildSettingsConfiguration(IConfiguration configuration, string? settingsPath, string configSection)
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        if (!string.IsNullOrEmpty(settingsPath))
        {
            configurationBuilder = configurationBuilder.AddJsonFile(settingsPath);
        }
        else
        {
            Console.Write($"Load settings from a file? (y/n):");
            var response = Console.ReadLine();
            if (IsYesResponse(response))
            {
                Console.Write("Path to file: ");
                var path = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(path))
                {
                    configurationBuilder = configurationBuilder.AddJsonFile(path);
                }
            }
            else
            {
                Console.Write($"Configuration section to read settings? (default={configSection}):");
                response = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    configSection = response;
                }
            }
        }

        return configurationBuilder
            .AddConfiguration(configuration.GetSection(configSection))
            .Build();
    }

    private static bool IsYesResponse(string? response)
    {
        if (response?.Equals("y", StringComparison.CurrentCultureIgnoreCase) == true)
            return true;
        if (response?.Equals("yes", StringComparison.CurrentCultureIgnoreCase) == true)
            return true;

        return false;
    }
}