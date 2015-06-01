
namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    sealed class IterableIntegerRange : IntegerRange
    {
        public int Current;

        public IterableIntegerRange(IntegerRange range)
            : base(range.Start, range.End)
        {
            Current = range.Start;
        }
    }
}
