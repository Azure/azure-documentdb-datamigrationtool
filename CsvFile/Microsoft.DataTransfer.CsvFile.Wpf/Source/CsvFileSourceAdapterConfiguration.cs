using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.CsvFile.Source;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.CsvFile.Wpf.Source
{
    sealed class CsvFileSourceAdapterConfiguration : ValidatableConfiguration, ICsvFileSourceAdapterConfiguration
    {
        private static readonly string EditableFilesPropertyName =
            ObjectExtensions.MemberName<CsvFileSourceAdapterConfiguration>(c => c.EditableFiles);

        public static readonly string FilesPropertyName =
            ObjectExtensions.MemberName<ICsvFileSourceAdapterConfiguration>(c => c.Files);

        public static readonly string NestingSeparatorPropertyName =
            ObjectExtensions.MemberName<ICsvFileSourceAdapterConfiguration>(c => c.NestingSeparator);

        public static readonly string TrimQuotedPropertyName =
            ObjectExtensions.MemberName<ICsvFileSourceAdapterConfiguration>(c => c.TrimQuoted);

        public static readonly string NoUnquotedNullsPropertyName =
            ObjectExtensions.MemberName<ICsvFileSourceAdapterConfiguration>(c => c.NoUnquotedNulls);

        private ObservableCollection<string> files;
        private string nestingSeparator;
        private bool trimQuoted;
        private bool noUnquotedNulls;

        public IEnumerable<string> Files
        {
            get { return files; }
        }

        public ObservableCollection<string> EditableFiles
        {
            get { return files; }
            private set { SetProperty(ref files, value, ValidateNonEmptyCollection); }
        }

        public string NestingSeparator
        {
            get { return nestingSeparator; }
            set { SetProperty(ref nestingSeparator, value); }
        }

        public bool TrimQuoted
        {
            get { return trimQuoted; }
            set { SetProperty(ref trimQuoted, value); }
        }

        public bool NoUnquotedNulls
        {
            get { return noUnquotedNulls; }
            set { SetProperty(ref noUnquotedNulls, value); }
        }

        public CsvFileSourceAdapterConfiguration()
        {
            EditableFiles = new ObservableCollection<string>();
            EditableFiles.CollectionChanged += OnFilesCollectionChanged;
        }

        private void OnFilesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetErrors(EditableFilesPropertyName, ValidateNonEmptyCollection(files));
        }
    }
}
