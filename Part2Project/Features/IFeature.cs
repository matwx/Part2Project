using System.Drawing;

namespace Part2Project.Features
{
    public interface IFeature
    {
        double ComputeFeature(Bitmap image);
    }
}