using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NRectangle : Figure
    {
        protected Rect rect;
        public NRectangle()
        {
            adaptedPath = new Path();
            adaptedGeometry = new RectangleGeometry();
            adaptedPath.Data = adaptedGeometry;
        }
        public override void MoveBy(Point point)
        {
            // ustawienie srodka prostokata tam gdzie jest myszka
            // obliczenie polozenia prostokata na osi XY
            double x = point.X - rect.Width / 2;
            double y = point.Y - rect.Height / 2;

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;

            this.SetStartPoint(new Point(x, y));

            // zakomentowany kod Bartka
            //double x = point.X - startPoint.X; //lub po prostu przyrownac oba
            //double y = point.Y - startPoint.Y;
            //**this.SetStartPoint(new Point(x, y));//=> Trzeba to zrobić w którymś momencie, ale aktualnie to przeszkadza rysowaniu

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

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
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;
        }
    }
}
