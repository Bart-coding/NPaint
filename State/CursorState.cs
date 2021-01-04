using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.State
{
    class CursorState : MenuState
    {
        double widthShift, lengthShift = 0;
        public override void MouseLeftButtonDown(Point point)
        {
            UIElementCollection FiguresPath = ((MainWindow)Application.Current.MainWindow).canvas.Children;
            Path CLickedFigurePath = null;

            // przechodzimy po wszystkich dzieciach canvasa
            foreach (UIElement figure in FiguresPath)
            {
                if (figure.IsMouseOver)  // jezeli najechalismy myszka na figure
                {
                    CLickedFigurePath = figure as Path; // wlasciwie to znalezlismy Path dodana do canvasa

                    // pobranie listy figur z MainWindow
                    List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();

                    // znajdujemy figure na podstawie kliknietego path
                    foreach (Figure f in figures)
                    {
                        if (f.adaptedPath.Equals(CLickedFigurePath))
                        {
                            // przypisanie obecnie wybranej figury w MainWindow
                            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(f);
                            Figure = f; // przypisanie obecnie wybranej figury do naszego Stanu
                            break; // przerywamy, bo juz znalezlismy figure zawieracjaca dana zmienna path
                        }
                    }
                    break; // przerywamy, bo juz znalezlismy kliknieta figure
                }
            }
            // jezeli kliknelismy gdzies indziej niz w figure
            if (CLickedFigurePath == null)
            {
                ((MainWindow)Application.Current.MainWindow).ResetSelectedFigure();
                Figure = null;
            }
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
                if (Figure.GetType() == typeof(NRectangle) || Figure.GetType() == typeof(NSquare))
                {
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        lengthShift = point.Y - Figure.GetStartPoint().Y; //stała odległość myszki od środka figury
                        widthShift = point.X - Figure.GetStartPoint().X;
                    }
                    point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
                    point.X -= widthShift;
                }
                Figure.MoveBy(point);
            }
        }
    }
}