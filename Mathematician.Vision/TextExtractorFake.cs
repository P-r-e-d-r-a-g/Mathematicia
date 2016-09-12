using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mathematician.Vision
{
    public class TextExtractorFake : ITextExtractor
    {
        public TextExtractorFake() { }

        public string ExtractText(string pathToImage)
        {
            return "x^2+5x-6=0";
        }
    }
}