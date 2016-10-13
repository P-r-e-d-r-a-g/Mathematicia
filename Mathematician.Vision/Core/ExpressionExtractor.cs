using Mathematician.Vision.JavaBinding;
using OpenCV.Core;
using OpenCV.ImgProc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathematician.Vision.Core
{
    public class ExpressionExtractor
    {

        protected readonly Size GaussianBlurSize;
        protected readonly Size CharacterSize;
        protected readonly int CharacterBorder;
        protected readonly float UpperNextCoefficient;

        public ExpressionExtractor() : this(new Size(9, 9), new Size(28, 28), 1, (float)0.3) { }

        public ExpressionExtractor(Size gaussianBlurSize, Size characterSize, int characterBorder,
                float upperNextCoefficient)
        {
            GaussianBlurSize = gaussianBlurSize;
            CharacterSize = characterSize;
            CharacterBorder = characterBorder;
            UpperNextCoefficient = upperNextCoefficient;
        }

        public Expression ExtractExpression(Mat image)
        {
            Mat aux = prepareImage(image);
            ISet<Rect> contours = findContours(aux);
            return createExpressionFromRects(aux, contours);
        }

        /// <summary>
        /// Prepares image for contour extraction. Applies Gaussian blur, converts it to binary, inverts colors.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected Mat prepareImage(Mat image)
        {
            Mat result = new Mat(image.Size(), CvType.Cv8uc1);

            Imgproc.CvtColor(image, result, Imgproc.ColorBgr2gray);
            // smoothing the image
            Imgproc.GaussianBlur(result, result, GaussianBlurSize, 0);
            // binarize it
            Imgproc.AdaptiveThreshold(result, result, 255, Imgproc.AdaptiveThreshMeanC, Imgproc.ThreshBinary, 75, 10);
            // invert black and white (background is black now, text is white)
            OpenCV.Core.Core.Bitwise_not(result, result);

            return result;
        }

        /// <summary>
        /// Find contours in image that represent characters.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        protected ISet<Rect> findContours(Mat image)
        {
            // Create copy of the image because findContours modifies original image
            Mat imageCopy = new Mat(image.Size(), image.Type());
            image.CopyTo(imageCopy);
            ISet<Rect> rectangles = new SortedSet<Rect>(new RectComparer());

            Java.Util.ArrayList contours = new Java.Util.ArrayList();
            Mat hierarchy = new Mat();

            JavaBindingHelper.Imgproc_FindContours(imageCopy, contours, hierarchy, Imgproc.RetrExternal, Imgproc.ChainApproxTc89Kcos);

            for (int i = 0; i < contours.Size(); i++)
            {
                MatOfPoint contour = (MatOfPoint)contours.Get(i);
                rectangles.Add(ExpressionExtractor.expand(imageCopy, Imgproc.BoundingRect(contour), 1));
            }

            return rectangles;
        }

        /// <summary>
        /// Gets Expression from given image and list of rectangles bounding characters.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rectangles"></param>
        /// <returns></returns>
        private Expression createExpressionFromRects(Mat image, ISet<Rect> rectangles)
        {
            if (rectangles.Count <= 0)
            {
                return new Expression("...");
            }

            OpenCV.Core.Core.Bitwise_not(image, image);
            List<Rect> rects = new List<Rect>(rectangles);
            Expression expression, start;
            Rect current = rects[0], next;
            expression = new Expression(resize(image.Submat(current), CharacterSize), current);
            start = expression;

            for (int i = 1; i < rectangles.Count - 1; i++)
            {
                current = rects[i];
                next = rects[i + 1];

                // check for = sign
                if (Math.Abs(current.Area() - next.Area()) < 50 && Math.Abs(current.X - next.X) < 1.5 * current.Height)
                {
                    Rect merged = new Rect(current.Tl(), next.Br());

                    expression = new Expression(resize(image.Submat(merged), CharacterSize), merged);

                    createExpression(start, expression);

                    i++;
                    continue;
                }

                if (current.Area() > 40)
                {// simple noise filter :)
                    expression = new Expression(ExpressionExtractor.resize(image.Submat(current), CharacterSize), current);

                    createExpression(start, expression);
                }
            }
            current = rects.Last();

            expression = new Expression(ExpressionExtractor.resize(image.Submat(current), CharacterSize), current);

            createExpression(start, expression);

            return start;
        }

        /// <summary>
        /// Creates expression tree.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="newExpression"></param>
        protected void createExpression(Expression root, Expression newExpression)
        {
            if (root.IsExponent(newExpression))
            {
                if (root.NextUpper == null)
                {
                    root.NextUpper = newExpression;
                }
                else
                {
                    createExpression(root.NextUpper, newExpression);
                }
            }
            else
            {
                if (root.Next == null)
                {
                    root.Next = newExpression;
                }
                else
                {
                    createExpression(root.Next, newExpression);
                }
            }
        }

        /// <summary>
        /// Expands rectangle if it is possible
        /// </summary>
        /// <param name="image"></param>
        /// <param name="r"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        protected static Rect expand(Mat image, Rect r, int padding)
        {
            Rect expanded = new Rect(r.X - padding, r.Y - padding, r.Width + (padding * 2), r.Height + (padding * 2));
            if (expanded.X < 0)
            {
                expanded.X = 0;
            }
            if (expanded.Y < 0)
            {
                expanded.Y = 0;
            }
            if (expanded.X + expanded.Width >= image.Cols())
            {
                expanded.Width = image.Cols() - expanded.X;
            }
            if (expanded.Y + expanded.Height >= image.Rows())
            {
                expanded.Height = image.Rows() - expanded.Y;
            }
            return expanded;
        }

        /// <summary>
        /// Resizes the character image to provided size, keeps aspect ratio.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected static Mat resize(Mat character, Size size)
        {
            Mat resized = Mat.Zeros(size, character.Type());

            int width = character.Cols();
            int height = character.Rows();
            int maxDim = (width >= height) ? width : height;
            ;

            float aspectRatio = ((float)size.Width) / maxDim;

            Rect r = new Rect();

            if (width >= height)
            {
                r.Width = (int)size.Width;
                r.X = 0;
                r.Height = (int)(height * aspectRatio);
                r.Y = (int)((size.Width - r.Height) / 2);
            }
            else
            {
                r.Y = 0;
                r.Height = (int)size.Height;
                r.Width = (int)(width * aspectRatio);
                r.X = (int)((size.Width - r.Width) / 2);
            }

            Imgproc.Resize(character, resized, size);

            return resized;
        }

    }

    class RectComparer : IComparer<Rect>
    {
        public int Compare(Rect x, Rect y)
        {
            return x.X.CompareTo(y.X);
        }
    }
}