using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    abstract class MenuState
    {
        protected Figure Figure;
        protected Point StartPoint;

        public abstract void MouseLeftButtonDown(Point point);
        public abstract void MouseLeftButtonUp(Point point);
        public abstract void MouseMove(Point point);
    }
}
