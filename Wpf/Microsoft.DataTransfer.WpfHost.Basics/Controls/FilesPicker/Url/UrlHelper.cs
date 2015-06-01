using System;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    static class UrlHelper
    {
        private readonly static Regex UrlRegex = new Regex("^https?://.+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsValidUrl(string url)
        {
            return !String.IsNullOrEmpty(url) && UrlRegex.IsMatch(url);
        }
    }
}
