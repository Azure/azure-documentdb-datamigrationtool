﻿namespace Microsoft.DataTransfer.AzureTable.Utils
{
    using Microsoft.Azure.Storage;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Execute an operation with retries
        /// </summary>
        /// <param name="func">Function to execute</param>
        /// <param name="retries">Number of retries</param>
        public static async Task ExecuteWithRetryAsync(Func<Task> func, int retries=3)
        {
            while (retries > 0)
            {
                try
                {
                    await func();
                    break;
                }
                catch (StorageException ex)
                {
                    if (ex.RequestInformation.HttpStatusCode == 429)
                    {
                        retries--;
                        if(retries == 0)
                        {
                            throw;
                        }
                        await Task.Delay(3000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
