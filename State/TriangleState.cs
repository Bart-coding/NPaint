using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class TriangleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            Figure = ShapeFactory.getShapeFactory().getFigure("Triangle") as NTriangle;
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(StartPoint, point);
        }

        public override void MouseMove(Point point)
        {
            Figure.Draw(StartPoint, point);
        }
    }
}
