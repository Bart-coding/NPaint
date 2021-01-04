using System;
using System.Windows;
using NPaint.Observer;

namespace NPaint.State
{
    class SelectionState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            Figure = new ObservableFigure();
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddObservable(Figure);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            // nothing to do here...
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
