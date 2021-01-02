using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NPaint.Figures;

namespace NPaint.State
{
    class BasicState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            UIElementCollection FiguresPath = ((MainWindow)Application.Current.MainWindow).canvas.Children;
            Path CLickedFigurePath = new Path();
            bool found = false; // zmienna pomocnicza okreslajaca czy kliknelismy w jakas figure

            // przechodzimy po wszystkich dzieciach canvasa
            foreach (UIElement figure in FiguresPath)
            {
                if(figure.IsMouseOver)  // jezeli najechalismy myszka na figure
                {
                    CLickedFigurePath = figure as Path; // wlasciwie to znalezlismy Path dodana do canvasa
                    found = true;
                    break; // przerywamy, bo juz znalezlismy dana figure
                }
            }

            // jezeli kliknelismy w jakas figure
            if(found)
            {
                List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();
                // znajdujemy figure na podstawie kliknietego path
                foreach(Figure f in figures)
                {
                    if(f.adaptedPath.Equals(CLickedFigurePath))
                    {
                        // przypisanie obecnie wybranej figury w MainWindow
                        ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(f);
                    }
                }
            }
        }

        public override void MouseMove(Point point)
        {
            // nothing to do here...
        }
    }
}
