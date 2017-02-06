using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.Infrastructure;

namespace Part2Project.Features
{
    static class FeatureBackgroundDistraction
    {
        public static double ComputeFeature(DirectBitmap image, bool[][] boundedBinarySaliencyMap)
        {
            // Every pixel that's false is considered part of the background
            // Quantise remaining pixels into 16 levels for each colour channel, leaving
            // 4096 possible colours. Then compute feature from the histogram
            int[] H = new int[4096];
            int numBackgroundPixels = 0;

            // Compute Histogram
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (!boundedBinarySaliencyMap[x][y])
                    {
                        Color c = image.GetPixel(x, y);
                        int r = (int) Math.Round(c.R / 255.0 * 15.0);
                        int g = (int) Math.Round(c.G / 255.0 * 15.0);
                        int b = (int) Math.Round(c.B / 255.0 * 15.0);

                        H[r + 16 * g + 16 * 16 * b]++;
                        numBackgroundPixels++;
                    }
                }
            }

            int hmax = 0;
            for (int i = 0; i < 4096; i++)
            {
                if (H[i] > hmax) hmax = H[i];
            }

            double result = 0.0;
            for (int i = 0; i < 4096; i++)
            {
                if (H[i] >= 0.01 * hmax) result++;
            }

            if ((int) result == 4096) return 0; // This is when the RoI is the whole image

            return result / 4096 / 0.11; // With a fudge factor
        }
    }
}
