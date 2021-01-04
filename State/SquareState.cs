using System;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class SquareState : RectangleState //:MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NSquare)shapeFactory.getFigure("Square");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMove(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            base.MouseLeftButtonUp(point);
            //((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
