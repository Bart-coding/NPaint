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
            adaptedPath.Data = adaptedGeometry;
        }

        public override void MoveBy(Point point)
        {
            // waiting for implementation
        }
        public override void MoveByInsideGroup(Point point)
        {
            // waiting for implementation
        }

        public override void Draw(Point point)
        {
            // waiting for implementation
        }

        protected override void SetPointCollection()
        {
            // waiting for implementation
        }

        public override void DecreaseSize()
        {
            // waiting for implementation
        }
        public override void IncreaseSize()
        {
            // waiting for implementation
        }

        protected override void Repaint()
        {
            // waiting for implementation
        }

        public override void ChangeBorderThickness(double value)
        {
            //if
            adaptedPath.StrokeThickness = value;
        }

        public override void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection)
        {
            // waiting for implementation
        }
    }
}
