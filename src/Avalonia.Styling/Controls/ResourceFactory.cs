using System;

namespace Avalonia.Controls
{
    [Obsolete("Internal use only!")]
    public sealed class ResourceFactory
    {
        public ResourceFactory(Func<IServiceProvider?, object> factory)
        {
            Factory = factory;
        }

        public Func<IServiceProvider?, object> Factory { get; }
    }
}
