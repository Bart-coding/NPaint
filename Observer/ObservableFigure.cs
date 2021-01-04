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
        }

        public void Detach(Figure figure)
        {
            Observers.Remove(figure);
        }

        public void Notify(Point point)
        {
            foreach(Figure figure in Observers)
            {
                figure.MoveBy(point);
            }
        }
        public override void MoveBy(Point point)
        {
            throw new NotImplementedException();
            //Notify(point);
        }
    }
}
