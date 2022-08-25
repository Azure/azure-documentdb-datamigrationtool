using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Microsoft.DataTransfer.DummyExtension
{
    [Export(typeof(IDataSourceExtension))]    
    public class DummyExtension : IDataSourceExtension, IDataSinkExtension
    {
        public string DisplayName => "Dummy System";
        public async IAsyncEnumerable<IDataItem> ReadAsync(IConfiguration config, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Dummy Extension ReadAsSource Executed");

            yield break;
        }

        public Task WriteAsync(IAsyncEnumerable<IDataItem> dataItems, IConfiguration config, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Dummy Extension WriteAsSink Executed");

            return Task.CompletedTask;
        }
    }
}
