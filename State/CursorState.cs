using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.Observer;

namespace NPaint.State
{
    class CursorState : MenuState
    {
        double widthShift, lengthShift = 0;
        double TriangleMargin;
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

        public override void MouseLeftButtonUp()
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
                    point.Y -= lengthShift; //podanie do metody od razu pktu startowego
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
                        //dynamic f_tmp = Figure;
                        //Point center = f_tmp.GetLeftDownCorner(); 
                        // dlaczego zmienione? powinno byc getcenterpoint
                        Point center = f_tmp.GetCenterPoint();
                        lengthShift = point.Y - center.Y; //stała odległość myszki od środka figury
                        widthShift = point.X - center.X;

                        
                    }
                    point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
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
                        //Vector vector = VisualTreeHelper.GetOffset(Figure.adaptedPath);
                        //Point positionOfTriangle = new Point(vector.X, vector.Y);
                        //Point positionOfTriangle = ((NTriangle)Figure).GetLeftDownCorner();
                        Point positionOfTriangle = ((NTriangle)Figure).GetTopCorner();

                        // triangleTopDistanceFromStartPoint = System.Math.Abs(position.Y - tmp_Triangle.GetPointCollection()[2].Y);

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
                    // waiting for implementation
                }

                Figure.MoveBy(point);
            }
        }
    }
}