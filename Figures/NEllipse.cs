using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NEllipse : Figure
    {
        protected Point CenterPoint;
        public NEllipse()
        {
            adaptedPath = new Path();
            adaptedGeometry = new EllipseGeometry();
            adaptedPath.Data = adaptedGeometry;
        }

        public override void MoveBy(Point point)
        {
            // obliczenie polozenia elipsy na osi XY
            CenterPoint.X = Math.Min(point.X, startPoint.X);
            CenterPoint.Y = Math.Min(point.Y, startPoint.Y);

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            tmp.Center = CenterPoint;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia elipsy na osi XY
            CenterPoint.X = Math.Min(point.X, startPoint.X);
            CenterPoint.Y = Math.Min(point.Y, startPoint.Y);

            // obliczenie wysokosci i szerokosci elipsy
            double width = Math.Max(point.X, startPoint.X) - CenterPoint.X;
            double height = Math.Max(point.Y, startPoint.Y) - CenterPoint.Y;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            tmp.Center = CenterPoint;
            tmp.RadiusX = width;
            tmp.RadiusY = height;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }
    }
}
