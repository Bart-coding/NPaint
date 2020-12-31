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
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
            //throw new NotImplementedException();
        }
    }
}
