using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.AzureTable.Resumption
{
    /// <summary>
    /// 
    /// </summary>
    public class AzureTablePrimaryKey
    {
        /// <summary>
        /// 
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RowKey { get; set; }
    }
}
