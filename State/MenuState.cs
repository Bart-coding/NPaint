using System.Windows;

namespace NPaint.State
{
    abstract class MenuState
    {
        public abstract void MouseMove(Point point);
        public abstract void MouseLeftButtonDown(Point point);
    }
}
