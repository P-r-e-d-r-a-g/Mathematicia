using System;
using Android.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Mathematician.Droid.Helpers
{
    internal class BitmapHelpers
    {
        /// <summary>
        /// Loads image from storage and resizes it.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newWidth"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal static Bitmap LoadAndResizeBitmap(string fileName, int newWidth)
        {
            Bitmap image = BitmapFactory.DecodeFile(fileName);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int width = image.Width;
            int height = image.Height;
            float ratio = 1;

            if (width > newWidth)
            {
                ratio = (float)width / (float)height;
                height = (int)(newWidth / ratio);
                width = newWidth;
            }

            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(image, width, height, false);// .DecodeFile(fileName, options);

            return resizedBitmap;
        }

        /// <summary>
        /// Fetches image from url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static Bitmap FetchImage(String url)
        {
            var webClient = new System.Net.WebClient();
            byte[] imageBytes = webClient.DownloadData(url);
            return BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        }

        /// <summary>
        /// Creates bitmap containing all provided bitmaps.
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        internal static Bitmap CombineBitmaps(IList<Bitmap> bitmaps)
        {
            if (bitmaps.Count <= 0)
            {
                return null;//TODO throw something
            }

            int combinedHeight = computeHeight(bitmaps);
            int currentHeight = 0;

            Bitmap result = Bitmap.CreateBitmap(bitmaps[0].Width, combinedHeight, Bitmap.Config.Argb8888);

            Canvas c = new Canvas(result);
            Paint p = new Paint();

            foreach (Bitmap b in bitmaps)
            {
                c.DrawBitmap(b, 0, currentHeight, p);
                currentHeight += b.Height;
            }

            return result;
        }

        /// <summary>
        /// Creates a new bitmap with lower dimensions. Keeps aspect ratio.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="wantedWidth"></param>
        /// <returns></returns>
        internal static Bitmap Decrease(Bitmap image, int wantedWidth)
        {
            int scale = image.Width / image.Height;

            return Bitmap.CreateScaledBitmap(image, wantedWidth, wantedWidth / scale, false);

            //return image;
        }

        /// <summary>
        /// Sums height of all bitmaps in the list.
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        private static int computeHeight(IList<Bitmap> bitmaps)
        {
            return bitmaps.Sum(b => b.Height);
        }
    }
}