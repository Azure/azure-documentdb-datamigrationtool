using Microsoft.DataTransfer.DynamoDb.Shared;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.DataTransfer.DynamoDb.Source
{
    /// <summary>
    /// Contains configuration for Amazon DynamoDB data source adapter.
    /// </summary>
    public interface IDynamoDbSourceAdapterConfiguration : IDynamoDbAdapterConfiguration
    {
        /// <summary>
        /// Gets the request content.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_Request")]
        string Request { get; }

        /// <summary>
        /// Gets the path to the file that contains request content.
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Source_RequestFile")]
        string RequestFile { get; }
    }
}
