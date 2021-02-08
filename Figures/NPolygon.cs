using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NPolygon : Figure
    {
        public PathFigure PathFigure;
        private List<LineSegment> Lines;
        private Point CenterPoint;

        public NPolygon(Path adaptedPath) : base(adaptedPath)
        {
            adaptedGeometry = new PathGeometry();
            PathFigure = new PathFigure();
            Lines = new List<LineSegment>();

            adaptedPath.Data = adaptedGeometry;
        }
        public override void SetFields(Path path)
        {
            adaptedPath = path;
            adaptedGeometry = path.Data;

            PathFigure = ((PathGeometry)adaptedGeometry).Figures[0];
            Lines.Clear();
            PathSegment pathSegment = PathFigure.Segments[0];

            if (PathFigure.Segments[0].GetType() == typeof(PolyLineSegment))
            {
                foreach (Point point in ((PolyLineSegment)pathSegment).Points)
                {
                    Lines.Add(new LineSegment(point, true));
                }
            }
            else
            {
                Lines.Add(new LineSegment(((LineSegment)pathSegment).Point, true));
            }

            SetPointCollection();

            CenterPoint = GetCenterPoint();
        }
        public override void Draw(Point startPoint, Point currentPoint)
        {
            // ustawienie konca obecnej linii w danym punkcie
            Lines.Last().Point = currentPoint;

            Repaint();
        }
        public override void MoveBy(Point point)
        {
            //wyliczenie przesunięcia -- odleglosci od punktu początkowego figury (i zarazem pktu koncowego)
            double widthShift = Lines.Last().Point.X - point.X; 
            double lengthShift = Lines.Last().Point.Y - point.Y;

            //przesuwanie wszystkich punktów figury o to przesunięcie
            foreach (LineSegment line in Lines.TakeWhile(l=>l!=Lines.Last()))
            {
                line.Point = new Point(line.Point.X - widthShift, line.Point.Y - lengthShift);
            }

            Lines.Last().Point = point;
            SetStartPoint(point);
        }
        public override void MoveByInsideGroup(Vector shiftVector)
        {
            foreach (LineSegment line in Lines)
            {
                line.Point = new Point(line.Point.X - shiftVector.X, line.Point.Y - shiftVector.Y);
            }

            SetStartPoint(Lines.Last().Point);
        }
        public override void IncreaseSize()
        {
            // punkt srodkowy wzgledem ktorego bedziemy transformowac geometrie
            CenterPoint = GetCenterPoint();
            ShiftApexes(1.01);// skalujemy o jeden pkt
        }
        public override void DecreaseSize()
        {
            // minimlne wartosci, ponizej ktorych nie mozemy zejsc
            if (adaptedGeometry.Bounds.Width > 10 && adaptedGeometry.Bounds.Height > 10)
            {
                // punkt srodkowy wzgledem ktorego bedziemy transformowac geometrie
                CenterPoint = GetCenterPoint();
                ShiftApexes(0.99);// skalujemy o jeden pkt
            }
        }
        public override object Clone()
        {
            NPolygon clonedFigure = base.Clone() as NPolygon;
            clonedFigure.PathFigure = new PathFigure();
            clonedFigure.Lines = new List<LineSegment>();
            return clonedFigure;
        }

        protected override void Repaint()
        {
            PathFigure.Segments.Clear();

            foreach (LineSegment line in Lines)
            {
                PathFigure.Segments.Add(line);
            }

            ((PathGeometry)adaptedGeometry).Figures.Clear();
            ((PathGeometry)adaptedGeometry).Figures.Add(PathFigure);

            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
        protected override void SetPointCollection()
        {
            // do zaznaczenia wielokata potrzebne sa wszystkie wierzcholki
            PointsList.Clear();
            PointsList.Add(PathFigure.StartPoint);

            foreach (LineSegment line in Lines)
            {
                PointsList.Add(line.Point);
            }
        }

        public void SetStartPoint(Point point)
        {
            PathFigure.StartPoint = point;
            LineSegment CurrentLine = new LineSegment();
            Lines.Add(CurrentLine);
            CurrentLine.Point = point;

            Repaint();
        }
        public Point GetStartPoint()
        {
            return PathFigure.StartPoint;
        }
        public void CloseLine(Point point)
        {
            Lines.Last().Point = point;
            LineSegment CurrentLine = new LineSegment();
            Lines.Add(CurrentLine);
            CurrentLine.Point = point;

            Repaint();
        }
        public void CloseFigure()
        {
            Lines.Last().Point = PathFigure.StartPoint;
            PathFigure.IsClosed = true; // domkniecie wielokata

            Repaint();
        }
        
        private void ShiftApexes(double scale)
        {
            Lines.Clear();

            // przesuwamy punkt poczatkowy
            PathFigure.StartPoint = ShiftPointFromCenter(PathFigure.StartPoint,scale);

            // przesuwamy wszystkie wierzcholki figury
            foreach (LineSegment line in PathFigure.Segments)
            {
                line.Point = ShiftPointFromCenter(line.Point,scale);
                Lines.Add(line);
            }

            Repaint();
        }
        private Point ShiftPointFromCenter(Point point, double scale)
        {
            Vector distance = CenterPoint - point;
            point = CenterPoint - distance * scale;

            return point;
        }
        private Point GetCenterPoint()
        {
            Point point = new Point
            {
                X = adaptedGeometry.Bounds.X + adaptedGeometry.Bounds.Width / 2,
                Y = adaptedGeometry.Bounds.Y + adaptedGeometry.Bounds.Height / 2
            };

            return point;
        }
        private bool WillHitMenu()
        {
            if(adaptedGeometry.Bounds.Y - adaptedPath.StrokeThickness / 2 <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
