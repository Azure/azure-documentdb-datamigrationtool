using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    abstract class DocumentDbAdapterConfiguration : ValidatableConfiguration, IDocumentDbAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.ConnectionString);

        public static readonly string ConnectionModePropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.ConnectionMode);

        public static readonly string RetriesPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.Retries);

        public static readonly string RetryIntervalPropertyName =
            ObjectExtensions.MemberName<IDocumentDbAdapterConfiguration>(c => c.RetryInterval);

        private string connectionString;
        private DocumentDbConnectionMode? connectionMode;

        private int? retries;
        private TimeSpan? retryInterval;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }

        public DocumentDbConnectionMode? ConnectionMode
        {
            get { return connectionMode; }
            set { SetProperty(ref connectionMode, value); }
        }

        public int? Retries
        {
            get { return retries; }
            set { SetProperty(ref retries, value, ValidateRetries); }
        }

        public TimeSpan? RetryInterval
        {
            get { return retryInterval; }
            set { SetProperty(ref retryInterval, value, ValidateRetryInterval); }
        }

        public DocumentDbAdapterConfiguration()
        {
            ConnectionMode = Defaults.Current.ConnectionMode;
            Retries = Defaults.Current.NumberOfRetries;
            RetryInterval = Defaults.Current.RetryInterval;
        }

        private static IReadOnlyCollection<string> ValidateRetries(int? value)
        {
            return value >= 0 ? null : new[] { Resources.InvalidNumberOfRetries };
        }

        private static IReadOnlyCollection<string> ValidateRetryInterval(TimeSpan? value)
        {
            return value >= TimeSpan.Zero ? null : new[] { Resources.InvalidRetryInterval };
        }
    }
}
