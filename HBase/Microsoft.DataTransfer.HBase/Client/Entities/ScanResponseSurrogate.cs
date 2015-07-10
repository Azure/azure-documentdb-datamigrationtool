using System.Collections.Generic;

namespace Microsoft.DataTransfer.HBase.Client.Entities
{
    sealed class ScanResponseSurrogate
    {
        public List<HBaseRow> Row { get; set; }
    }
}
