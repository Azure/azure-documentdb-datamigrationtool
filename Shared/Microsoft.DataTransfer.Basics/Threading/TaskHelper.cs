using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Basics.Threading
{
    /// <summary>
    /// Helper class for working with <see cref="Task" />.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Singleton instance of <see cref="Task" /> that does not perform any operations.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Singleton instance")]
        public static readonly Task NoOp = Task.FromResult<object>(null);
    }
}
