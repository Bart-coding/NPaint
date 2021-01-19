using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class PolygonState : FigureState
    {
        public PolygonState(Figure prototype) : base(prototype) { }

        private bool IsClosed = true;
        private readonly double margin = 10; // odleglosc jaka trzeba kliknac od StartPointa, zeby zakonczyc rysowanie figury

        public override void MouseLeftButtonDown(Point point)
        {
            // jezeli zaczynamy rysowac figure
            if (IsClosed == true || (Figure as NPolygon).PathFigure.IsClosed == true)
            {
                ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
                Figure = (Figure)prototype.Clone();
                StartPoint = point;
                ((NPolygon)Figure).SetStartPoint(point);
                ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
                Figure.Draw(StartPoint, point);
                IsClosed = false;
            }
            // w przeciwnym wypadku updetujemy widok figury
            else
            {
                Figure.Draw(StartPoint, point);
            }
        }

        public override void MouseLeftButtonUp(Point point)
        {
            // wyznaczamy punkt w ktorym konczy sie obecna linia
            Point StartPoint = ((NPolygon)Figure).GetStartPoint();

            // sprawdzamy czy puscilismy myszkę w poblizu StartPointa, jezeli tak to zamykamy figure
            if (StartPoint.X - margin <= point.X && StartPoint.X + margin >= point.X && StartPoint.Y - margin <= point.Y && StartPoint.Y + margin >= point.Y)
            {
                ((NPolygon)Figure).CloseFigure();
                ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
                IsClosed = true;
            }
            // w przeciwnym wypadku ustawiamy koniec danej linii w kliknietym punkcie
            else
            {
                ((NPolygon)Figure).CloseLine(point);
            }
        }
    }
}