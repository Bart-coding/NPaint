using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Figures;

namespace NPaint.State
{
    class RectangleState : MenuState
    {
        double widthShift, lengthShift = 0;
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NRectangle)shapeFactory.getFigure("Rectangle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMoveToResize(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
            lengthShift = 0;
            widthShift = 0;
        }

        public override void MouseMoveToMove(Point point)//póki co nieużywane
        {
            if (lengthShift==0 && widthShift==0) //kod do utrzymywania myszki w tym samym miejscu w figurze podczas rysowania
            {
                lengthShift = point.Y - Figure.GetStartPoint().Y; //stała odległość myszki od środka figury
                widthShift =  point.X - Figure.GetStartPoint().X;
                //MessageBox.Show(lengthShift + "a" + widthShift + "a");
            }
           // MessageBox.Show(lengthShift + "a" + widthShift + "a");
            point.Y -= lengthShift; //podanie do metody od razu pktu startowgo
            point.X -= widthShift;

            Figure.MoveBy(point);
        }

        public override void MouseMoveToResize(Point point)
        {
            Figure.Resize(point);
            //throw new NotImplementedException();
        }
    }
}
