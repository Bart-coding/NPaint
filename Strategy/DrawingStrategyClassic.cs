using System;
using System.Windows;
using System.Windows.Media;

namespace NPaint.Strategy
{
    class DrawingStrategyClassic : DrawingStrategy
    {
        public EllipseGeometry ChangeGeometry(Geometry geometry, Point startPoint, Point point)
        {
            EllipseGeometry tmp = geometry as EllipseGeometry;

            Point CenterPoint;

            double radius = Math.Min(Math.Abs(startPoint.X - point.X), Math.Abs(startPoint.Y - point.Y)) / 2;

            tmp.RadiusX = radius;
            tmp.RadiusY = radius;
            tmp.Center = MidPoint(startPoint, point);

            if(startPoint.X > point.X)
            {
                CenterPoint.X = startPoint.X - radius;
            }
            else
            {
                CenterPoint.X = startPoint.X + radius;
            }
            if(startPoint.Y > point.Y)
            {
                CenterPoint.Y = startPoint.Y - radius;
            }
            else
            {
                CenterPoint.Y = startPoint.Y + radius;
            }

            tmp.Center = CenterPoint;

            return tmp;
        }

        private Point MidPoint(Point a, Point b)
        {
            Point tmp;
            tmp.X = (a.X + b.X) / 2;
            tmp.Y = (a.Y + b.Y) / 2;
            return tmp;
        }
    }
}
