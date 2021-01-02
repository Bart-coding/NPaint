using System;
using System.Windows;
using NPaint.Observer;

namespace NPaint.State
{
    class SelectionState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            Figure = new ConcreteObservable();
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
