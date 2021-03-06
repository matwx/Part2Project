﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    static class FeatureSaturation
    {
        public static double ComputeFeature(DirectBitmap image)
        {
            // Compute the average saturation of all of the pixels in the image
            double totalSat = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color c = image.GetPixel(x, y);
                    double r = c.R/255.0;
                    double g = c.G/255.0;
                    double b = c.B/255.0;
                    double M = Math.Max(r, Math.Max(g, b));
                    double m = Math.Min(r, Math.Max(g, b));
                    double C = M - m;

                    double sat = 0;
                    if (M != 0.0) sat = C / M;

                    totalSat += sat;
                }
            }

            return totalSat / image.Width / image.Height;
        }
    }
}
