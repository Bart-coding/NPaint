using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NSquare : NRectangle
    {
        public NSquare() : base() {}
        public override void Resize(Point point)
        {
            double squarelength;

            //ustalenie dlugosci boku kwadratu
            squarelength = Math.Min(Math.Abs(startPoint.X - point.X), Math.Abs(startPoint.Y - point.Y));

            rect.Width = squarelength;
            rect.Height = squarelength;

            //ustalenie pozycji kwadratu
            rect.X = startPoint.X;

            if(startPoint.X > point.X)
            {
                rect.X = startPoint.X - squarelength;
            }

            rect.Y = startPoint.Y;

            if (startPoint.Y > point.Y)
            {
                rect.Y = startPoint.Y - squarelength;
            }

            Repaint();
        }
    }
}
