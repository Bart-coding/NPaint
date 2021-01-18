using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class TriangleState : FigureState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            Figure = ShapeFactory.getShapeFactory().getFigure("Triangle") as NTriangle;
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(StartPoint, point);
        }
    }
}
