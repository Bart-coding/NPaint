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
            bool found = false;

            // przechodzimy po wszystkich dzieciach canvasa
            foreach (UIElement figure in FiguresPath)
            {
                if(figure.IsMouseOver)  // jezeli najechalismy myszka na figure
                {
                    CLickedFigurePath = figure as Path; // wlasciwie to znalezlismy Path dodana do canvasa
                    
                    // znajdujemy figure na podstawie kliknietego path
                    found = true;
                }
            }
            if (found)
            {
                List<Figure> figures = ((MainWindow)Application.Current.MainWindow).GetFigureList();
                foreach (Figure f in figures)
                {
                    if (f.adaptedPath.Equals(CLickedFigurePath))
                    {
                        // przypisanie obecnie wybranej figury w MainWindow
                        ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(f);
                    }
                }
                break; // przerywamy, bo juz znalezlismy dana figure}


            }

        public override void MouseMove(Point point)
        {
            // nothing to do here...
        }
    }
}
