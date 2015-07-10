using Microsoft.DataTransfer.Basics.Extensions;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile
{
    sealed class SaveFileViewModel : BindableBase
    {
        public static readonly string UseBlobPropertyName =
            ObjectExtensions.MemberName<SaveFileViewModel>(m => m.UseBlob);

        public static readonly string LocalFilePropertyName =
            ObjectExtensions.MemberName<SaveFileViewModel>(m => m.LocalFile);

        public static readonly string BlobUrlPropertyName =
            ObjectExtensions.MemberName<SaveFileViewModel>(m => m.BlobUrl);

        public static readonly string OverwritePropertyName =
            ObjectExtensions.MemberName<SaveFileViewModel>(m => m.Overwrite);

        private bool useBlob;

        private string localFile;
        private string localFileFilter;
        private string localFileDefaultExtension;

        private string blobUrl;
        private bool overwrite;

        public bool UseBlob
        {
            get { return useBlob; }
            set
            {
                if (SetProperty(ref useBlob, value))
                    OnPropertyChanged(OverwritePropertyName);
            }
        }

        public string LocalFile
        {
            get { return localFile; }
            set { SetProperty(ref localFile, value); }
        }

        public string LocalFileFilter
        {
            get { return localFileFilter; }
            set { SetProperty(ref localFileFilter, value); }
        }

        public string LocalFileDefaultExtension
        {
            get { return localFileDefaultExtension; }
            set { SetProperty(ref localFileDefaultExtension, value); }
        }

        public string BlobUrl
        {
            get { return blobUrl; }
            set { SetProperty(ref blobUrl, value); }
        }

        public bool Overwrite
        {
            get { return useBlob ? overwrite : true; }
            set
            {
                if (!SetProperty(ref overwrite, value) && !useBlob)
                    // Enforce refresh, as it may not be in sync with incoming value
                    OnPropertyChanged();
            }
        }
    }
}
