using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.DataTransfer.Basics
{
    /// <summary>
    /// Base class for errors provider.
    /// </summary>
    public class CommonErrors
    {
        /// <summary>
        /// Creates a new instance of <see cref="CommonErrors" />.
        /// </summary>
        protected CommonErrors() { }

        /// <summary>
        /// Creates a new instance of <see cref="KeyNotFoundException" /> to be thrown in cases when 
        /// data artifact does not contain requested field.
        /// </summary>
        /// <param name="name">Name of the requested field.</param>
        /// <returns>New instance of <see cref="KeyNotFoundException" /> representing the error.</returns>
        public static Exception DataItemFieldNotFound(string name)
        {
            return new KeyNotFoundException(FormatMessage(Resources.DataItemFieldNotFoundFormat, name));
        }

        /// <summary>
        /// Formats the message using <see cref="CultureInfo.InvariantCulture" /> culture.
        /// </summary>
        /// <param name="format">Message format.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>Formatted message.</returns>
        protected static string FormatMessage(string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
