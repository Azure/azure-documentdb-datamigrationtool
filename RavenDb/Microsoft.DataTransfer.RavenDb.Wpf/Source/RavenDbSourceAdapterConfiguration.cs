using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.DataTransfer.RavenDb.Source;
using Microsoft.DataTransfer.RavenDb.Wpf.Shared;
using Microsoft.DataTransfer.WpfHost.Basics.Extensions;

namespace Microsoft.DataTransfer.RavenDb.Wpf.Source
{
    sealed class RavenDbSourceAdapterConfiguration : RavenDbDataAdapterConfiguration, IRavenDbSourceAdapterConfiguration
    {
        public static readonly string CollectionsPropertyName =
            ObjectExtensions.MemberName<IRavenDbSourceAdapterConfiguration>(c => c.Collections);

        private ObservableCollection<string> _collections;

        public IEnumerable<string> Collections
        {
            get { return _collections; }
        }

        public string EditableCollections
        {
            get { return string.Join(Environment.NewLine, _collections); }
            private set
            {
                var strings = new ObservableCollection<string>(value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
                SetProperty(ref _collections, strings, ValidateNonEmptyCollection);
            }
        }

        public RavenDbSourceAdapterConfiguration()
        {
            EditableCollections = "";
        }
    }
}
