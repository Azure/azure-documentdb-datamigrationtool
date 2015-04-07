using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac.LooseNaming
{
    sealed class LooselyNamedRegistrationSource : IRegistrationSource
    {
        public static readonly MethodInfo ResolveAllLooselyNamedMethod =
            typeof(LooselyNamedRegistrationSource).GetMethod("ResolveAllLooselyNamedWrapper", BindingFlags.Static | BindingFlags.NonPublic);

        public bool IsAdapterForIndividualComponents
        {
            get { return false; }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Guard.NotNull("registrationAccessor", registrationAccessor);

            var typedService = service as IServiceWithType;
            if (typedService == null || !IsReadOnlyStringDictionary(typedService.ServiceType))
                return Enumerable.Empty<IComponentRegistration>();

            return new IComponentRegistration[]
			{
				new ComponentRegistration(
                    Guid.NewGuid(), 
                    new DelegateActivator(
                        typedService.ServiceType, 
                        (c, p) => ResolveAllLooselyNamedMethod
                            // Make generic by requested element type
                            .MakeGenericMethod(typedService.ServiceType.GetGenericArguments()[1])
                            .Invoke(null, new[] { c })),
                    new CurrentScopeLifetime(),
                    InstanceSharing.None,
                    InstanceOwnership.ExternallyOwned,
                    new Service[] { service },
                    new Dictionary<string, object>())
			};
        }

        private static IReadOnlyDictionary<string, TService> ResolveAllLooselyNamedWrapper<TService>(IComponentContext componentContext)
        {
            Guard.NotNull("componentContext", componentContext);
            return componentContext.ResolveAllLooselyNamed<TService>();
        }

        private static bool IsReadOnlyStringDictionary(Type type)
        {
            Guard.NotNull("type", type);

            return type.IsGenericType &&
                type.GetGenericTypeDefinition().Equals(typeof(IReadOnlyDictionary<,>)) &&
                type.GetGenericArguments()[0].Equals(typeof(string));
        }
    }
}
