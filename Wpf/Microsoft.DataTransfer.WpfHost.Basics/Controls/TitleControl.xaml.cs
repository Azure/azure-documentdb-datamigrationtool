using Microsoft.DataTransfer.Basics.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Displays the title text with bottom border.
    /// </summary>
    public partial class TitleControl : UserControl
    {
        private static readonly DependencyProperty FormattedTextProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<TitleControl>(c => c.FormattedText), typeof(string), typeof(TitleControl));

        /// <summary>
        /// Identifies the <see cref="TitleControl.Text" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<TitleControl>(c => c.Text),
            typeof(string), typeof(TitleControl),
            new FrameworkPropertyMetadata(OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="TitleControl.TextFormat" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<TitleControl>(c => c.TextFormat),
            typeof(string), typeof(TitleControl),
            new FrameworkPropertyMetadata(OnPropertyChanged));

        private string FormattedText
        {
            get { return (string)GetValue(FormattedTextProperty); }
            set { SetValue(FormattedTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format string for the title text.
        /// </summary>
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="TitleControl" />.
        /// </summary>
        public TitleControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TitleControl;
            if (self == null)
                return;

            self.FormattedText = String.IsNullOrEmpty(self.TextFormat) ? self.Text : String.Format(CultureInfo.CurrentUICulture, self.TextFormat, self.Text);
        }
    }
}
