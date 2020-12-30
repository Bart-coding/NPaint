using System.Windows;
using System.Windows.Media;

namespace NPaint.Figures
{
    interface FigureBase
    {
        public void ChangeFillColor(Brush brush);
        public void ChangeBorderColor(Brush brush);
        public void ChangeBorderThickness(int value);
        public void ChangeTransparency(double value);
        public void MoveBy(Point point);
        public void Resize(Point point);

        public void SetStartPoint(Point point);
    }
}
