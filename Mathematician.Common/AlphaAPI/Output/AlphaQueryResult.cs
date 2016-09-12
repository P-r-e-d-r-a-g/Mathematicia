using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mathematician.Common.AlphaAPI.Output
{
    [XmlRoot("queryresult")]
    public class AlphaQueryResult : QueryResult
    {
        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlAttribute("error")]
        public bool HasError { get; set; }

        [XmlAttribute("numpods")]
        public int NumberOfPods { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("datatypes")]
        public string DataTypes { get; set; }

        [XmlAttribute("timing")]
        public float Timing { get; set; }

        [XmlAttribute("timedout")]
        public string TimedOutPods { get; set; }

        [XmlAttribute("parsetiming")]
        public float ParseTiming { get; set; }

        [XmlAttribute("parsetimedout")]
        public bool ParseTimedOut { get; set; }

        [XmlAttribute("recalculate")]
        public string RecalculateUrl { get; set; }

        [XmlElement("pod")]
        public List<Pod> Pods = new List<Pod>();

        [XmlElement("error")]
        public Error Error { get; set; }
    }
}