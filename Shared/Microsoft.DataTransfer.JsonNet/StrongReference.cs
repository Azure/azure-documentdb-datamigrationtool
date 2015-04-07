
namespace Microsoft.DataTransfer.JsonNet
{
    /// <summary>
    /// Attributes are not treated as a reference, thus all dependants that are using JsonConverter in the attribute
    /// should call to this explicitly for the dll to be bin-placed.
    /// </summary>
    public static class StrongReference
    {
        /// <summary>
        /// Adds a strong reference.
        /// </summary>
        public static void Add() { }
    }
}
