using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using Mathematician.Common.WikiAPI.Output;
using System.IO;
using System.Xml;

namespace Mathematician.Common.WikiAPI
{
    class WikiQueryService : IQueryService
    {
        protected readonly IRequest request;
        private XmlSerializer responseSerializer;

        public WikiQueryService(IRequest request)
        {
            this.request = request;
            this.responseSerializer = new XmlSerializer(typeof(WikiQueryResult));
        }

        public QueryResult ExecuteQuery(string query)
        {
            WikiQueryResult result = new WikiQueryResult();

            using (XmlReader response = XmlReader.Create(request.CreateRequest(query)))
            {
                result.ReadXml(response);
            }

            return result;
        }
    }
}