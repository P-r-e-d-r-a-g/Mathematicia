using OpenCV.Core;
using OpenCV.ML;
using System;
using System.Collections.Generic;

namespace Mathematician.Vision.Core.KNN
{
    public class KNNClassifier : IClassifier
    {
        private KNearest knn = KNearest.Create();
        private Dictionary<float, string> classesMap;

        private Mat results = new Mat();
        private Mat neighborResponses = new Mat();
        private Mat dists = new Mat();

        public KNNClassifier(Mat features, Mat classes, Dictionary<float, string> classesMap)
        {
            knn.Train(features, Ml.RowSample, classes);
            this.classesMap = classesMap;
        }

        public String Classify(Mat image)
        {
            float nearest = knn.FindNearest(KNNFeatureExtractor.Extract(image), 4, results, neighborResponses, dists);
            return classesMap[nearest];
        }

    }
}