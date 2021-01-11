using NPaint.Figures;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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
