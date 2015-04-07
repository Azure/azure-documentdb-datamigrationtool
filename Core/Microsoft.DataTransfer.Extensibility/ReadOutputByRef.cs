
namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// Container for additional information of data read operation.
    /// </summary>
    /// <remarks>
    /// Since passing variables by reference is not supported with async operations, this container class is used to obtain additional information.
    /// </remarks>
    public sealed class ReadOutputByRef
    {
        /// <summary>
        /// Singleton instance to use where actual value does not matter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Singleton instance to use where actual value does not matter")]
        public static readonly ReadOutputByRef None = new ReadOutputByRef();

        /// <summary>
        /// Gets or sets data artifact identifier.
        /// </summary>
        public string DataItemId { get; set; }

        /// <summary>
        /// Removes all values from the current container.
        /// </summary>
        public void Wipe()
        {
            DataItemId = null;
        }
    }
}
