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
            NRectangle rectangle = new NRectangle(myPath);
            prototypedFigures.Add(type, rectangle);
        }

        private void CreateSquarePrototype()
        {
            Path myPath = new Path();
            string type = "Square";
            myPath.Tag = type;
            NSquare square = new NSquare(myPath);
            prototypedFigures.Add(type, square);
        }

        private void CreateEllipsePrototype()
        {
            Path myPath = new Path();
            string type = "Ellipse";
            myPath.Tag = type;
            NEllipse ellipse = new NEllipse(myPath);
            prototypedFigures.Add(type, ellipse);
        }

        private void CreateCirclePrototype()
        {
            Path myPath = new Path();
            string type = "Circle";
            myPath.Tag = type;
            NCircle circle = new NCircle(myPath);
            prototypedFigures.Add(type, circle);
        }

        private void CreateTrianglePrototype()
        {
            Path myPath = new Path();
            string type = "Triangle";
            myPath.Tag = type;
            NTriangle triangle = new NTriangle(myPath);
            prototypedFigures.Add(type, triangle);
        }

        private void CreatePolygonPrototype()
        {
            Path myPath = new Path();
            string type = "Polygon";
            myPath.Tag = type;
            NPolygon polygon = new NPolygon(myPath);
            prototypedFigures.Add(type, polygon);
        }
    }
}
