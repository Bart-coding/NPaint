using NPaint.Figures;
using NPaint.Strategy;
using System.Windows;
using System.Windows.Input;

namespace NPaint.State
{
    class CircleState : FigureState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NCircle)shapeFactory.getFigure("Circle");
            StartPoint = point;
            Figure.Draw(StartPoint, point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);

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