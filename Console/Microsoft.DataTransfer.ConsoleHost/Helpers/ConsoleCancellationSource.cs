using Microsoft.DataTransfer.Basics;
using System;
using System.Threading;

namespace Microsoft.DataTransfer.ConsoleHost.Helpers
{
    sealed class ConsoleCancellationSource : IDisposable
    {
        private CancellationTokenSource tokenSource;
        private object cleanupLock;

        public CancellationToken Token { get { return tokenSource.Token; } }

        public ConsoleCancellationSource()
        {
            tokenSource = new CancellationTokenSource();
            cleanupLock = new object();

            Console.CancelKeyPress += OnCancelKeyPress;
        }

        public void OnCancelKeyPress(object sender, ConsoleCancelEventArgs eventArgs)
        {
            if (tokenSource == null)
                return;

            lock (cleanupLock)
            {
                if (tokenSource == null)
                    return;

                tokenSource.Cancel();
                eventArgs.Cancel = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "tokenSource",
            Justification = "Disposed through TrashCan helper")]
        public void Dispose()
        {
            lock (cleanupLock)
            {
                TrashCan.Throw(ref tokenSource);
            }
        }
    }
}
