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
using System.Net;
using System.IO;

namespace Mathematician.Common.AlphaAPI
{
    public class AlphaWebRequest : IRequest
    {
        protected const string BaseUrl = "http://api.wolframalpha.com/v2/query?";
        protected const string AppId = "Q4QRXG-VJLA6Y5EPL";
        protected const string Format = "plaintext,image";
        protected const string IncludePodId = "";//skip for now (e.g. get all pods)
        protected const string Width = "500";

        private string requestBody;

        public AlphaWebRequest()
        {
            requestBody = string.Format("{0}appid={1}&format={2}&width={3}&plotwidth={4}&input=", BaseUrl, AppId, Format, Width, Width);
        }

        public Stream CreateRequest(string query)
        {
            string r = getFullRequestBody(query);
            WebRequest request = WebRequest.Create(r);
            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        private string getFullRequestBody(string query)
        {
            return requestBody + System.Uri.EscapeDataString(query);
        }
    }
}