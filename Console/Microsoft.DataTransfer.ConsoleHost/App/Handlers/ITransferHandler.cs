using Microsoft.DataTransfer.ServiceModel.Entities;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.ConsoleHost.App.Handlers
{
    interface ITransferHandler
    {
        Task RunAsync();
    }
}
