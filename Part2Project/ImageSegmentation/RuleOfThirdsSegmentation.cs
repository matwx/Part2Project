using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.ImageSegmentation
{
    class RuleOfThirdsSegmentation : SaliencySegmentation
    {
        private double[] _powerPointDistances;

        public RuleOfThirdsSegmentation(Segmentation s, Bitmap image, double sigma)
            : base(s, image, sigma)
        {
            _powerPointDistances = new double[NumSegments];

            int[] totalX = new int[NumSegments];
            int[] totalY = new int[NumSegments];

            // For each segment, compute the position of its centre, and then
            // the distance from that to the closest powerpoint for the RoT

            // First, total the x and y values for all pixels in each segment
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    totalX[_pixelAssignments[x][y]] += x;
                    totalY[_pixelAssignments[x][y]] += y;
                }
            }

            // Find a max distance to normalise these distances
            double norm =
                Math.Sqrt((Width / 3.0) * (Width / 3.0) +
                          (Height / 3.0) * (Height / 3.0));


            for (int i = 0; i < NumSegments; i++)
            {
                // Compute centre position
                double centreX = (double)totalX[i] / _segmentSizes[i];
                double centreY = (double)totalY[i] / _segmentSizes[i];

                // Compute distance to each powerpoint, and keep the minimum
                // Top-left
                double minDist =
                    Math.Sqrt((centreX - (Width / 3.0)) * (centreX - (Width / 3.0)) +
                              (centreY - (Height / 3.0)) * (centreY - (Height / 3.0)));
                // Top-right
                double nextDist =
                    Math.Sqrt((centreX - (Width * 2.0 / 3.0)) * (centreX - (Width * 2.0 / 3.0)) +
                              (centreY - (Height / 3.0)) * (centreY - (Height / 3.0)));
                if (nextDist < minDist) minDist = nextDist;
                // Bottom-right
                nextDist =
                    Math.Sqrt((centreX - (Width * 2.0 / 3.0)) * (centreX - (Width * 2.0 / 3.0)) +
                              (centreY - (Height * 2.0 / 3.0)) * (centreY - (Height * 2.0 / 3.0)));
                if (nextDist < minDist) minDist = nextDist;
                // Bottom-left
                nextDist =
                    Math.Sqrt((centreX - (Width / 3.0)) * (centreX - (Width / 3.0)) +
                              (centreY - (Height * 2.0 / 3.0)) * (centreY - (Height * 2.0 / 3.0)));
                if (nextDist < minDist) minDist = nextDist;

                _powerPointDistances[i] = minDist / norm;
            }
        }

        public double GetSegmentsDistance(int i)
        {
            return _powerPointDistances[i];
        }
    }
}
