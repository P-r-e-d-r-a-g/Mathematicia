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

namespace Mathematician.Common.AlphaAPI.Output
{
    public class Error
    {
        [XmlAttribute("code")]
        public int Code { get; set; }

        [XmlAttribute("msg")]
        public string Message { get; set; }
    }
}