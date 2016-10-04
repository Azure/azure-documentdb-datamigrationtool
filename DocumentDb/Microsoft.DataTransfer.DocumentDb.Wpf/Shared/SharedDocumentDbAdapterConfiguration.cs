using Microsoft.DataTransfer.DocumentDb.Shared;
using Microsoft.DataTransfer.WpfHost.Basics;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    class SharedDocumentDbAdapterConfiguration : ValidatableBindableBase, ISharedDocumentDbAdapterConfiguration
    {
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
            set { SetProperty(ref retries, value, ValidateNonNegativeInteger); }
        }

        public TimeSpan? RetryInterval
        {
            get { return retryInterval; }
            set { SetProperty(ref retryInterval, value, ValidateRetryInterval); }
        }

        public SharedDocumentDbAdapterConfiguration()
        {
            ConnectionMode = Defaults.Current.ConnectionMode;
            Retries = Defaults.Current.NumberOfRetries;
            RetryInterval = Defaults.Current.RetryInterval;
        }

        private static IReadOnlyCollection<string> ValidateRetryInterval(TimeSpan? value)
        {
            return value >= TimeSpan.Zero ? null : new[] { Resources.InvalidRetryInterval };
        }
    }
}
