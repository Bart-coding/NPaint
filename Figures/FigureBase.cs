using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    interface FigureBase
    {
        public void SetFields(Path path);
        public void ChangeFillColor(Brush brush);
        public void ChangeBorderColor(Brush brush);
        public void ChangeBorderThickness(double value);
        public void ChangeTransparency(double value);
        public void Draw(Point startPoint, Point currentPoint);
        public void MoveBy(Point point);
        public void MoveByInsideGroup(Vector shiftVector);
        public void IncreaseSize();
        public void DecreaseSize();
    }
}
