using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.DataTransfer.Basics.Net
{
    /// <summary>
    /// Provides an object representation of a BLOB uniform resource identifier (URI)
    /// and easy access to the parts of the URI.
    /// </summary>
    public class BlobUri
    {
        private readonly static Regex BlobAddressRegex =
            new Regex("^blob(?<secure>s)?://((?<accountKey>[^@]*)@)?(?<containerUrl>(?<accountName>[^/.]*)([^/]*/){2})(?<blobName>.*)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets the address of the storage container.
        /// </summary>
        public Uri ContainerUri { get; protected set; }

        /// <summary>
        /// Gets the name of the storage account.
        /// </summary>
        public string AccountName { get; protected set; }

        /// <summary>
        /// Gets the storage account key.
        /// </summary>
        public string AccountKey { get; protected set; }

        /// <summary>
        /// Gets the name of the BLOB.
        /// </summary>
        public string BlobName { get; protected set; }

        /// <summary>
        /// Creates a new instance of <see cref="BlobUri" />.
        /// </summary>
        protected BlobUri() { }

        /// <summary>
        /// Creates a new <see cref="BlobUri" /> using the specified <see cref="string" /> instance.
        /// </summary>
        /// <param name="url">The <see cref="String" /> representing the <see cref="BlobUri" />.</param>
        /// <param name="blobUri">When this method returns, contains the constructed <see cref="BlobUri" />.</param>
        /// <returns>true if the <see cref="BlobUri" /> was successfully created; otherwise, false.</returns>
        public static bool TryParse(string url, out BlobUri blobUri)
        {
            blobUri = null;

            if (String.IsNullOrEmpty(url))
                return false;

            var urlMatch = BlobAddressRegex.Match(url);
            if (!urlMatch.Success)
                return false;

            blobUri = new BlobUri
            {
                ContainerUri = new Uri(String.Format(CultureInfo.InvariantCulture,
                    "http{0}://{1}", urlMatch.Groups["secure"].Value, urlMatch.Groups["containerUrl"].Value)),
                AccountName = urlMatch.Groups["accountName"].Value,
                AccountKey = urlMatch.Groups["accountKey"].Value,
                BlobName = urlMatch.Groups["blobName"].Value
            };

            return true;
        }

        /// <summary>
        /// Validates whether provided <see cref="string" /> represents a valid <see cref="BlobUri" />.
        /// </summary>
        /// <param name="url">The <see cref="string" /> to validate.</param>
        /// <returns>true if provided <see cref="string" /> represents a valid <see cref="BlobUri" />; otherwise, false.</returns>
        public static bool IsValid(string url)
        {
            return !String.IsNullOrEmpty(url) && BlobAddressRegex.IsMatch(url);
        }
    }
}
