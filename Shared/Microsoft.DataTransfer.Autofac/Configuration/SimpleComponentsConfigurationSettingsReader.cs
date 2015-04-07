using Autofac.Core;
using Autofac.LooseNaming;
using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Autofac.Configuration
{
    /// <summary>
    /// Registrar of simple components defined in the application configuration file.
    /// </summary>
    public class SimpleComponentsConfigurationSettingsReader : Module
    {
        private readonly string sectionName;

        /// <summary>
        /// Creates a new instance of <see cref="SimpleComponentsConfigurationSettingsReader" />.
        /// </summary>
        /// <param name="sectionName">Name of the configuration section to read components registration data from.</param>
        public SimpleComponentsConfigurationSettingsReader(string sectionName)
        {
            Guard.NotEmpty("sectionName", sectionName);

            this.sectionName = sectionName;
        }

        /// <summary>
        /// Registers components in the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            var section = ConfigurationManager.GetSection(sectionName);
            if (section == null)
                return;

            RegisterComponents(builder, ((ComponentsConfigurationSection)section).Components.OfType<ComponentConfigurationElement>());
        }

        private void RegisterComponents(ContainerBuilder builder, IEnumerable<ComponentConfigurationElement> components)
        {
            Guard.NotNull("builder", builder);
            Guard.NotNull("components", components);

            foreach (var component in components)
            {
                var services = GetImplementedServices(component);
                if (!services.Any())
                    continue;

                var registration = builder.RegisterType(component.ComponentType).As(services);

                foreach (var property in component.Metadata)
                    registration.WithMetadata(property.Key, property.Value);
            }
        }

        private Service[] GetImplementedServices(ComponentConfigurationElement component)
        {
            Guard.NotNull("component", component);

            return component.ComponentType
                .FindInterfaces(InterfaceFilter, null)
                .Select(t => CreateService(component.Name, t))
                .ToArray();
        }

        /// <summary>
        /// Creates new instance of autofac service definition.
        /// </summary>
        /// <param name="name">Name of the service for named services.</param>
        /// <param name="type">Type of the service.</param>
        /// <returns>Autofac service definition.</returns>
        protected virtual Service CreateService(string name, Type type)
        {
            return String.IsNullOrEmpty(name)
                ? (Service)new TypedService(type)
                : new LooselyNamedService(name, type);
        }

        private bool InterfaceFilter(Type interfaceType, object filterCriteria)
        {
            return IsInterfaceOfInterest(interfaceType);
        }

        /// <summary>
        /// Determines if implementor of provided interface type should be registered in the container.
        /// </summary>
        /// <param name="interfaceType">Type of the implemented interface.</param>
        /// <returns>true if implementor should be registered; otherwise, false.</returns>
        protected virtual bool IsInterfaceOfInterest(Type interfaceType)
        {
            return true;
        }
    }
}
