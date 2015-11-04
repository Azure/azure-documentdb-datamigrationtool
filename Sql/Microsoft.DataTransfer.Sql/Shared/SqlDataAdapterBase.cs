using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.SqlServer.Types;
using System;
using System.Data.SqlClient;
using System.Spatial;
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
            InitializeSqlTypesLibrary();
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(Configuration.ConnectionString);
        }

        private static void InitializeSqlTypesLibrary()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        }

        public abstract Task<IDataItem> ReadNextAsync(ReadOutputByRef readOutput, CancellationToken cancellation);

        protected object AsPublicType(object input)
        {
            var spatial = input as SqlGeography;
            if (spatial == null)
                return input;

            var builder = SpatialImplementation.CurrentImplementation.CreateBuilder();
            spatial.Populate(new SystemSpatialGeographySink(builder.GeographyPipeline));
            return builder.ConstructedGeography;
        }

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
