using System;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class PolygonState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            Figure = ShapeFactory.getShapeFactory().getFigure("Polygon") as NPolygon;
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMove(point);
        }

        public override void MouseMove(Point point)
        {
            Figure.Draw(point);
        }
    }
}