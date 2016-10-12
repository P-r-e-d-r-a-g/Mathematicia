using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Mathematician.Common.AlphaAPI.Output;
using Android.Graphics.Drawables;
using System.IO;
using Android.Graphics;
using System.Net;

namespace Mathematician.Droid.Adapters
{
    class PodListAdapter : BaseAdapter<Pod>
    {
        private static int minPlotWidth = 700;
        private static readonly string[] excludeList = { "NumberLine" };
        private static readonly string imgshow = "plot";
        private Context context;
        private ICollection<Pod> pods;

        public PodListAdapter(Context context, ICollection<Pod> pods)
        {
            this.pods = pods;
            this.context = context;
        }
        public override Pod this[int position]
        {
            get
            {
                return pods.ElementAt(position);
            }
        }

        public override int Count
        {
            get
            {
                return pods.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (exclude(this[position]))
            {
                return new View(context);
            }

            Pod pod = this[position];

            View v = convertView;
            if (v == null)
            {
                v = ((LayoutInflater)context.GetSystemService(Context.LayoutInflaterService)).Inflate(Resource.Layout.pod, null);
            }
            View pv = v.FindViewById<LinearLayout>(Resource.Id.apod);

            TextView podTitle = pv.FindViewById<TextView>(Resource.Id.podTitle);
            TextView podContent = pv.FindViewById<TextView>(Resource.Id.podContent);
            ImageView podContentImg = pv.FindViewById<ImageView>(Resource.Id.podContentImg);
            podContentImg.SetImageDrawable(null);

            podTitle.Text = "\t" + pod.Title;

            StringBuilder sb = new StringBuilder();

            foreach (Subpod s in pod.Subpods)
            {
                sb.Append("\t \t");
                sb.Append(s.PlainText);
                sb.Append(System.Environment.NewLine);

            }

            if (showImage(pod))
            {
                podContentImg.SetImageBitmap(fetchImageFromWeb(getImageUrl(pod)));
            }

            podContent.SetText(sb.ToString(), TextView.BufferType.Normal);

            return pv;
        }

        private bool exclude(Pod pod)
        {
            return excludeList.Contains(pod.Id);
        }

        private string getImageUrl(Pod pod)
        {
            return pod.Subpods.First(s => s.Image != null).Image.Src;
        }

        private bool showImage(Pod pod)
        {
            return pod.Id.ToLower().Contains(PodListAdapter.imgshow);
        }

        private Bitmap fetchImageFromWeb(string url)
        {
            try
            {
                Bitmap image = null;

                using (var webClient = new WebClient())
                {
                    var data = webClient.DownloadData(url);
                    if (data != null && data.Length > 0)
                    {
                        image = BitmapFactory.DecodeByteArray(data, 0, data.Length);
                    }
                }

                if(image.Width < minPlotWidth)
                {
                    image = Bitmap.CreateScaledBitmap(image, minPlotWidth, minPlotWidth / image.Width * image.Height, true);
                }

                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}