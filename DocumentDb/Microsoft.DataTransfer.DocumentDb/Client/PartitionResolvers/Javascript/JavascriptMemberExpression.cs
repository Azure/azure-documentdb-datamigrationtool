using Microsoft.DataTransfer.Basics.IO;
using Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript
{
    sealed class JavascriptMemberExpression
    {
        private string expression;

        public JavascriptMemberExpression(string expression)
        {
            this.expression = expression;
        }

        public void Accept(IJavascriptMemberExpressionVisitor visitor)
        {
            JavascriptMemberExpressionReader.Read(new SimpleStringReader(expression), visitor);
        }
    }
}
