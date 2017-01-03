using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    static class FeatureBrightness
    {
        public static double ComputeFeature(DirectBitmap image)
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
            total = total / image.Width / image.Height / 255;

            return total;
        }
    }
}
