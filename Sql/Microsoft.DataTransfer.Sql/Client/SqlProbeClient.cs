using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.Client
{
    /// <summary>
    /// Simple SQL client to verify the connection.
    /// </summary>
    public sealed class SqlProbeClient
    {
        /// <summary>
        /// Tests the SQL connection.
        /// </summary>
        /// <param name="connectionString">SQL connection string to use to connect.</param>
        /// <returns>Task that represents asynchronous connection operation.</returns>
        public async Task TestConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw Errors.ConnectionStringMissing();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
            }
        }
    }
}
