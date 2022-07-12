namespace Microsoft.DataTransfer.Interfaces
{
    public interface IDataTransferExtension
    {
        string DisplayName { get; }
        void ReadAsSource();
        void WriteAsSink();
    }
}
