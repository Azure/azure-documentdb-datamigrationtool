using Microsoft.DataTransfer.Basics.IO;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    sealed class RangeSubstitution
    {
        private readonly string pattern;

        public RangeSubstitution(string pattern)
        {
            this.pattern = pattern;
        }

        public void Accept(IRangeSubstitutionVisitor visitor)
        {
            RangeSubstitutionReader.Read(new SimpleStringReader(pattern), visitor);
        }
    }
}
