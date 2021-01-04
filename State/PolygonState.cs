using System;
using System.Windows;

namespace NPaint.State
{
    class PolygonState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            throw new NotImplementedException();
            //MouseMove(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }

        public override void MouseMove(Point point)
        {
            throw new NotImplementedException();
        }
    }
}