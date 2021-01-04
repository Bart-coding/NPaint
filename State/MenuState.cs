using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    abstract class MenuState
    {
        public Figure Figure; //Było protected; można przemyśleć
        public abstract void MouseMoveToResize(Point point);
        public abstract void MouseMoveToMove(Point point);
        public abstract void MouseLeftButtonDown(Point point);
        public abstract void MouseLeftButtonUp(Point point);
    }
}
