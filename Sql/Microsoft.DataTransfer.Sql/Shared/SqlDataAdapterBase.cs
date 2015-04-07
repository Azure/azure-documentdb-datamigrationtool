using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DataTransfer.Sql.Shared
{
    abstract class SqlDataAdapterBase<TConfiguration> : IDataSourceAdapter
        where TConfiguration : class, ISqlDataAdapterConfiguration
    {
        private Lazy<SqlConnection> connection;

        protected TConfiguration Configuration { get; private set; }

        protected SqlConnection Connection
        {
            get { return connection.Value; }
        }

        public SqlDataAdapterBase(TConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            Configuration = configuration;
            connection = new Lazy<SqlConnection>(CreateConnection);
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(Configuration.ConnectionString);
        }

        public abstract Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation);

        public virtual void Dispose()
        {
            if (connection != null && connection.IsValueCreated && connection.Value != null)
            {
                connection.Value.Dispose();
                connection = null;
            }
        }
    }
}
