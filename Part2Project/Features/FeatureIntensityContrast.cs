using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    class FeatureIntensityContrast : IFeature // Uses Weber Constrast
    {
        public double ComputeFeature(Bitmap image)
        {
            // Compute the average intensity of the image
            double total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y));
                }
            }
            double I_average = total/image.Width/image.Height;

            // Compute the average pixel intensity difference from that average
            total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += Math.Abs(MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y)) - I_average);
                }
            }
            total = total/I_average/image.Width/image.Height;

            return total;
        }
    }
}
