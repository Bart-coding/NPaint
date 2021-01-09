using NPaint.Figures;
using System;
using System.Windows;
using System.Windows.Input;
using NPaint.Strategy;

namespace NPaint.State
{
    class CircleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NCircle)shapeFactory.getFigure("Circle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(point);

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
