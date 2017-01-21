using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    static class FeatureColourContrast
    {
        public static double ComputeFeature(DirectBitmap image, Segmentation s)
        {
            double[] centreX = new double[s.NumSegments];
            double[] centreY = new double[s.NumSegments];
            double[] totalX = new double[s.NumSegments];
            double[] totalY = new double[s.NumSegments];

            // Compute segment centres
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    totalX[s.GetPixelsSegmentIndex(x, y)] += x;
                    totalY[s.GetPixelsSegmentIndex(x, y)] += y;
                }
            }
            for (int i = 0; i < s.NumSegments; i++)
            {
                centreX[i] = totalX[i] / s.GetSegmentsSize(i);
                centreY[i] = totalY[i] / s.GetSegmentsSize(i);
            }

            double result = 0.0;
            for (int i = 0; i < s.NumSegments; i++)
            {
                for (int j = i + 1; j < s.NumSegments; j++)
                {
                    double distPart = 1 - (1 - GetSegmentDistance(centreX[i], centreY[i], centreX[j], centreY[j]));
                    double col = MyColorSpaceHelper.CIEDE2000(s.GetSegmentsColour(i), s.GetSegmentsColour(j));
                    double val = distPart * col / (s.GetSegmentsSize(i) * s.GetSegmentsSize(j));
                    result += val;
                }
            }

            return result;
        }

        private static double GetSegmentDistance(double c1X, double c1Y, double c2X, double c2Y)
        {
            return Math.Sqrt((c1X - c2X) * (c1X - c2X) + (c1Y - c2Y) * (c1Y - c2Y));
        }
    }
}
