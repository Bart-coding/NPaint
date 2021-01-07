using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NEllipse : Figure
    {
        protected Point CenterPoint;
        public NEllipse() : base()
        {
            adaptedPath = new Path();
            adaptedGeometry = new EllipseGeometry();
            adaptedPath.Data = adaptedGeometry;
        }

        public override void MoveBy(Point point) // Tu znów kwestia nieustawianego startpointu i nie wiadomo kiedy
        {

            /*if (((MainWindow)Application.Current.MainWindow).observer == true)
            {
                x = CenterPoint.X - point.X; //-> w obserwatorze
                y = CenterPoint.Y - point.Y;
            }*/
            
             double x = point.X;
             double y = point.Y;
            
            CenterPoint.X = x;
            CenterPoint.Y = y;
            ((EllipseGeometry)adaptedGeometry).Center = CenterPoint;

            // zakomentowany kod Bartka
            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            //EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            //CenterPoint.X = point.X - startPoint.X + tmp.RadiusX;
            //CenterPoint.Y = point.Y - startPoint.Y + tmp.RadiusY;
            //tmp.Center = CenterPoint;

            Repaint();
        }

        public override void MoveByInsideGroup(Point point)
        {
            double x = CenterPoint.X - point.X; //-> w obserwatorze
            double y = CenterPoint.Y - point.Y;
            CenterPoint.X = x;
            CenterPoint.Y = y;
            ((EllipseGeometry)adaptedGeometry).Center = CenterPoint;

            Repaint();
        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia elipsy na osi XY
            CenterPoint = MidPoint(point, startPoint);

            // obliczenie wysokosci i szerokosci elipsy
            double width = Math.Max(point.X, startPoint.X) - CenterPoint.X;
            double height = Math.Max(point.Y, startPoint.Y) - CenterPoint.Y;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            tmp.Center = CenterPoint;
            tmp.RadiusX = width;
            tmp.RadiusY = height;

            Repaint();
        }

        public override void IncreaseSize()
        {
            ((EllipseGeometry)adaptedGeometry).RadiusX++;
            ((EllipseGeometry)adaptedGeometry).RadiusY++;
            Repaint();
        }
        public override void DecreaseSize()
        {
            ((EllipseGeometry)adaptedGeometry).RadiusX--;
            ((EllipseGeometry)adaptedGeometry).RadiusY--;
            Repaint();
        }

        protected Point MidPoint(Point a, Point b)
        {
            Point tmp;
            tmp.X = (a.X + b.X) / 2;
            tmp.Y = (a.Y + b.Y) / 2;
            return tmp;
        }

        public Point GetCenterPoint()//
        {
            EllipseGeometry tmp = this.adaptedGeometry as EllipseGeometry;
            return tmp.Center;
        }

        protected override void SetPointCollection()
        {
            // do zaznaczania elipsy wystarcza dwa rogi
            Rect rect = ((EllipseGeometry)adaptedGeometry).Bounds;  // protokat w ktory wpisana jest elipsa
            PointsList.Clear();
            PointsList.Add(rect.TopLeft);     // lewy gorny
            PointsList.Add(rect.BottomRight); // prawy dolny
        }

        protected override void Repaint()
        {
            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
    }
}
