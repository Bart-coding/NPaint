using System;
using System.Linq;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class PolygonState : MenuState
    {
        bool IsClosed = false;
        double margin = 5; // odleglosc jaka trzeba kliknac od StartPointa, zeby zakonczyc rysowanie figury
        public override void MouseLeftButtonDown(Point point)
        {
            // jezeli zaczynamy rysowac figure
            if (Figure == null || IsClosed == true || (Figure as NPolygon).PathFigure.IsClosed == true)
            {
                Figure = ShapeFactory.getShapeFactory().getFigure("Polygon") as NPolygon;
                Figure.SetStartPoint(point);
                ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
                Figure.Draw(point);
                IsClosed = false;
            }
            // w przeciwnym wypadku updetujemy widok figury
            else
            {
                Figure.Draw(point);
            }
        }

        public override void MouseMove(Point point)
        {
            Figure.Draw(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            // wyznaczamy punkt w ktorym konczy sie obecna linia
            Point StartPoint = Figure.GetStartPoint();

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