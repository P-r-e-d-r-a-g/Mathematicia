using OpenCV.Core;
using System;
using System.Text;

namespace Mathematician.Vision.Core
{
    public class Expression
    {
        private Mat ContentImage { get; set; }
        private String Content { get; set; }
        private Rect Rectangle { get; set; }

        public Expression NextUpper { get; set; }
        public Expression Next { get; set; }

        public Expression(String content)
        {
            Content=content;
            ContentImage=null;
            Rectangle = null;
            NextUpper=null;
            Next=null;
        }

        public Expression(Mat contentImage, Rect rectangle)
        {
            ContentImage=contentImage;
            Rectangle=rectangle;
            Content=null;
            NextUpper=null;
            Next=null;
        }

        public Expression(String content, Mat contentImage, Rect rectangle)
        {
            ContentImage=contentImage;
            Content=content;
            Rectangle=rectangle;
            NextUpper=null;
            Next=null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Content);

            if (NextUpper != null)
            {
                sb.Append("^(");
                sb.Append(NextUpper);
                sb.Append(")");
            }

            if (Next != null)
            {
                sb.Append(Next);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extracts content from contentImage and sets it.
        /// </summary>
        /// <param name="c">Classifier to use for content extraction</param>
        public void ExtractContentFromImages(IClassifier c)
        {
            if (this.ContentImage != null)
            {
                this.Content=c.Classify(this.ContentImage);
            }

            if (this.NextUpper != null)
            {
                this.NextUpper.ExtractContentFromImages(c);
            }

            if (this.Next != null)
            {
                this.Next.ExtractContentFromImages(c);
            }
        }

        /// <summary>
        /// Determines if second expression is in the same line as first.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsNext(Expression exp)
        {
            int firstVerticalMiddleUp = (int)(this.Rectangle.Y + this.Rectangle.Height * 0.4);
            int firstVerticalMiddleDown = (int)(this.Rectangle.Y + this.Rectangle.Height * 0.6);
            int secondVerticalMiddle = (int)(exp.Rectangle.Y + exp.Rectangle.Height * 0.5);

            return secondVerticalMiddle >= firstVerticalMiddleUp && secondVerticalMiddle <= firstVerticalMiddleDown;
        }

        /// <summary>
        /// Determines if second expression is an exponent of first. (Example:\n In X' ' is nextUpper for X.In XY Y is exponent of X)
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsExponent(Expression exp)
        {
            int firstVerticalBorder = (int)(this.Rectangle.Y + this.Rectangle.Height * 0.3);
            int secondLowerY = exp.Rectangle.Y + exp.Rectangle.Height;

            return secondLowerY < firstVerticalBorder;
        }
    }
}