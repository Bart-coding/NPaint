using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    abstract class MenuState
    {
        protected Figure Figure;
        public abstract void MouseMove(Point point);
        public abstract void MouseLeftButtonDown(Point point);
    }
}
