using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Strategy;
namespace NPaint.Figures
{
    [Serializable]
    class NCircle : NEllipse
    {
        private DrawingStrategy strategy;

        public NCircle() : base() {}

        public override void Draw(Point point)
        {
            // obliczenie polozenia elipsy na osi XY
            CenterPoint = MidPoint(point, startPoint);

            // obliczenie długości promienia
            double radius = Math.Abs(Math.Sqrt(Math.Pow(startPoint.X-point.X, 2) + Math.Pow(startPoint.Y - point.Y, 2)))/2;

            // przypisanie wyliczonych wartosci do zmiennej (geometrii)
            EllipseGeometry tmp = adaptedGeometry as EllipseGeometry;

            //strategy.draw(tmp, startPoint, point);



            ////w momencie, gdy koło miałoby wchodzić na menu, wartości nie są aktualizowane, ale wygląda to lipnie i używa się tego lipnie
            //if(CenterPoint.Y - radius - ((MainWindow)System.Windows.Application.Current.MainWindow).BorderThicknessySlider.Value > 0)
            //{
            //    tmp.Center = CenterPoint;
            //    tmp.RadiusX = radius;
            //    tmp.RadiusY = radius;
            //}

            Repaint();
        }
    }
}
