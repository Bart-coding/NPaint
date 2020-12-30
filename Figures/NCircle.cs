using System;
using System.Windows;

namespace NPaint.Figures
{
    class NCircle : NEllipse
    {
        public NCircle() : base() {}
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
