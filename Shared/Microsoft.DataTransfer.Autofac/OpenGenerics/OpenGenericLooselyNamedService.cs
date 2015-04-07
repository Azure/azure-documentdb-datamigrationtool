using Autofac.Core;
using System;

namespace Autofac.OpenGenerics
{
    /// <summary>
    /// Lookup key to resolve named open generic services.
    /// </summary>
    public sealed class OpenGenericLooselyNamedService : OpenGenericTypedService
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a human-readable description of the service.
        /// </summary>
        public override string Description
        {
            get { return "Loosely named open generic (" + ServiceType.FullName + ")"; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenGenericLooselyNamedService" />.
        /// </summary>
        /// <param name="name">Name of the service.</param>
        /// <param name="type">Type of the service.</param>
        public OpenGenericLooselyNamedService(string name, Type type)
            : base(type)
        {
            Name = name;
        }

        /// <summary>
        /// Return a new service of the same kind, but carrying <paramref name="newType" />
        /// as the <see cref="IServiceWithType.ServiceType" />.
        /// </summary>
        /// <param name="newType">The new service type.</param>
        /// <returns>A new service with the service type.</returns>
        public override Service ChangeType(Type newType)
        {
            return new OpenGenericLooselyNamedService(Name, newType);
        }

        /// <summary>
        /// Determines if the service type of the current <see cref="OpenGenericLooselyNamedService" />
        /// is the same generic as the service type of the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> whose service type is to be compared with
        /// the service type of the current <see cref="OpenGenericLooselyNamedService" />.
        /// </param>
        /// <returns>
        /// True if the service type of <paramref name="obj"/> is the same generic as
        /// the service type of the current <see cref="OpenGenericLooselyNamedService" />, otherwise False.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj as OpenGenericLooselyNamedService);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
