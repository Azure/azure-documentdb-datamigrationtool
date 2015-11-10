﻿using Microsoft.DataTransfer.ServiceModel.Errors;
using System;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    sealed class ErrorHandler : IErrorHandler
    {
        private readonly IHelpHandler helpHandler;
        private readonly IErrorDetailsProvider errorDetailsProvider;

        public ErrorHandler(IHelpHandler helpHandler, IErrorDetailsProvider errorDetailsProvider)
        {
            this.helpHandler = helpHandler;
            this.errorDetailsProvider = errorDetailsProvider;
        }

        public int Handle(Exception error)
        {
            Console.WriteLine();

            Console.Write(Resources.CriticalFailurePrefix);

            if (error is AggregateException)
            {
                foreach (var exception in ((AggregateException)error).Flatten().InnerExceptions)
                    Console.WriteLine(errorDetailsProvider.GetCritical(exception));
            }
            else
            {
                Console.WriteLine(errorDetailsProvider.GetCritical(error));
            }

            Console.WriteLine();
            helpHandler.Print();
            return 1;
        }
    }
}
