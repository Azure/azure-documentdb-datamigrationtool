using Autofac.Builder;
using Autofac.Core;
using Autofac.LooseNaming;
using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac
{
    /// <summary>
    /// Contains extension methods to enable registration and resolution of loosely named services.
    /// </summary>
    public static class LooseNamingContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the service as typed service with the name.
        /// </summary>
        /// <typeparam name="TService">Type of the service.</typeparam>
        /// <param name="builder">Registration builder.</param>
        /// <param name="name">Name of the service.</param>
        /// <returns>Registration builder to continue the registration.</returns>
        public static IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> LooselyNamed<TService>(
            this IRegistrationBuilder<TService, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder, string name)
        {
            Guard.NotNull("builder", builder);
            return builder.As(new LooselyNamedService(name, typeof(TService)));
        }

        /// <summary>
        /// Resolves all loosely named services.
        /// </summary>
        /// <typeparam name="TService">Type of the service.</typeparam>
        /// <param name="context">Component context to lookup registrations in.</param>
        /// <returns>Collection of all loosely named services.</returns>
        public static Dictionary<string, TService> ResolveAllLooselyNamed<TService>(this IComponentContext context)
        {
            Guard.NotNull("context", context);
            var lookupService = new LooselyNamedService(String.Empty, typeof(TService));
            return context
                .ComponentRegistry
                .RegistrationsFor(lookupService)
                .ToDictionary(
                    r => r.Services.OfType<LooselyNamedService>().First(s => lookupService.Equals(s)).Name,
                    r => (TService)context.ResolveComponent(r, Enumerable.Empty<Parameter>()));
        }
    }
}
