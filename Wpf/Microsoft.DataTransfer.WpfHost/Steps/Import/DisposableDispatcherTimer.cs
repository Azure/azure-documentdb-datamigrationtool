using Microsoft.DataTransfer.Basics;
using System;
using System.Windows.Threading;

namespace Microsoft.DataTransfer.WpfHost.Steps.Import
{
    sealed class DisposableDispatcherTimer<TData> : IDisposable
    {
        private Action<TData> callback;
        private DispatcherTimer timer;

        public DisposableDispatcherTimer(TimeSpan interval, Action<TData> callback, TData data)
        {
            Guard.NotNull("callback", callback);

            this.callback = callback;

            timer = new DispatcherTimer
            {
                Interval = interval,
                Tag = data
            };
            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            callback((TData)((DispatcherTimer)sender).Tag);
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Tick -= OnTick;
        }
    }
}
