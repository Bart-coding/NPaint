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
        private static readonly ShapeFactory shapeFactory = new ShapeFactory();
        private Dictionary<string, Figure> prototypedFigures = new Dictionary<string, Figure>();

        private ShapeFactory()
        {
            this.CreateRectanglePrototype();
            this.CreateSquarePrototype();
            this.CreateEllipsePrototype();
            this.CreateCirclePrototype();
            this.CreateTrianglePrototype();
            this.CreatePolygonPrototype();
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
            rect.X = 100;
            rect.Y = 100;
            rect.Width = 100;
            rect.Height = 50;
            rectangleG.Rect = rect;
            Path myPath = new Path();
            myPath.Data = rectangleG;
            string type = "Rectangle";
            myPath.Tag = type;/////
            NRectangle rectangle = new NRectangle();
            rectangle.adaptedPath = myPath;

            prototypedFigures.Add(type, rectangle);
        }

        private void CreateSquarePrototype()
        {
            
            RectangleGeometry squareG = new RectangleGeometry();
            Rect rect = new Rect();
            rect.X = 100;
            rect.Y = 100;
            rect.Width = 50;//
            rect.Height = 50;//
            squareG.Rect = rect;
            Path myPath = new Path();
            string type = "Square";
            myPath.Tag = type;/////
            myPath.Data = squareG;
            NSquare square = new NSquare();
            square.adaptedPath = myPath;
            
            prototypedFigures.Add(type, square);
        }

        private void CreateEllipsePrototype()
        {
            
       
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new Point(200, 200);
            ellipseGeometry.RadiusX = 40;
            ellipseGeometry.RadiusY = 60;
            Path myPath = new Path();
            string type = "Ellipse";
            myPath.Tag = type;/////
            myPath.Data = ellipseGeometry;

            NEllipse ellipse = new NEllipse();
            ellipse.adaptedPath = myPath;
            
            prototypedFigures.Add(type, ellipse);
        }

        private void CreateCirclePrototype()
        {
            EllipseGeometry circleGeometry = new EllipseGeometry();
            circleGeometry.Center = new Point(200, 200);
            circleGeometry.RadiusX = 50;//
            circleGeometry.RadiusY = 50;//
            Path myPath = new Path();
            string type = "Circle";
            myPath.Tag = type;/////
            myPath.Data = circleGeometry;

            NCircle circle = new NCircle();
            circle.adaptedPath = myPath;
            
            prototypedFigures.Add(type, circle);
        }

        private void CreateTrianglePrototype()
        {
            Path myPath = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.IsClosed = true;

            pathFigure.StartPoint = new Point(200, 200);
            pathFigure.Segments.Add(new LineSegment(new Point(50, 100), true));
            pathFigure.Segments.Add(new LineSegment(new Point(100, 100), true));
            pathGeometry.Figures.Add(pathFigure);

            string type = "Triangle";
            myPath.Tag = type;
            myPath.Data = pathGeometry;
            NTriangle triangle = new NTriangle();
            triangle.adaptedPath = myPath;
            
            prototypedFigures.Add(type, triangle);
        }

        private void CreatePolygonPrototype()/////
        {

            NPolygon polygon = new NPolygon();
            string type = "Polygon";
            polygon.adaptedPath.Tag = type;
            prototypedFigures.Add(type, polygon);
        }
    }
}
