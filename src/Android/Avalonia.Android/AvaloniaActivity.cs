using Android.App;
using Android.OS;
using Android.Views;

using Avalonia.Rendering;

namespace Avalonia.Android
{
    public abstract class AvaloniaActivity : Activity
    {
        internal AvaloniaView View;
        object _content;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            View = new AvaloniaView(this);
            if (_content != null)
                View.Content = _content;
            SetContentView(View);
            TakeKeyEvents(true);
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (AvaloniaLocator.Current.GetService<IRenderTimer>() is ChoreographerTimer timer)
            {
                timer.OnResume();
            }

            View.Root.Renderer.Start();
        }

        protected override void OnPause()
        {
            base.OnPause();

            View.Root.Renderer.Stop();

            if (AvaloniaLocator.Current.GetService<IRenderTimer>() is ChoreographerTimer timer)
            {
                timer.OnPause();
            }
        }

        public object Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                if (View != null)
                    View.Content = value;
            }
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            return View.DispatchKeyEvent(e);
        }
    }
}
