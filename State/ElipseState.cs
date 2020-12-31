using System;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class EllipseState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NEllipse)shapeFactory.getFigure("Ellipse");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
