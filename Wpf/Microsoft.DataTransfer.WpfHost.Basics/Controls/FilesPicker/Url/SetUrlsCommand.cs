using Microsoft.DataTransfer.Basics;
using Microsoft.DataTransfer.Basics.Extensions;
using Microsoft.DataTransfer.WpfHost.Basics.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls.FilesPicker.Url
{
    sealed class SetUrlsCommand : CommandBase
    {
        private static readonly string UrlsPropertyName = ObjectExtensions.MemberName<IUrlsProvider>(p => p.Urls);
        private static readonly string[] NewLineSplitSeparator = new[] { Environment.NewLine };

        private readonly IUrlsProvider provider;
        private readonly IUrlsListener listener;

        public SetUrlsCommand(IUrlsProvider provider, IUrlsListener listener)
        {
            Guard.NotNull("provider", provider);
            Guard.NotNull("listener", listener);

            this.provider = provider;
            this.listener = listener;

            provider.PropertyChanged += UpdateCanExecute;
        }

        private void UpdateCanExecute(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == UrlsPropertyName)
                RaiseCanExecuteChanged();
        }

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

            listener.Notify(urls.Where(l => UrlHelper.IsValidUrl(l)));
        }

        private IEnumerable<string> GetUrlsFromProvider()
        {
            return String.IsNullOrEmpty(provider.Urls) 
                ? Enumerable.Empty<string>()
                : provider.Urls.Split(NewLineSplitSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
