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
            //Figure = new NRectangle();
            Figure.SetStartPoint(point);
            //Figure.adaptedPath.Fill = Brushes.LemonChiffon;
            //Figure.adaptedPath.Stroke = Brushes.Black;
            //Figure.adaptedPath.StrokeThickness = 1;
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
            //throw new NotImplementedException();
        }
    }
}
