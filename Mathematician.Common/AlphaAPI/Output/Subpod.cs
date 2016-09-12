using System.Xml;
using System.Xml.Serialization;

namespace Mathematician.Common.AlphaAPI.Output
{
    public class Subpod
    {
        [XmlElement("plaintext")]
        public string PlainText { get; set; }

        [XmlElement("img")]
        public HtmlImage Image { get; set; }
    }
}