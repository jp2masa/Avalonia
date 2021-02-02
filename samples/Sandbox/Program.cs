using System;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace Sandbox
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder =
                AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .LogToTrace()
                    .SetupWithoutStarting();

            while (true)
            {
                var lifetime = new ClassicDesktopStyleApplicationLifetime();
                builder.Instance.ApplicationLifetime = lifetime;
                builder.Instance.OnFrameworkInitializationCompleted();
                lifetime.Start(args);

                ((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Dispose();
            }
        }
    }
}
