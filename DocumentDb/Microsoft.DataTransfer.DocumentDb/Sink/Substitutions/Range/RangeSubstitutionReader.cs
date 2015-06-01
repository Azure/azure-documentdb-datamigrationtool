using Microsoft.DataTransfer.Basics.IO;
using System;
using System.Text;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    sealed class RangeSubstitutionReader
    {
        private SimpleStringReader reader;
        private IRangeSubstitutionVisitor visitor;

        private RangeSubstitutionReader(SimpleStringReader reader, IRangeSubstitutionVisitor visitor)
        {
            this.reader = reader;
            this.visitor = visitor;
        }

        public static void Read(SimpleStringReader reader, IRangeSubstitutionVisitor visitor)
        {
            try
            {
                new RangeSubstitutionReader(reader, visitor).Read();
            }
            catch (Exception error)
            {
                throw Errors.FailedToReadSubstituion(error.Message);
            }
        }

        private void Read()
        {
            var constantExpression = new StringBuilder();
            char character;
            while (reader.ReadNext(out character))
            {
                switch (character)
                {
                    case '\\':
                        constantExpression.Append(ReadEscapedCharacter());
                        break;
                    case '[':
                        visitor.VisitConstant(constantExpression.ToString());
                        constantExpression.Clear();
                        visitor.VisitRange(ReadRange());
                        break;
                    default:
                        constantExpression.Append(character);
                        break;
                }
            }

            if (constantExpression.Length > 0)
                visitor.VisitConstant(constantExpression.ToString());
        }

        private char ReadEscapedCharacter()
        {
            char character;
            if (!reader.ReadNext(out character))
                throw UnexpectedCharacter();

            switch (character)
            {
                case '\\':
                case '[':
                case ']':
                    return character;
                default: break;
            }

            throw UnexpectedCharacter();
        }

        private IntegerRange ReadRange()
        {
            int start = 0, end = 0;
            var number = new StringBuilder();

            char character;
            while (reader.ReadNext(out character))
            {
                if (Char.IsDigit(character))
                {
                    number.Append(character);
                    continue;
                }

                if (character == '-')
                {
                    int numberValue;
                    if (!int.TryParse(number.ToString(), out numberValue))
                        throw Errors.NotANumber(number.ToString());
                    start = numberValue;
                    number.Clear();
                    continue;
                }

                if (character == ']')
                {
                    int numberValue;
                    if (!int.TryParse(number.ToString(), out numberValue))
                        throw Errors.NotANumber(number.ToString());
                    end = numberValue;
                    return new IntegerRange(start, end);
                }
            }

            throw UnexpectedCharacter();
        }

        private Exception UnexpectedCharacter()
        {
            char character;
            reader.Read(out character);
            return Errors.UnexpectedCharacter(reader.Position, character);
        }
    }
}
