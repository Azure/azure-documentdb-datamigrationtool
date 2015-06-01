
namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range
{
    interface IRangeSubstitutionVisitor
    {
        void VisitConstant(string constant);
        void VisitRange(IntegerRange range);
    }
}
