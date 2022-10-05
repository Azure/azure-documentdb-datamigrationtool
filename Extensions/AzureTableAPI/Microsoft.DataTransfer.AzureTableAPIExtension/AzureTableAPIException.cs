namespace Microsoft.DataTransfer.AzureTableAPIExtension
{
    internal class AzureTableAPIException : Exception
    {
        public AzureTableAPIException(string message) : base(message)
        { }

        public AzureTableAPIException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
