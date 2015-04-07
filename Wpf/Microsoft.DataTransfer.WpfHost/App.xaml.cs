using Autofac;
using Microsoft.DataTransfer.WpfHost.ServiceModel;
using Microsoft.DataTransfer.WpfHost.Shell;
using System.Windows;

namespace Microsoft.DataTransfer.WpfHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var builder = new DataTransferContainerBuilder();

            builder.RegisterModule<DefaultRuntimeEnvironment>();

            builder.Build().Resolve<IApplicationController>().GetMainWindow().Show();
        }
    }
}
