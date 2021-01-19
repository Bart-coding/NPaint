using NPaint.Figures;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace NPaint
{
    class ShapeFactory
    {
        private static readonly ShapeFactory shapeFactory = new ShapeFactory();
        private readonly Dictionary<string, Figure> prototypedFigures = new Dictionary<string, Figure>();

        private ShapeFactory()
        {
            this.CreateRectanglePrototype();
            this.CreateSquarePrototype();
            this.CreateEllipsePrototype();
            this.CreateCirclePrototype();
            this.CreateTrianglePrototype();
            this.CreatePolygonPrototype();
        }

        public static ShapeFactory GetShapeFactory()
        {
            return shapeFactory;
        }

        public Figure GetFigure (String figureType)
        {
            if (prototypedFigures.ContainsKey(figureType))
                return prototypedFigures[figureType];
            else
                return null;
        }
        
        private void CreateRectanglePrototype()
        {
            Path myPath = new Path();
            string type = "Rectangle";
            myPath.Tag = type;

            NRectangle rectangle = new NRectangle();
            rectangle.adaptedPath = myPath;

            prototypedFigures.Add(type, rectangle);
        }

        private void CreateSquarePrototype()
        {
            Path myPath = new Path();
            string type = "Square";
            myPath.Tag = type;

            NSquare square = new NSquare();
            square.adaptedPath = myPath;
            
            prototypedFigures.Add(type, square);
        }

        private void CreateEllipsePrototype()
        {
            Path myPath = new Path();
            string type = "Ellipse";
            myPath.Tag = type;

            NEllipse ellipse = new NEllipse();
            ellipse.adaptedPath = myPath;
            
            prototypedFigures.Add(type, ellipse);
        }

        private void CreateCirclePrototype()
        {
            Path myPath = new Path();
            string type = "Circle";
            myPath.Tag = type;

            NCircle circle = new NCircle();
            circle.adaptedPath = myPath;
            
            prototypedFigures.Add(type, circle);
        }

        private void CreateTrianglePrototype()
        {
            Path myPath = new Path();
            string type = "Triangle";
            myPath.Tag = type;

            NTriangle triangle = new NTriangle();
            triangle.adaptedPath = myPath;
            
            prototypedFigures.Add(type, triangle);
        }

        private void CreatePolygonPrototype()
        {
            NPolygon polygon = new NPolygon();
            string type = "Polygon";
            polygon.adaptedPath.Tag = type;
            prototypedFigures.Add(type, polygon);
        }
    }
}
