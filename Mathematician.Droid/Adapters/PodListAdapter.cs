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
using Android.Graphics.Drawables;
using Android.Graphics;
using Mathematician.Droid.Helpers;
using Android.Text;

namespace Mathematician.Droid.Adapters
{
    class PodListAdapter : BaseAdapter<Pod>
    {
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
            View v = convertView;
            if (v == null)
            {
                v = ((LayoutInflater)context.GetSystemService(Context.LayoutInflaterService)).Inflate(Resource.Layout.pod, null);
            }
            View pv = v.FindViewById<LinearLayout>(Resource.Id.apod);

            TextView podTitle = pv.FindViewById<TextView>(Resource.Id.podTitle);
            TextView podContent = pv.FindViewById<TextView>(Resource.Id.podContent);

            podTitle.Text = this[position].Title;

            StringBuilder sb = new StringBuilder();

            //IList<Bitmap> bitmaps = new List<Bitmap>(this[position].Subpods.Count);

            //Bitmap b;

            foreach (Subpod s in this[position].Subpods)
            {

                //b = BitmapHelpers.FetchImage(s.Image.Src);
                //int width = context.Resources.DisplayMetrics.WidthPixels;

                //bitmaps.Add(b);
                ////b.Recycle();

                sb.Append(s.PlainText);
                sb.Append(System.Environment.NewLine);

            }

            //b = BitmapHelpers.CombineBitmaps(bitmaps);

            //podContent.SetImageBitmap(b);

            podContent.SetText(sb.ToString(), TextView.BufferType.Normal);

            return pv;
        }


    }
}