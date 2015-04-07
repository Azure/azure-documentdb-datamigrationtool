using Autofac.Core;
using Autofac.OpenGenerics;
using Microsoft.DataTransfer.Basics;
using System;

namespace Autofac.Configuration
{
    /// <summary>
    /// Registrar of the components defined in the application configuration file limited by specific type.
    /// </summary>
    public class TypeLimitedComponentsConfigurationSettingsReader : SimpleComponentsConfigurationSettingsReader
    {
        private Type baseType;

        /// <summary>
        /// Creates a new instance of <see cref="TypeLimitedComponentsConfigurationSettingsReader" />.
        /// </summary>
        /// <param name="sectionName">Name of the configuration section to read components registration data from.</param>
        /// <param name="baseType">Type limit.</param>
        public TypeLimitedComponentsConfigurationSettingsReader(string sectionName, Type baseType)
            : base(sectionName)
        {
            Guard.NotNull("baseType", baseType);

            this.baseType = baseType;
        }

        /// <summary>
        /// Creates new instance of autofac service definition.
        /// </summary>
        /// <param name="name">Name of the service for named services.</param>
        /// <param name="type">Type of the service.</param>
        /// <returns>Autofac service definition.</returns>
        protected override Service CreateService(string name, Type type)
        {
            Guard.NotNull("type", type);

            if (!baseType.IsGenericTypeDefinition || !type.IsGenericType)
                return base.CreateService(name, type);

            // If base is open generic, lets allow this type to be found by open generic definition as well
            return String.IsNullOrEmpty(name)
                ? (Service)new OpenGenericTypedService(type)
                : new OpenGenericLooselyNamedService(name, type);
        }

        /// <summary>
        /// Determines if specified type limit is assignable from provided interface type.
        /// </summary>
        /// <param name="interfaceType">Interface type.</param>
        /// <returns>true if specified type limit is assignable from provided interface type; otherwise, false.</returns>
        protected override bool IsInterfaceOfInterest(Type interfaceType)
        {
            Guard.NotNull("interfaceType", interfaceType);

            if (baseType.IsGenericTypeDefinition && interfaceType.IsGenericType)
                interfaceType = interfaceType.GetGenericTypeDefinition();
            return baseType.IsAssignableFrom(interfaceType);
        }
    }
}
