using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NRectangle : Figure
    {
        public NRectangle(Point point)
        {
            adaptedPath = new Path();
            adaptedGeometry = new RectangleGeometry();
            startPoint = point;
        }
        public override void MoveBy(Point point)
        {
            throw new NotImplementedException();
        }

        public override void Resize(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
