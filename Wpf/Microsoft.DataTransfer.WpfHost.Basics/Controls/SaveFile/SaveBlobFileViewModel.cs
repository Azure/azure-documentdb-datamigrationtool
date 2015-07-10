using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Controls.Shared;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.SaveFile
{
    sealed class SaveBlobFileViewModel : BlobUrlViewModelBase
    {
        public static readonly string BlobUrlPropertyName = 
            ObjectExtensions.MemberName<SaveBlobFileViewModel>(m => m.BlobUrl);

        private string blobUrl;

        public string BlobUrl
        {
            get { return blobUrl; }
            set
            {
                if (SetProperty(ref blobUrl, value))
                    PopulateFromBlobUrl(blobUrl);
            }
        }

        protected override void SetBlobUrl(string url)
        {
            BlobUrl = url;
        }
    }
}
