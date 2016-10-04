using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.DataTransfer.CsvFile.Reader
{
    sealed class CsvReader : IDisposable
    {
        private const string NullString = "null";

        private TextReader reader;
        private readonly CsvReaderConfiguration configuration;

        private int position;

        public int Row { get; private set; }

        public CsvReader(TextReader reader, CsvReaderConfiguration configuration)
        {
            this.reader = reader;
            this.configuration = configuration;
        }

        public IReadOnlyList<object> Read()
        {
            Row += 1;
            position = 0;

            if (reader.Peek() <= 0)
                return null;

            var result = new List<object>();

            var readingValue = true;
            var value = new StringBuilder();
            char character;
            while (ReadNext(out character))
            {
                if (character == '\r')
                {
                    continue;
                }
                else if (character == '\n')
                {
                    if (readingValue)
                        result.Add(ConvertUnquotedValue(value.ToString()));

                    if (!result.Any())
                        // Skip empty row and continue
                        continue;

                    return result;
                }
                // Currently there seem to be no standard cultures that use multiple characters as a list separator.
                // Lets not complicate the parsing logic for now.
                else if (character == configuration.ParserCulture.TextInfo.ListSeparator[0])
                {
                    if (readingValue)
                        result.Add(ConvertUnquotedValue(value.ToString()));

                    value.Clear();
                    readingValue = true;
                    continue;
                }
                else if (character == '"')
                {
                    if (value.Length > 0 || !readingValue)
                        throw UnexpectedCharacter(character);

                    result.Add(ReadQuotedValue());
                    readingValue = false;
                    continue;
                }
                else if (character == ' ' || character == '\t') 
                {
                    // Trim spaces and tabs at the beginning of unquoted values
                    if (value.Length > 0)
                        value.Append(character);
                    continue;
                }
                else
                {
                    if (!readingValue)
                        throw UnexpectedCharacter(character);
                    value.Append(character);
                }
            }

            // At the EOF, only append a value if it actualy has anything in it, or we had values before it
            if (readingValue && (value.Length > 0 || result.Any()))
                result.Add(ConvertUnquotedValue(value.ToString()));

            return result.Any() ? result : null;
        }

        private string ReadQuotedValue()
        {
            var result = new StringBuilder();
            char character;
            while (ReadNext(out character))
            {
                switch (character)
                {
                    case '"':
                        if (reader.Peek() != '"')
                        {
                            var resultString = result.ToString();
                            return configuration.TrimQuoted ? resultString.TrimEnd() : resultString;
                        }

                        ReadNext(out character);
                        result.Append('"');
                        break;
                    case ' ':
                        // Trim spaces at the beginning
                        if (result.Length > 0 || !configuration.TrimQuoted)
                            result.Append(character);
                        break;
                    default:
                        result.Append(character);
                        break;
                }
            }

            throw UnexpectedCharacter(character);
        }

        private object ConvertUnquotedValue(string value)
        {
            value = value.TrimEnd(' ', '\t');

            if (String.IsNullOrEmpty(value))
                return null;

            var numberFormat = NumberFormatInfo.GetInstance(configuration.ParserCulture);

            long number;
            if (long.TryParse(value, NumberStyles.Integer, numberFormat, out number))
                return number;

            double doubleNumber;
            if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, numberFormat, out doubleNumber))
                return doubleNumber;

            DateTime dateTime;
            if (DateTime.TryParse(value, DateTimeFormatInfo.GetInstance(configuration.ParserCulture),
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dateTime))
                return dateTime;

            bool boolean;
            if (bool.TryParse(value, out boolean))
                return boolean;

            if (!configuration.IgnoreUnquotedNulls && NullString.Equals(value, StringComparison.OrdinalIgnoreCase))
                return null;

            return value;
        }

        private bool ReadNext(out char character)
        {
            position += 1;
            var code = reader.Read();
            character = code < 0 ? default(char) : (char)code;
            return code >= 0;
        }

        private Exception UnexpectedCharacter(char character)
        {
            return Errors.UnexpectedCharacter(Row, position, character);
        }

        public void Dispose()
        {
            TrashCan.Throw(ref reader);
        }
    }
}
