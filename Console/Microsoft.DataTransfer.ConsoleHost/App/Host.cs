using Microsoft.DataTransfer.ConsoleHost.App.Handlers;
using System;

namespace Microsoft.DataTransfer.ConsoleHost.App
{
    sealed class Host
    {
        private readonly ITransferHandler transferHandler;
        private readonly IErrorHandler errorHandler;

        public Host(ITransferHandler transferHandler, IErrorHandler errorHandler)
        {
            this.transferHandler = transferHandler;
            this.errorHandler = errorHandler;
        }

        public int Run()
        {
            try
            {
                var transferResult = transferHandler.RunAsync().Result;

                return transferResult.Failed > 0
                    ? errorHandler.HandleSoftFailure()
                    : 0;
            }
            catch (Exception ex)
            {
                return errorHandler.Handle(ex);
            }
        }
    }
}
