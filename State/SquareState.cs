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
            MouseMoveToResize(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            base.MouseLeftButtonUp(point);
            //((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }

        public override void MouseMoveToMove(Point point)
        {
            base.MouseMoveToMove(point);
            //Figure.MoveBy(point);
        }

        public override void MouseMoveToResize(Point point)
        {
            Figure.Resize(point);
        }
    }
}
