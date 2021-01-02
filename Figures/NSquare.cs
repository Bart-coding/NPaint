using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NSquare : NRectangle
    {
        public NSquare() : base() {}
        public override void Resize(Point point)
        {
            // wersja z mozliwoscia rysowania w kazdym z czterech kierunkow

            // obliczenie polozenia kwadratu na osi XY
            // zmienne pomocnicze
            double x, y, width, height, squarelength;

            // sytuacja gdy rysujemy prawy dolny rog
            if(point.X >= startPoint.X && point.Y >= startPoint.Y)
            {
                x = startPoint.X;
                y = startPoint.Y;
                width = point.X - x;
                height = point.Y - y;
                squarelength = Math.Min(width, height);
            }
            // sytuacja gdy rysujemy prawy gorny rog
            else if (point.X > startPoint.X && point.Y < startPoint.Y)
            {
                x = startPoint.X;
                y = point.Y;
                width = point.X - x;
                height = startPoint.Y - y;
                squarelength = Math.Min(width, height);
                y = startPoint.Y - squarelength;
            }
            // sytuacja gdy rysujemy lewy dolny rog
            else if (point.X <= startPoint.X && point.Y >= startPoint.Y)
            {
                x = point.X;
                y = startPoint.Y;
                width = startPoint.X - x;
                height = point.Y - y;
                squarelength = Math.Min(width, height);
                x = startPoint.X - squarelength;
            }
            // sytuacja gdy rysujemy lewy gorny rog
            else
            {
                x = point.X;
                y = point.Y;
                width = startPoint.X - x;
                height = startPoint.Y - y;
                squarelength = Math.Min(width, height);
                x = startPoint.X - squarelength;
                y = startPoint.Y - squarelength;
            }

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;
            rect.Width = squarelength;
            rect.Height = squarelength;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }
    }
}
