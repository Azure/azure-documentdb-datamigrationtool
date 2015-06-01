using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.JsonFile.Sink;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics;

namespace Microsoft.DataTransfer.JsonFile.Wpf.Sink
{
    sealed class JsonFileSinkAdapterConfiguration : ValidatableConfiguration, IJsonFileSinkAdapterConfiguration
    {
        public static readonly string FilePropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.File);

        public static readonly string PrettifyPropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.Prettify);

        public static readonly string OverwritePropertyName = 
            ObjectExtensions.MemberName<IJsonFileSinkAdapterConfiguration>(c => c.Overwrite);

        private string file;
        private bool prettify;

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
            get { return true; }
        }
    }
}
