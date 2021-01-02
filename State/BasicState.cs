using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPaint.State
{
    class BasicState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            UIElementCollection FiguresPath = ((MainWindow)Application.Current.MainWindow).canvas.Children;
            UIElement CLickedFigurePath = new UIElement();
            bool found = false; // zmienna pomocnicza okreslajaca czy kliknelismy w jakas figure

            // przechodzimy po wszystkich dzieciach canvasa
            foreach (UIElement figure in FiguresPath)
            {
                if(figure.IsMouseOver)  // jezeli najechalismy myszka na figure
                {
                    CLickedFigurePath = figure;
                    found = true;
                    break; // przerywamy, bo juz znalezlismy dana figure
                }
            }

            // jezeli kliknelismy w jakas figure
            if(found)
            {
                Path x = CLickedFigurePath as Path; // wlasciwie to znalezlismy Path dodana do canvasa
                x.Fill = Brushes.Green; // mozemy sobie cos zmienic
            }
        }

        public override void MouseMove(Point point)
        {
            // nothing to do here...
        }
    }
}
