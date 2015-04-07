using Microsoft.DataTransfer.Basics;
using System;
using System.IO;

namespace Microsoft.DataTransfer.TestsCommon
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception TestSettingsFileMissing(string path)
        {
            return new FileNotFoundException(Resources.TestSettingsFileMissingFormat, path);
        }
    }
}
