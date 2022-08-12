using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.DummyExtension
{
    [Export(typeof(IDataTransferExtension))]    
    public class DummyExtension : IDataTransferExtension
    {
        public string DisplayName => "Dummy System";
        public async IAsyncEnumerable<IDataItem> ReadAsSourceAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Dummy Extension ReadAsSource Executed");

            yield break;
        }

        public Task WriteAsSinkAsync(IAsyncEnumerable<IDataItem> dataItems, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Dummy Extension WriteAsSink Executed");

            return Task.CompletedTask;
        }

        public Task Configure(IConfiguration configuration)
        {
            return Task.CompletedTask;
        }
    }
}
