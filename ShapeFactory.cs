using NPaint.Figures;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Data;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace NPaint
{
    class ShapeFactory
    {
        private static ShapeFactory shapeFactory = new ShapeFactory();
        private Dictionary<string, Figure> prototypedFigures = new Dictionary<string, Figure>();

        private ShapeFactory()
        {
            NRectangle rectangle = new NRectangle();
            RectangleGeometry rectangleG = new RectangleGeometry();
            Rect rect = new Rect();
            //rect.X = 200;
            //rect.Y = 100;
            //rect.Width = 200;
            //rect.Width = 200;
            //rect.Height = 50;
            rectangleG.Rect = rect;
            //rectangle.Rect = new Rect(100, 100, 200, 50);
            Path myPath = new Path();
            myPath.Fill = Brushes.Red;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 2;
            myPath.Data = rectangleG;

            rectangle.adaptedPath = myPath;
            //metoda do ustawienia domyslnej wielkosci
            rectangle.ChangeBorderColor(Brushes.Black);
            rectangle.ChangeBorderThickness(1);
            rectangle.ChangeTransparency(.1);
            prototypedFigures.Add("Rectangle", rectangle);



            NSquare square = new NSquare();
            //metoda do ustawienia domyslnej wielkosci
            square.ChangeBorderColor(Brushes.Black);
            square.ChangeBorderThickness(4);
            //square.ChangeTransparency(.5);
            prototypedFigures.Add("Square", square);

            NTriangle triangle = new NTriangle();
            //metoda do ustawienia domyslnej wielkosci
            triangle.ChangeBorderColor(Brushes.Black);
            triangle.ChangeBorderThickness(4);
            //triangle.ChangeTransparency(.5);
            prototypedFigures.Add("Triangle", triangle);

            NEllipse elipse = new NEllipse();
            //metoda do ustawienia domyslnej wielkosci
            elipse.ChangeBorderColor(Brushes.Black);
            elipse.ChangeBorderThickness(4);
            //elipse.ChangeTransparency(.5);
            prototypedFigures.Add("Elipse", elipse);

            NCircle circle = new NCircle();
            //metoda do ustawienia domyslnej wielkosci
            circle.ChangeBorderColor(Brushes.Black);
            circle.ChangeBorderThickness(4);
            //circle.ChangeTransparency(.5);
            prototypedFigures.Add("Circle", circle);

            NPolygon polygon = new NPolygon();
            //metoda do ustawienia domyslnej wielkosci
            polygon.ChangeBorderColor(Brushes.Black);
            polygon.ChangeBorderThickness(4);
            //polygon.ChangeTransparency(.5);
            prototypedFigures.Add("Polygon", polygon);



        }

        public static ShapeFactory getShapeFactory()
        {
            return shapeFactory;
        }

        public Figure getFigure (String figureType)
        {
            return prototypedFigures[figureType].Clone() as Figure; // https://www.dotnetperls.com/clone lub (Figure) prototypedFigures[figureType].Clone()
        }
    }
}
