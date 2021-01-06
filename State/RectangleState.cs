using System;
using System.Windows;
using System.Windows.Media;
using NPaint.Figures;

namespace NPaint.State
{
    class RectangleState : MenuState
    {
        //double widthShift, lengthShift = 0;
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NRectangle)shapeFactory.getFigure("Rectangle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMove(point);
        }

        public override void MouseLeftButtonUp(Point point)
        {
            ((MainWindow)Application.Current.MainWindow).SetSelectedFigure(Figure);
            /*lengthShift = 0;
            widthShift = 0;*/
        }

        public override void MouseMove(Point point)
        {
            Figure.Resize(point);//*czasem Figure = null exception
            //throw new NotImplementedException();
        }
    }
}
