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
        private Point point1; //można by było chyba bezpośrednio ustawiać ten StartPoint i Line'y
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
            
            //////Canvas.SetTop(this.adaptedPath, y);
            //////Canvas.SetLeft(this.adaptedPath, x);

            double lengthShift = point3.Y - point.Y;
            double widthShift = point3.X - point.X;
             
            point3.Y = point.Y;
            point3.X = point.X;
            point1.Y -= lengthShift;
            point1.X -= widthShift;
            point2.Y -= lengthShift;
            point2.X -= widthShift;


            //PathFigure.StartPoint = new Point(x, y);
            //Nie wiem czy nie trza ustawić pól figury czy już są ustawione
            // trzeba, bo geometry zostaje w tym samym miejscu, choc path sie przemieszcza
            
            Repaint();
        }

        public override void MoveByInsideGroup(Point point)
        {
            //////////Vector vector = VisualTreeHelper.GetOffset(this.adaptedPath);
            ///////////Point positionOfTriangle = new Point(vector.X, vector.Y);
            ///////Canvas.SetTop(this.adaptedPath, positionOfTriangle.Y - point.Y);
            //////Canvas.SetLeft(this.adaptedPath, positionOfTriangle.X - point.X);
            point1.Y -= point.Y;
            point1.X -= point.X;
            point2.Y -= point.Y;
            point2.X -= point.X;
            point3.Y -= point.Y;
            point3.X -= point.X;


            Repaint();
        }
        public override void Draw(Point point)
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

            // jezeli zaczelismy rysowac czyli wszystkie punkty sa w tym samym miejscu
            if(point1 == point2 && point2 == point3)
            {
                Repaint();
            }
            else
            {
                // jezeli gorny wierzcholek nie wchodzi na Menu
                if (point3.Y - CalculateMargin() >= 1)
                {
                    Repaint();
                }
            }
        }

        public override void IncreaseSize()
        {
            // zabezpieczenie, zebysmy nie weszli na Menu
            if ( point3.Y - CalculateMargin() >= 1 || (point1 == point2 && point2 == point3))
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
        private double CalculateDistance(Point p1, Point p2)
        {
            return Math.Abs(Point.Subtract(p2, p1).Length);
        }
        public double CalculateMargin()
        {
            // skorzystanie z podobienstwa trojkatow
            // obliczenia wedlug wzoru
            // cosB = 1 - a^2 / 2*b^2   // zaleznosc miedzy dlugoscia podstawy i ramienia, B kat miedzy ramionami
            // A = (PI - B)/2           // kat przy podstawie

            double a = CalculateDistance(point1, point2);   // dlugosc podstawy duzego trojkata
            double b = CalculateDistance(point1, point3);   // dlugosc ramienia duzego trojkata
            double cosB = 1 - (a*a) / (2 * b*b);            // cosinus kata miedzy ramionami
            double Beta = Math.Acos(cosB);                  // wyliczony kat miedzy ramionami
            double Alpha = (Math.PI - Beta) / 2;            // kat przy podstawie

            // wlasnosci trygonometryczne trojkata prostokatnego
            // c = b/cosA
            double x = (adaptedPath.StrokeThickness / 2.0) / Math.Cos(Alpha);
            return x;
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
        public Point GetTopCorner()
        {
            return point3;
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

        public override void ChangeBorderThickness(double value)
        {
            if (this.GetTopCorner().Y - CalculateMargin() <= 0 && value > adaptedPath.StrokeThickness)
                value = adaptedPath.StrokeThickness;

            adaptedPath.StrokeThickness = value;
        }

        public override void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection)
        {
            /*if (GetTopCorner().Y + adaptedPath.ActualHeight + value / 2 > PointsList[1].Y)
            {
                return;
            }*/
            //else
            {
                adaptedPath.StrokeThickness = value;
            }
        }

        public override void SetFields(Path path)
        {
            adaptedPath = path;
            adaptedGeometry = path.Data;

            PathFigure = ((PathGeometry)adaptedGeometry).Figures[0];
            //MessageBox.Show(PathFigure.Segments[0] + "");
            //
            PolyLineSegment polyLineSegment = (PolyLineSegment)PathFigure.Segments[0];
            line1.Point = polyLineSegment.Points[0];
            line2.Point = polyLineSegment.Points[1];
            //
            point1 = PathFigure.StartPoint;
            point2 = line1.Point;
            point3 = line2.Point;
            //
            SetPointCollection();
            //



            /*private PathFigure PathFigure;
        private Point point1; //można by było chyba bezpośrednio ustawiać ten StartPoint i Line'y
        private LineSegment line1;
        private Point point2;
        private LineSegment line2;
        private Point point3;*/

        }
    }
}
