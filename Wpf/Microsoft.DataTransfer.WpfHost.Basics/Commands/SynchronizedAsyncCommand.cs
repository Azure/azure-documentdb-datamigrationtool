using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// Base synchronized view model command. This command can only be executed once at a time.
    /// </summary>
    public abstract class SynchronizedAsyncCommand : CommandBase
    {
        private int isExecuting;

        /// <summary>
        /// Determines whether the command is already running.
        /// </summary>
        /// <param name="parameter">Parameter is not used.</param>
        /// <returns>true if this command is not running yet; otherwise, false.</returns>
        public override bool CanExecute(object parameter)
        {
            return isExecuting == 0;
        }

        /// <summary>
        /// Executes the action defined by the command.
        /// </summary>
        /// <param name="parameter">Parameter is not used.</param>
        public sealed override void Execute(object parameter)
        {
            if (Interlocked.Exchange(ref isExecuting, 1) == 1)
                return;

            RaiseCanExecuteChanged();

            ExecuteAsync(parameter)
                .ContinueWith(OnCompleted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnCompleted(Task completed)
        {
            Interlocked.Exchange(ref isExecuting, 0);

            RaiseCanExecuteChanged();

            if (completed.IsFaulted)
                HandleError(completed.Exception.InnerException);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>Task that represents asynchronous operation.</returns>
        protected abstract Task ExecuteAsync(object parameter);

        /// <summary>
        /// Defines the method to be called when command invocation results in an error.
        /// </summary>
        /// <param name="error">Invocation error.</param>
        protected virtual void HandleError(Exception error) { }
    }
}
