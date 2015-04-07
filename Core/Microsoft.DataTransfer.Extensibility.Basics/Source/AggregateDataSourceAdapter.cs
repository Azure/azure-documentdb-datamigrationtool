using Microsoft.DataTransfer.Basics;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Extensibility.Basics.Source
{
    /// <summary>
    /// Encapsulates multiple data source adapters and aggregates read results.
    /// </summary>
    public sealed class AggregateDataSourceAdapter : IDataSourceAdapter
    {
        private bool started, finished;
        private IEnumerator<IDataSourceAdapter> adapters;

        /// <summary>
        /// Creates a new instance of <see cref="AggregateDataSourceAdapter" />.
        /// </summary>
        /// <param name="adapters">Collection of <see cref="IDataSourceAdapter" /> to encapsulate.</param>
        public AggregateDataSourceAdapter(IEnumerable<IDataSourceAdapter> adapters)
        {
            Guard.NotNull("adapters", adapters);
            this.adapters = adapters.GetEnumerator();
        }

        /// <summary>
        /// Reads one data artifact from the encapsulated data source adapters.
        /// </summary>
        /// <param name="readOutput">Object holding additional information about the data artifact.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Task that represents asynchronous read operation.</returns>
        public async Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation)
        {
            if (!started)
            {
                started = true;

                if (!adapters.MoveNext())
                    finished = true;
            }

            while (!finished)
            {
                Exception readError = null; 

                try
                {
                    var dataItem = await adapters.Current.ReadNextAsync(readOutput, cancellation);
                    if (dataItem != null)
                        return dataItem;
                }
                catch (NonFatalReadException)
                {
                    throw;
                }
                catch (AggregateException aggregateException)
                {
                    readError = aggregateException.Flatten().InnerException;
                }
                catch (Exception exception)
                {
                    readError = exception;
                }

                if (adapters.Current != null)
                    try { adapters.Current.Dispose(); }
                    catch { }

                if (!adapters.MoveNext())
                    finished = true;

                if (readError != null)
                    throw new NonFatalReadException(readError.Message, readError.InnerException);
            }
            return null;
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose()
        {
            TrashCan.Throw(ref adapters, a =>
            {
                do
                {
                    try
                    {
                        if (a.Current != null)
                            a.Current.Dispose();
                    } catch { }
                } while (a.MoveNext());
            });
        }
    }
}
