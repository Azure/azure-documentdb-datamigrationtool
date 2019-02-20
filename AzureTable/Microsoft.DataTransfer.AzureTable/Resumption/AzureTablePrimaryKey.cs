using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Resumption
{
    /// <summary>
    /// Define the checkpoint for Azure Table data transfer
    /// </summary>
    public class AzureTablePrimaryKey
    {
        /// <summary>
        /// The partition key of the checkpoint
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// The row key of the checkpoint
        /// </summary>
        public string RowKey { get; set; }
    }
}
