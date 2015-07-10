using Microsoft.DataTransfer.Basics;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.DataTransfer.WpfHost.Basics.Controls
{
    /// <summary>
    /// Provides a simple way to create a control bound to a view model.
    /// </summary>
    public abstract class DataBoundUserControl : UserControl
    {
        private INotifyPropertyChanged viewModel;

        private ConcurrentDictionary<DependencyProperty, Counter> notificationsSuppressionMap;

        /// <summary>
        /// Creates a new instance of <see cref="DataBoundUserControl" />.
        /// </summary>
        /// <param name="viewModel">View model instance to which this control should be bound.</param>
        public DataBoundUserControl(INotifyPropertyChanged viewModel)
        {
            Guard.NotNull("viewModel", viewModel);

            notificationsSuppressionMap = new ConcurrentDictionary<DependencyProperty, Counter>();

            this.viewModel = viewModel;
            this.viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <summary>
        /// Gets the current view model instance used by the control.
        /// </summary>
        /// <returns>Instance of the current view model.</returns>
        protected T GetViewModel<T>()
        {
            return (T)viewModel;
        }

        /// <summary>
        /// Sets the local value of a dependency property, specified by its dependency
        /// property identifier.
        /// </summary>
        /// <param name="property">The identifier of the dependency property to set.</param>
        /// <param name="value">The new local value.</param>
        /// <remarks>
        /// This method only prevents <see cref="DataBoundUserControl.OnDependencyPropertyChanged" /> method from firing.
        /// Any events registered as property metadata are still going to fire. To check whether metadata
        /// event handler should be suppressed or not, use <see cref="DataBoundUserControl.ShouldHandleDependencyPropertyChange" />.
        /// </remarks>
        protected void SetValueSuppressHandler(DependencyProperty property, object value)
        {
            Counter counter = null;
            try
            {
                counter = notificationsSuppressionMap.GetOrAdd(property, CreateCounter);
                Interlocked.Increment(ref counter.Value);
                SetValue(property, value);
            }
            finally
            {
                if (counter != null)
                    Interlocked.Decrement(ref counter.Value);
            }
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="FrameworkElement" />
        /// has been updated. The specific dependency property that changed is reported
        /// in the arguments parameter.
        /// </summary>
        /// <param name="e">
        /// The event data that describes the property that changed, as well as old and new values.
        /// </param>
        protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (ShouldHandleDependencyPropertyChange(e.Property))
                OnDependencyPropertyChanged(e);
        }

        /// <summary>
        /// Checks whether dependency property change handler should be executed.
        /// </summary>
        /// <param name="property">The identifier of the dependency property.</param>
        /// <returns>true if dependency property change handler should be executed; otherwise, false.</returns>
        protected bool ShouldHandleDependencyPropertyChange(DependencyProperty property)
        {
            Counter counter;
            return !notificationsSuppressionMap.TryGetValue(property, out counter) ||
                Interlocked.CompareExchange(ref counter.Value, 0, 0) == 0;
        }

        /// <summary>
        /// Handles property changes of the view model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs" /> that contains the event data.</param>
        protected abstract void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Handles dependency property changes.
        /// </summary>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs" /> that contains the event data.</param>
        protected abstract void OnDependencyPropertyChanged(DependencyPropertyChangedEventArgs e);
        
        private static Counter CreateCounter(DependencyProperty property)
        {
            return new Counter();
        }

        private sealed class Counter { public int Value; }
    }
}
