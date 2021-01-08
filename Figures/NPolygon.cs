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
            Lines.Last().Point = point;
            Repaint();
        }

        protected override void SetPointCollection()
        {
            // waiting for implementation
            PointsList.Clear();
            foreach (LineSegment line in Lines)
                PointsList.Add(line.Point);
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

        public override void DecreaseSize()
        {
            // waiting for implementation
        }
        public override void IncreaseSize()
        {
            // waiting for implementation
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
