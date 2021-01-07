using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]///
    class NTriangle : Figure
    {
        private PathFigure PathFigure;
        private Point point1;
        private LineSegment line1;
        private Point point2;
        private LineSegment line2;
        private Point point3;

        public NTriangle() : base()
        {
            // inicjalizacja zmiennych
            adaptedPath = new Path();
            adaptedGeometry = new PathGeometry();
            adaptedPath.Data = adaptedGeometry;
            PathFigure = new PathFigure();
            line1 = new LineSegment();
            line2 = new LineSegment();

            PathFigure.IsClosed = true; // domkniecie trojkata 
        }

        public override void MoveBy(Point point)
        {
            
            double y = point.Y;
            double x = point.X;

            //adaptedGeometry.Transform = adaptedGeometry.Transform.TransformBounds(rect);
            //adaptedGeometry.Bounds.Y = y;
            Canvas.SetTop(this.adaptedPath, y);
            Canvas.SetLeft(this.adaptedPath, x);
            //PathFigure.StartPoint = new Point(x, y);
            //Nie wiem czy nie trza ustawić pól figury czy już są ustawione
            // trzeba, bo geometry zostaje w tym samym miejscu, choc path sie przemieszcza

            Repaint();
        }

        public override void MoveByInsideGroup(Point point)
        {
            Vector vector = VisualTreeHelper.GetOffset(this.adaptedPath);
            // Convert the vector to a point value.
            Point positionOfTriangle = new Point(vector.X, vector.Y);


            //MessageBox.Show(Canvas.GetTop(this.adaptedPath) + "");
            Canvas.SetTop(this.adaptedPath, positionOfTriangle.Y - point.Y);
            Canvas.SetLeft(this.adaptedPath, positionOfTriangle.X - point.X);
            /*PathFigure.StartPoint.X -= point.X;
            double y = this.GetLeftDownCorner().Y - point.Y;
            double x2 = this.line1.Point.X - point.X;
            double y2 = this.line1.Point.Y - point.Y;
            double x3 = this.line2.Point.X - point.X;
            double y3 = this.line2.Point.Y - point.Y;*/


            Repaint();
        }
        public override void Resize(Point point)
        {
            // obliczenie polozenia lewego dolnego wierzcholka
            point1.X = Math.Min(point.X, startPoint.X);
            point1.Y = Math.Max(point.Y, startPoint.Y);

            // obliczenie polozenia prawego dolnego wierzcholka
            point2.X = Math.Max(point.X, startPoint.X);
            point2.Y = Math.Max(point.Y, startPoint.Y);

            // obliczenie polozenia gornego wierzcholka
            point3.X = MidPointX(point.X,startPoint.X);
            point3.Y = Math.Min(point.Y, startPoint.Y);

            // jezeli gorny wierzcholek nie wchodzi na Menu
            if(point3.Y - adaptedPath.StrokeThickness >= 1)
            {
                Repaint();
            }
        }

        public override void IncreaseSize()
        {
            // idealnie musielibysmy liczyc dlugosc przekatnej rombu, ktorego a = border thickness

            // zabezpieczenie, zebysmy nie weszli na Menu
            // wyglada ok tylko dla malych border thickness
            if(point3.Y - adaptedPath.StrokeThickness >= 1)
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

        protected override void SetPointCollection()
        {
            // do zaznaczenia trojkata potrzebne sa wszystkie 3 wierzcholki
            PointsList.Clear();
            PointsList.Add(PathFigure.StartPoint);    // lewy dolny
            PointsList.Add(line1.Point);              // prawy dolny
            PointsList.Add(line2.Point);              // gorny
        }

        private double MidPointX(double a, double b)
        {
            return (a+b)/2;
        }

        // musimy klonowac tez prywatne obiekty NTriangle, ale tylko te ktore sa inicjowane przez new
        public override object Clone()
        {
            NTriangle clonedFigure = base.Clone() as NTriangle;
            clonedFigure.PathFigure = PathFigure.Clone();
            clonedFigure.PathFigure.IsClosed = true;
            clonedFigure.line1 = line1.Clone();
            clonedFigure.line2 = line2.Clone();
            return clonedFigure;
        }

        public Point GetTopLeft()
        {
            return this.adaptedGeometry.Bounds.TopLeft;
        }

        public Point GetLeftDownCorner()
        {
            return PathFigure.StartPoint;
        }

        protected override void Repaint()
        {
            PathFigure.StartPoint = point1;
            line1.Point = point2;
            line2.Point = point3;

            PathFigure.Segments.Clear(); //czyszczenie niezbedne nwm dlaczego insert powinien zalatwic sprawe
            PathFigure.Segments.Insert(0, line1);
            PathFigure.Segments.Insert(1, line2);

            ((PathGeometry)adaptedGeometry).Figures.Clear(); //czyszczenie ten sam problem
            ((PathGeometry)adaptedGeometry).Figures.Insert(0, PathFigure);    // przypisanie figury trojkata do geometrii

            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
    }
}
