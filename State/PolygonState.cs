using System;
using System.Windows;

namespace NPaint.State
{
    class PolygonState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            throw new NotImplementedException();
            //MouseMove(point);
        }

        public override void MouseMoveToMove(Point point)
        {
            throw new NotImplementedException();
        }

        public override void MouseMoveToResize(Point point)
        {
            throw new NotImplementedException();
        }
    }
}