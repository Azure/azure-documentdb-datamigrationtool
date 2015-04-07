using Autofac;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Shell;
using System;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class ApplicationController : IApplicationController, IDisposable
    {
        private ILifetimeScope rootScope;

        private object mainWindowLock;
        private Window mainWindow;

        private ILifetimeScope currentScope;

        public ApplicationController(ILifetimeScope rootScope)
        {
            Guard.NotNull("rootScope", rootScope);

            this.rootScope = rootScope;
            mainWindowLock = new Object();
        }

        public Window GetMainWindow()
        {
            if (mainWindow == null) lock (mainWindowLock) if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                ResetState();
            }

            return mainWindow;
        }

        public INavigationService ResetState()
        {
            using (var oldScope = currentScope)
            {
                currentScope = rootScope.BeginLifetimeScope();
                mainWindow.DataContext = currentScope.Resolve<IMainWindowViewModel>();
                return currentScope.Resolve<INavigationService>();
            }
        }

        public void Dispose()
        {
            TrashCan.Throw(ref currentScope);
        }
    }
}
