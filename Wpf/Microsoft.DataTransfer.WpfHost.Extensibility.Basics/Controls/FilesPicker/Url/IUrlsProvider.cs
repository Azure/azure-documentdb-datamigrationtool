using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker.Url
{
    interface IUrlsProvider : INotifyPropertyChanged
    {
        string Urls { get; }
    }
}
