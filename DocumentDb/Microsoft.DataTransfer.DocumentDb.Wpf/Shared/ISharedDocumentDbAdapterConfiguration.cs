using Microsoft.DataTransfer.DocumentDb.Shared;
using System;
using System.ComponentModel;

namespace Microsoft.DataTransfer.DocumentDb.Wpf.Shared
{
    interface ISharedDocumentDbAdapterConfiguration : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        string ConnectionString { get; set; }
        DocumentDbConnectionMode? ConnectionMode { get; set; }
        int? Retries { get; set; }
        TimeSpan? RetryInterval { get; set; }
    }
}
