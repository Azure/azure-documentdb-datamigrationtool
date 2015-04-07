using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
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

        public string Get(
            string sourceName, IReadOnlyDictionary<string, string> sourceArguments,
            string sinkName, IReadOnlyDictionary<string, string> sinkArguments)
        {
            var commandLine = new StringBuilder();

            AppendAdapterArgument(commandLine, SourceSwitch, sourceName);
            commandLine.Append(' ');
            AppendAdapterConfiguration(commandLine, SourceSwitch, sourceArguments);
            commandLine.Append(' ');

            AppendAdapterArgument(commandLine, TargetSwitch, sinkName);
            commandLine.Append(' ');
            AppendAdapterConfiguration(commandLine, TargetSwitch, sinkArguments);

            return commandLine.ToString();
        }

        private static void AppendAdapterArgument(StringBuilder commandLine, string adapterSwitch, string name)
        {
            Guard.NotNull("commandLine", commandLine);

            commandLine.Append(SwitchCharacter);
            commandLine.Append(adapterSwitch);
            commandLine.Append(AssignmentCharacter);
            commandLine.Append(EscapeValue(name));
        }

        private static void AppendAdapterConfiguration(StringBuilder commandLine, string adapterSwitch, IReadOnlyDictionary<string, string> arguments)
        {
            Guard.NotNull("commandLine", commandLine);

            if (arguments == null)
                return;

            var first = true;
            foreach (var argument in arguments)
            {
                if (!first) commandLine.Append(' '); else first = false;

                commandLine.Append(SwitchCharacter);
                commandLine.Append(adapterSwitch);
                commandLine.Append(PropertyAccessorCharacter);
                commandLine.Append(EscapeValue(argument.Key));

                if (argument.Value != null)
                {
                    commandLine.Append(AssignmentCharacter);
                    commandLine.Append(EscapeValue(argument.Value));
                }
            }
        }

        private static string EscapeValue(string value)
        {
            value = value.Replace("\"", "\"\"\"");
            return value.Contains(" ") ? "\"" + value + "\"" : value;
        }
    }
}
