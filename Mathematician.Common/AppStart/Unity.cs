using Mathematician.Common.AlphaAPI;
using Mathematician.Common.WikiAPI;
using Mathematician.Vision;
using Microsoft.Practices.Unity;

namespace Mathematician.Common.AppStart
{
    public static class Unity
    {
        public const string AlphaQuery = "AlphaQuery";
        public const string AlphaWebRequest = "AlphaWebRequest";
        public const string WikiQuery = "WikiQuery";
        public const string WikiWebRequest = "WikiWebRequest";

        public static UnityContainer Container { get; set; }

        static Unity()
        {
            Unity.Container = new UnityContainer();

            Unity.Container.RegisterType<ITextExtractor, TextExtractor>();

            Unity.Container.RegisterType<IRequest, AlphaWebRequest>(AlphaWebRequest);
            Unity.Container.RegisterType<IQueryService, AlphaQueryService>(AlphaQuery, new InjectionConstructor(new ResolvedParameter(typeof(IRequest), AlphaWebRequest)));

            Unity.Container.RegisterType<IRequest, WikiWebRequest>(WikiWebRequest);
            Unity.Container.RegisterType<IQueryService, WikiQueryService>(WikiQuery, new InjectionConstructor(new ResolvedParameter(typeof(IRequest), WikiWebRequest)));
        }
    }
}