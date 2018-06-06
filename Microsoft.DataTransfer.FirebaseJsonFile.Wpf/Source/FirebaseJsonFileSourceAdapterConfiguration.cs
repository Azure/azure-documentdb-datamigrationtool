using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.FirebaseJsonFile.Source;
using Microsoft.DataTransfer.WpfHost.Basics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Microsoft.DataTransfer.FirebaseJsonFile.Wpf.Source
{
    class FirebaseJsonFileSourceAdapterConfiguration : ValidatableBindableBase, IFirebaseJsonFileSourceAdapterConfiguration
    {
        private static readonly string EditableFilesPropertyName =
            ObjectExtensions.MemberName<FirebaseJsonFileSourceAdapterConfiguration>(c => c.EditableFiles);

        public static readonly string FilesPropertyName =
            ObjectExtensions.MemberName<IFirebaseJsonFileSourceAdapterConfiguration>(c => c.Files);

        public static readonly string DecompressPropertyName =
            ObjectExtensions.MemberName<IFirebaseJsonFileSourceAdapterConfiguration>(c => c.Decompress);

        public static readonly string NodePropertyName =
            ObjectExtensions.MemberName<IFirebaseJsonFileSourceAdapterConfiguration>(c => c.Node);

        public static readonly string IdFieldPropertyName =
            ObjectExtensions.MemberName<IFirebaseJsonFileSourceAdapterConfiguration>(c => c.IdField);
        
        private ObservableCollection<string> files;
        private bool decompress;
        private string node;
        private string idField;

        public IEnumerable<string> Files => files;

        public ObservableCollection<string> EditableFiles
        {
            get { return files; }
            private set { SetProperty(ref files, value, ValidateNonEmptyCollection); }
        }

        public bool Decompress
        {
            get => decompress;
            set { SetProperty(ref decompress, value); }
        }

        public string Node
        {
            get => node;
            set { SetProperty(ref node, value); }
        }

        public string IdField
        {
            get => idField;
            set { SetProperty(ref idField, value); }
        }
        
        public FirebaseJsonFileSourceAdapterConfiguration()
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
