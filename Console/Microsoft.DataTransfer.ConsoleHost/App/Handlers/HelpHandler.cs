using Microsoft.DataTransfer.ConsoleHost.Configuration;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class HelpHandler : IHelpHandler
    {
        private const int SectionIndentation = 2;

        private readonly IDataTransferService service;
        private readonly IInfrastructureConfigurationFactory infrastructureConfigurationFactory;
        private readonly IDataAdapterConfigurationFactory adaptersConfigurationFactory;

        public HelpHandler(IDataTransferService service, IInfrastructureConfigurationFactory infrastructureConfigurationFactory,
            IDataAdapterConfigurationFactory adaptersConfigurationFactory)
        {
            this.service = service;
            this.infrastructureConfigurationFactory = infrastructureConfigurationFactory;
            this.adaptersConfigurationFactory = adaptersConfigurationFactory;
        }

        public void Print()
        {
            Console.WriteLine(Resources.HelpHeaderFormat, Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine();

            PrintUsage();
            Console.WriteLine();

            PrintAdapters(Resources.HelpKnownSourcesHeader, CommandLineConfiguration.SourceSwitch, service.GetKnownSources());
            Console.WriteLine();

            PrintAdapters(Resources.HelpKnownTargetsHeader, CommandLineConfiguration.TargetSwitch, service.GetKnownSinks());
        }

        private void PrintUsage()
        {
            Console.WriteLine(Resources.HelpUsageExampleHeader);
            Console.WriteLine();
            WriteLineSection(1, Resources.HelpUsageExample, 
                CommandLineConfiguration.SwitchCharacter,
                CommandLineConfiguration.SourceSwitch,
                CommandLineConfiguration.TargetSwitch);
            Console.WriteLine();

            Console.WriteLine(Resources.HelpConfigurationHeader);
            Console.WriteLine();
            PrintOptions();
        }

        private void PrintOptions()
        {
            const int IndentLevel = 1;
            IReadOnlyDictionary<string, string> options;

            try
            {
                options = infrastructureConfigurationFactory.DescribeOptions();
            }
            catch (Exception ex)
            {
                WriteLineSection(IndentLevel, Resources.HelpConfigurationNotAvailableFormat, ex.Message);
                return;
            }

            foreach (var option in options)
            {
                WriteLineSection(IndentLevel, Resources.HelpConfigurationOptionFormat,
                    CommandLineConfiguration.SwitchCharacter, option.Key, option.Value);
            }
        }

        private void PrintAdapters(string header, string adapterCharacter, IReadOnlyDictionary<string, IDataAdapterDefinition> adapters)
        {
            const int IndentLevel = 1;

            Console.WriteLine(header);
            foreach (var adapter in adapters)
            {
                Console.WriteLine();
                WriteLineSection(IndentLevel, Resources.HelpKnownAdapterFormat, 
                    CommandLineConfiguration.SwitchCharacter, adapterCharacter,
                    adapter.Key, adapter.Value.Description);
                PrintAdapterOptions(adapterCharacter, adapter.Value.ConfigurationType);
            }
        }

        private void PrintAdapterOptions(string adapterCharacter, Type configurationType)
        {
            const int IndentLevel = 2;
            IReadOnlyDictionary<string, string> options;

            try
            {
                options = adaptersConfigurationFactory.TryGetConfigurationOptions(configurationType);
            }
            catch (Exception ex)
            {
                WriteLineSection(IndentLevel, Resources.HelpConfigurationNotAvailableFormat, ex.Message);
                return;
            }

            foreach (var option in options)
            {
                WriteLineSection(IndentLevel, Resources.HelpAdapterConfigurationOptionFormat,
                    CommandLineConfiguration.SwitchCharacter, adapterCharacter,
                    option.Key, option.Value);
            }
        }

        private static void WriteLineSection(int indentLevel, string text, params object[] args)
        {
            Console.Write(new String(' ', SectionIndentation * indentLevel));
            if (args == null)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }
    }
}
