using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.Figures
{
    [Serializable]
    class NCircle : NEllipse
    {
        public NCircle() : base() {}

        public override void Resize(Point point)
        {
            // obliczenie polozenia elipsy na osi XY
            CenterPoint = MidPoint(point, startPoint);

            // obliczenie długości promienia
            double radius = Math.Abs(Math.Sqrt(Math.Pow(startPoint.X-point.X, 2) + Math.Pow(startPoint.Y - point.Y, 2)))/2;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;
            //w momencie, gdy koło miałoby wchodzić na menu, wartości nie są aktualizowane, ale wygląda to lipnie i używa się tego lipnie - do poprawy, na razie pushuję takie bo działa. <-- najdłuższy komentarz na dzikim zachodzie
            if(CenterPoint.Y - radius - ((MainWindow)System.Windows.Application.Current.MainWindow).BorderThicknessySlider.Value > 0)
            {
                tmp.Center = CenterPoint;
                tmp.RadiusX = radius;
                tmp.RadiusY = radius;
            }

            // przypisanie zmienionej geometrii do Path
            adaptedPath.Data = adaptedGeometry;

            SetPointCollection();
        }
    }
}
