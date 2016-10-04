namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class TransferResult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "Singleton instance to use where empty value is required")]
        public static readonly TransferResult Empty = new TransferResult(0, 0);

        public int Transferred { get; private set; }
        public int Failed { get; private set; }

        public TransferResult(int transferred, int failed)
        {
            this.Transferred = transferred;
            this.Failed = failed;
        }
    }
}
