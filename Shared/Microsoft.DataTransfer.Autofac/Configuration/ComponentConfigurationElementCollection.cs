using Microsoft.DataTransfer.Basics;
using System.Configuration;

namespace Autofac.Configuration
{
    /// <summary>
    /// Represents a collection of autofac component configuration elements.
    /// </summary>
    [ConfigurationCollection(typeof(ComponentConfigurationElement),
        AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    public sealed class ComponentConfigurationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new instance of autofac component configuration element.
        /// </summary>
        /// <returns>Autofac component configuration element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentConfigurationElement();
        }

        /// <summary>
        /// Gets the element key for autofac component configuration element composed from component name and type.
        /// </summary>
        /// <param name="element">Autofac component configuration element</param>
        /// <returns>An <see cref="System.Object" /> that acts as the key for the specified <see cref="ConfigurationElement" />.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            Guard.NotNull("element", element);

            var component = (ComponentConfigurationElement)element;
            return component.Name + component.TypeString;
        }
    }
}
