using System.Windows;
using System.Windows.Media;
using NPaint.Figures;

namespace NPaint.Observer
{
    interface Observable
    {
        public void Attach(Figure figure);
        public void DetachAll();
        public void Notify_MoveBy(Point point);
        public void Notify_ChangeFillColor(Brush brush);
        public void Notify_ChangeBorderColor(Brush brush);
        public void Notify_ChangeTransparency(double value);
        public void Notify_ChangeBorderThickness(double value);
        public void Notify_IncreaseSize();
        public void Notify_DecreaseSize();
        public void Notify_AddSelectionVisualEffect();
        public void Notify_DeleteSelectionVisualEffect();
    }
}
