using NPaint.Figures;
using NPaint.Strategy;
using System.Windows;
using System.Windows.Input;

namespace NPaint.State
{
    class CircleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NCircle)shapeFactory.getFigure("Circle");
            Figure.SetStartPoint(point);
            Figure.Draw(point);
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

        public override void MouseMove(Point point)
        {
            Figure.Draw(point);
        }
    }
}