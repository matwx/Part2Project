using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;

namespace Part2Project.Features
{
    class FeatureRuleOfThirds : IFeature
    {
        public double ComputeFeature(Bitmap image)
        {
            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 150, 0.8);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.8);

            double[] segmentSpreads = GetRoTSpreads(rots);

            double factor = 0;
            double result = 0;
            const double sigma = 0.17;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                if (rots.GetSegmentsSize(i) > 0.01 * rots.Width * rots.Height)
                {
                    result += rots.GetSegmentsSaliency(i)/segmentSpreads[i]* 
                              Math.Exp(-rots.GetSegmentsDistance(i)*rots.GetSegmentsDistance(i)/(2*sigma));
                    factor += rots.GetSegmentsSaliency(i) / segmentSpreads[i];
                }
                
            }

            result /= factor;

            return result;
        }

        private double[] GetRoTSpreads(RuleOfThirdsSegmentation rots)
        {
            // Get max x and y values for each segment
            double[] segmentLefts = new double[rots.NumSegments];
            double[] segmentRights = new double[rots.NumSegments];
            double[] segmentTops = new double[rots.NumSegments];
            double[] segmentBottoms = new double[rots.NumSegments];
            bool[] initialised = new bool[rots.NumSegments];
            double[] segmentSpreads = new double[rots.NumSegments];

            for (int x = 0; x < rots.Width; x++)
            {
                for (int y = 0; y < rots.Height; y++)
                {
                    int i = rots.GetPixelsSegmentIndex(x, y);
                    if (initialised[i])
                    {
                        if (x < segmentLefts[i]) segmentLefts[i] = x;
                        if (x > segmentRights[i]) segmentRights[i] = x;
                        if (y < segmentTops[i]) segmentTops[i] = y;
                        if (y > segmentBottoms[i]) segmentBottoms[i] = y;
                    }
                    else
                    {
                        segmentLefts[i] = x;
                        segmentRights[i] = x;
                        segmentTops[i] = y;
                        segmentBottoms[i] = y;

                        initialised[i] = true;
                    }
                }
            }

            double maxSpread = 0;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                segmentSpreads[i] = Math.Max((segmentRights[i] - segmentLefts[i]) / rots.Width, (segmentBottoms[i] - segmentTops[i]) / rots.Height);
                if (segmentSpreads[i] > maxSpread) maxSpread = segmentSpreads[i];
            }
            for (int i = 0; i < rots.NumSegments; i++)
            {
                segmentSpreads[i] /= maxSpread;
            }

            return segmentSpreads;
        }
    }
}
