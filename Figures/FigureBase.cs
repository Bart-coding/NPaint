using System.Windows;
using System.Windows.Media;

namespace NPaint.Figures
{
    interface FigureBase
    {
        public void ChangeFillColor(Brush brush);
        public void ChangeBorderColor(Brush brush);
        public void ChangeBorderThickness(double value);
        public void ChangeBorderThicknessInsideGroup(double value, PointCollection pointCollectionOfSelection);
        public void ChangeTransparency(double value);
        public void MoveBy(Point point);
        public void MoveByInsideGroup(Point point);
        public void Draw(Point point);
        public void SetStartPoint(Point point);
    }
}
