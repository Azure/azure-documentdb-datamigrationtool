using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.DataTransfer.DocumentDb.Client.PartitionResolvers.Javascript.Visitors
{
    sealed class DataItemMemberEvaluationVisitor : IJavascriptMemberExpressionVisitor
    {
        private static readonly MethodInfo GetValueMethod = 
            typeof(IDataItem).GetMethod(ObjectExtensions.MemberName<IDataItem>(i => i.GetValue(null)));

        private static readonly MethodInfo OfTypeMethod = 
            new Func<IEnumerable, IEnumerable<object>>(Enumerable.OfType<object>).Method;

        private static readonly MethodInfo SkipMethod = 
            new Func<IEnumerable<object>, int, IEnumerable<object>>(Enumerable.Skip<object>).Method;

        private static readonly MethodInfo FirstMethod = 
            new Func<IEnumerable<object>, object>(Enumerable.First<object>).Method;

        private ParameterExpression dataItem;
        private Expression expression;

        public DataItemMemberEvaluationVisitor()
        {
            dataItem = Expression.Parameter(typeof(IDataItem), "i");
            expression = dataItem;
        }

        public void VisitMember(string name)
        {
            // ((IDataItem)expression).GetValue(name)
            expression = Expression.Call(Expression.Convert(expression, typeof(IDataItem)), GetValueMethod, Expression.Constant(name));
        }

        public void VisitArrayElement(int index)
        {
            // ((IEnumerable)expression).OfType<object>().Skip(index).First()
            expression = Expression.Call(OfTypeMethod, Expression.Convert(expression, typeof(IEnumerable)));
            if (index > 0)
                expression = Expression.Call(SkipMethod,expression, Expression.Constant(index));
            expression = Expression.Call(FirstMethod, expression);
        }

        public Func<IDataItem, object> GetAccessor()
        {
            return Expression.Lambda<Func<IDataItem, object>>(expression, dataItem).Compile();
        }
    }
}
