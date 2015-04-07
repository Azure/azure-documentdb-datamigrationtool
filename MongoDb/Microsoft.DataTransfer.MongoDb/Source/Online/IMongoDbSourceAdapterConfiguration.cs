using Microsoft.DataTransfer.MongoDb.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.MongoDb.Source.Online
{
    /// <summary>
    /// Contains configuration for MongoDB data source adapter.
    /// </summary>
    public interface IMongoDbSourceAdapterConfiguration : IMongoDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the mongo query document.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "OnlineSource_Query")]
        string Query { get; }

        /// <summary>
        /// Gets the path to the file that contains mongo query document.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "OnlineSource_QueryFile")]
        string QueryFile { get; }

        /// <summary>
        /// Gets the mongo projection document.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "OnlineSource_Projection")]
        string Projection { get; }

        /// <summary>
        /// Gets the path to the file that contains mongo projection document.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "OnlineSource_ProjectionFile")]
        string ProjectionFile { get; }
    }
}
