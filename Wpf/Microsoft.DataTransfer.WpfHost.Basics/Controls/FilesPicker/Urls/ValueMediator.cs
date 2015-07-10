using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls
{
    abstract class ValueMediator<TSource, TTarget> : CommandBase
    {
        private static readonly string ValuePropertyName = ObjectExtensions.MemberName<IValueProvider<TSource>>(p => p.Value);

        private readonly IValueProvider<TSource> provider;
        private readonly IValueListener<TTarget> listener;

        public ValueMediator(IValueProvider<TSource> provider, IValueListener<TTarget> listener)
        {
            Guard.NotNull("provider", provider);
            Guard.NotNull("listener", listener);

            this.provider = provider;
            this.listener = listener;

            provider.PropertyChanged += UpdateCanExecute;
        }

        private void UpdateCanExecute(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ValuePropertyName)
                RaiseCanExecuteChanged();
        }

        protected TSource ReadValue()
        {
            return provider.Value;
        }

        protected void WriteValue(TTarget value)
        {
            listener.Notify(value);
        }
    }
}
