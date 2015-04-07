using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoDb.Shared
{
    /// <summary>
    /// Contains basic configuration for MongoDB data adapters.
    /// </summary>
    public interface IMongoDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the MongoDB instance.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Online_ConnectionString")]
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the MongoDB collection.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Online_Collection")]
        string Collection { get; }
    }
}
