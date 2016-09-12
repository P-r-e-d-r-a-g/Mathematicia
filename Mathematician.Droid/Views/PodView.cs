using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Util;
using Android.Graphics;
using Mathematician.Common.AlphaAPI.Output;

namespace Mathematician.Droid.Views
{
    [Register("Mathematician.Droid.Views.PodView")]
    public class LinearLayout : Android.Views.View
    {
        public Pod Pod { get; set; }

        public LinearLayout(Context context) : base(context) { }

        public LinearLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public LinearLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        public LinearLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

        protected LinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawText(Pod.Title, 5, 5, new Paint() { Color = Color.White, TextSize = 16 });

            int xPos = 30;
            int yPos = 5;
            Paint textPaint = new Paint() { Color = Color.White, TextSize = 12 };

            foreach(Subpod subpod in Pod.Subpods)
            {
                canvas.DrawText(subpod.PlainText, xPos, yPos, textPaint);
                xPos += 15;
            }
        }
    }
}