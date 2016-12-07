using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    class FeatureSaturation : IFeature
    {
        public double ComputeFeature(Bitmap image)
        {
            // Compute the average saturation of all of the pixels in the image
            double totalSat = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color c = image.GetPixel(x, y);
                    double M = (double) Math.Max(c.R, Math.Max(c.G, c.B));
                    double m = (double) Math.Min(c.R, Math.Max(c.G, c.B));
                    double C = M - m;
                    double V = MyColorSpaceHelper.GetIntensityFromRGB(c) / 255.0;

                    double sat = 0;
                    if (V != 0.0) sat = C/V;

                    totalSat += sat;
                }
            }

            return totalSat/image.Width/image.Height;
        }
    }
}
