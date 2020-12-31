using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Figures;

namespace NPaint.State
{
    class TriangleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NTriangle) shapeFactory.getFigure("Triangle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
