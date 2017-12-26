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

        /// <summary>
        /// Gets a value indicating whether this mongo database instance is cosmosdb hosted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this mongo database instance is cosmosdb hosted; otherwise, <c>false</c>.
        /// </value>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Online_IsCosmosDBHosted")]
        bool IsCosmosDBHosted { get; }
    }
}
