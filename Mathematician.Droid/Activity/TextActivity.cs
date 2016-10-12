using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Mathematician.Common.AlphaAPI;
using Mathematician.Common.AppStart;
using Microsoft.Practices.Unity;
using Mathematician.Common.AlphaAPI.Output;
using System.Linq;
using Mathematician.Droid.Util;
using System.Xml.Serialization;
using System.IO;
using Mathematician.Common;
using Android.Support.Design.Widget;

namespace Mathematician.Droid
{
    [Activity(Label = "TextActivity")]
    public class TextActivity : Activity
    {
        #region view components
        private EditText textArea;
        private FloatingActionButton solveButton;
        #endregion

        private string extractedText;

        protected IQueryService QueryService { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            QueryService = Unity.Container.Resolve<IQueryService>(Unity.AlphaQuery);

            SetContentView(Resource.Layout.Text);

            Bundle passedData = Intent.Extras;

            extractedText = passedData.GetString(Util.Constants.BundleKeys.ExtractedText);

            registerViews();

            registerViewEventsHandlers();

            textArea.SetText(extractedText, TextView.BufferType.Editable);

        }

        /// <summary>
        /// Registers view components
        /// </summary>
        private void registerViews()
        {
            textArea = FindViewById<EditText>(Resource.Id.TextArea);

            solveButton = FindViewById<FloatingActionButton>(Resource.Id.SolveButton);
        }

        /// <summary>
        /// Register event handlers for view components
        /// </summary>
        private void registerViewEventsHandlers()
        {
            solveButton.Click += fetchResults;
        }

        /// <summary>
        /// Fetches results from API.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fetchResults(object sender, EventArgs e)
        {
            AlphaQueryResult result = QueryService.ExecuteQuery(textArea.Text) as AlphaQueryResult;
            XmlSerializer serializer = new XmlSerializer(typeof(AlphaQueryResult));
            string serializedResult;

            using(StringWriter tw=new StringWriter())
            {
                serializer.Serialize(tw, result);
                serializedResult = tw.ToString();
            }

            Intent intent = new Intent(this, typeof(ResultActivity));
            Bundle data = new Bundle();
            data.PutString(Constants.BundleKeys.QueryResult, serializedResult);
            intent.PutExtras(data);

            StartActivity(intent);
        }
    }
}