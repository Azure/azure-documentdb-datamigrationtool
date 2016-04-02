using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Basics.ValueConverters
{
    /// <summary>
    /// Encapsulates base type convertion class to provide display names for enum values.
    /// </summary>
    public sealed class EnumDisplayNameValueConverter : EnumDisplayNameValueConverterTypeLimiter<Enum>
    {
        private EnumDisplayNameValueConverter() { }
    }

    /// <summary>
    /// Represents type-limiting wrapper for base type convertion class to provide display names for enum values.
    /// </summary>
    /// <remarks>
    /// Since C# syntax does not allow explicit Enum constraint on the generic argument, inherit from this type-limiting wrapper
    /// to apply Enum constraint to the <typeparamref name="TEnum"/>.
    /// Do not use this class directly, use <see cref="EnumDisplayNameValueConverter" /> instead.
    /// </remarks>
    /// <typeparam name="TEnum">Enum type limit.</typeparam>
    public abstract class EnumDisplayNameValueConverterTypeLimiter<TEnum>
        where TEnum : class
    {
        /// <summary>
        /// Creates a new instance of <see cref="EnumDisplayNameValueConverterTypeLimiter{TEnum}" />.
        /// </summary>
        protected EnumDisplayNameValueConverterTypeLimiter() { }

        /// <summary>
        /// Converts enum values to user-friendly display names based on the provided mapping.
        /// </summary>
        /// <typeparam name="T">Enum type to convert.</typeparam>
        public abstract class Base<T> : DisplayNameValueConverter<T>
            where T : struct, TEnum
        {
            private IDictionary<T, string> knownValues;

            /// <summary>
            /// Creates a new instance of <see cref="EnumDisplayNameValueConverterTypeLimiter{TEnum}.Base{T}" />.
            /// </summary>
            /// <param name="knownValues">Collection of user-friendly display names for known enum values.</param>
            public Base(IDictionary<T, string> knownValues)
            {
                this.knownValues = knownValues;
            }

            /// <summary>
            /// Provides mapping between enum values and their user-friendly representations.
            /// </summary>
            /// <returns>Display names mapping for all enum values.</returns>
            public override IReadOnlyDictionary<T, string> GetDisplayNames()
            {
                var result = new Dictionary<T, string>(knownValues);

                foreach (T value in Enum.GetValues(typeof(T)))
                    if (!result.ContainsKey(value))
                        result.Add(value, ConvertFromUnknownValue(value));

                return result;
            }

            /// <summary>
            /// Retrieves the name of enum value that does not have a known mapping.
            /// </summary>
            /// <param name="value">Enum value without the mapping.</param>
            /// <returns>Name of the enum value.</returns>
            protected override string ConvertFromUnknownValue(T value)
            {
                return Enum.GetName(typeof(T), value);
            }

            /// <summary>
            /// Parses the name of enum value.
            /// </summary>
            /// <param name="displayName">Name of enum value to parse.</param>
            /// <returns>Enum value.</returns>
            protected override T ConvertFromUnknownDisplayName(string displayName)
            {
                T parsed;
                if (Enum.TryParse<T>(displayName, out parsed))
                    return parsed;

                return default(T);
            }
        }
    }
}
