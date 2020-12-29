using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NSquare : NRectangle
    {
        public NSquare(Point point) : base(point) {}
        public override void MoveBy(Point point)
        {
            throw new NotImplementedException();
        }

        public override void Resize(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
