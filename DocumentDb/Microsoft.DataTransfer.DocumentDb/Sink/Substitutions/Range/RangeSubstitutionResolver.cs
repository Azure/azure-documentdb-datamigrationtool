using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    sealed class RangeSubstitutionResolver : ISubstitutionResolver
    {
        public IEnumerable<string> Resolve(string pattern)
        {
            var visitor = new StringFormatRangeSubstitutionVisitor();
            new RangeSubstitution(pattern).Accept(visitor);

            var nameFormat = visitor.GetFormat();
            var ranges = visitor.GetRanges().Select(r => new IterableIntegerRange(r)).ToArray();

            if (!ranges.Any())
                return new[] { nameFormat };

            var result = new List<string>();
            var currentRangeIndex = ranges.Length - 1;
            do
            {
                result.Add(String.Format(CultureInfo.InvariantCulture, nameFormat,
                    ranges.Select(r => r.Current).OfType<object>().ToArray()));
                IncrementRanges(ranges, currentRangeIndex);
            } while (ranges[0].Current <= ranges[0].End);

            return result;
        }

        private void IncrementRanges(IterableIntegerRange[] ranges, int currentRangeIndex)
        {
            var index = currentRangeIndex;
            for (; index > 0; --index)
            {
                var currentRange = ranges[index];
                if (currentRange.Current < currentRange.End)
                    break;

                currentRange.Current = currentRange.Start;
            }

            ++ranges[index].Current;
        }
    }
}
