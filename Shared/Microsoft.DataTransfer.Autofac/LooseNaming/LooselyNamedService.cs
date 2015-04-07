using Autofac.Core;
using Microsoft.DataTransfer.Basics;
using System;

namespace Autofac.LooseNaming
{
    /// <summary>
    /// Lookup key to resolve named services.
    /// </summary>
    public sealed class LooselyNamedService : Service, IServiceWithType, IEquatable<LooselyNamedService>
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets a human-readable description of the service.
        /// </summary>
        public override string Description
        {
            get { return "Loosely named (" + ServiceType.FullName + ")"; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="LooselyNamedService" />.
        /// </summary>
        /// <param name="name">Name of the service.</param>
        /// <param name="serviceType">Type of the service.</param>
        public LooselyNamedService(string name, Type serviceType)
        {
            Guard.NotNull("serviceType", serviceType);

            Name = name;
            ServiceType = serviceType;
        }

        /// <summary>
        /// Return a new service of the same kind, but carrying <paramref name="newType" />
        /// as the <see cref="IServiceWithType.ServiceType" />.
        /// </summary>
        /// <param name="newType">The new service type.</param>
        /// <returns>A new service with the service type.</returns>
        public Service ChangeType(Type newType)
        {
            return new LooselyNamedService(Name, newType);
        }

        /// <summary>
        /// Determines if the service type of the current <see cref="LooselyNamedService" />
        /// is the same as the service type of the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> whose service type is to be compared with
        /// the service type of the current <see cref="LooselyNamedService" />.
        /// </param>
        /// <returns>
        /// True if the service type of <paramref name="obj"/> is the same as
        /// the service type of the current <see cref="LooselyNamedService" />, otherwise False.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as LooselyNamedService);
        }

        /// <summary>
        /// Determines if the service type of the current <see cref="LooselyNamedService" />
        /// is the same as the service type of the specified <see cref="LooselyNamedService" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="LooselyNamedService" /> whose service type is to be compared with
        /// the service type of the current <see cref="LooselyNamedService" />.
        /// </param>
        /// <returns>
        /// True if the service type of <paramref name="other" /> is the same as
        /// the service type of the current <see cref="LooselyNamedService" />, otherwise False.
        /// </returns>
        public bool Equals(LooselyNamedService other)
        {
            return other != null && ServiceType.Equals(other.ServiceType);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return ServiceType.GetHashCode();
        }
    }
}
