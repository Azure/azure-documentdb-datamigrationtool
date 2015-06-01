using System;

namespace Microsoft.DataTransfer.Basics.IO
{
    /// <summary>
    /// Reads characters from a <see cref="String" /> in a streaming fashion.
    /// </summary>
    public sealed class SimpleStringReader
    {
        private readonly string source;

        /// <summary>
        /// Gets the index of current <see cref="Char" />.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="SimpleStringReader" />.
        /// </summary>
        /// <param name="source"><see cref="String" /> to read data from.</param>
        public SimpleStringReader(string source)
        {
            this.source = source;
            Position = -1;
        }

        /// <summary>
        /// Adjusts the position to the next <see cref="Char" /> and reads it.
        /// </summary>
        /// <param name="character">Next <see cref="Char" />.</param>
        /// <returns>true if there is a <see cref="Char" /> available to read; otherwise, false.</returns>
        public bool ReadNext(out char character)
        {
            if (Position < source.Length)
                Position += 1;

            return Read(out character);
        }

        /// <summary>
        /// Reads current <see cref="Char" /> from the source <see cref="String" />.
        /// </summary>
        /// <param name="character">Current <see cref="Char" />.</param>
        /// <returns>true if there is a <see cref="Char" /> available to read; otherwise, false.</returns>
        public bool Read(out char character)
        {
            if (Position < source.Length)
            {
                character = source[Position];
                return true;
            }

            character = default(char);
            return false;
        }
    }
}
