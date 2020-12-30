using System;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class RectangleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            NRectangle newRectangle = (NRectangle)shapeFactory.getFigure("Rectangle");
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(newRectangle.adaptedPath);
            throw new NotImplementedException();
        }

        public override void MouseMove(Point point)
        {
            throw new NotImplementedException();
            // resize
        }
    }
}
