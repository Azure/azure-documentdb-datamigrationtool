using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls
{
    interface IValueProvider<T> : INotifyPropertyChanged
    {
        T Value { get; }
    }
}
