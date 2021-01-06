using Microsoft.VisualBasic.FileIO;
using NPaint.Observer;
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
        public NRectangle() : base()
        {
            adaptedPath = new Path();
            adaptedGeometry = new RectangleGeometry();
            adaptedPath.Data = adaptedGeometry;
        }
        public override void MoveBy(Point point)
        {
            //przesuwanie wewnątrz zaznaczenia
            /*if (((MainWindow)Application.Current.MainWindow).observer == true && this.GetType()!=typeof(ObservableFigure))
            {
                 x = rect.X - point.X; //-> w obserwatorze
                 y = rect.Y - point.Y;
            }*/
            //zwykłe przesuwanie
            
            double x = point.X;
            double y = point.Y;
            

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;

            //this.SetStartPoint(new Point(x, y));//Do przemyślenia

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }

        public override void MoveByInsideGroup(Point point)
        {
            double x = rect.X - point.X; //-> w obserwatorze
            double y = rect.Y - point.Y;

            rect.X = x;
            rect.Y = y;
            ((RectangleGeometry)adaptedGeometry).Rect = rect;
            adaptedPath.Data = adaptedGeometry;
            SetPointCollection();

        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia prostokata na osi XY
            rect.X = Math.Min(point.X, startPoint.X);
            rect.Y = Math.Min(point.Y, startPoint.Y);

            // obliczenie wysokosci i szerokosci prostokata
            rect.Width = Math.Max(point.X, startPoint.X) - rect.X;
            rect.Height = Math.Max(point.Y, startPoint.Y) - rect.Y;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }

        protected override void SetPointCollection()
        {
            // do zaznaczania prostokata wystarcza dwa rogi
            PointsList.Clear();
            PointsList.Add(rect.TopLeft);     // lewy gorny
            PointsList.Add(rect.BottomRight);// prawy dolny
        }

        public Point GetTopLeft()
        {
            return rect.TopLeft;
        }
    }
}
