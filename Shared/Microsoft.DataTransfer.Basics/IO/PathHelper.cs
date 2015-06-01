using System;
using System.IO;

namespace Microsoft.DataTransfer.Basics.IO
{
    /// <summary>
    /// Helper class for working with paths.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Appends path segment to the base path.
        /// </summary>
        /// <param name="basePath">Base path.</param>
        /// <param name="segment">Segment of the path to append.</param>
        /// <returns>Single path string representing combined path.</returns>
        public static string Combine(string basePath, string segment)
        {
            if (!String.IsNullOrEmpty(basePath) && basePath[basePath.Length - 1] != Path.DirectorySeparatorChar)
                basePath = basePath + Path.DirectorySeparatorChar;

            return Path.Combine(basePath, segment);
        }
    }
}
