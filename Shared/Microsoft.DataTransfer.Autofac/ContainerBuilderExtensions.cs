using Autofac.Builder;
using Autofac.Core;
using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac
{
    /// <summary>
    /// Contains extension methods for <see cref="ContainerBuilder" /> to enable complex registrations.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers aggregation decorator (decorator that will broadcast all calls to all decorated instances)
        /// for the services of type <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of the decorated services.</typeparam>
        /// <param name="builder">Container builder.</param>
        /// <param name="decoratorFactory">Factory delegate that can create new instances of the aggregate decorator.</param>
        /// <returns>Registration builder to continue the registration.</returns>
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterAggregationDecorator<TService>(
            this ContainerBuilder builder, Func<IComponentContext, IEnumerable<TService>, TService> decoratorFactory)
        {
            return RegisterDecorator<IEnumerable<TService>, TService>(builder, decoratorFactory);
        }

        /// <summary>
        /// Registers default decorator for the services of type <typeparamref name="TService" />.
        /// </summary>
        /// <typeparam name="TService">Type of the decorated services.</typeparam>
        /// <param name="builder">Container builder.</param>
        /// <param name="decoratorFactory">Factory delegate that can create new instances of the decorator.</param>
        /// <returns>Registration builder to continue the registration.</returns>
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterDecorator<TService>(
            this ContainerBuilder builder, Func<IComponentContext, TService, TService> decoratorFactory)
        {
            return RegisterDecorator<TService, TService>(builder, decoratorFactory);
        }

        private static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterDecorator<TDecorationTarget, TService>(
            ContainerBuilder builder, Func<IComponentContext, TDecorationTarget, TService> decoratorFactory)
        {
            Guard.NotNull("builder", builder);
            Guard.NotNull("decoratorFactory", decoratorFactory);

            var originalKey = Guid.NewGuid();
            var registrationBuilder = RegistrationBuilder.ForDelegate((c, p) => decoratorFactory(c, c.ResolveKeyed<TDecorationTarget>(originalKey))).As<TService>();

            builder.RegisterCallback(cr =>
            {
                Guard.NotNull("componentRegistry", cr);

                var service = new TypedService(typeof(TService));
                var originalRegistrations = cr.RegistrationsFor(service);

                // Register original component as keyed
                foreach (var originalRegistration in originalRegistrations)
                    cr.Register(RegistrationBuilder.CreateRegistration(
                        Guid.NewGuid(),
                        CopyRegistrationData(originalRegistration),
                        originalRegistration.Activator,
                        new[] { new KeyedService(originalKey, typeof(TService)) }));

                // Override default registration with decorator
                RegistrationBuilder.RegisterSingleComponent<TService, SimpleActivatorData, SingleRegistrationStyle>(cr, registrationBuilder);
            });

            return registrationBuilder;
        }

        private static RegistrationData CopyRegistrationData(IComponentRegistration originalRegistration)
        {
            Guard.NotNull("originalRegistration", originalRegistration);

            var result = new RegistrationData(originalRegistration.Services.FirstOrDefault())
            {
                Lifetime = originalRegistration.Lifetime,
                Ownership = originalRegistration.Ownership,
                Sharing = originalRegistration.Sharing
            };

            result.PreparingHandlers.Add((s, e) =>
            {
                var parameters = e.Parameters;
                originalRegistration.RaisePreparing(e.Context, ref parameters);
                if (parameters != e.Parameters)
                    e.Parameters = parameters;
            });
            result.ActivatingHandlers.Add((s, e) =>
            {
                var instance = e.Instance;
                originalRegistration.RaiseActivating(e.Context, e.Parameters, ref instance);
                if (instance != e.Instance)
                    e.ReplaceInstance(instance);
            });
            result.ActivatedHandlers.Add((s, e) => originalRegistration.RaiseActivated(e.Context, e.Parameters, e.Instance));

            foreach (var property in originalRegistration.Metadata)
                result.Metadata.Add(property.Key, property.Value);

            return result;
        }
    }
}
