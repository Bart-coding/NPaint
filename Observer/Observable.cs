using System.Drawing;
using NPaint.Figures;

namespace NPaint.Observer
{
    interface Observable
    {
        public void Attach(Figure figure);
        public void Detach(Figure figure);
        public void Notify(Point point);
    }
}
