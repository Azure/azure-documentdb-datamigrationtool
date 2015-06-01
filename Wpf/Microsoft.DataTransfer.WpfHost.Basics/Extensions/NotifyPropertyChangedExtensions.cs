using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Extensions;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Microsoft.DataTransfer.WpfHost.Basics.Extensions
{
    /// <summary>
    /// Provides a set of static methods to simplify work with <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public static class NotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Subscribes with provided <paramref name="action" /> to the change notifications of the specified property.
        /// </summary>
        /// <typeparam name="TOwner">Type of the property's owner.</typeparam>
        /// <typeparam name="TProperty">Type of the property.</typeparam>
        /// <param name="target">Property's owner.</param>
        /// <param name="propertySelector">Expression that evaluates to the desired property.</param>
        /// <param name="action">Action to subscribe with.</param>
        public static void Subscribe<TOwner, TProperty>(this TOwner target, Expression<Func<TOwner, TProperty>> propertySelector, Action<TProperty> action) 
            where TOwner : class, INotifyPropertyChanged
        {
            Guard.NotNull("target", target);
            Guard.NotNull("propertySelector", propertySelector);
            Guard.NotNull("action", action);

            var memberName = ObjectExtensions.MemberName<TOwner, TProperty>(target, propertySelector);

            var valueSelector = propertySelector.Compile();

            target.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == memberName)
                    action(valueSelector(target));
            };

            action(valueSelector(target));
        }
    }
}
