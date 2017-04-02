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

        public static DirectBitmap GetHistogram(DirectBitmap image, bool[][] boundedBinarySaliencyMap)
        {
            DirectBitmap result = new DirectBitmap(512, 512);

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
                        int r = (int)Math.Round(c.R / 255.0 * 15.0);
                        int g = (int)Math.Round(c.G / 255.0 * 15.0);
                        int b = (int)Math.Round(c.B / 255.0 * 15.0);

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

            for (int i = 0; i < 4096; i++)
            {
                int row = i / 64;
                int col = i % 64;

                // Compute rgb from i
                int r = 0, g = 0, b = 0, colourNum = i;
                while (colourNum > 0)
                {
                    b++;
                    colourNum -= 16 * 16;
                }
                if (colourNum < 0)
                {
                    colourNum += 16 * 16;
                    b--;
                }
                while (colourNum > 0)
                {
                    g++;
                    colourNum -= 16;
                }
                if (colourNum < 0)
                {
                    colourNum += 16;
                    g--;
                }
                r = colourNum;

                double weight = H[i] / (double) hmax;

                // Convert to colour
                Color c = Color.FromArgb((int)Math.Round(r * weight / 15.0 * 255.0), (int)Math.Round(g * weight / 15.0 * 255.0), (int)Math.Round(b * weight / 15.0 * 255.0));

                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        result.SetPixel(row * 8 + x, col * 8 + y, c);
                    }
                }
            }

            return result;
        }
    }
}
