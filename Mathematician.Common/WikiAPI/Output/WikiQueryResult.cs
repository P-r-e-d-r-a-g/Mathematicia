using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Mathematician.Common.WikiAPI.Output
{
    public class WikiQueryResult : QueryResult, IXmlSerializable
    {
        public string Text { get; set; }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadToFollowing("extract");
            Text = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}