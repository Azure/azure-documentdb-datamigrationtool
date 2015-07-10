using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Urls.Http
{
    sealed class SetHttpUrlsCommand : ValueMediator<string, IEnumerable<string>>
    {
        private static readonly string[] NewLineSplitSeparator = new[] { Environment.NewLine };

        public SetHttpUrlsCommand(IValueProvider<string> provider, IValueListener<IEnumerable<string>> listener)
            : base(provider, listener) { }

        public override bool CanExecute(object parameter)
        {
            var urls = parameter as IEnumerable<string> ?? GetUrlsFromProvider();

            var hasUrls = false;
            foreach (var url in urls)
            {
                hasUrls = true;
                if (!UrlHelper.IsValidUrl(url))
                    return false;
            }
            return hasUrls;
        }

        public override void Execute(object parameter)
        {
            var urls = GetUrlsFromProvider();

            if (!CanExecute(urls))
                return;

            WriteValue(urls.Where(l => UrlHelper.IsValidUrl(l)));
        }

        private IEnumerable<string> GetUrlsFromProvider()
        {
            var value = ReadValue();
            return String.IsNullOrEmpty(value)
                ? Enumerable.Empty<string>()
                : value.Split(NewLineSplitSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
