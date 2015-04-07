using System;

namespace Microsoft.DataTransfer.Basics
{
    /// <summary>
    /// Simple helper to validate method arguments.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Validates that provided argument is not null.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="value">Argument value to validate.</param>
        public static void NotNull<T>(string argumentName, T value)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Validates that provided string argument is not null or empty.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="value">Argument value to validate.</param>
        public static void NotEmpty(string argumentName, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(Resources.NonEmptyStringExpected, argumentName);
        }
    }
}
