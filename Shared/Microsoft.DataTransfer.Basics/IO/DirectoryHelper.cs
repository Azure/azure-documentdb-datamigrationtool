using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DataTransfer.Basics.IO
{
    /// <summary>
    /// Defines helper methods for working with directories.
    /// </summary>
    public static class DirectoryHelper
    {
        private const string RecursiveSearchPath = "**";

        /// <summary>
        /// Enumerates files matching specified search pattern.
        /// </summary>
        /// <remarks>
        /// Recursive search can be expressed by using special sequence ("**") in the pattern.
        /// </remarks>
        /// <param name="searchPattern">Files search pattern.</param>
        /// <returns>List of files matching the search pattern.</returns>
        public static IEnumerable<string> EnumerateFiles(string searchPattern)
        {
            Guard.NotEmpty("searchPattern", searchPattern);

            var directory = Path.GetDirectoryName(searchPattern);
            var filenamePattern = Path.GetFileName(searchPattern);

            if (directory.EndsWith(RecursiveSearchPath, StringComparison.Ordinal))
                return Directory.EnumerateFiles(
                    directory.Substring(0, directory.Length - RecursiveSearchPath.Length),
                    filenamePattern,
                    SearchOption.AllDirectories);
            else
                return Directory.EnumerateFiles(
                    directory,
                    filenamePattern,
                    SearchOption.TopDirectoryOnly);
        }
    }
}
