using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.JsonFile.Sink;
using Microsoft.DataTransfer.WpfHost.Basics;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Sink
{
    sealed class JsonFileSinkAdapterConfiguration : ValidatableBindableBase, IJsonFileSinkAdapterConfiguration
    {
        public static readonly string FilePropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.File);

        public static readonly string PrettifyPropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.Prettify);

        public static readonly string OverwritePropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.Overwrite);

        public static readonly string CompressPropertyName =
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.Compress);

        private string file;
        private bool prettify;
        private bool overwrite;
        private bool compress;

        public string File
        {
            get { return file; }
            set { SetProperty(ref file, value, ValidateNonEmptyString); }
        }

        public bool Prettify
        {
            get { return prettify; }
            set { SetProperty(ref prettify, value); }
        }

        public bool Overwrite
        {
            get { return overwrite; }
            set { SetProperty(ref overwrite, value); }
        }

        public bool Compress
        {
            get { return compress; }
            set { SetProperty(ref compress, value); }
        }
    }
}
