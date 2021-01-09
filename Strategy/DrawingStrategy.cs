using System.Windows;
using System.Windows.Media;

namespace NPaint.Strategy
{
    interface DrawingStrategy
    {
        public EllipseGeometry ChangeGeometry(Geometry geometry, Point startPoint, Point point);
    }
}
