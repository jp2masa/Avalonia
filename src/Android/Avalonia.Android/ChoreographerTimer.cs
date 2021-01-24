using System;

using Android.OS;
using Android.Views;

using Avalonia.Rendering;

using Java.Lang;

namespace Avalonia.Android
{
    internal sealed class ChoreographerTimer : Java.Lang.Object, IRenderTimer, Choreographer.IFrameCallback
    {
        private readonly object _lock = new object();

        private readonly Thread _thread;

        private Choreographer _choreographer;
        private bool _running;

        public ChoreographerTimer()
        {
            _thread = new Thread(Loop);
            _thread.Start();
        }

        public event Action<TimeSpan> Tick;

        internal void OnResume()
        {
            lock (_lock)
            {
                _running = true;
                _choreographer.PostFrameCallback(this);
            }
        }

        internal void OnPause()
        {
            lock (_lock)
            {
                _running = false;
            }
        }

        private void Loop()
        {
            Looper.Prepare();
            _choreographer = Choreographer.Instance;
            Looper.Loop();
        }

        public void DoFrame(long frameTimeNanos)
        {
            Tick?.Invoke(TimeSpan.FromTicks(frameTimeNanos / 100));

            lock (_lock)
            {
                if (_running)
                {
                    Choreographer.Instance.PostFrameCallback(this);
                }
            }
        }
    }
}
