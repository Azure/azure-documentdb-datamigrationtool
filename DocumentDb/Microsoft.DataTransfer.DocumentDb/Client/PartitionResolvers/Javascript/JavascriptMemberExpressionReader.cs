using Microsoft.DataTransfer.Basics.IO;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors;
using System;
using System.Text;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript
{
    sealed class JavascriptMemberExpressionReader
    {
        private SimpleStringReader reader;
        private IJavascriptMemberExpressionVisitor visitor;

        private JavascriptMemberExpressionReader(SimpleStringReader reader, IJavascriptMemberExpressionVisitor visitor)
        {
            this.reader = reader;
            this.visitor = visitor;
        }

        public static void Read(SimpleStringReader reader, IJavascriptMemberExpressionVisitor visitor)
        {
            try
            {
                new JavascriptMemberExpressionReader(reader, visitor).ReadMemberName();
            }
            catch (Exception error)
            {
                throw Errors.FailedToReadJavascriptMemberExpression(error.Message);
            }
        }

        private void ReadMemberName()
        {
            var memberName = new StringBuilder();
            char character;
            while (reader.ReadNext(out character))
            {
                if (character == '.' || character == '[')
                {
                    if (memberName.Length > 0)
                    {
                        visitor.VisitMember(memberName.ToString());
                        memberName.Clear();
                    }

                    if (character == '[')
                    {
                        ReadDictionaryStyleMember();
                    }
                }
                else
                {
                    if (!IsAlphaNumeric(character))
                        throw UnexpectedCharacter();

                    memberName.Append(character);
                }
            }

            if (memberName.Length > 0)
                visitor.VisitMember(memberName.ToString());
        }

        private void ReadDictionaryStyleMember()
        {
            char character;
            if (reader.ReadNext(out character))
            {
                if (character == '\'' || character == '\"')
                {
                    visitor.VisitMember(ReadStringMemberName(character));
                }
                else
                {
                    visitor.VisitArrayElement(ReadArrayIndex());
                }

                if (reader.Read(out character) && character == ']')
                    return;
            }

            throw UnexpectedCharacter();
        }

        private string ReadStringMemberName(char quoteCharacter)
        {
            char character;
            var memberName = new StringBuilder();
            while (reader.ReadNext(out character))
            {
                if (character == quoteCharacter)
                {
                    // Adjust position past the quote character
                    reader.ReadNext(out character);
                    return memberName.ToString();
                }

                if (character == '\\')
                {
                    memberName.Append(ReadEscapedCharacter());
                    continue;
                }

                memberName.Append(character);
            }

            throw UnexpectedCharacter();
        }

        private char ReadEscapedCharacter()
        {
            char character;
            if (!reader.ReadNext(out character))
                throw UnexpectedCharacter();

            switch (character)
            {
                case '\'':
                case '"':
                case '\\':
                case '/':
                    return character;
                case 'b':
                    return '\b';
                case 'f':
                    return '\f';
                case 'n':
                    return '\n';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
                case 'u':
                    return ReadUnicodeCharacter();
                default: break;
            }

            throw UnexpectedCharacter();
        }

        private char ReadUnicodeCharacter()
        {
            var hexValue = new StringBuilder(4);
            char character;
            while (reader.ReadNext(out character) && hexValue.Length < 4)
            {
                if (!IsHex(character))
                    break;

                hexValue.Append(character);
            }

            if (hexValue.Length == 4)
                return Convert.ToChar(Convert.ToInt16(hexValue.ToString(), 16));

            throw UnexpectedCharacter();
        }

        private int ReadArrayIndex()
        {
            var index = new StringBuilder(10);
            char character;
            reader.Read(out character);
            do
            {
                if (!Char.IsDigit(character))
                {
                    int indexValue;
                    if (!int.TryParse(index.ToString(), out indexValue))
                        throw Errors.NotANumber(index.ToString());
                    return indexValue;
                }

                index.Append(character);
            } while (reader.ReadNext(out character));

            throw UnexpectedCharacter();
        }

        private static bool IsAlphaNumeric(char character)
        {
            character = Char.ToLowerInvariant(character);
            return Char.IsDigit(character) || (character >= 'a' && character <= 'z');
        }

        private static bool IsHex(char character)
        {
            character = Char.ToLowerInvariant(character);
            return Char.IsDigit(character) || (character >= 'a' && character <= 'f');
        }

        private Exception UnexpectedCharacter()
        {
            char character;
            reader.Read(out character);
            return Errors.UnexpectedCharacter(reader.Position, character);
        }
    }
}
