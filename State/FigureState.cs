using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class FigureState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            // must implement inherited method
        }

        public override void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }

        public override void MouseMove(Point point)
        {
            Figure.Draw(StartPoint, point);
        }
    }
}
