using System;
using System.Windows;
using NPaint.Observer;

namespace NPaint.State
{
    class SelectionState : MenuState
    {
        private ConcreteObservable observable;
        public override void MouseLeftButtonDown(Point point)
        {
            observable = new ConcreteObservable();
            observable.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(observable.ObservablePath);
        }

        public override void MouseMove(Point point)
        {
            observable.Resize(point);
        }
    }
}
