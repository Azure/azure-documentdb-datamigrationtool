using Autofac.LooseNaming;

namespace Autofac
{
    /// <summary>
    /// Autofac container builder with custom registration sources.
    /// </summary>
    public class DataTransferContainerBuilder : ContainerBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="DataTransferContainerBuilder" />.
        /// </summary>
        public DataTransferContainerBuilder()
        {
            this.RegisterSource(new LooselyNamedRegistrationSource());
        }
    }
}
