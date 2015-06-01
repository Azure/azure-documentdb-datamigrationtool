using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.ConsoleHost.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration
{
    /// <summary>
    /// Generates a proxy class for a given configuration type based on the input command line arguments.
    /// </summary>
    public sealed class DynamicConfigurationFactory : IDataAdapterConfigurationFactory
    {
        private const char CollectionValuesEscapeCharacter = '\\';
        private const char CollectionValuesSeparatorCharacter = ';';

        private static readonly MethodInfo ConvertCollectionValueMethod =
            typeof(DynamicConfigurationFactory).GetMethod("ConvertCollectionValue", BindingFlags.Static | BindingFlags.NonPublic);

        private readonly SimpleProxyGenerator proxyGenerator;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicConfigurationFactory" />.
        /// </summary>
        public DynamicConfigurationFactory()
        {
            proxyGenerator = new SimpleProxyGenerator("Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration.Instances");
        }

        /// <summary>
        /// Creates a new proxy intance of required configuration type, populating it from the command line arguments.
        /// </summary>
        /// <param name="configurationType">Type of the required configuration.</param>
        /// <param name="arguments">Command line arguments.</param>
        /// <returns>New instance of required configuration type.</returns>
        public object TryCreate(Type configurationType, IReadOnlyDictionary<string, string> arguments)
        {
            Guard.NotNull("configurationType", configurationType);
            Guard.NotNull("arguments", arguments);

            var properties = proxyGenerator.GetInterfaceProperties(configurationType);
            var values = new Dictionary<string, object>(properties.Length);

            foreach (var argument in arguments)
            {
                var property = properties.FirstOrDefault(p => p.Name.Equals(argument.Key, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                    throw Errors.UnknownOption(argument.Key);

                values.Add(property.Name, ConvertValue(argument.Value, property.PropertyType));
            }

            try
            {
                return proxyGenerator.CreateProxy(configurationType, values);
            }
            catch (Exception ex)
            {
                throw Errors.DynamicConfigurationGenerationFailed(configurationType, ex);
            }
        }

        private static object ConvertValue(string value, Type type)
        {
            Guard.NotNull("type", type);

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsEnum)
                return Enum.Parse(type, value, true);

            if (type.IsAssignableFrom(typeof(TimeSpan)))
                return TimeSpan.Parse(value, CultureInfo.InvariantCulture);

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                return ConvertCollectionValueMethod.MakeGenericMethod(type.GetGenericArguments()[0]).Invoke(null, new[] { value });

            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        private static IEnumerable<T> ConvertCollectionValue<T>(string value)
        {
            var result = new List<T>();
            var currentValue = new StringBuilder();
            var isEscaping = false;
            for (var index = 0; index < value.Length; ++index)
            {
                switch (value[index])
                {
                    case CollectionValuesEscapeCharacter:
                        if (isEscaping)
                            currentValue.Append(value[index]);
                        break;
                    case CollectionValuesSeparatorCharacter:
                        if (isEscaping)
                        {
                            // Escaped, flush escaped symbol to the output and swallow escape character 
                            currentValue.Append(value[index]);
                            break;
                        }
                        result.Add((T)ConvertValue(currentValue.ToString(), typeof(T)));
                        currentValue.Clear();
                        break;
                    default:
                        if (isEscaping)
                            // Nothing to escape, flush escape character to the output
                            currentValue.Append(CollectionValuesEscapeCharacter);
                        currentValue.Append(value[index]);
                        break;
                }

                isEscaping = value[index] == CollectionValuesEscapeCharacter && !isEscaping;
            }

            if (currentValue.Length > 0)
                result.Add((T)ConvertValue(currentValue.ToString(), typeof(T)));

            return result;
        }

        /// <summary>
        /// Based on the configuration type assumes description for configuration options.
        /// </summary>
        /// <param name="configurationType">Type of the configuration to get description for.</param>
        /// <returns>Description of configuration options for specified type.</returns>
        public IReadOnlyDictionary<string, string> TryGetConfigurationOptions(Type configurationType)
        {
            Guard.NotNull("configurationType", configurationType);

            var properties = proxyGenerator.GetInterfaceProperties(configurationType);
            var options = new Dictionary<string, string>(properties.Length);

            foreach (var property in properties)
                options.Add(property.Name, GetPropertyDescription(property));

            return options;
        }

        private static string GetPropertyDescription(PropertyInfo property)
        {
            var displayAttribute = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayAttribute != null)
                return displayAttribute.GetDescription();

            var rawType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            return rawType.IsEnum
                    ? String.Format(CultureInfo.InvariantCulture,
                        Resources.ConfigurationEnumOptionDescriptionFormat, String.Join(", ", Enum.GetNames(rawType)))
                    : String.Format(CultureInfo.InvariantCulture,
                        Resources.ConfigurationOptionDescriptionFormat, rawType.Name);
        }
    }
}
