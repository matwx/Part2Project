using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project.Features
{
    static class FeatureSimplicity
    {
        const double alpha = 0.79;

        public static double ComputeFeature(DirectBitmap image, Segmentation s, double sigma)
        {
            SaliencySegmentation ss = new SaliencySegmentation(s, image, sigma);

            bool[] trueSegments = new bool[ss.NumSegments];
            double[] newSaliencies = new double[ss.NumSegments];
            int numTrueSegments = 0;

            // Re-normalise the segment saliencies, excluding the smallest ones
            double maxSal = 0;
            for (int i = 0; i < ss.NumSegments; i++)
            {
                if (ss.GetSegmentsSize(i) > 0.01*ss.Width*ss.Height && ss.GetSegmentsSaliency(i) > maxSal)
                    maxSal = ss.GetSegmentsSaliency(i);
            }
            for (int i = 0; i < ss.NumSegments; i++)
            {
                newSaliencies[i] = ss.GetSegmentsSaliency(i)/maxSal;
            }

            // Convert segment saliency map into a binary map, using a threshold, alpha
            for (int i = 0; i < ss.NumSegments; i++)
            {
                if (ss.GetSegmentsSize(i) > 0.01 * ss.Width * ss.Height && newSaliencies[i] > alpha)
                {
                    trueSegments[i] = true;
                    numTrueSegments++;
                }
                else trueSegments[i] = false;
            }

            int[] trueSegmentIndicies = new int[numTrueSegments];
            int count = 0;
            for (int i = 0; i < ss.NumSegments; i++)
            {
                if (trueSegments[i])
                {
                    trueSegmentIndicies[count] = i;
                    count++;
                }
            }

            // Generate bounding boxes for all of the segments that are 'true' in the binary ROI map
            int[] lefts = new int[ss.NumSegments];
            int[] rights = new int[ss.NumSegments];
            int[] tops = new int[ss.NumSegments];
            int[] bottoms = new int[ss.NumSegments];
            bool[] initialised = new bool[ss.NumSegments];

            for (int x = 0; x < ss.Width; x++)
            {
                for (int y = 0; y < ss.Height; y++)
                {
                    int i = ss.GetPixelsSegmentIndex(x, y);
                    if (trueSegments[i])
                    {
                        if (initialised[i])
                        {
                            if (x < lefts[i]) lefts[i] = x;
                            if (x > rights[i]) rights[i] = x;
                            if (y < tops[i]) tops[i] = y;
                            if (y > bottoms[i]) bottoms[i] = y;
                        }
                        else
                        {
                            lefts[i] = x;
                            rights[i] = x;
                            tops[i] = y;
                            bottoms[i] = y;

                            initialised[i] = true;
                        }
                    }
                }
            }

            bool[][] boundedBinarySaliencyMap = new bool[ss.Width][];
            for (int x = 0; x < ss.Width; x++)
            {
                boundedBinarySaliencyMap[x] = new bool[ss.Height];
            }

            foreach (int i in trueSegmentIndicies)
            {
                // Add this segment's bounding box to the map
                for (int x = lefts[i]; x < rights[i] + 1; x++)
                {
                    for (int y = tops[i]; y < bottoms[i] + 1; y++)
                    {
                        boundedBinarySaliencyMap[x][y] = true;
                    }
                }
            }

            // Feature value is the proportion of the image taken up by these bounding boxes.
            int total = 0;
            for (int x = 0; x < ss.Width; x++)
            {
                for (int y = 0; y < ss.Height; y++)
                {
                    if (boundedBinarySaliencyMap[x][y]) total++;
                }
            }

            return ((double)total) / ss.Width / ss.Height;
        }
    }
}
