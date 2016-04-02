using Microsoft.DataTransfer.DocumentDb.Sink;
using Microsoft.DataTransfer.DocumentDb.Wpf.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Sink
{
    sealed class SharedDocumentDbSinkAdapterConfiguration : SharedDocumentDbAdapterConfiguration, ISharedDocumentDbSinkAdapterConfiguration
    {
        private bool useIndexingPolicyFile;
        private string indexingPolicy;
        private string indexingPolicyFile;

        private string idField;
        private bool disableIdGeneration;
        private bool updateExisting;

        private DateTimeHandling? dates;

        public bool UseIndexingPolicyFile
        {
            get { return useIndexingPolicyFile; }
            set { SetProperty(ref useIndexingPolicyFile, value); }
        }

        public string IndexingPolicy
        {
            get { return useIndexingPolicyFile ? null : indexingPolicy; }
            set { SetProperty(ref indexingPolicy, value); }
        }

        public string IndexingPolicyFile
        {
            get { return useIndexingPolicyFile ? indexingPolicyFile : null; }
            set { SetProperty(ref indexingPolicyFile, value); }
        }

        public string IdField
        {
            get { return idField; }
            set { SetProperty(ref idField, value); }
        }

        public bool DisableIdGeneration
        {
            get { return disableIdGeneration; }
            set { SetProperty(ref disableIdGeneration, value); }
        }

        public bool UpdateExisting
        {
            get { return updateExisting; }
            set { SetProperty(ref updateExisting, value); }
        }

        public DateTimeHandling? Dates
        {
            get { return dates; }
            set { SetProperty(ref dates, value); }
        }

        public SharedDocumentDbSinkAdapterConfiguration()
        {
            Dates = Defaults.Current.SinkDateTimeHandling;
        }
    }
}
