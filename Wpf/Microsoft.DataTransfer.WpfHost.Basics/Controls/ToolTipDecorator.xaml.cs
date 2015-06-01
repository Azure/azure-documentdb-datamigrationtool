using Microsoft.DataTransfer.Basics.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Adds tooltip button to the target content.
    /// </summary>
    [ContentProperty("DecoratedContent")]
    public partial class ToolTipDecorator : UserControl
    {
        /// <summary>
        /// Identifies the <see cref="ToolTipDecorator.DecoratedContent" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecoratedContentProperty = DependencyProperty.Register(
            ObjectExtensions.MemberName<ToolTipDecorator>(c => c.DecoratedContent), typeof(object), typeof(ToolTipDecorator));

        /// <summary>
        /// Gets or sets the content that should be decorated with tooltip button.
        /// </summary>
        public object DecoratedContent
        {
            get { return (object)GetValue(DecoratedContentProperty); }
            set { SetValue(DecoratedContentProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ToolTipDecorator" />.
        /// </summary>
        public ToolTipDecorator()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
