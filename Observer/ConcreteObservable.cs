using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.Observer
{
    class ConcreteObservable : Observable
    {
        private Point startPoint;
        public Path ObservablePath;
        private RectangleGeometry ObservableGeometry;
        private Rect rect;
        private List<Figure> Observers;
        public ConcreteObservable()
        {
            startPoint = new Point();
            ObservablePath = new Path();
            ObservablePath.Fill = Brushes.Transparent;
            ObservablePath.StrokeThickness = 1;
            ObservablePath.Stroke = Brushes.Gray;
            ObservablePath.StrokeDashArray = new DoubleCollection() { 3 };
            ObservableGeometry = new RectangleGeometry();
            rect = new Rect();
            Observers = new List<Figure>();
            ObservableGeometry.Rect = rect;
            ObservablePath.Data = ObservableGeometry;
        }
        public void SetStartPoint(Point point)
        {
            startPoint = point;
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
        public void MoveBy(Point point)
        {
            throw new NotImplementedException();
            //Notify(point);
        }
        public void Resize(Point point)
        {
            // wersja z mozliwoscia rysowania w kazdym z czterech kierunkow

            // obliczenie polozenia prostokata na osi XY
            double x = Math.Min(point.X, startPoint.X);
            double y = Math.Min(point.Y, startPoint.Y);

            // obliczenie wysokosci i szerokosci prostokata
            double width = Math.Max(point.X, startPoint.X) - x;
            double height = Math.Max(point.Y, startPoint.Y) - y;

            // przypisanie wyliczonych wartosci do zmiennej
            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            ObservableGeometry.Rect = rect;

            // przypisanie zmienionej geometrii do Path
            ObservablePath.Data = ObservableGeometry;
        }
    }
}
