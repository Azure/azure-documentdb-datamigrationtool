using System;
using System.Diagnostics;

namespace Microsoft.DataTransfer.WpfHost.Basics.Commands
{
    /// <summary>
    /// View model command to navigate to the URI. 
    /// </summary>
    public sealed class NavigateUriCommand : CommandBase
    {
        /// <summary>
        /// Determines whether or not the command can navigate to the URI.
        /// </summary>
        /// <param name="parameter">The URI to navigate to.</param>
        /// <returns>true if the command can navigate to the URI; otherwise, false.</returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Navigates to the URI using default browser.
        /// </summary>
        /// <param name="parameter">The URI to navigate to.</param>
        public override void Execute(object parameter)
        {
            Uri uri;
            if (Uri.TryCreate(parameter as string, UriKind.Absolute, out uri))
                Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
    }
}
