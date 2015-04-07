using System;
using System.Configuration;

namespace Autofac.Configuration
{
    /// <summary>
    /// Represents an autofac components configuration section.
    /// </summary>
    public sealed class ComponentsConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the collection of autofac component configuration elements.
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ComponentConfigurationElementCollection Components
        {
            get { return (ComponentConfigurationElementCollection)base[""]; }
        }
    }
}
