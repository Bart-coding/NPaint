using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NTriangle : Figure
    {

        private PathFigure PathFigure;
        private Point point1;
        private LineSegment line1;
        private Point point2;
        private LineSegment line2;
        private Point point3;

        public NTriangle(Path adaptedPath) : base(adaptedPath)
        {
            adaptedGeometry = new PathGeometry();
            adaptedPath.Data = adaptedGeometry;
            PathFigure = new PathFigure();
            line1 = new LineSegment();
            line2 = new LineSegment();

            PathFigure.IsClosed = true; // domkniecie trojkata 
        }

        public override void SetFields(Path path)
        {
            adaptedPath = path;
            adaptedGeometry = path.Data;

            PathFigure = ((PathGeometry)adaptedGeometry).Figures[0];

            PolyLineSegment polyLineSegment = (PolyLineSegment)PathFigure.Segments[0];
            line1.Point = polyLineSegment.Points[0];
            line2.Point = polyLineSegment.Points[1];

            point1 = PathFigure.StartPoint;
            point2 = line1.Point;
            point3 = line2.Point;

            SetPointCollection();
        }
        public override void Draw(Point startPoint, Point currentPoint)
        {
            // obliczenie polozenia lewego dolnego wierzcholka
            point1.X = Math.Min(currentPoint.X, startPoint.X);
            point1.Y = Math.Max(currentPoint.Y, startPoint.Y);

            // obliczenie polozenia prawego dolnego wierzcholka
            point2.X = Math.Max(currentPoint.X, startPoint.X);
            point2.Y = Math.Max(currentPoint.Y, startPoint.Y);

            // obliczenie polozenia gornego wierzcholka
            point3.X = MidPointX(currentPoint.X, startPoint.X);
            point3.Y = Math.Min(currentPoint.Y, startPoint.Y);

            Repaint();
        }
        public override void MoveBy(Point point)
        {
            double lengthShift = point3.Y - point.Y;
            double widthShift = point3.X - point.X;
             
            point3.Y = point.Y;
            point3.X = point.X;
            point1.Y -= lengthShift;
            point1.X -= widthShift;
            point2.Y -= lengthShift;
            point2.X -= widthShift;
            
            Repaint();
        }
        public override void MoveByInsideGroup(Vector shiftVector)
        {
            point1.Y -= shiftVector.Y;
            point1.X -= shiftVector.X;
            point2.Y -= shiftVector.Y;
            point2.X -= shiftVector.X;
            point3.Y -= shiftVector.Y;
            point3.X -= shiftVector.X;

            Repaint();
        }
        public override void IncreaseSize()
        {
            // lewy dolny 
            point1.X--;
            point1.Y++;

            // prawy dolny
            point2.X++;
            point2.Y++;

            //srodkowy
            point3.Y--;

            Repaint();
        }
        public override void DecreaseSize()
        {
            // zabezpieczenie, zeby rozmiary trojkata nie spadly ponizej 0
            if( (point1.X < point2.X - 1) && (point3.Y + 1 < point1.Y) )
            {
                // lewy dolny 
                point1.X++;
                point1.Y--;

                // prawy dolny
                point2.X--;
                point2.Y--;

                //srodkowy
                point3.Y++;

                Repaint();
            }
        }
        public override object Clone()
        {
            NTriangle clonedFigure = base.Clone() as NTriangle;
            clonedFigure.PathFigure = PathFigure.Clone();
            clonedFigure.PathFigure.IsClosed = true;
            clonedFigure.line1 = line1.Clone();
            clonedFigure.line2 = line2.Clone();
            return clonedFigure;
        }

        protected override void Repaint()
        {
            PathFigure.StartPoint = point1;
            line1.Point = point2;
            line2.Point = point3;

            PathFigure.Segments.Clear();
            PathFigure.Segments.Insert(0, line1);
            PathFigure.Segments.Insert(1, line2);

            ((PathGeometry)adaptedGeometry).Figures.Clear();
            ((PathGeometry)adaptedGeometry).Figures.Insert(0, PathFigure);

            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
        protected override void SetPointCollection()
        {
            // do zaznaczenia trojkata potrzebne sa wszystkie 3 wierzcholki
            PointsList.Clear();
            PointsList.Add(PathFigure.StartPoint);    // lewy dolny
            PointsList.Add(line1.Point);              // prawy dolny
            PointsList.Add(line2.Point);              // gorny
        }

        public Point GetTopCorner()
        {
            return point3;
        }

        private double MidPointX(double a, double b)
        {
            return (a+b)/2;
        }
    }
}
