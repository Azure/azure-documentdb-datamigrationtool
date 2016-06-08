﻿using Microsoft.DataTransfer.AzureTable.Shared;
using Microsoft.DataTransfer.AzureTable.Source;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.AzureTable.Wpf.Shared
{
    abstract class AzureTableAdapterConfiguration : ValidatableBindableBase, IAzureTableAdapterConfiguration
    {
        public static readonly string ConnectionStringPropertyName =
            ObjectExtensions.MemberName<IAzureTableAdapterConfiguration>(c => c.ConnectionString);

        public static readonly string LocationModePropertyName =
            ObjectExtensions.MemberName<IAzureTableAdapterConfiguration>(c => c.LocationMode);

        private string connectionString;
        private AzureTableLocationMode? locationMode;

        public string ConnectionString
        {
            get { return connectionString; }
            set { SetProperty(ref connectionString, value, ValidateNonEmptyString); }
        }

        public AzureTableLocationMode? LocationMode
        {
            get { return locationMode; }
            set { SetProperty(ref locationMode, value); }
        }
    }
}
