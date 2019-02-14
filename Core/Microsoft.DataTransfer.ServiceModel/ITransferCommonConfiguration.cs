using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ServiceModel
{
    /// <summary>
    /// The common configuration for data transfer
    /// </summary>
    public interface ITransferCommonConfiguration
    {
        /// <summary>
        /// Whether to allow saving or resuming from a checkpoint
        /// </summary>
        [Display(ResourceType = typeof(ConfigurationResources), Description = "Statistics_EnableResumeFunction")]
        bool EnableResumeFunction { get; }
    }
}
