using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.Observer
{
    class ObservableFigure : NRectangle, Observable
    {
        private List<Figure> Observers;
        public ObservableFigure()
        {
            adaptedPath = new Path();
            adaptedPath.Fill = Brushes.Transparent;
            adaptedPath.StrokeThickness = 1;
            adaptedPath.Stroke = Brushes.Gray;
            adaptedPath.StrokeDashArray = new DoubleCollection() { 3 };
            adaptedGeometry = new RectangleGeometry();
            rect = new Rect();
            Observers = new List<Figure>();
        }
        public void Attach(Figure figure)
        {
            Observers.Add(figure);
            // a to do sprawdzania jakie figury sie zaznaczyly
            figure.ChangeFillColor(Brushes.Silver);
        }

        public void Detach(Figure figure)
        {
            Observers.Remove(figure);
        }

        public void Notify(Point point)
        {
            foreach(Figure figure in Observers)
            {
                // poki co to nie przejdzie, przez te shifty
                //figure.MoveBy(point);
            }
        }
        public override void MoveBy(Point point)
        {
            // poki co to nie przejdzie, przez te shifty
            //base.MoveBy(point);
            //Notify(point);
        }
        public bool Contains(Figure figure)
        {
            PointCollection points = figure.GetPointCollection();
            // poki wszystkie figury nie maja ustalonej PointCollection
            if (points.Count == 0)
                return false;
            foreach (Point point in points)
            {
                // jezeli zaznaczenie nie zawiera danego punktu figury to zwracamy falsz
                if(!this.rect.Contains(point))
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            // jezeli przeszlo po wszystkich punktach i nie zwrocilo falszu tzn. ze wszystkie punkty sie zawieraja w zaznaczeniu
            return true;
        }
    }
}
