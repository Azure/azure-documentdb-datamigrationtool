using System;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls
{
    static class UrlHelper
    {
        private readonly static Regex UrlRegex = new Regex("^https?://.+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly static Regex BlobUrlRegex = new Regex("^blobs?://.+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsValidUrl(string url)
        {
            return !String.IsNullOrEmpty(url) && UrlRegex.IsMatch(url);
        }

        public static bool IsValidBlobUrl(string url)
        {
            return !String.IsNullOrEmpty(url) && BlobUrlRegex.IsMatch(url);
        }
    }
}
