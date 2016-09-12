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
using System.IO;

namespace Mathematician.Droid.Helpers
{
    internal static class FeedHelper
    {
        private static readonly string[] Mathematicians = readAllNamesFromStream(Android.App.Application.Context.Assets.Open("Mathematicians.txt"));

        /// <summary>
        /// Reads all names from stream. Assumes there is one name per line.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string[] readAllNamesFromStream(Stream s)
        {
            int i = 0;
            string line;
            string[] names = new string[100];
            using (TextReader tr = new StreamReader(s))
            {
                while ((line = tr.ReadLine()) != null && i < 100)
                {
                    names[i++] = line;
                }
            }

            return names;
        }

        /// <summary>
        /// Gets a randomly chosen name of a famous mathematician.
        /// </summary>
        /// <returns></returns>
        internal static string GetName()
        {
            int i = new Random().Next() % 100;
            return Mathematicians.ElementAt(i);
        }
    }
}