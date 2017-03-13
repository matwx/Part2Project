using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 125, 0.0);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.6);

            double[] segmentSpreads = GetRoTSpreads(rots);

            double factor = 0;
            double result = 0;
            const double sigma = 0.17;
            for (int i = 0; i < rots.NumSegments; i++)
            {
//                1) result += rots.GetSegmentsSaliency(i) * rots.GetSegmentsSize(i) * Math.Exp(-rots.GetSegmentsDistance(i)*rots.GetSegmentsDistance(i)/(2*sigma));
//                2) result += rots.GetSegmentsSaliency(i) * Math.Exp(-rots.GetSegmentsDistance(i)*rots.GetSegmentsDistance(i)/(2*sigma));
//                3) result += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i)) * Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
//                4) (with c0.01) result += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i)) * Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
//                (5) (with c0.01) result += rots.GetSegmentsSaliency(i) / segmentSpreads[i] * Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
//                6) (with c0.01) result += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i)) / segmentSpreads[i] * Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
                

//                1) factor += rots.GetSegmentsSaliency(i)*rots.GetSegmentsSize(i);
//                2) factor += rots.GetSegmentsSaliency(i);
//                3) factor += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i));
//                4) (with c0.01) factor += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i));
//                (5) (with c0.01) factor += rots.GetSegmentsSaliency(i) / segmentSpreads[i];
//                6) (with c0.01) factor += rots.GetSegmentsSaliency(i) * Math.Log(rots.GetSegmentsSize(i)) / segmentSpreads[i];
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

        public Bitmap GetRoTHeatMap(Bitmap image)
        {
            Bitmap result = new Bitmap(image);

            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 125, 0.0);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.6);

            double[] segmentSpreads = GetRoTSpreads(rots);
            double[] segmentTemps = new double[rots.NumSegments];

            // Compute segment contributions for the RoT
            const double sigma = 0.17;
            double maxTemp = 0;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                if (rots.GetSegmentsSize(i) > 0.01 * rots.Width * rots.Height)
                    segmentTemps[i] = rots.GetSegmentsSaliency(i) / segmentSpreads[i] * Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
                if (segmentTemps[i] > maxTemp) maxTemp = segmentTemps[i];
            }

            // Draw the image
            for (int x = 0; x < rots.Width; x++)
            {
                for (int y = 0; y < rots.Height; y++)
                {
                    double val = segmentTemps[rots.GetPixelsSegmentIndex(x, y)];
                    result.SetPixel(x, y, Color.FromArgb((int)(val / maxTemp * 255), (int)(val / maxTemp * 255), (int)(val / maxTemp * 255)));
                }
            }

            return result;
        }

        public Bitmap GetRoTSpreadMap(Bitmap image)
        {
            Bitmap result = new Bitmap(image);

            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 125, 0.0);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.6);

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
                segmentSpreads[i] = Math.Max((segmentRights[i] - segmentLefts[i])/rots.Width, (segmentBottoms[i] - segmentTops[i])/rots.Height);
                if (segmentSpreads[i] > maxSpread) maxSpread = segmentSpreads[i];
            }

            // Draw the image
            for (int x = 0; x < rots.Width; x++)
            {
                for (int y = 0; y < rots.Height; y++)
                {
                    double val = segmentSpreads[rots.GetPixelsSegmentIndex(x, y)];
                    result.SetPixel(x, y, Color.FromArgb((int)(val / maxSpread * 255), (int)(val / maxSpread * 255), (int)(val / maxSpread * 255)));
                }
            }

            return result;
        }

        public Bitmap GetRoTDistanceMap(Bitmap image)
        {
            Bitmap result = new Bitmap(image);

            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 125, 0.0);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.6);

            double[] segmentDists = new double[rots.NumSegments];

            // Compute segment distances to the RoT
            const double sigma = 0.17;
            double maxDist = 0;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                segmentDists[i] = Math.Exp(-rots.GetSegmentsDistance(i) * rots.GetSegmentsDistance(i) / (2 * sigma));
                if (segmentDists[i] > maxDist) maxDist = segmentDists[i];
            }

            // Draw the image
            for (int x = 0; x < rots.Width; x++)
            {
                for (int y = 0; y < rots.Height; y++)
                {
                    double val = segmentDists[rots.GetPixelsSegmentIndex(x, y)];
                    result.SetPixel(x, y, Color.FromArgb((int)(val / maxDist * 255), (int)(val / maxDist * 255), (int)(val / maxDist * 255)));
                }
            }

            return result;
        }

        public double[] GetRoTSpreads(RuleOfThirdsSegmentation rots)
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
