using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility
{
    public interface IDataTransferResumptionAdapter<TCheckpoint>
    {
        void SaveCheckpoint(TCheckpoint checkpoint);

        TCheckpoint GetCheckpoint();
    }
}
