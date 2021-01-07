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

            Repaint();
        }

        public override void MoveByInsideGroup(Point point)
        {
            double x = rect.X - point.X; //-> w obserwatorze
            double y = rect.Y - point.Y;

            rect.X = x;
            rect.Y = y;

            Repaint();
        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia prostokata na osi XY
            rect.X = Math.Min(point.X, startPoint.X);
            rect.Y = Math.Min(point.Y, startPoint.Y);

            // obliczenie wysokosci i szerokosci prostokata
            rect.Width = Math.Max(point.X, startPoint.X) - rect.X;
            rect.Height = Math.Max(point.Y, startPoint.Y) - rect.Y;

            Repaint();
        }

        public override void IncreaseSize()
        {
            // zabezpieczenie, zebysmy nie weszli na Menu
            if (rect.Y - adaptedPath.StrokeThickness / 2 > 1)
            {
                rect.X--;
                rect.Y--;
                rect.Width += 2;
                rect.Height += 2;

                Repaint();
            }
        }
        public override void DecreaseSize()
        {
            // zabezpieczenie, zeby rozmiary prostokata nie spadly ponizej 0
            if(rect.Width >= 2 && rect.Height >= 2)
            {
                rect.X++;
                rect.Y++;
                rect.Width -= 2;
                rect.Height -= 2;

                Repaint();
            }
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

        protected override void Repaint()
        {
            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ((RectangleGeometry)adaptedGeometry).Rect = rect;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
    }
}
