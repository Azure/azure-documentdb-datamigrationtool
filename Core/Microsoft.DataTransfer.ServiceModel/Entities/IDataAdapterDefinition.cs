using System;

namespace Microsoft.DataTransfer.ServiceModel.Entities
{
    /// <summary>
    /// Contains basic information about data adapter. 
    /// </summary>
    public interface IDataAdapterDefinition
    {
        /// <summary>
        /// Gets the display name of data adapter.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the description of data adapter.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the type of data adapter configuration.
        /// </summary>
        Type ConfigurationType { get; }
    }
}
