using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.Basics.Net;
using System;
using System.Collections.Generic;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.Shared
{
    abstract class BlobUrlViewModelBase : ValidatableBindableBase
    {
        private static readonly string ContainerUrlPropertyName =
            ObjectExtensions.MemberName<BlobUrlViewModelBase>(m => m.ContainerUrl);

        private static readonly string AccountKeyPropertyName =
            ObjectExtensions.MemberName<BlobUrlViewModelBase>(m => m.AccountKey);

        private static readonly string BlobNamePropertyName =
            ObjectExtensions.MemberName<BlobUrlViewModelBase>(m => m.BlobName);

        private string containerUrl;
        private string accountKey;
        private string blobName;

        public string ContainerUrl
        {
            get { return containerUrl; }
            set
            {
                if (SetProperty(ref containerUrl, value, ValidateContainerUrl))
                    UpdateBlobUrl();
            }
        }

        public string AccountKey
        {
            get { return accountKey; }
            set
            {
                if (SetProperty(ref accountKey, value, ValidateNonEmptyString))
                    UpdateBlobUrl();
            }
        }

        public string BlobName
        {
            get { return blobName; }
            set
            {
                if (SetProperty(ref blobName, value, ValidateNonEmptyString))
                    UpdateBlobUrl();
            }
        }

        public BlobUrlViewModelBase()
        {
            Validate();
        }

        public sealed override void Validate()
        {
            base.Validate();
        }

        protected void PopulateFromBlobUrl(string url)
        {
            BlobUri blobUri;
            if (BlobUri.TryParse(url, out blobUri))
            {
                SetProperty(ref containerUrl, blobUri.ContainerUri.ToString(), ValidateContainerUrl, ContainerUrlPropertyName);
                SetProperty(ref accountKey, blobUri.AccountKey, ValidateNonEmptyString, AccountKeyPropertyName);
                SetProperty(ref blobName, blobUri.BlobName, ValidateNonEmptyString, BlobNamePropertyName);
                SetBlobUrl(url);
            }
        }

        protected abstract void SetBlobUrl(string url);

        private void UpdateBlobUrl()
        {
            if (!IsValidContainerUrl(containerUrl) || String.IsNullOrEmpty(accountKey) || String.IsNullOrEmpty(blobName))
            {
                SetBlobUrl(null);
                return;
            }

            var builder = new UriBuilder(containerUrl);
            builder.Scheme = builder.Scheme.Replace(Uri.UriSchemeHttp, "blob");
            builder.UserName = accountKey;
            builder.Path += builder.Path.EndsWith("/", StringComparison.Ordinal) ? blobName : "/" + blobName;
            SetBlobUrl(builder.ToString());
        }

        private IReadOnlyCollection<string> ValidateContainerUrl(string url)
        {
            return IsValidContainerUrl(url) ? null : new[] { Resources.InvalidStorageContainerUrl };
        }

        private bool IsValidContainerUrl(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) &&
                uri.Scheme.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) &&
                uri.AbsolutePath.Length > 1; // Check if container name present
        }
    }
}
