using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker.Url
{
    interface IUrlsListener
    {
        void Notify(IEnumerable<string> urls);
    }
}
