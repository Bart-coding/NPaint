using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NPaint.Figures;

namespace NPaint.State
{
    class CursorState : MenuState
    {
        private double widthShift, lengthShift = 0;
        private double TriangleMargin;

        public override void MouseLeftButtonDown(Point point)
        {
            // jezeli kliknelismy w ta sama figure co poprzednio
            if (Figure != null && Figure.adaptedPath.IsMouseOver)
            {
                ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
                return;
            }

            // pobranie listy figur z MainWindow
            List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();

            // przechodzimy po wszystkich figurach
            foreach (Figure figure in figures)
            {
                if (figure.adaptedPath.IsMouseOver)  // jezeli najechalismy myszka na figure
                {
                    // wlasciwie to znalezlismy Path dodana do canvasa
                    ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(figure);
                    Figure = figure; // przypisanie obecnie wybranej figury do naszego Stanu
                    return; // przerywamy, bo juz znalezlismy kliknieta figure
                }
            }

            // jezeli kliknelismy gdzies indziej niz w figure
            ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
            Figure = null;
        }

        public override void MouseLeftButtonUp(Point point)
        {
            lengthShift = 0;
            widthShift = 0;
            TriangleMargin = 0;
        }

        public override void MouseMove(Point point)
        {
            if(Figure != null)
            {
                // gdy przesuwamy prostokat lub kwadrat
                if (Figure.GetType() == typeof(NRectangle) || Figure.GetType() == typeof(NSquare))
                {
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        lengthShift = point.Y - ((NRectangle)Figure).GetTopLeft().Y;
                        widthShift = point.X - ((NRectangle)Figure).GetTopLeft().X;
                    }
                    point.Y -= lengthShift;
                    point.X -= widthShift;
                    //Zabezpieczenie przed umieszczeniem figury na Menu
                    if (point.Y < 0 + Figure.GetBorderThickness() / 2) //można wziąc też thickness z figury
                    {
                        point.Y = 0 + Figure.GetBorderThickness() / 2;
                    }
                }
                // gdy przesuwamy elipse lub kolo
                else if (Figure.GetType() == typeof(NEllipse) || Figure.GetType() == typeof(NCircle))
                {
                    dynamic f_tmp = Figure;
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        Point center = f_tmp.GetCenterPoint();
                        lengthShift = point.Y - center.Y; //stała odległość myszki od środka figury
                        widthShift = point.X - center.X;
                    }
                    point.Y -= lengthShift;
                    point.X -= widthShift;
                    //Zabezpieczenie przed umieszczeniem figury na Menu
                    if (point.Y < f_tmp.adaptedGeometry.RadiusY + Figure.GetBorderThickness()/2) //można wziąc też thickness z figury
                    {
                        point.Y = f_tmp.adaptedGeometry.RadiusY + Figure.GetBorderThickness()/2;
                    }

                }
                // gdy przesywamy trojkat
                else if (Figure.GetType() == typeof(NTriangle))
                {
                    NTriangle tmp_Triangle = Figure as NTriangle;

                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        Point positionOfTriangle = ((NTriangle)Figure).GetTopCorner();

                        lengthShift = point.Y - positionOfTriangle.Y;
                        widthShift = point.X - positionOfTriangle.X;
                        TriangleMargin = ((NTriangle)Figure).CalculateMargin();
                    }
                    point.Y -= lengthShift;
                    point.X -= widthShift;
                    if (point.Y < 0 + TriangleMargin)
                    {
                        point.Y = TriangleMargin;
                    }
                }
                // gdy przesywamy dowolny wielokat
                else if (Figure.GetType() == typeof(NPolygon))
                {
                    if (lengthShift == 0 && widthShift == 0)
                    {
                        Point startPointOfPolygon = Figure.GetPointCollection().Last();
                        lengthShift = point.Y - startPointOfPolygon.Y;
                        widthShift = point.X - startPointOfPolygon.X;
                    }
                    point.Y -= lengthShift;
                    point.X -= widthShift;
                    if (Figure.GetPointCollection().Any(e => e.Y-(Figure.GetPointCollection().Last().Y-point.Y) < 0))
                    {
                        point.Y++;
                    }
                }
                Figure.MoveBy(point);
            }
        }
    }
}