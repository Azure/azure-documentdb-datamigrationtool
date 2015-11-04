
namespace Microsoft.DataTransfer.DocumentDb.Client
{
    sealed class StoredProcedureResult<T>
    {
        public string ActivityId { get; private set; }
        public T Data { get; private set; }

        public StoredProcedureResult(string activityId, T data)
        {
            ActivityId = activityId;
            Data = data;
        }
    }
}
