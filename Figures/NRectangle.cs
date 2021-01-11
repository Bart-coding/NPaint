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
        public override void SetFields(Path path)
        {
            adaptedPath = path;
            adaptedGeometry = path.Data;

            rect = ((RectangleGeometry)adaptedGeometry).Rect;

            SetPointCollection();
        }
        public override void ChangeBorderThickness(double value)
        {
            // pierwsze wywolanie - prostokat ma polozenie 0,0 
            if (rect.Width == 0 || rect.Height == 0)
            {
                ;   // nothing to do here
            }
            else if ((rect.Y - (GetBorderThickness() / 2)) <= 0 && value > adaptedPath.StrokeThickness)
                value = adaptedPath.StrokeThickness;
            adaptedPath.StrokeThickness = value;
        }
        public override void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection)
        {
            if (PointsList[1].X + value / 2 > pointCollectionOfSelection[1].X
                || PointsList[0].X - value / 2 < pointCollectionOfSelection[0].X
                || PointsList[0].Y + value / 2 > pointCollectionOfSelection[1].Y
                || PointsList[1].Y - value / 2 < pointCollectionOfSelection[0].Y)
            {
                return;
            }
            else
            {
                adaptedPath.StrokeThickness = value;
            }
        }
        public override void Draw(Point startPoint, Point currentPoint)
        {
            // obliczenie polozenia prostokata na osi XY
            rect.X = Math.Min(currentPoint.X, startPoint.X);
            rect.Y = Math.Min(currentPoint.Y, startPoint.Y);

            // obliczenie wysokosci i szerokosci prostokata
            rect.Width = Math.Max(currentPoint.X, startPoint.X) - rect.X;
            rect.Height = Math.Max(currentPoint.Y, startPoint.Y) - rect.Y;

            Repaint();
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

        protected override void Repaint()
        {
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
