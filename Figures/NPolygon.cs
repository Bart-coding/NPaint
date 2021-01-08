using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NPolygon : Figure
    {
        private PathFigure PathFigure;
        private List<LineSegment> Lines;
        private double scale = 1;
        Point CenterPoint;
        public NPolygon() : base()
        {
            // inicjalizacja zmiennych
            adaptedPath = new Path();
            adaptedGeometry = new PathGeometry();
            PathFigure = new PathFigure();
            Lines = new List<LineSegment>();

            adaptedPath.Data = adaptedGeometry;
        }

        public override void MoveBy(Point point)
        {
            double widthShift = Lines.Last().Point.X - point.X;
            double lengthShift = Lines.Last().Point.Y - point.Y;
            foreach (LineSegment line in Lines.TakeWhile(l=>l!=Lines.Last()))
            {
                line.Point = new Point(line.Point.X - widthShift, line.Point.Y - lengthShift);
            }
            Lines.Last().Point = point;
            SetStartPoint(point); //Repaint() inside
        }
        public override void MoveByInsideGroup(Point point)
        {
            foreach (LineSegment line in Lines)
            {
                line.Point = new Point(line.Point.X - point.X, line.Point.Y - point.Y);
            }
            SetStartPoint(Lines.Last().Point);
        }

        public override void Draw(Point point)
        {
            // ustawienie konca obecnej linii w danym punkcie
            Lines.Last().Point = point;

            Repaint();
        }

        protected override void SetPointCollection()
        {
            // do zaznaczenia wielokata potrzebne sa wszystkie wierzcholki
            PointsList.Clear();
            foreach (LineSegment line in Lines)
            {
                PointsList.Add(line.Point);
            }
        }
        public override void SetStartPoint(Point point)
        {
            base.SetStartPoint(point);
            PathFigure.StartPoint = startPoint;
            LineSegment CurrentLine = new LineSegment();
            Lines.Add(CurrentLine);
            CurrentLine.Point = point;

            Repaint();
        }

        public override void IncreaseSize()
        {
            // punkt srodkowy wzgledem ktorego bedziemy transformowac geometrie
            CenterPoint = GetCenterPoint();
            scale = 1.01;   // skalujemy o jeden pkt
            adaptedGeometry.Transform = new ScaleTransform(scale, scale, CenterPoint.X, CenterPoint.Y);
            
            DecodeTransform();
        }
        public override void DecreaseSize()
        {
            // punkt srodkowy wzgledem ktorego bedziemy transformowac geometrie
            CenterPoint = GetCenterPoint();
            scale = 0.99;   // skalujemy o jeden pkt
            adaptedGeometry.Transform = new ScaleTransform(scale, scale, CenterPoint.X, CenterPoint.Y);
            
            DecodeTransform();
        }
        private void DecodeTransform()
        {
            Lines.Clear();

            // przesuwamy punkt poczatkowy
            PathFigure.StartPoint = ShiftPoint(PathFigure.StartPoint);

            // przesuwamy wszystkie wierzcholki figury
            foreach (LineSegment line in PathFigure.Segments)
            {
                line.Point = ShiftPoint(line.Point);
                Lines.Add(line);
            }

            // zerujemy transformacje, bo juz ja zdekodowalismy
            adaptedGeometry.Transform = null;

            Repaint();
        }
        private double CalculateDistance(Point p1, Point p2)
        {
            // dlugosc odcinka miedzy dwoma punktami
            return Math.Abs(Point.Subtract(p2, p1).Length);
        }
        private Point ShiftPoint(Point point)
        {
            // skorzystanie z podobienstwa trojkatow
            // z,x,y - duzy trojkat, gdzie wierzcholki to CenterPoint, point (obecnie przetwarzany wierzcholek)
            // oraz punkt tworzacy z nimi trojkat prostokatny - zmienna CornerPoint
            // z1,x1,y1 - maly trojkat, gdzie wierzcholki to CenterPoint, return point ( wierzcholek po shifcie)
            // oraz punkt tworzacy z nimi trojkat prostokatny
            double z = CalculateDistance(point, CenterPoint);
            Point CornerPoint = new Point(CenterPoint.X, point.Y);
            double x = CalculateDistance(point, CornerPoint);
            double z1 = z * scale;

            // z1/z = x1/x => x1 = z1*x/z
            double x1 = z1 * x / z;

            // z1 = x1 + y1 => y1 = sqrt(z1^2 - x1^2)
            double y1 = Math.Sqrt(z1 * z1 - x1 * x1);

            // to mozna uproscic chyba?
            // polozenie wierzcholka wzgledem CenterPointa

            // lewy dol
            if (point.X <= CenterPoint.X && point.Y >= CenterPoint.Y)
            {
                point.X = CenterPoint.X - x1;
                point.Y = CenterPoint.Y + y1;
            }
            // prawy dol
            else if (point.X >= CenterPoint.X && point.Y >= CenterPoint.Y)
            {
                point.X = CenterPoint.X + x1;
                point.Y = CenterPoint.Y + y1;
            }
            // prawy gora
            else if (point.X >= CenterPoint.X && point.Y <= CenterPoint.Y)
            {
                point.X = CenterPoint.X + x1;
                point.Y = CenterPoint.Y - y1;
            }
            // lewy gora
            else if (point.X <= CenterPoint.X && point.Y <= CenterPoint.Y)
            {
                point.X = CenterPoint.X - x1;
                point.Y = CenterPoint.Y - y1;
            }

            return point;
        }
        private Point GetCenterPoint()
        {
            Point point = new Point();
            point.X = adaptedGeometry.Bounds.X + adaptedGeometry.Bounds.Width / 2;
            point.Y = adaptedGeometry.Bounds.Y + adaptedGeometry.Bounds.Height / 2;
            // jezeli MoveBy() popsuje to bedzie trzeba recznia znajdowac te punkty
            //point.X = MostRight() - MostLeft();
            //point.Y = Bottom() - Top();
            return point;
        }
        public override object Clone()
        {
            NPolygon clonedFigure = base.Clone() as NPolygon;
            clonedFigure.PathFigure = PathFigure.Clone();
            clonedFigure.Lines = new List<LineSegment>();
            return clonedFigure;
        }
        protected override void Repaint()
        {
            PathFigure.Segments.Clear(); //czyszczenie niezbedne nwm dlaczego insert powinien zalatwic sprawe
            foreach(LineSegment line in Lines)
            {
                PathFigure.Segments.Add(line);
            }

            ((PathGeometry)adaptedGeometry).Figures.Clear(); //czyszczenie ten sam problem
            ((PathGeometry)adaptedGeometry).Figures.Add(PathFigure);    // przypisanie figury wielokata do geometrii

            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }

        public override void ChangeBorderThickness(double value)
        {
            //if
            adaptedPath.StrokeThickness = value;
        }

        public override void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection)
        {
            // waiting for implementation
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
            //Lines.Add(CurrentLine);
            Lines.Last().Point = startPoint;
            PathFigure.IsClosed = true; // domkniecie wielokata

            Repaint();
        }
    }
}
