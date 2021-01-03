using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Figures;

namespace NPaint.State
{
    class RectangleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NRectangle)shapeFactory.getFigure("Rectangle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMoveToResize(point);
        }

        public override void MouseMoveToMove(Point point)
        {
            Figure.MoveBy(point);
        }

        public override void MouseMoveToResize(Point point)
        {
            Figure.Resize(point);
            //throw new NotImplementedException();
        }
    }
}
