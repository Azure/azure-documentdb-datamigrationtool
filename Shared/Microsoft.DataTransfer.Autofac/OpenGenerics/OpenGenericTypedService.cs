using Autofac.Core;
using Microsoft.DataTransfer.Basics;
using System;

namespace Autofac.OpenGenerics
{
    /// <summary>
    /// Lookup key to resolve open generic services.
    /// </summary>
    public class OpenGenericTypedService : Service, IServiceWithType, IEquatable<OpenGenericTypedService>
    {
        private Type openGenericType;

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets a human-readable description of the service.
        /// </summary>
        public override string Description
        {
            get { return "Open generic (" + ServiceType.FullName + ")"; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenGenericTypedService" />.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public OpenGenericTypedService(Type serviceType)
        {
            Guard.NotNull("serviceType", serviceType);

            ServiceType = serviceType;
            openGenericType = GetOpenGenericType(serviceType);
        }

        /// <summary>
        /// Return a new service of the same kind, but carrying <paramref name="newType" />
        /// as the <see cref="IServiceWithType.ServiceType" />.
        /// </summary>
        /// <param name="newType">The new service type.</param>
        /// <returns>A new service with the service type.</returns>
        public virtual Service ChangeType(Type newType)
        {
            return new OpenGenericTypedService(newType);
        }

        private static Type GetOpenGenericType(Type type)
        {
            Guard.NotNull("type", type);

            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();

            if (!type.IsGenericTypeDefinition)
                throw Errors.NonGenericTypeForOpenGeneric(type);

            return type;
        }

        /// <summary>
        /// Determines if the service type of the current <see cref="OpenGenericTypedService" />
        /// is the same generic as the service type of the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> whose service type is to be compared with
        /// the service type of the current <see cref="OpenGenericTypedService" />.
        /// </param>
        /// <returns>
        /// True if the service type of <paramref name="obj"/> is the same generic as
        /// the service type of the current <see cref="OpenGenericTypedService" />, otherwise False.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as OpenGenericTypedService);
        }

        /// <summary>
        /// Determines if the service type of the current <see cref="OpenGenericTypedService" />
        /// is the same generic as the service type of the specified <see cref="OpenGenericTypedService" />.
        /// </summary>
        /// <param name="other">
        /// The <see cref="OpenGenericTypedService" /> whose service type is to be compared with
        /// the service type of the current <see cref="OpenGenericTypedService" />.
        /// </param>
        /// <returns>
        /// True if the service type of <paramref name="other" /> is the same generic as
        /// the service type of the current <see cref="OpenGenericTypedService" />, otherwise False.
        /// </returns>
        public bool Equals(OpenGenericTypedService other)
        {
            return other != null && 
                (ServiceType.Equals(other.ServiceType) || openGenericType.Equals(other.openGenericType));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            // Sorry, but we need to execute actual comparison logic
            return Int32.MinValue;
        }
    }
}
