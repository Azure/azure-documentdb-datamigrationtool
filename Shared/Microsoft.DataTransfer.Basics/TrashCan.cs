using System;
using System.Threading;

namespace Microsoft.DataTransfer.Basics
{
    /// <summary>
    /// Helper class to simplify objects disposing.
    /// </summary>
    public static class TrashCan
    {
        /// <summary>
        /// Disposes provided object instance and removes the reference.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="disposable">Object instance reference.</param>
        public static void Throw<T>(ref T disposable)
            where T : class, IDisposable
        {
            Throw<T>(ref disposable, null);
        }

        /// <summary>
        /// Disposes provided object instance and removes the reference.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="disposable">Object instance reference.</param>
        /// <param name="finalizer">Delegate to perform before the dispose.</param>
        public static void Throw<T>(ref T disposable, Action<T> finalizer)
            where T : class, IDisposable
        {
            var toDispose = Interlocked.Exchange<T>(ref disposable, null);
            if (toDispose != null)
                try {
                    if (finalizer != null)
                        finalizer(toDispose);
                    toDispose.Dispose();
                }
                catch { }
        }
    }
}
