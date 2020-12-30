using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NRectangle : Figure
    {
        private Rect rect;
        public NRectangle()
        {
            adaptedPath = new Path();
            adaptedGeometry = new RectangleGeometry();
            rect = new Rect();
            //startPoint = point;
        }
        public override void MoveBy(Point point)
        {
            // obliczenie polozenia prostokata na osi XY
            double x = Math.Min(point.X, startPoint.X);
            double y = Math.Min(point.Y, startPoint.Y);

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;

            // przypisanie zmiennej ze zmienionymi wartosciami do geometrii
            RectangleGeometry tmp = (RectangleGeometry)adaptedGeometry;
            tmp.Rect = rect;
            adaptedGeometry = tmp;
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

            // przypisanie zmiennej ze aktualnymi wartosciami do geometrii
            RectangleGeometry tmp = (RectangleGeometry)adaptedGeometry;
            tmp.Rect = rect;
            adaptedGeometry = tmp;  // czy to jest potrzebne?? czy referencja wystarczy...?
        }
    }
}
