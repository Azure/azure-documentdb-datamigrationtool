using Microsoft.Azure.Documents.Client;
using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.DocumentDb.Client;
using Microsoft.DataTransfer.Extensibility;
using Microsoft.DataTransfer.Extensibility.Basics;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.DataTransfer.DocumentDb.Shared
{
    abstract class DocumentDbAdapterFactoryBase : DataAdapterFactoryBase
    {
        protected static void ValidateBaseConfiguration(IDocumentDbAdapterConfiguration configuration)
        {
            Guard.NotNull("configuration", configuration);

            if (String.IsNullOrEmpty(configuration.ConnectionString))
                throw Errors.ConnectionStringMissing();
        }

        protected static DocumentDbClient CreateClient(IDocumentDbAdapterConfiguration configuration, IDataTransferContext context, bool isShardedImport, int? maxConnectionLimit)
        {
            Guard.NotNull("configuration", configuration);

            var connectionSettings = ParseConnectionString(configuration.ConnectionString);
            return new DocumentDbClient(
                CreateRawClient(connectionSettings, configuration.ConnectionMode, context, isShardedImport, maxConnectionLimit, configuration.Retries, configuration.RetryInterval), 
                connectionSettings.Database
            );
        }

        private static DocumentClient CreateRawClient(IDocumentDbConnectionSettings connectionSettings, DocumentDbConnectionMode? connectionMode, IDataTransferContext context,
            bool isShardedImport, int? maxConnectionLimit, int? retries, TimeSpan? retryInterval)
        {
            Guard.NotNull("connectionSettings", connectionSettings);

            return new DocumentClient(
                new Uri(connectionSettings.AccountEndpoint),
                connectionSettings.AccountKey,
                CreateConnectionPolicy(connectionMode, context, isShardedImport, maxConnectionLimit, retries, retryInterval)
            );
        }

        private static ConnectionPolicy CreateConnectionPolicy(DocumentDbConnectionMode? connectionMode, IDataTransferContext context, bool isShardedImport, int? maxConnectionLimit, int? retries, TimeSpan? retryInterval)
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            var connectionPolicy =
                new ConnectionPolicy
                {
                    UserAgentSuffix = String.Format(CultureInfo.InvariantCulture, Resources.CustomUserAgentSuffixFormat,
                        entryAssembly == null ? Resources.UnknownEntryAssembly : entryAssembly.GetName().Name,
                        Assembly.GetExecutingAssembly().GetName().Version,
                        context.SourceName, context.SinkName,
                        isShardedImport ? Resources.ShardedImportDesignator : String.Empty)
                };

            RetryOptions retryOptions = new RetryOptions();
            if (retries.HasValue)
                retryOptions.MaxRetryAttemptsOnThrottledRequests = retries.Value;

            if (retryInterval.HasValue)
                retryOptions.MaxRetryWaitTimeInSeconds = retryInterval.Value.Seconds;

            connectionPolicy.RetryOptions = retryOptions;

            if (maxConnectionLimit.HasValue)
                connectionPolicy.MaxConnectionLimit = maxConnectionLimit.Value;


            return DocumentDbClientHelper.ApplyConnectionMode(connectionPolicy, connectionMode);
        }

        private static IDocumentDbConnectionSettings ParseConnectionString(string connectionString)
        {
            var connectionSettings = DocumentDbConnectionStringBuilder.Parse(connectionString);

            if (String.IsNullOrEmpty(connectionSettings.AccountEndpoint))
                throw Errors.AccountEndpointMissing();

            if (String.IsNullOrEmpty(connectionSettings.AccountKey))
                throw Errors.AccountKeyMissing();

            if (String.IsNullOrEmpty(connectionSettings.Database))
                throw Errors.DatabaseNameMissing();

            return connectionSettings;
        }

        protected static int GetValueOrDefault(int? value, int defaultValue, Func<Exception> errorProvider)
        {
            return GetValueOrDefault(value, defaultValue, v => v > 0, errorProvider);
        }

        protected static TimeSpan GetValueOrDefault(TimeSpan? value, TimeSpan defaultValue, Func<Exception> errorProvider)
        {
            return GetValueOrDefault(value, defaultValue, v => v >= TimeSpan.Zero, errorProvider);
        }

        protected static T GetValueOrDefault<T>(T? value, T defaultValue)
            where T : struct
        {
            return value.HasValue ? value.Value : defaultValue;
        }

        protected static T GetValueOrDefault<T>(T? value, T defaultValue, Func<T, bool> isValidValue, Func<Exception> errorProvider)
            where T : struct
        {
            if (!value.HasValue)
                return defaultValue;

            if (!isValidValue(value.Value))
                throw errorProvider();

            return value.Value;
        }
    }
}
