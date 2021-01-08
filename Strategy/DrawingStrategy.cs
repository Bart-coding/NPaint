using System.Windows;
using System.Windows.Media;

namespace NPaint.Strategy
{
    interface DrawingStrategy
    {
        public void draw(Geometry geometry, Point startPoint, Point point);
    }
}
