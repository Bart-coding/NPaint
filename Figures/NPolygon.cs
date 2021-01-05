using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NPolygon : Figure
    {
        public NPolygon()
        {
            adaptedPath = new Path();
            adaptedGeometry = new PathGeometry();
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

        protected override void SetPointCollection()
        {
            throw new NotImplementedException();
        }
    }
}
