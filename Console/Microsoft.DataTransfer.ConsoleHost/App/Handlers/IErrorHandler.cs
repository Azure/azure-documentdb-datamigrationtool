using System;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface IErrorHandler
    {
        int Handle(Exception error);
        int HandleSoftFailure();
    }
}
