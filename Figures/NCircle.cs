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

        public override void Draw(Point startPoint, Point currentPoint)
        {
            adaptedGeometry = strategy.ChangeGeometry(adaptedGeometry, startPoint, currentPoint);
            CenterPoint = ((EllipseGeometry)adaptedGeometry).Center;
            Repaint();
        }

        public void SetStrategy(DrawingStrategy strategy)
        {
            this.strategy = strategy;
        }
    }
}
