using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class FigureState : MenuState
    {
        public FigureState(Figure prototype) 
        {
            Figure = prototype;
        }
        public override void MouseLeftButtonDown(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            Figure = (Figure)Figure.Clone();
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(StartPoint, point);
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
