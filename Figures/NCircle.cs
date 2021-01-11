using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Strategy;
namespace NPaint.Figures
{
    [Serializable]
    class NCircle : NEllipse
    {
        private DrawingStrategy strategy;

        public NCircle() : base()
        {
            strategy = new DrawingStrategyClassic();
        }

        public override void Draw(Point point)
        {
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;

            Repaint();
        }

        public void SetStrategy(DrawingStrategy strategy)
        {
            this.strategy = strategy;
        }
    }
}
