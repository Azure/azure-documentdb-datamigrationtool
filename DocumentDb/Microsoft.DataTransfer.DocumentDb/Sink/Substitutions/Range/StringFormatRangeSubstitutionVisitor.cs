using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    sealed class StringFormatRangeSubstitutionVisitor : IRangeSubstitutionVisitor
    {
        private StringBuilder format;
        private List<IntegerRange> ranges;

        public StringFormatRangeSubstitutionVisitor()
        {
            format = new StringBuilder();
            ranges = new List<IntegerRange>();
        }

        public void VisitConstant(string constant)
        {
            format.Append(Regex.Replace(constant, @"[{|}]", @"$0$0"));
        }

        public void VisitRange(IntegerRange range)
        {
            format.AppendFormat(CultureInfo.InvariantCulture, "{{{0}}}", ranges.Count);
            ranges.Add(range);
        }

        public string GetFormat()
        {
            return format.ToString();
        }

        public IEnumerable<IntegerRange> GetRanges()
        {
            return ranges;
        }
    }
}
