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
        private Path ObservablePath;
        private Geometry ObservableGeometry;
        private List<Figure> Observers;
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
            // Notify(point);
        }
        public void Resize(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
