using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class SquareState :MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NSquare)shapeFactory.getFigure("Square");
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
