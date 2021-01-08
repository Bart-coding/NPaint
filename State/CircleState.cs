﻿using NPaint.Figures;
using System;
using System.Windows;

namespace NPaint.State
{
    class CircleState : MenuState
    {
        public override void MouseLeftButtonDown(Point point)
        {
            ShapeFactory shapeFactory = ShapeFactory.getShapeFactory();
            Figure = (NCircle)shapeFactory.getFigure("Circle");
            Figure.SetStartPoint(point);
            ((MainWindow)Application.Current.MainWindow).AddFigure(Figure);
            MouseMove(point);
        }

        public override void MouseMove(Point point)
        {
            Figure.Draw(point);            
        }
    }
}
