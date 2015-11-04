using Microsoft.DataTransfer.Basics.Collections;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    abstract class DocumentDbAdapterConfiguration<TShared> : ShareableDataAdapterConfigurationBase<TShared>, IDocumentDbAdapterConfiguration
        where TShared : class, ISharedDocumentDbAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.ConnectionString);

        public static readonly string ConnectionModePropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.ConnectionMode);

        public static readonly string RetriesPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.Retries);

        public static readonly string RetryIntervalPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.RetryInterval);

        public string ConnectionString
        {
            get { return SharedConfiguration.ConnectionString; }
            set { SharedConfiguration.ConnectionString = value; }
        }

        public DocumentDbConnectionMode? ConnectionMode
        {
            get { return SharedConfiguration.ConnectionMode; }
            set { SharedConfiguration.ConnectionMode = value; }
        }

        public int? Retries
        {
            get { return SharedConfiguration.Retries; }
            set { SharedConfiguration.Retries = value; }
        }

        public TimeSpan? RetryInterval
        {
            get { return SharedConfiguration.RetryInterval; }
            set { SharedConfiguration.RetryInterval = value; }
        }

        public DocumentDbAdapterConfiguration(TShared sharedConfiguration)
            : base(sharedConfiguration) { }

        protected override Map<string, string> GetSharedPropertiesMapping()
        {
            return new Map<string, string>
            {
                { SharedDocumentDbAdapterConfigurationProperties.ConnectionString, ConnectionStringPropertyName },
                { SharedDocumentDbAdapterConfigurationProperties.ConnectionMode, ConnectionModePropertyName },
                { SharedDocumentDbAdapterConfigurationProperties.Retries, RetriesPropertyName },
                { SharedDocumentDbAdapterConfigurationProperties.RetryInterval, RetryIntervalPropertyName }
            };
        }
    }
}
