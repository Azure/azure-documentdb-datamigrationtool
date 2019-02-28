﻿using Microsoft.DataTransfer.ServiceModel;
using Microsoft.DataTransfer.ServiceModel.Errors;
using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.ServiceModel.Configuration;
using System;

namespace Microsoft.DataTransfer.WpfHost.Steps.InfrastructureSetup
{
    sealed class InfrastructureConfiguration : BindableBase, IInfrastructureConfiguration
    {
        private string errorLog;
        private ErrorDetails? errorDetails;
        private TimeSpan? progressUpdateInterval;
        private string cosmosTableLogConnectionString;

        public string ErrorLog
        {
            get { return errorLog; }
            set { SetProperty(ref errorLog, value); }
        }

        public bool OverwriteErrorLog
        {
            get { return true; }
        }

        public ErrorDetails? ErrorDetails
        {
            get { return errorDetails; }
            set { SetProperty(ref errorDetails, value); }
        }

        public TimeSpan? ProgressUpdateInterval
        {
            get { return progressUpdateInterval; }
            set { SetProperty(ref progressUpdateInterval, value); }
        }

        public bool EnableCosmosTableLog
        {
            get { return true; }
        }

        public string CosmosTableLogConnectionString
        {
            get { return cosmosTableLogConnectionString; }
            set { SetProperty(ref cosmosTableLogConnectionString, value); }
        }

        public InfrastructureConfiguration()
        {
            ErrorDetails = InfrastructureDefaults.Current.ErrorDetails;
            ProgressUpdateInterval = InfrastructureDefaults.Current.ProgressUpdateInterval;
        }
    }
}
