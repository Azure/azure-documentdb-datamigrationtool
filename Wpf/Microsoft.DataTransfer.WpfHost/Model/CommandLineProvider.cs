using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.Helpers;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class CommandLineProvider : ICommandLineProvider
    {
        private const string SwitchCharacter = "/";
        private const string SourceSwitch = "s";
        private const string TargetSwitch = "t";
        private const string PropertyAccessorCharacter = ".";
        private const string AssignmentCharacter = ":";

        public string Get(IInfrastructureConfiguration infrastructureConfiguration,
            string sourceName, IReadOnlyDictionary<string, string> sourceArguments,
            string sinkName, IReadOnlyDictionary<string, string> sinkArguments)
        {
            var commandLine = new StringBuilder();

            if (AppendInfrastructureConfiguration(commandLine, infrastructureConfiguration))
                commandLine.Append(' ');

            AppendAdapterArgument(commandLine, SourceSwitch, sourceName);
            commandLine.Append(' ');
            if (AppendConfiguration(commandLine, AppendSourceAdapterConfigurationArgumentName, sourceArguments))
                commandLine.Append(' ');

            AppendAdapterArgument(commandLine, TargetSwitch, sinkName);
            commandLine.Append(' ');
            AppendConfiguration(commandLine, AppendTargetAdapterConfigurationArgumentName, sinkArguments);

            return commandLine.ToString();
        }

        private bool AppendInfrastructureConfiguration(StringBuilder commandLine, IInfrastructureConfiguration infrastructureConfiguration)
        {
            if (infrastructureConfiguration == null)
                return false;

            var arguments = new Dictionary<string, string>(2);

            if (!String.IsNullOrEmpty(infrastructureConfiguration.ErrorLog))
            {
                arguments.Add(InfrastructureConfigurationProperties.ErrorLog, infrastructureConfiguration.ErrorLog);

                if (infrastructureConfiguration.OverwriteErrorLog)
                    arguments.Add(InfrastructureConfigurationProperties.OverwriteErrorLog, null);
            }

            return AppendConfiguration(commandLine, AppendInfrastructureConfigurationArgumentName, arguments);
        }

        private static void AppendAdapterArgument(StringBuilder commandLine, string adapterSwitch, string name)
        {
            Guard.NotNull("commandLine", commandLine);

            commandLine.Append(SwitchCharacter);
            commandLine.Append(adapterSwitch);
            commandLine.Append(AssignmentCharacter);
            commandLine.Append(EscapeValue(name));
        }

        private static bool AppendConfiguration(StringBuilder commandLine, Action<StringBuilder, string> switchNameRenderer,
            IReadOnlyDictionary<string, string> arguments)
        {
            Guard.NotNull("commandLine", commandLine);

            if (arguments == null)
                return false;

            var first = true;
            foreach (var argument in arguments)
            {
                if (!first) commandLine.Append(' '); else first = false;

                switchNameRenderer(commandLine, argument.Key);

                if (argument.Value != null)
                {
                    commandLine.Append(AssignmentCharacter);
                    commandLine.Append(EscapeValue(argument.Value));
                }
            }

            return !first;
        }

        private static void AppendSourceAdapterConfigurationArgumentName(StringBuilder commandLine, string argumentName)
        {
            AppendAdapterConfigurationArgumentName(commandLine, SourceSwitch, argumentName);
        }

        private static void AppendTargetAdapterConfigurationArgumentName(StringBuilder commandLine, string argumentName)
        {
            AppendAdapterConfigurationArgumentName(commandLine, TargetSwitch, argumentName);
        }

        private static void AppendAdapterConfigurationArgumentName(StringBuilder commandLine, string adapterSwitch, string argumentName)
        {
            commandLine.Append(SwitchCharacter);
            commandLine.Append(adapterSwitch);
            commandLine.Append(PropertyAccessorCharacter);
            commandLine.Append(EscapeValue(argumentName));
        }

        private static void AppendInfrastructureConfigurationArgumentName(StringBuilder commandLine, string argumentName)
        {
            commandLine.Append(SwitchCharacter);
            commandLine.Append(EscapeValue(argumentName));
        }

        private static string EscapeValue(string value)
        {
            value = value.Replace("\"", "\"\"\"");
            return value.Contains(" ") ? "\"" + value + "\"" : value;
        }
    }
}
