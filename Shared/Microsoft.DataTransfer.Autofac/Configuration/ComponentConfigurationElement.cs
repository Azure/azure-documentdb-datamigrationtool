using System;
using System.Collections.Generic;
using System.Configuration;

namespace Autofac.Configuration
{
    /// <summary>
    /// Represents an autofac component configuration element.
    /// </summary>
    public sealed class ComponentConfigurationElement : ConfigurationElement
    {
        private Dictionary<string, string> metadata;

        /// <summary>
        /// Gets a component name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        /// <summary>
        /// Gets component type name.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeString
        {
            get { return (string)base["type"]; }
        }

        /// <summary>
        /// Gets component type.
        /// </summary>
        public Type ComponentType { get; private set; }

        /// <summary>
        /// Gets additional component metadata.
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata
        {
            get { return metadata; }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ComponentConfigurationElement" />.
        /// </summary>
        public ComponentConfigurationElement()
        {
            metadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// Populates <see cref="ComponentConfigurationElement.ComponentType" /> from the <see cref="ComponentConfigurationElement.TypeString" />.
        /// </summary>
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            ComponentType = Type.GetType(TypeString);
            if (ComponentType == null)
                throw Errors.TypeNotFound(TypeString);
        }

        /// <summary>
        /// Adds property to metadata collection.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        /// <returns>true as property is added to metadata collection.</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            metadata.Add(name, value);
            return true;
        }
    }
}
