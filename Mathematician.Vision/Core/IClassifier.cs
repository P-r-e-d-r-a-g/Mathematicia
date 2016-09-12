using OpenCV.Core;
using System;

namespace Mathematician.Vision.Core
{
    public interface IClassifier
    {
        String Classify(Mat image);
    }
}