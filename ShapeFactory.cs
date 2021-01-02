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
            this.CreateRectanglePrototype();
            this.CreateSquarePrototype();
            this.CreateEllipsePrototype();
            this.CreateCirclePrototype();
            this.CreateTrianglePrototype();
            this.CreatePolygonPrototype();//
            
        }

        public static ShapeFactory getShapeFactory()
        {
            return shapeFactory;
        }

        public Figure getFigure (String figureType)
        {
            return prototypedFigures[figureType].Clone() as Figure; // https://www.dotnetperls.com/clone lub (Figure) prototypedFigures[figureType].Clone()
        }
        
        private void CreateRectanglePrototype()
        {
            
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
            NRectangle rectangle = new NRectangle();
            rectangle.adaptedPath = myPath;

            prototypedFigures.Add("Rectangle", rectangle);
        }

        private void CreateSquarePrototype()
        {
            
            RectangleGeometry squareG = new RectangleGeometry();
            Rect rect = new Rect();
            rect.X = 200;//
            rect.Y = 200;//
            rect.Width = 50;//
            rect.Height = 50;//
            squareG.Rect = rect;
            //rectangle.Rect = new Rect(100, 100, 200, 50);
            Path myPath = new Path();
            myPath.Data = squareG;
            NSquare square = new NSquare();
            square.adaptedPath = myPath;
            //metoda do ustawienia domyslnej wielkosci
            
            prototypedFigures.Add("Square", square);
        }

        private void CreateEllipsePrototype()
        {
            
       
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new Point(20, 20);//
            ellipseGeometry.RadiusX = 40;//
            ellipseGeometry.RadiusY = 60;//
            Path myPath = new Path();
            myPath.Fill = Brushes.Red;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 2;
            myPath.Data = ellipseGeometry;

            NEllipse ellipse = new NEllipse();
            ellipse.adaptedPath = myPath;
            
            prototypedFigures.Add("Ellipse", ellipse);
        }

        private void CreateCirclePrototype()
        {
            EllipseGeometry circleGeometry = new EllipseGeometry();
            circleGeometry.Center = new Point(40, 40);//
            circleGeometry.RadiusX = 50;//
            circleGeometry.RadiusY = 50;//
            Path myPath = new Path();
            myPath.Fill = Brushes.Red; //albo przy uzyciu metody circle
            myPath.StrokeThickness = 2;
            myPath.Data = circleGeometry;

            NCircle circle = new NCircle();
            circle.adaptedPath = myPath;
            
            prototypedFigures.Add("Circle", circle);
        }

        private void CreateTrianglePrototype()
        {
            Path myPath = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.IsClosed = true;
            pathFigure.StartPoint = new Point(50, 50); //na razie bez pktu poczatkowego
            
            pathFigure.Segments.Add(new LineSegment(new Point(50, 100), true)); //bez wielkosci
            pathFigure.Segments.Add(new LineSegment(new Point(100, 100), true));
            pathGeometry.Figures.Add(pathFigure);
            myPath.Data = pathGeometry;
            NTriangle triangle = new NTriangle(myPath);
            
            prototypedFigures.Add("Triangle", triangle);
        }

        private void CreatePolygonPrototype()/////
        {
            NPolygon polygon = new NPolygon();

            prototypedFigures.Add("Polygon", polygon);
        }
    }
}
