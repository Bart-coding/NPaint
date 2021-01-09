using System;
using System.Windows;
using System.Windows.Media;

namespace NPaint.Strategy
{
    class DrawingStrategyFitted : DrawingStrategy
    {
        public EllipseGeometry ChangeGeometry(Geometry geometry, Point startPoint, Point point)
        {
            EllipseGeometry tmp = geometry as EllipseGeometry;

            Point CenterPoint = MidPoint(startPoint, point);
                
            double radius = Math.Abs(Math.Sqrt(Math.Pow(startPoint.X - point.X, 2) + Math.Pow(startPoint.Y - point.Y, 2))) / 2;

            if (CenterPoint.Y - radius - ((MainWindow)System.Windows.Application.Current.MainWindow).BorderThicknessySlider.Value > 0)
            {
                tmp.Center = CenterPoint;
                tmp.RadiusX = radius;
                tmp.RadiusY = radius;
            }

            return tmp;
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
