using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    public abstract class Figure : FigureBase, ICloneable
    {
        protected Path adaptedPath;
        protected Geometry adaptedGeometry;
        protected Point startPoint;
        protected PointCollection PointsList;

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

        public abstract void MoveBy(Point point);

        public abstract void Resize(Point point);

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
