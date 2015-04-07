using Autofac;
using Microsoft.DataTransfer.ConsoleHost.App;

namespace Microsoft.DataTransfer.ConsoleHost
{
    class Program
    {
        static int Main(string[] args)
        {
            var builder = new DataTransferContainerBuilder();

            builder.RegisterModule(new DefaultRuntimeEnvironment(args));
            builder
                .RegisterType<Host>()
                .AsSelf()
                .SingleInstance();

            return builder.Build().Resolve<Host>().Run();
        }
    }
}
