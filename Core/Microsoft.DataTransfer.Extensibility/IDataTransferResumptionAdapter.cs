using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    /// <summary>
    /// The interface for the resumption adapters in data transfer
    /// </summary>
    /// <typeparam name="TCheckpoint">The type of checkpoint that you want to create</typeparam>
    public interface IDataTransferResumptionAdapter<TCheckpoint>
    {
        /// <summary>
        /// Save the resumption checkpoint
        /// </summary>
        /// <param name="checkpoint">The checkpoint that you want to save</param>
        void SaveCheckpoint(TCheckpoint checkpoint);

        /// <summary>
        /// Get the resumption checkpoint
        /// </summary>
        /// <returns>The checkpoint that you saved in the last run of data transfer action</returns>
        TCheckpoint GetCheckpoint();

        /// <summary>
        /// Cleanup the checkpoint file if needed
        /// </summary>
        void DeleteCheckpoint();
    }
}
