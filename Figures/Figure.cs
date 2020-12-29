using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    public class Figure : FigureBase, ICloneable
    {
        private Path adaptedPath;
        private Geometry adaptedGeometry;
        private Point startPoint;
        private PointCollection PointsList;

        public void ChangeFillColor(Brush brush)
        {
            adaptedPath.Fill = brush;
        }

        public void ChangeBorderColor(Brush brush)
        {
            adaptedPath.Stroke = brush;
        }

        public void ChangeBorderThickness(int value)
        {
            adaptedPath.StrokeThickness = value;
        }

        public void ChangeTransparency(double value)
        {
            Brush brush = adaptedPath.Fill;
            brush.Opacity = value;
            adaptedPath.Fill = brush;
        }

        public void MoveBy(Point point)
        {
            throw new NotImplementedException();
        }

        public void Resize(Point point)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
