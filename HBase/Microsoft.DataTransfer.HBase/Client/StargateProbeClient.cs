using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.HBase.Client
{
    /// <summary>
    /// Simple HBase Stargate client to verify the connection.
    /// </summary>
    public sealed class StargateProbeClient
    {
        /// <summary>
        /// Tests the HBase Stargate connection.
        /// </summary>
        /// <param name="connectionString">HBase connection string to use to connect.</param>
        public async Task TestConnectionAsync(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            await StargateClientFactory
                .Create(connectionString)
                .GetClusterVersionAsync(CancellationToken.None);
        }
    }
}
