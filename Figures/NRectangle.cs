using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NRectangle : Figure
    {
        protected Rect rect;
        public NRectangle(Path adaptedPath) : base(adaptedPath)
        {
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
            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = point.X;
            rect.Y = point.Y;

            Repaint();
        }
        public override void MoveByInsideGroup(Vector shiftVector)
        {
            double x = rect.X - shiftVector.X;
            double y = rect.Y - shiftVector.Y;

            rect.X = x;
            rect.Y = y;

            Repaint();
        }
        public override void IncreaseSize()
        {
            rect.X--;
            rect.Y--;
            rect.Width += 2;
            rect.Height += 2;

            Repaint();
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
            PointsList.Add(rect.TopLeft);       // lewy gorny
            PointsList.Add(rect.BottomRight);   // prawy dolny
        }

        public Point GetTopLeft()
        {
            return rect.TopLeft;
        }
    }
}
