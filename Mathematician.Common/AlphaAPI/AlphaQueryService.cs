using Mathematician.Common.AlphaAPI.Output;
using System.IO;
using System.Xml.Serialization;

namespace Mathematician.Common.AlphaAPI
{
    class AlphaQueryService : IQueryService
    {
        protected readonly IRequest request;
        private XmlSerializer responseSerializer;

        public AlphaQueryService(IRequest request)
        {
            this.request = request;
            this.responseSerializer = new XmlSerializer(typeof(AlphaQueryResult));
        }

        public QueryResult ExecuteQuery(string query)
        {
            AlphaQueryResult result;

            using(TextReader response = new StreamReader(request.CreateRequest(query)))
            {
                result = responseSerializer.Deserialize(response) as AlphaQueryResult;
            }

            return result;
        }
    }
}