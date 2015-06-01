using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    interface IUrlsListener
    {
        void Notify(IEnumerable<string> urls);
    }
}
