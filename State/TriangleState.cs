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
            Figure = new NTriangle();
            Figure.SetStartPoint(point);
            Figure.adaptedPath.Fill = Brushes.Red;
            Figure.adaptedPath.Stroke = Brushes.Black;
            Figure.adaptedPath.StrokeThickness = 2;
            Figure.ChangeTransparency(.1);
            ((MainWindow)Application.Current.MainWindow).canvas.Children.Add(Figure.adaptedPath);
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);
        }
    }
}
