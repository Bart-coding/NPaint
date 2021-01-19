using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.Observer;

namespace NPaint.State
{
    class SelectionState : MenuState
    {
        private bool ToMove;
        private bool selectedAtLeastOne = false;
        private double widthShift, lengthShift = 0;

        public override void MouseLeftButtonDown(Point point)
        {
            if(Figure != null) // jesli nie mamy zadnej figury to wiadomo, ze musimy dodac nowa
            {
                // jezeli myszka jest nad obserwowanym to chcemy przesuwac
                if (Figure.adaptedPath.IsMouseOver)
                {
                    ToMove = true;
                    widthShift = 0;
                    lengthShift = 0;
                    return;
                }
            }
            // w przeciwnym wypadku tworzymy nowego obserwowanego
            if (selectedAtLeastOne == true) //Odpinamy wszystkich
            {
                ObservableFigure tmp = Figure as ObservableFigure;
                tmp.DetachAll();
                selectedAtLeastOne = false;
            }

            Figure = new ObservableFigure(new Path());
            StartPoint = point;
            ((MainWindow)Application.Current.MainWindow).AddObservable(Figure);

            ToMove = false;
        }

        public override void MouseLeftButtonUp(Point point)
        {
            if (selectedAtLeastOne)
            {
               return;
            }
            if (Figure != null)
            {
                ObservableFigure tmp = Figure as ObservableFigure; // aby moc wywolac np. ObservableFigure.Attach()

                // po puszczeniu myszki sprawdzamy jakie figury są zaznaczone
                // pobranie listy figur z MainWindow
                List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();

                // przechodzimy po wszystkich figurach
                foreach (Figure figure in figures)
                {
                    if ( tmp.Contains(figure) )   // jezeli obserwowany obejmuje dana figure
                    {
                        tmp.Attach(figure);     // dodajemy dana figure do listy obserwatorow
                        
                        if (selectedAtLeastOne == false)
                        {
                            selectedAtLeastOne = true;
                        }
                    }
                }
            } 
        }

        public override void MouseMove(Point point)
        {
            if(Figure != null)
            {
                // jezeli przesuwamy
                if (ToMove)
                {
                    if (lengthShift == 0 && widthShift == 0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
                    {
                        lengthShift = point.Y - ((ObservableFigure)Figure).GetTopLeft().Y; //stała odległość myszki od środka figury
                        widthShift = point.X - ((ObservableFigure)Figure).GetTopLeft().X;
                    }

                    point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
                    point.X -= widthShift;

                    //Zabezpieczenie przed umieszczeniem figury na Menu
                    if (point.Y < 0 + Figure.GetBorderThickness() / 2)
                    {
                        point.Y = 0 + Figure.GetBorderThickness() / 2;
                    }

                    Figure.MoveBy(point);
                }
                // jezeli rysujemy
                else
                {
                    Figure.Draw(StartPoint, point);
                }
            }
        }
    }
}
