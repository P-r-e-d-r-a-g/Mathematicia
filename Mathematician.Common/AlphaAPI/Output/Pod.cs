using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Mathematician.Common.AlphaAPI.Output
{
    public class Pod
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("error")]
        public bool Error { get; set; }

        [XmlAttribute("position")]
        public int Position { get; set; }

        [XmlAttribute("scanner")]
        public string Scanner { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("numsubpods")]
        public int NumberOfSubpods { get; set; }

        [XmlElement("subpod")]
        public List<Subpod> Subpods = new List<Subpod>();
    }
}