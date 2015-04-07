using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.ConsoleHost.Configuration
{
    sealed class CommandLineOneTimeTransferConfiguration : IOneTimeDataTransferConfiguration
    {
        public const string SwitchCharacter = "/";
        public const string SourceSwitch = "s";
        public const string TargetSwitch = "t";

        private static readonly Regex ArgumentRegex = new Regex(String.Format(CultureInfo.InvariantCulture, 
            @"{0}(?<type>[{1}{2}])(\.(?<name>[^:]*))?(:(?<value>.*))?", SwitchCharacter, SourceSwitch, TargetSwitch), RegexOptions.Compiled);

        public string SourceName { get; private set; }
        public IReadOnlyDictionary<string, string> SourceConfiguration { get; private set; }

        public string TargetName { get; private set; }
        public IReadOnlyDictionary<string, string> TargetConfiguration { get; private set; }

        public CommandLineOneTimeTransferConfiguration(string[] arguments)
        {
            var sourceConfiguration = new Dictionary<string, string>();
            var targetConfiguration = new Dictionary<string, string>();

            foreach (var argument in arguments)
            {
                var match = ArgumentRegex.Match(argument);
                if (match.Success)
                    AddValue(match.Groups,
                        SourceSwitch.Equals(match.Groups["type"].Value, StringComparison.Ordinal)
                            ? sourceConfiguration : targetConfiguration);
            }

            string name;
            if (sourceConfiguration.TryGetValue(String.Empty, out name))
                SourceName = name;
            sourceConfiguration.Remove(String.Empty);
            
            if (targetConfiguration.TryGetValue(String.Empty, out name))
                TargetName = name;
            targetConfiguration.Remove(String.Empty);

            SourceConfiguration = sourceConfiguration;
            TargetConfiguration = targetConfiguration;
        }

        private static void AddValue(GroupCollection groups, Dictionary<string, string> target)
        {
            target[groups["name"].Value] = groups["value"].Success ? groups["value"].Value : bool.TrueString;
        }
    }
}
