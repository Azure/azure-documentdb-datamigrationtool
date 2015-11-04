using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Hides <see cref="Expander" /> content container.
    /// </summary>
    /// <remarks>
    /// To prevent <see cref="Expander" /> height changes when you want to use header alone as
    /// a toggle button - place this control in the <see cref="Expander" />.
    /// </remarks>
    public sealed class CollapseExpanderContentContainer : FrameworkElement
    {
        /// <summary>
        /// Creates a new instance of <see cref="CollapseExpanderContentContainer" />.
        /// </summary>
        public CollapseExpanderContentContainer()
        {
            Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Changes <see cref="UIElement.Visibility" /> of parent ExpandSiteBorder <see cref="Border" /> to <see cref="Visibility.Collapsed" />.
        /// </summary>
        /// <param name="oldParent">
        /// The old parent element. May be null to indicate that the element did not
        /// have a visual parent previously.
        /// </param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            
            var expanderSiteBorder = FindParent<Border>(this, "ExpandSiteBorder");
            if (expanderSiteBorder != null)
                expanderSiteBorder.Visibility = Visibility.Collapsed;
        }

        private static T FindParent<T>(DependencyObject obj, string name)
            where T : FrameworkElement
        {
            if (obj == null)
                return null;

            var element = obj as T;
            if (element != null && element.Name == name)
                return (T)element;

            return FindParent<T>(VisualTreeHelper.GetParent(obj), name);
        }
    }
}
