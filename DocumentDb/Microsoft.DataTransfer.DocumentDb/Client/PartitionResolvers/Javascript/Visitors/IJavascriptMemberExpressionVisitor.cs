
namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors
{
    interface IJavascriptMemberExpressionVisitor
    {
        void VisitMember(string name);
        void VisitArrayElement(int index);
    }
}
