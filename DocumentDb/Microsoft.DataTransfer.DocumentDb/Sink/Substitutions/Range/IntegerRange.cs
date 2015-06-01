
namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    class IntegerRange
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        public IntegerRange(int start, int end)
        {
            if (start > end)
                throw Errors.InvalidRange(start, end);

            Start = start;
            End = end;
        }
    }
}
