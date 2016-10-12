using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.IO;
using Android.Provider;
using Android.Content.PM;
using Mathematician.Droid.Helpers;
using System.Collections.Generic;
using Mathematician.Vision;
using Mathematician.Droid.Util;
using Microsoft.Practices.Unity;
using Mathematician.Common.AppStart;
using Mathematician.Common;
using Mathematician.Common.WikiAPI.Output;
using OpenCV.Android;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Preferences;

namespace Mathematician.Droid
{
    [Activity(Label = "Mathematician", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        public static string IMAGE_DIR = "/sdcard/Mathematician/Images";
        public static string IMAGE_FILE_PATTERN = "solver_{0}.jpg";

        public static class NewImageInfo
        {
            public static Bitmap Image { get; set; }
            public static File Dir = new Java.IO.File(IMAGE_DIR);
            public static File File { get; set; }
        }

        public static class RequestCodes
        {
            public const int CaptureImage = 0;
            public const int SelectImage = 1;
            public const int Text = 2;
        }

        #region view components
        private TextView feedView;
        private FloatingActionButton enterTextButton;
        private FloatingActionButton selectButton;
        #endregion

        protected ITextExtractor TextExtractor
        {
            get; private set;
        }

        protected IQueryService QueryService { get; private set; }

        protected ILoaderCallbackInterface LoaderCallback;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            LoaderCallback = new OpenCVLoaderCallback(this);

            QueryService = Unity.Container.Resolve<IQueryService>(Unity.WikiQuery);

            loadOpenCV();

            //try to this through Unity as planned. Loading OpenCV is problem here
            TextExtractor = new TextExtractor();//Unity.Container.Resolve<ITextExtractor>();

            SetContentView(Resource.Layout.Main);

            registerViews();

            feedView.SetText(getAndCacheFeed(), TextView.BufferType.Normal);

            selectButton.Click += selectImage;

            enterTextButton.Click += gotoTextActivity;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode != Result.Ok)
            {
                return;
            }

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = feedView.Width;

            switch (requestCode)
            {
                case RequestCodes.CaptureImage:
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri = Android.Net.Uri.FromFile(NewImageInfo.File);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);

                    NewImageInfo.Image = BitmapHelpers.LoadAndResizeBitmap(NewImageInfo.File.Path, width);

                    startTextActivity(TextExtractor.ExtractText(NewImageInfo.File.Path));

                    //NewImageInfo.Image.Recycle();

                    //GC.Collect();//TODO is it needed here while processing picture?  try
                    break;
                case RequestCodes.SelectImage:
                    if (data != null && data.Data != null)
                    {
                        string imagePath = GetPathToImage(data.Data);

                        //NewImageInfo.Image = BitmapHelpers.LoadAndResizeBitmap(uri.Path, width);

                        startTextActivity(TextExtractor.ExtractText(imagePath));

                        //NewImageInfo.Image.Recycle();

                        //GC.Collect();
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets path to image on storage.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string GetPathToImage(Android.Net.Uri uri)
        {
            string doc_id = "";
            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                String document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ManagedQuery(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
        }

        /// <summary>
        /// Dynamic load of OpenCV library.
        /// </summary>
        private void loadOpenCV()
        {
            if (!OpenCVLoader.InitDebug())
            {
                OpenCVLoader.InitAsync(OpenCVLoader.OpencvVersion300, this, LoaderCallback);
            }
            else
            {
                LoaderCallback.OnManagerConnected(LoaderCallbackInterface.Success);
            }
        }

        /// <summary>
        /// Registers view components
        /// </summary>
        private void registerViews()
        {
            selectButton = FindViewById<FloatingActionButton>(Resource.Id.SelectButton);
            enterTextButton = FindViewById<FloatingActionButton>(Resource.Id.EnterTextButton);
            feedView = FindViewById<TextView>(Resource.Id.FeedResult);
            feedView.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
        }

        /// <summary>
        /// Creates directory for stroing images
        /// </summary>
        private void createDirectoryForPictures()
        {
            if (!NewImageInfo.Dir.Exists())
            {
                NewImageInfo.Dir.Mkdirs();
            }
        }

        /// <summary>
        /// Checks if there is an camera app
        /// </summary>
        /// <returns></returns>
        private bool cameraAppExists()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        /// <summary>
        /// Opens camera app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void takeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            NewImageInfo.File = new Java.IO.File(NewImageInfo.Dir, String.Format(IMAGE_FILE_PATTERN, Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(NewImageInfo.File));
            StartActivityForResult(intent, RequestCodes.CaptureImage);
        }

        /// <summary>
        /// Shows toast that there is no camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noCameraMessage(object sender, EventArgs e)
        {
            Toast.MakeText(this, Resource.String.NoCameraApp, ToastLength.Short).Show();
        }

        /// <summary>
        /// Opens activity for selecting image
        /// </summary>
        /// <param name="sendet"></param>
        /// <param name="eventArgs"></param>
        private void selectImage(object sendet, EventArgs eventArgs)
        {
            Intent intent = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Images.Media.ExternalContentUri);
            //Intent.SetType("image/*");
            //Intent.SetAction(Intent.ActionPick);
            StartActivityForResult(intent, RequestCodes.SelectImage);
        }

        /// <summary>
        /// Opens activity for entering text directly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gotoTextActivity(object sender, EventArgs e)
        {
            startTextActivity("");
        }

        /// <summary>
        /// Starts text activity for given extracted text
        /// </summary>
        /// <param name="extractedText"></param>
        private void startTextActivity(string extractedText)
        {
            Bundle data = new Bundle();
            data.PutString(Constants.BundleKeys.ExtractedText, extractedText);

            Intent intent = new Intent(this, typeof(TextActivity));
            intent.PutExtras(data);
            StartActivity(intent);
        }

        /// <summary>
        /// Gets info about randomly chosen mathemtician.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string getAndCacheFeed()
        {
            string name = FeedHelper.GetName();

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            if (prefs.Contains(name))
            {
                return prefs.GetString(name, Resources.GetString(Resource.String.WikiQueryError));
            }

            try
            {
                string text = (QueryService.ExecuteQuery(name) as WikiQueryResult).Text;
                prefs.Edit().PutString(name, text);
                prefs.Edit().Commit();
                return text;
            }
            catch (Exception)
            {
                return Resources.GetString(Resource.String.WikiQueryError);
            }
        }

    }
}

