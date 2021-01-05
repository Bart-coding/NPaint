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
        public override void MouseLeftButtonDown(Point point)
        {
            if (Figure != null && Figure.adaptedPath.IsMouseOver)
            {
                return;
            }

            // pobranie listy figur z MainWindow
            List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();

            // przechodzimy po wszystkich dzieciach canvasa
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
            }

        public override void MouseMove(Point point)
        {
            if(Figure != null)
            {
                // gdy przesuwamy prostokat lub kwadrat
                if (Figure.GetType() == typeof(NRectangle) || Figure.GetType() == typeof(NSquare) ||/*optionally*/ Figure.GetType() == typeof(ObservableFigure))
                {
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        lengthShift = point.Y - Figure.GetStartPoint().Y; //stała odległość myszki od środka figury
                        widthShift = point.X - Figure.GetStartPoint().X;

                        
                    }
                    point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
                    point.X -= widthShift;
                    //Zabezpieczenie przed umieszczeniem figury na Menu
                    if (point.Y < 0 + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2) //można wziąc też thickness z figury
                    {
                        point.Y = 0 + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2;
                    }
                }
                else if (Figure.GetType() == typeof(NEllipse) || Figure.GetType() == typeof(NCircle))
                {
                    dynamic f_tmp = Figure;
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        //dynamic f_tmp = Figure;
                        Point center = f_tmp.GetCenterPoint();
                        lengthShift = point.Y - center.Y; //stała odległość myszki od środka figury
                        widthShift = point.X - center.X;

                        
                    }
                    point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
                    point.X -= widthShift;
                    //Zabezpieczenie przed umieszczeniem figury na Menu
                    if (point.Y < f_tmp.adaptedGeometry.RadiusY + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2) //można wziąc też thickness z figury
                    {
                        point.Y = f_tmp.adaptedGeometry.RadiusY + ((MainWindow)Application.Current.MainWindow).BorderThicknessySlider.Value / 2;
                    }

                }

                

                Figure.MoveBy(point);
            }
        }
    }
}