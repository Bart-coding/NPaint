using NPaint.Figures;
using NPaint.Strategy;
using System.Windows;
using System.Windows.Input;

namespace NPaint.State
{
    class CircleState : FigureState
    {
        public CircleState(Figure prototype) : base(prototype) { }
        public override void MouseLeftButtonDown(Point point)
        {
            // must implement inherited method
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            Figure = (Figure)Figure.Clone();
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(StartPoint, point);


            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                ((NCircle)Figure).SetStrategy(new DrawingStrategyCenter());
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                ((NCircle)Figure).SetStrategy(new DrawingStrategyFitted());
            }
        }
    }
}