
namespace Microsoft.DataTransfer.DocumentDb.Sink.Bulk
{
    sealed class BulkInsertItemState
    {
        public int DocumentIndex { get; set; }
        public string ErrorMessage { get; set; }
    }
}
