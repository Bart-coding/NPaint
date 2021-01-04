using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using NPaint.Figures;

namespace NPaint
{
    [Serializable]
    [XmlRoot("FigureListClass")]//
    public class FigureListClass
    {
        [XmlArray("FigureList"),XmlArrayItem(typeof(Figure), ElementName="Figure")]
        public List<Figure> FigureList { get; set; }
    }
}
