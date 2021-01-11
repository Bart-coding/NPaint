using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    abstract class MenuState
    {
        protected Figure Figure;
        protected Point StartPoint;

        public abstract void MouseLeftButtonDown(Point point);
        public virtual void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
        }
        public abstract void MouseMove(Point point);
    }
}
