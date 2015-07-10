using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System;

namespace Microsoft.DataTransfer.CsvFile
{
    sealed class Errors : CommonErrors
    {
        private Errors() { }

        public static Exception UnexpectedCharacter(long row, long position, char character)
        {
            return new FormatException(FormatMessage(Resources.UnexpectedCharacterFormat, row, position, character));
        }

        public static Exception InvalidNumberOfColumns(int actual, int expected)
        {
            return new NonFatalReadException(FormatMessage(Resources.InvalidNumberOfColumnsFormat, actual, expected));
        }
    }
}
