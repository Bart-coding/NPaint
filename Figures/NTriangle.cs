using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]///
    class NTriangle : Figure
    {
        private PathFigure PathFigure;
        private Point sPoint;
        private LineSegment line1;
        private Point point1;
        private LineSegment line2;
        private Point point2;
        public NTriangle(Path path)//path może być wszędzie w konstruktorze                  //można w sumie dodać jakieś obostrzenia żeby nie można było podać path z wieloma figurami w środku
        {
            adaptedPath = path;
            adaptedGeometry = path.Data;
            PathGeometry tmp = adaptedGeometry as PathGeometry;  //Może w ogóle tych wartości nie ustawiać jak na starcie są niepotrzebne?
            PathFigure = tmp.Figures[0];
            line1 = PathFigure.Segments[0] as LineSegment;
            line2 = PathFigure.Segments[1] as LineSegment;
            
            /*
            // inicjalizacja zmiennych
            adaptedPath = new Path();
            adaptedGeometry = new PathGeometry();
            adaptedPath.Data = adaptedGeometry;
            PathFigure = new PathFigure();
            line1 = new LineSegment();
            line2 = new LineSegment();

            PathFigure.IsClosed = true; // domkniecie trojkata
            PathGeometry tmp = adaptedGeometry as PathGeometry;

            // przypisanie linii do figury
            //PathFigure.Segments.Clear(); czyszczenie bedzie niezbedne przy shapefactory chyba?
            PathFigure.Segments.Add(line1);
            PathFigure.Segments.Add(line2);

            //tmp.Figures.Clear(); czyszczenie bedzie niezbedne przy shapefactory chyba?
            tmp.Figures.Add(PathFigure);    // przypisanie figury trojkata do geometrii  */
        }

        public override void MoveBy(Point point)
        {
            throw new NotImplementedException();
        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia lewego dolnego wierzcholka
            sPoint.X = Math.Min(point.X, startPoint.X);
            sPoint.Y = Math.Max(point.Y, startPoint.Y);
            PathFigure.StartPoint = sPoint;

            // obliczenie polozenia prawego dolnego wierzcholka
            point1.X = Math.Max(point.X, startPoint.X);
            point1.Y = Math.Max(point.Y, startPoint.Y);
            line1.Point = point1;

            // obliczenie polozenia gornego wierzcholka
            point2.X = MidPointX(point.X,startPoint.X);
            point2.Y = Math.Min(point.Y, startPoint.Y);
            line2.Point = point2;
        }
        private double MidPointX(double a, double b)
        {
            double tmp;
            tmp = (a + b) / 2;
            return tmp;
        }
    }
}
