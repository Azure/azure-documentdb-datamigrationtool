using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.LooseNaming;
using Autofac.OpenGenerics;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Core.FactoryAdapters;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.Core.Autofac
{
    using AutofacService = global::Autofac.Core.Service;

    sealed class DataAdapterFactoryAdaptersRegistrationSource : IRegistrationSource
    {
        private const string DisplayNameMetadataKey = "displayName";

        public bool IsAdapterForIndividualComponents
        {
            get { return true; }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            AutofacService service,
            Func<AutofacService, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Guard.NotNull("registrationAccessor", registrationAccessor);

            var typedService = service as IServiceWithType;
            if (typedService == null || !typedService.ServiceType
                    .FindInterfaces(TypesHelper.IsOpenGenericType, typeof(IDataAdapterFactoryAdapter<>)).Any())
                return Enumerable.Empty<IComponentRegistration>();

            if (typeof(IDataSourceAdapterFactoryAdapter).Equals(typedService.ServiceType))
            {
                return AdaptFactories(typeof(IDataSourceAdapterFactory<>), typeof(DataSourceAdapterFactoryAdapter<>), typedService, registrationAccessor);
            }
            else if (typeof(IDataSinkAdapterFactoryAdapter).Equals(typedService.ServiceType))
            {
                return AdaptFactories(typeof(IDataSinkAdapterFactory<>), typeof(DataSinkAdapterFactoryAdapter<>), typedService, registrationAccessor);
            }
            else
            {
                return Enumerable.Empty<IComponentRegistration>();
            }
        }

        private static IEnumerable<IComponentRegistration> AdaptFactories(Type from, Type to,
            IServiceWithType requestedService, Func<AutofacService, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Guard.NotNull("from", from);
            Guard.NotNull("to", to);
            Guard.NotNull("requestedService", requestedService);
            Guard.NotNull("registrationAccessor", registrationAccessor);

            var factoryService = new OpenGenericLooselyNamedService(String.Empty, from);

            return registrationAccessor(factoryService)
                .Select(r => 
                    {
                        var targetService = r.Services.OfType<OpenGenericLooselyNamedService>().First(s => factoryService.Equals(s));
                        return new ComponentRegistration(
                            Guid.NewGuid(),
                            new DelegateActivator(
                                requestedService.ServiceType,
                                (c, p) => Activator.CreateInstance(
                                    // Since we looked up factory interfaces only (from argument) - generic argument of s.ServiceType will be the type of configuration
                                    to.MakeGenericType(targetService.ServiceType.GetGenericArguments()[0]),
                                    c.ResolveComponent(r, Enumerable.Empty<Parameter>()), GetDisplayName(r.Metadata, targetService.Name))
                            ),
                            new CurrentScopeLifetime(),
                            InstanceSharing.None,
                            InstanceOwnership.ExternallyOwned,
                            new AutofacService[] { new LooselyNamedService(targetService.Name, requestedService.ServiceType) },
                            new Dictionary<string, object>());
                    });
        }

        private static string GetDisplayName(IDictionary<string, object> metadata, string name)
        {
            if (metadata == null)
                return name;

            object displayName;
            if (metadata.TryGetValue(DisplayNameMetadataKey, out displayName) && displayName != null)
                name = displayName.ToString();

            return name;
        }
    }
}
