using System;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class ErrorHandler : IErrorHandler
    {
        private readonly IHelpHandler helpHandler;

        public ErrorHandler(IHelpHandler helpHandler)
        {
            this.helpHandler = helpHandler;
        }

        public int Handle(Exception error)
        {
            if (Console.CursorLeft != 0)
                Console.WriteLine();

            Console.Write(Resources.CriticalFailurePrefix);

            if (error is AggregateException)
            {
                foreach (var exception in ((AggregateException)error).Flatten().InnerExceptions)
                    Console.WriteLine(exception.Message);
            }
            else
            {
                Console.WriteLine(error.Message);
            }

            Console.WriteLine();
            helpHandler.Print();
            return 1;
        }
    }
}
