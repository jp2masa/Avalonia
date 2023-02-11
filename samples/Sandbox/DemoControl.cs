using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Sandbox
{
    internal abstract class DemoControlBase : Control
    {
        public static readonly StyledProperty<IBrush> DemoBrushProperty =
            AvaloniaProperty.Register<DemoControl, IBrush>("DemoBrush", Brushes.DarkRed);

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.DrawRectangle(GetValue(DemoBrushProperty), null, new Rect(0, 0, 200, 120));
        }
    }

    internal sealed class DemoControl : DemoControlBase
    {
        public IBrush DemoBrush
        {
            get => GetValue(DemoBrushProperty);
            set => SetValue(DemoBrushProperty, value);
        }
    }
}
