using System;

namespace Avalonia.Media
{
    public sealed class QuadraticBezierSegment : PathSegment
    {
        /// <summary>
        /// Defines the <see cref="Point1"/> property.
        /// </summary>
        public static readonly StyledProperty<Point> Point1Property
                        = AvaloniaProperty.Register<QuadraticBezierSegment, Point>(nameof(Point1));

        /// <summary>
        /// Defines the <see cref="Point2"/> property.
        /// </summary>
        public static readonly StyledProperty<Point> Point2Property
                        = AvaloniaProperty.Register<QuadraticBezierSegment, Point>(nameof(Point2));

        /// <summary>
        /// Gets or sets the point1.
        /// </summary>
        /// <value>
        /// The point1.
        /// </value>
        public Point Point1
        {
            get { return GetValue(Point1Property); }
            set { SetValue(Point1Property, value); }
        }

        /// <summary>
        /// Gets or sets the point2.
        /// </summary>
        /// <value>
        /// The point2.
        /// </value>
        public Point Point2
        {
            get { return GetValue(Point2Property); }
            set { SetValue(Point2Property, value); }
        }

        internal override void ApplyTo(StreamGeometryContext ctx)
        {
            ctx.QuadraticBezierTo(Point1, Point2, IsStroked);
        }

        public override string ToString()
            => FormattableString.Invariant($"Q {Point1} {Point2}");
    }
}
