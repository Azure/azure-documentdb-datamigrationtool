using Autofac;
using Autofac.OpenGenerics;
using Microsoft.DataTransfer.Core.Autofac;
using Microsoft.DataTransfer.Core.FactoryAdapters;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Microsoft.DataTransfer.Core.UnitTests.FactoryAdapters
{
    [TestClass]
    public class DataAdapterFactoryAdaptersRegistrationSourceTests
    {
        [TestMethod]
        public void RegistrationsFor_DataSourceFactoryAdapter_Resolved()
        {
            var builder = new ContainerBuilder();
            
            builder
                .RegisterSource(new DataAdapterFactoryAdaptersRegistrationSource());

            RegisterDataAdapterFactoryMocks<IDataSourceAdapterFactory<ITestAdapterConfiguration>>(builder, 2);
            RegisterDataAdapterFactoryMocks<IDataSinkAdapterFactory<ITestAdapterConfiguration>>(builder, 3);

            var sources = builder.Build().ResolveAllLooselyNamed<IDataSourceAdapterFactoryAdapter>();

            Assert.IsNotNull(sources, TestResources.NullFactoryAdapters);
            Assert.AreEqual(2, sources.Count(), TestResources.InvalidNumberOfFactoryAdaptersResolved);
        }

        [TestMethod]
        public void RegistrationsFor_DataSinkFactoryAdapter_Resolved()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterSource(new DataAdapterFactoryAdaptersRegistrationSource());

            RegisterDataAdapterFactoryMocks<IDataSourceAdapterFactory<ITestAdapterConfiguration>>(builder, 2);
            RegisterDataAdapterFactoryMocks<IDataSinkAdapterFactory<ITestAdapterConfiguration>>(builder, 3);

            var sources = builder.Build().ResolveAllLooselyNamed<IDataSinkAdapterFactoryAdapter>();

            Assert.IsNotNull(sources, TestResources.NullFactoryAdapters);
            Assert.AreEqual(3, sources.Count(), TestResources.InvalidNumberOfFactoryAdaptersResolved);
        }

        private static void RegisterDataAdapterFactoryMocks<T>(ContainerBuilder builder, int count)
            where T : class
        {
            for (var index = 0; index < count; ++index)
                builder
                    .Register(c => new Mock<T>().Object)
                    .As(new OpenGenericLooselyNamedService(typeof(T).Name + index, typeof(T)));
        }
    }
}
