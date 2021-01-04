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
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMove(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
