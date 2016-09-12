using OpenCV.Core;

namespace Mathematician.Vision.Core.KNN
{
    public class KNNFeatureExtractor
    {
        /// <summary>
        /// Extract features from image for KNN classifier.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Mat Extract(Mat image)
        {

            //		pooling
            //		Mat pooledImage = new Mat(0, 1, image.type());
            //
            //		double max, max1, max2;
            //		Mat m = new Mat(1,1,image.type());
            //
            //		for (int i = 0; i < image.rows(); i+=2) {
            //			for (int j = 0; j < image.cols(); j+=2) {
            //				max1 = Math.max(image.get(i, j)[0], image.get(i, j + 1)[0]);
            //				max2 = Math.max(image.get(i + 1, j)[0], image.get(i + 1, j + 1)[0]);
            //				max = Math.max(max1, max2);
            //				
            ////				max=average(image.get(i, j)[0], image.get(i, j + 1)[0], image.get(i + 1, j)[0], image.get(i + 1, j + 1)[0]);
            //				
            //				m.put(0, 0, max);
            //				pooledImage.push_back(m);
            //			}
            //		}
            //		
            //		image=pooledImage;

            image = image.Reshape(0, 1);
            image.ConvertTo(image, CvType.Cv32f);
            OpenCV.Core.Core.Norm(image);
            return image;
        }

        private static double average(double d1, double d2, double d3, double d4)
        {
            return (d1 + d2 + d3 + d4) / 4;
        }
    }
}