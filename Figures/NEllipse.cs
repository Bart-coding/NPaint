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
            // ustawienie srodka elipsy tam gdzie jest myszka
            // obliczenie polozenia elipsy na osi XY
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            CenterPoint.X = point.X;
            CenterPoint.Y = point.Y;
            tmp.Center = CenterPoint;

            // zakomentowany kod Bartka
            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            //EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            //CenterPoint.X = point.X - startPoint.X + tmp.RadiusX;
            //CenterPoint.Y = point.Y - startPoint.Y + tmp.RadiusY;
            //tmp.Center = CenterPoint;

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
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

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
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
            PointsList.Insert(0, rect.TopLeft);     // lewy gorny
            PointsList.Insert(1, rect.BottomRight); // prawy dolny
        }
    }
}
