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
            /*if (((MainWindow)Application.Current.MainWindow).observer == true)
            {
                // źle te gety pobierają; przesuwanie w grp działa, ale dopiero po zwykłym MoveBy Trójkąta
                // dlatego też pewnie jest ten bug z przesuwaniem na początku
                //Ten GetTop i GetLeft to na starcie NaN
                
                y = Canvas.GetTop(this.adaptedPath) - point.Y;
                x = Canvas.GetLeft(this.adaptedPath) - point.X;
               
            }*/
            

            double y = point.Y;
            double x = point.X;

            //adaptedGeometry.Transform = adaptedGeometry.Transform.TransformBounds(rect);
            //adaptedGeometry.Bounds.Y = y;

            Canvas.SetTop(this.adaptedPath, y);
            Canvas.SetLeft(this.adaptedPath, x);
            //PathFigure.StartPoint = new Point(x, y);

            
            //SetStartPoint(point);

            /*if (point.Y < this.GetPointCollection()[2].Y + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2) //można wziąc też thickness z figury
            {
                point.Y = this.GetPointCollection()[2].Y + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2;
                Canvas.SetTop(this.adaptedPath, point.Y);
                SetStartPoint(point);
            }*/

            SetPointCollection();
        }

        public override void MoveByInsideGroup(Point point)
        {
            
            //MessageBox.Show(Canvas.GetTop(this.adaptedPath) + "");
            Canvas.SetTop(this.adaptedPath, this.adaptedPath.ActualHeight - point.Y);
            Canvas.SetLeft(this.adaptedPath, this.adaptedPath.ActualWidth - point.X);
            /*PathFigure.StartPoint.X -= point.X;
            double y = this.GetLeftDownCorner().Y - point.Y;
            double x2 = this.line1.Point.X - point.X;
            double y2 = this.line1.Point.Y - point.Y;
            double x3 = this.line2.Point.X - point.X;
            double y3 = this.line2.Point.Y - point.Y;*/


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

            PathFigure.Segments.Clear(); //czyszczenie niezbedne nwm dlaczego insert powinien zalatwic sprawe
            PathFigure.Segments.Insert(0, line1);
            PathFigure.Segments.Insert(1, line2);

            ((PathGeometry)adaptedGeometry).Figures.Clear(); //czyszczenie ten sam problem
            ((PathGeometry)adaptedGeometry).Figures.Insert(0,PathFigure);    // przypisanie figury trojkata do geometrii

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
    }
}
