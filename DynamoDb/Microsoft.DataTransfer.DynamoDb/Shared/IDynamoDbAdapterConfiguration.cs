using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DynamoDb.Shared
{
    /// <summary>
    /// Contains basic configuration for Amazon DynamoDB data adapters.
    /// </summary>
    public interface IDynamoDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the connection string for the Amazon DynamoDB service.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "ConnectionString")]
        string ConnectionString { get; }
    }
}
