using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    class NSquare : NRectangle
    {
        public NSquare() : base() {}
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
