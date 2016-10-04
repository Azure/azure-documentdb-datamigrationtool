using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.MongoDb.Source.Mongoexport;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.MongoDb.Wpf.Source.Mongoexport
{
    sealed class MongoexportFileSourceAdapterConfiguration : ValidatableBindableBase, IMongoexportFileSourceAdapterConfiguration
    {
        private static readonly string EditableFilesPropertyName =
            ObjectExtensions.MemberName<MongoexportFileSourceAdapterConfiguration>(c => c.EditableFiles);

        public static readonly string FilesPropertyName =
            ObjectExtensions.MemberName<IMongoexportFileSourceAdapterConfiguration>(c => c.Files);

        public static readonly string DecompressPropertyName =
            ObjectExtensions.MemberName<IMongoexportFileSourceAdapterConfiguration>(c => c.Decompress);

        private ObservableCollection<string> files;
        private bool decompress;

        public IEnumerable<string> Files
        {
            get { return files; }
        }

        public ObservableCollection<string> EditableFiles
        {
            get { return files; }
            private set { SetProperty(ref files, value, ValidateNonEmptyCollection); }
        }

        public bool Decompress
        {
            get { return decompress; }
            set { SetProperty(ref decompress, value); }
        }

        public MongoexportFileSourceAdapterConfiguration()
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
