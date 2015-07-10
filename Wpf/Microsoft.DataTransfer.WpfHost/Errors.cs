using Microsoft.DataTransfer.Basics;
using System;

namespace Microsoft.DataTransfer.WpfHost
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception NoAvailableSteps()
        {
            return new InvalidOperationException(Resources.NoAvailableSteps);
        }
    }
}
