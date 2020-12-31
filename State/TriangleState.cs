using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.State
{
    class TriangleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            Figure = ShapeFactory.getShapeFactory().getFigure("Triangle") as NTriangle;
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
