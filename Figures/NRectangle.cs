using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NRectangle : Figure
    {
        protected Rect rect;
        public NRectangle()
        {
            adaptedPath = new Path();
            adaptedGeometry = new RectangleGeometry();
            rect = new Rect();
            RectangleGeometry tmp = (RectangleGeometry)adaptedGeometry;
            tmp.Rect = rect;
            adaptedGeometry = tmp;
            adaptedPath.Data = adaptedGeometry;
        }
        public override void MoveBy(Point point)
        {
            // obliczenie polozenia prostokata na osi XY
            double x = Math.Min(point.X, startPoint.X);
            double y = Math.Min(point.Y, startPoint.Y);

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            RectangleGeometry tmp = (RectangleGeometry)adaptedGeometry;
            tmp.Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }

        public override void Resize(Point point)
        {
            // wersja z mozliwoscia rysowania w kazdym z czterech kierunkow

            // obliczenie polozenia prostokata na osi XY
            double x = Math.Min(point.X, startPoint.X);
            double y = Math.Min(point.Y, startPoint.Y);
            
            // obliczenie wysokosci i szerokosci prostokata
            double width = Math.Max(point.X, startPoint.X) - x;
            double height = Math.Max(point.Y, startPoint.Y) - y;

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            RectangleGeometry tmp = (RectangleGeometry)adaptedGeometry;
            tmp.Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }
    }
}
