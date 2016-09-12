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
using Mathematician.Common.AlphaAPI.Output;
using System.Xml.Serialization;
using System.IO;
using Android.Content.Res;

namespace Mathematician.Common.AlphaAPI
{
    public class QueryServiceFake : IQueryService
    {
        XmlSerializer serializer;

        public QueryResult ExecuteQuery(string query)
        {
            serializer = new XmlSerializer(typeof(AlphaQueryResult));
            AlphaQueryResult result;

            using (StreamReader r = new StreamReader(Android.App.Application.Context.Assets.Open("Output1.xml")))
            {
                result = serializer.Deserialize(r) as AlphaQueryResult;
            }

            return result;
        }
    }
}