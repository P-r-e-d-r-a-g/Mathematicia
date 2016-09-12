using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;

namespace Mathematician.Common.WikiAPI
{
    class WikiWebRequest : IRequest
    {
        protected const string UrlPattern = "https://en.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&exintro=true&explaintext=true&titles={0}";

        public Stream CreateRequest(string query)
        {
            string r = getFullRequestBody(query);
            WebRequest request = WebRequest.Create(r);
            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        private string getFullRequestBody(string query)
        {
            return string.Format(UrlPattern, query);
        }
    }
}