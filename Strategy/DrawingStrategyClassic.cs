using System;
using System.Windows;
using System.Windows.Media;

namespace NPaint.Strategy
{
    class DrawingStrategyClassic : DrawingStrategy
    {
        public void draw(Geometry geometry, Point startPoint, Point point)
        {
            EllipseGeometry tmp = geometry as EllipseGeometry;

            tmp.Center = MidPoint(startPoint, point);

            double radius = Math.Min(Math.Abs(startPoint.X - point.X), Math.Abs(startPoint.Y - point.Y));

            tmp.RadiusX = radius;
            tmp.RadiusY = radius;
        }

        protected Point MidPoint(Point a, Point b)
        {
            Point tmp;
            tmp.X = (a.X + b.X) / 2;
            tmp.Y = (a.Y + b.Y) / 2;
            return tmp;
        }
    }
}
