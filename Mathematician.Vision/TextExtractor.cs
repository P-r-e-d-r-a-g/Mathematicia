using System;
using Android.Graphics;
using System.IO;
using Mathematician.Vision.Core;
using Mathematician.Vision.Core.KNN;
using OpenCV.Core;
using OpenCV.Android;
using OpenCV.ImgCodecs;

namespace Mathematician.Vision
{
    public class TextExtractor : ITextExtractor
    {

        public static IClassifier KNN;
        public static readonly ExpressionExtractor ExpressionExtractor = new ExpressionExtractor();

        public TextExtractor()
        {
            Mat features = new Mat(928, 784, CvType.Cv32f);
            features.Put(0, 0, Training.TrainingFeatures);

            Mat classes = new Mat(928, 1, CvType.Cv32f);
            classes.Put(0, 0, Training.Classes);

            KNN = new KNNClassifier(features, classes, Training.ClassesMap);
        }

        public string ExtractText(string pathToImage)
        {
            Mat imageMat = Imgcodecs.Imread(pathToImage, CvType.Cv8uc1);// new Mat(image.Height, image.Width, CvType.Cv8uc4);
            //Utils.BitmapToMat(image, imageMat);

            Expression e = ExpressionExtractor.ExtractExpression(imageMat);

            e.ExtractContentFromImages(KNN);

            return e.ToString();
        }
    }
}