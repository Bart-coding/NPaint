using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using NPaint.Figures;
using NPaint.Observer;

namespace NPaint.State
{
    class SelectionState : MenuState
    {
        private bool ToMove;
        public override void MouseLeftButtonDown(Point point)
        {
            if(Figure != null) // jesli nie mamy zadnej figury to wiadomo, ze musimy dodac nowa
            {
                // jezeli myszka jest nad obserwowanym to chcemy przesuwac
                if (Figure.adaptedPath.IsMouseOver)
                {
                    ToMove = true;

                    return;
                }
            }

            // w przeciwnym wypadku tworzymy nowego obserwowanego
            Figure = new ObservableFigure();
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddObservable(Figure);
            /*ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();//*Pewnie lepiej fabryką prostokątów
            Figure = (NRectangle)shapeFactory.getFigure("Rectangle");*/

            //MouseMove(point);//Test*******
            ToMove = false;
        }

        public override void MouseLeftButtonUp(Point point)
        {
            // po puszczeniu myszki sprawdzamy jakie figury są zaznaczone
            ObservableFigure tmp = Figure as ObservableFigure; // aby moc wywolac ObservableFigure.Attach()

            // pobranie listy figur z MainWindow
            List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();

            // przechodzimy po wszystkich figurach
            foreach (Figure figure in figures)
            {
                if (tmp.Contains(figure) /*Added*/ && figure!=this.Figure)   // jezeli obserwowany obejmuje dana figure
                {
                    tmp.Attach(figure);     // dodajemy dana figure do listy obserwatorow
                }
            }
        }

        public override void MouseMove(Point point)
        {
            if (ToMove)
                Figure.MoveBy(point);
            else
                Figure.Resize(point);
        }
    }
}
