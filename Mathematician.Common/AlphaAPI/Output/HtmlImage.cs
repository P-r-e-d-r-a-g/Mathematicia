using System.Xml.Serialization;

namespace Mathematician.Common.AlphaAPI.Output
{
    public class HtmlImage
    {
        [XmlAttribute("src")]
        public string Src { get; set; }

        [XmlAttribute("alt")]
        public string Alt { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }

        public override string ToString()
        {
            return string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" width=\"{3}\" height=\"{4}\" />",
                Src, Alt, Title, Width, Height);
        }
    }
}