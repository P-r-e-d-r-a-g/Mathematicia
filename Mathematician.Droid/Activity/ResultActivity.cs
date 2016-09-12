using Android.App;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using Mathematician.Common.AlphaAPI.Output;
using Mathematician.Droid.Adapters;
using Mathematician.Droid.Util;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Mathematician.Droid
{
    [Activity(Label = "ResultActivity")]
    public class ResultActivity : Activity
    {
        AlphaQueryResult result;

        #region view components
        private TextView resultHeader;
        private ListView podList;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Result);
            this.result = GetResult(Intent.Extras);

            if (this.result.HasError)
            {
                //TODO display some kind of error activity
                return;
            }

            resultHeader = FindViewById<TextView>(Resource.Id.ResultHeader);
            podList = FindViewById<ListView>(Resource.Id.PodList);
            podList.Adapter = new PodListAdapter(this, result.Pods);
        }

        /// <summary>
        /// Gets QueryResult from bundle
        /// </summary>
        /// <param name="passedData"></param>
        /// <returns></returns>
        private AlphaQueryResult GetResult(Bundle passedData)
        {
            AlphaQueryResult result;
            XmlSerializer serializer = new XmlSerializer(typeof(AlphaQueryResult));

            using (StringReader sr = new StringReader(passedData.GetString(Constants.BundleKeys.QueryResult)))
            {
                result = serializer.Deserialize(sr) as AlphaQueryResult;
            }

            return result;
        }
    }
}