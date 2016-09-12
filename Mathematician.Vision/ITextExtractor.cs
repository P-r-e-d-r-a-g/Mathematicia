using Android.Graphics;

namespace Mathematician.Vision
{
    public interface ITextExtractor
    {
        string ExtractText(string pathToImage);
    }
}