using System;
using System.Linq.Expressions;

namespace Microsoft.DataTransfer.Basics.Extensions
{
    /// <summary>
    /// Provides a set of static methods to extend functionality of <see cref="Object" />.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Extracts member name from the provided expression.
        /// </summary>
        /// <typeparam name="T">Type of the member's owner.</typeparam>
        /// <param name="target">Member's owner.</param>
        /// <param name="memberSelector">Expression that evaluates to the desired member.</param>
        /// <returns>Name of the member.</returns>
        public static string MemberName<T>(this T target, Expression<Func<T, object>> memberSelector)
        {
            return MemberName(memberSelector);
        }

        /// <summary>
        /// Extracts member name from the provided expression.
        /// </summary>
        /// <typeparam name="T">Type of the member's owner.</typeparam>
        /// <param name="memberSelector">Expression that evaluates to the desired member.</param>
        /// <returns>Name of the member.</returns>
        public static string MemberName<T>(Expression<Func<T, object>> memberSelector)
        {
            Guard.NotNull("memberSelector", memberSelector);
            return GetMemberName(memberSelector);
        }

        /// <summary>
        /// Extracts member name from the provided expression.
        /// </summary>
        /// <typeparam name="TOwner">Type of the member's owner.</typeparam>
        /// <typeparam name="TMember">Type of the member.</typeparam>
        /// <param name="target">Member's owner.</param>
        /// <param name="memberSelector">Expression that evaluates to the desired member.</param>
        /// <returns>Name of the member.</returns>
        public static string MemberName<TOwner, TMember>(this TOwner target, Expression<Func<TOwner, TMember>> memberSelector)
        {
            Guard.NotNull("memberSelector", memberSelector);
            return GetMemberName(memberSelector);
        }

        private static string GetMemberName(Expression memberSelector)
        {
            var visitor = new FindMemberVisitor();
            visitor.Visit(memberSelector);
            return visitor.LastVisitedMember;
        }

        sealed class FindMemberVisitor : ExpressionVisitor
        {
            public string LastVisitedMember { get; private set; }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node != null)
                    LastVisitedMember = node.Member.Name;
                return base.VisitMember(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node != null)
                    LastVisitedMember = node.Method.Name;
                return base.VisitMethodCall(node);
            }
        }
    }
}
