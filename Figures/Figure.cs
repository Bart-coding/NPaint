using System;
using System.Drawing;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using Path = System.Windows.Shapes.Path;////

namespace NPaint.Figures
{
    public abstract class Figure : FigureBase, ICloneable
    {
        protected Path adaptedPath;
        protected Geometry adaptedGeometry;
        protected Point startPoint;
        protected PointCollection PointsList;

        public void ChangeFillColor(Brush brush)
        {
            adaptedPath.Fill = brush;
        }

        public void ChangeBorderColor(Brush brush)
        {
            adaptedPath.Stroke = brush;
        }

        public void ChangeBorderThickness(int value)
        {
            adaptedPath.StrokeThickness = value;
        }

        public void ChangeTransparency(double value)
        {
            Brush brush = adaptedPath.Fill; //jakiś wyjątek wywala
            brush.Opacity = value;
            adaptedPath.Fill = brush;
        }

        public void SetStartPoint (Point point)
        {
            startPoint = point;
        }

        public abstract void MoveBy(Point point);

        public abstract void Resize(Point point);


        public object Clone()
        {
            //throw new NotImplementedException();
            Figure clonedFigure = this.MemberwiseClone() as Figure;
            clonedFigure.adaptedPath = clonePath();
            clonedFigure.adaptedGeometry = this.adaptedGeometry.Clone(); //lub metoda ala clonePath
            clonedFigure.startPoint.X = this.startPoint.X;
            clonedFigure.startPoint.Y = this.startPoint.Y;
            if (this.PointsList != null)
                clonedFigure.PointsList = this.PointsList.Clone();//
            else
                clonedFigure.PointsList = null;

            return clonedFigure;

        }
        private Path clonePath()
        {
            string pathXaml = XamlWriter.Save(this.adaptedPath);
            StringReader stringReader = new StringReader(pathXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (Path) XamlReader.Load(xmlReader);
        }
    }
}
