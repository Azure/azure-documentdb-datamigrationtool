using System.ComponentModel;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    interface IUrlsProvider : INotifyPropertyChanged
    {
        string Urls { get; }
    }
}
