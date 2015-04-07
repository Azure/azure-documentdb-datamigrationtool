using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception DynamicConfigurationGenerationFailed(Type configurationType, Exception details)
        {
            return new InvalidOperationException(FormatMessage(
                Resources.DynamicConfigurationGenerationFailedFormat, configurationType), details);
        }

        public static Exception UnknownOption(string name)
        {
            return new ArgumentException(FormatMessage(Resources.UnknownOptionFormat, name));
        }
    }
}
