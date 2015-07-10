using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.DataTransfer.WpfHost.Basics
{
    /// <summary>
    /// Provides basic functionality of <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Updates property value.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Storage of the property value.</param>
        /// <param name="value">New value for the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>true if storage value was changed; otherwise, false.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
        /// <summary>
        /// Notifies all subscribers that property value has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
