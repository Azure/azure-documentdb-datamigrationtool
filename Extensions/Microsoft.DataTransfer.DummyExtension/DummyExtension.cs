using Microsoft.DataTransfer.Interfaces;
using System.ComponentModel.Composition;

namespace Microsoft.DataTransfer.DummyExtension
{
    [Export(typeof(IDataTransferExtension))]    
    public class DummyExtension : IDataTransferExtension
    {
        public string DisplayName => "Dummy System";

        public void ReadAsSource()
        {
            Console.WriteLine("Dummy Extension ReadAsSource Executed");
        }

        public void WriteAsSink()
        {
            Console.WriteLine("Dummy Extension WriteAsSink Executed");
        }
    }
}
