using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    sealed class CommandLineConfiguration : IOneTimeDataTransferConfiguration, IRawInfrastructureConfiguration
    {
        public const string SwitchCharacter = "/";
        public const string SourceSwitch = "s";
        public const string TargetSwitch = "t";

        private static readonly Regex ArgumentRegex = new Regex(String.Format(CultureInfo.InvariantCulture,
            @"{0}(((?<type>[{1}{2}](?=[\.:]))(\.(?<name>[^:]*))?)|(?<name>[^:]+))(:(?<value>.*))?",
                SwitchCharacter, SourceSwitch, TargetSwitch), RegexOptions.Compiled);

        public IReadOnlyDictionary<string, string> InfrastructureConfiguration { get; private set; }

        public string SourceName { get; private set; }
        public IReadOnlyDictionary<string, string> SourceConfiguration { get; private set; }

        public string TargetName { get; private set; }
        public IReadOnlyDictionary<string, string> TargetConfiguration { get; private set; }

        private CommandLineConfiguration() { }

        public static CommandLineConfiguration Parse(string[] arguments)
        {
            var infrastructureConfiguration = new Dictionary<string, string>();
            var sourceConfiguration = new Dictionary<string, string>();
            var targetConfiguration = new Dictionary<string, string>();

            foreach (var argument in arguments)
            {
                var match = ArgumentRegex.Match(argument);
                if (!match.Success)
                    continue;

                Dictionary<string, string> configurationCollection;

                if (SourceSwitch.Equals(match.Groups["type"].Value, StringComparison.Ordinal))
                {
                    configurationCollection = sourceConfiguration;
                }
                else if (TargetSwitch.Equals(match.Groups["type"].Value, StringComparison.Ordinal))
                {
                    configurationCollection = targetConfiguration;
                }
                else
                {
                    configurationCollection = infrastructureConfiguration;
                }

                AddValue(match.Groups, configurationCollection);
            }

            string sourceName, targetName;
            if (sourceConfiguration.TryGetValue(String.Empty, out sourceName))
                sourceConfiguration.Remove(String.Empty);

            if (targetConfiguration.TryGetValue(String.Empty, out targetName))
                targetConfiguration.Remove(String.Empty);

            return new CommandLineConfiguration
            {
                InfrastructureConfiguration = infrastructureConfiguration,
                SourceName = sourceName,
                SourceConfiguration = sourceConfiguration,
                TargetName = targetName,
                TargetConfiguration = targetConfiguration
            };
        }

        private static void AddValue(GroupCollection groups, Dictionary<string, string> target)
        {
            target[groups["name"].Value] = groups["value"].Success ? groups["value"].Value : bool.TrueString;
        }
    }
}
