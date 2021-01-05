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

        //path może być wszędzie w konstruktorze                  
        //można w sumie dodać jakieś obostrzenia żeby nie można było podać path z wieloma figurami w środku
        public NTriangle(Path path) : base()
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

            // to chyba nie potrzebne
            //PathGeometry tmp = adaptedGeometry as PathGeometry;

            //// przypisanie linii do figury
            //PathFigure.Segments.Clear(); //czyszczenie bedzie niezbedne przy shapefactory chyba?
            //PathFigure.Segments.Add(line1);
            //PathFigure.Segments.Add(line2);

            //// przypisanie figury do geometrii
            //tmp.Figures.Clear(); //czyszczenie bedzie niezbedne przy shapefactory chyba?
            //tmp.Figures.Add(PathFigure);    // przypisanie figury trojkata do geometrii  
        }

        public override void MoveBy(Point point)
        {
            Canvas.SetTop(this.adaptedPath, point.Y);
            Canvas.SetLeft(this.adaptedPath, point.X);

            //PathFigure.StartPoint = point;
            SetStartPoint(point);

            /*if (point.Y < this.GetPointCollection()[2].Y + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2) //można wziąc też thickness z figury
            {
                point.Y = this.GetPointCollection()[2].Y + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2;
                Canvas.SetTop(this.adaptedPath, point.Y);
                SetStartPoint(point);
            }*/

            SetPointCollection();
        }

        public override void Resize(Point point)
        {
            // obliczenie polozenia lewego dolnego wierzcholka
            point1.X = Math.Min(point.X, startPoint.X);
            point1.Y = Math.Max(point.Y, startPoint.Y);
            PathFigure.StartPoint = point1;

            // obliczenie polozenia prawego dolnego wierzcholka
            point2.X = Math.Max(point.X, startPoint.X);
            point2.Y = Math.Max(point.Y, startPoint.Y);
            line1.Point = point2;

            // obliczenie polozenia gornego wierzcholka
            point3.X = MidPointX(point.X,startPoint.X);
            point3.Y = Math.Min(point.Y, startPoint.Y);
            line2.Point = point3;

            PathFigure.Segments.Clear(); //czyszczenie bedzie niezbedne przy shapefactory chyba?
            PathFigure.Segments.Insert(0,line1);
            PathFigure.Segments.Insert(1,line2);

            ((PathGeometry)adaptedGeometry).Figures.Clear(); //czyszczenie bedzie niezbedne przy shapefactory chyba?
            ((PathGeometry)adaptedGeometry).Figures.Insert(0,PathFigure);    // przypisanie figury trojkata do geometrii

            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }

        protected override void SetPointCollection()
        {
            // do zaznaczenia trojkata potrzebne sa wszystkie 3 wierzcholki
            PointsList.Insert(0, PathFigure.StartPoint);    // lewy dolny
            PointsList.Insert(1, line1.Point);              // prawy dolny
            PointsList.Insert(2, line2.Point);              // gorny
        }

        private double MidPointX(double a, double b)
        {
            return (a+b)/2;
        }
    }
}
