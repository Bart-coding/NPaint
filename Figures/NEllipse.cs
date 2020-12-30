using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NEllipse : Figure
    {
        public NEllipse()
        {
            adaptedPath = new Path();
            adaptedGeometry = new EllipseGeometry();
            //startPoint = point;
        }

        public override void MoveBy(Point point)
        {
            throw new System.NotImplementedException();
        }

        public override void Resize(Point point)
        {
            throw new System.NotImplementedException();
        }
    }
}
