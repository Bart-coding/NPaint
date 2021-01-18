using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class EllipseState : FigureState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NEllipse)shapeFactory.getFigure("Ellipse");
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            Figure.Draw(StartPoint, point);
        }
    }
}
