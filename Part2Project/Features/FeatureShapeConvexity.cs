﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kaliko.ImageLibrary.ColorSpace;
using MIConvexHull;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    static class FeatureShapeConvexity
    {
        //https://www.researchgate.net/profile/Jia_Li87/publication/221304720_Studying_Aesthetics_in_Photographic_Images_Using_a_Computational_Approach/links/55a71b9b08ae51639c5762ed.pdf
        //https://designengrlab.github.io/MIConvexHull/
        public const double salRequired = 0.7;
        public const double convexityRequired = 0.7;

        public static double ComputeFeature(SaliencySegmentation ss, DirectBitmap image = null)
        {
            int numSufficientlySalientPixels = 0;

            // Re-organise data into lists of points for each segment
            List<DefaultVertex>[] segments = new List<DefaultVertex>[ss.NumSegments];
            for (int i = 0; i < ss.NumSegments; i++)
            {
                if (ss.GetSegmentsSize(i) >= 0.01 * ss.Width * ss.Height && ss.GetSegmentsSaliency(i) > salRequired)
                    segments[i] = new List<DefaultVertex>();
            }
            for (int x = 0; x < ss.Width; x++)
            {
                for (int y = 0; y < ss.Height; y++)
                {
                    if (ss.GetPixelsSegmentSize(x, y) >= 0.01 * ss.Width * ss.Height && ss.GetSegmentsSaliency(ss.GetPixelsSegmentIndex(x, y)) > salRequired)
                    {
                        DefaultVertex v = new DefaultVertex {Position = new double[] {x, y}};
                        segments[ss.GetPixelsSegmentIndex(x, y)].Add(v);
                        numSufficientlySalientPixels++;
                    }
                }
            }

//            // For each segment larger than 1% of image that is sufficiently salient,
//            //  - Compute its convex hull
//            //  - Compute the proportion of segment pixels in its convex hull
//            //  - Compute proportion of sufficiently salient pixels that are taken up by >80% convex segments
//
            int totalSufficientlySalientConvexSegmentArea = 0;
            int totalCHPoints = 0, numCHsegments = 0;
            for (int seg = 0; seg < ss.NumSegments; seg++)
            {
                if (ss.GetSegmentsSize(seg) >= 0.01 * ss.Width * ss.Height && ss.GetSegmentsSaliency(seg) > salRequired)
                {
                    // Compute its convex hull
                    var ch = ConvexHull.Create(segments[seg]).Points.ToList();

                    // Set reference to centre of the CH, and sort the points by clockwise rotation
                    reference = new DefaultVertex {Position = new double[2]};
                    for (int j = 0; j < ch.Count; j++)
                    {
                        reference.Position[0] += ch.ElementAt(j).Position[0];
                        reference.Position[1] += ch.ElementAt(j).Position[1];
                    }
                    reference.Position[0] /= ch.Count;
                    reference.Position[1] /= ch.Count;

                    ch.Sort(SortCornersClockwise);
                    
                    // Compute the area of the convex hull. That is the number of pixels that lie within it
                    int chArea = ss.GetSegmentsSize(seg);
                    for (int x = 0; x < ss.Width; x++)
                    {
                        for (int y = 0; y < ss.Height; y++)
                        {
                            if (ss.GetPixelsSegmentIndex(x, y) != seg)
                            {
                                // We want the pixel to lie on the same side of all of the CH edges
                                bool pixelInsideHull = true;
                                for (int i = 0; i < ch.Count - 1; i++)
                                {
                                    double d = (x - ch[i].Position[0]) * (ch[i + 1].Position[1] - ch[i].Position[1]) -
                                               (y - ch[i].Position[1]) * (ch[i + 1].Position[0] - ch[i].Position[0]);

                                    if (d > 0)
                                    {
                                        pixelInsideHull = false;
                                        break;
                                    }
                                }

                                if (pixelInsideHull) chArea++;
                            }
                        }
                    }

                    // Also make sure that our convex hull isn't basically the whole image?
                    if (chArea < 0.8 * ss.Height * ss.Width &&
                        (double) ss.GetSegmentsSize(seg) / chArea > convexityRequired)
                    {
                        totalSufficientlySalientConvexSegmentArea += ss.GetSegmentsSize(seg);
                        totalCHPoints += ch.Count;
                        numCHsegments++;
                    }
                        

                    //***
                    // testing: draw convex hull over the image
                    if (image != null)
                    {
                        using (var gfx = Graphics.FromImage(image.Bitmap))
                        {
                            Color c = Color.Red;
                            if (chArea < 0.8 * ss.Height * ss.Width && (double)ss.GetSegmentsSize(seg) / chArea > convexityRequired) c = Color.Green;

                            for (int i = 0; i < ch.Count - 1; i++)
                            {
                                gfx.DrawLine(new Pen(Color.FromArgb(200, c), 2f), (int)ch.ElementAt(i).Position[0], (int)ch.ElementAt(i).Position[1],
                                    (int)ch.ElementAt(i + 1).Position[0], (int)ch.ElementAt(i + 1).Position[1]);
                            }

                            gfx.DrawLine(new Pen(Color.FromArgb(200, c), 2f), (int)ch.Last().Position[0], (int)ch.Last().Position[1],
                                (int)ch.ElementAt(0).Position[0], (int)ch.ElementAt(0).Position[1]);
                        }
                    }
                    //***
                }
            }
            
            double cPart = 0.0;
            const double alpha = 0.1109530472;
            double averageCHPoints = (double) totalCHPoints / numCHsegments;
            if (totalSufficientlySalientConvexSegmentArea > 0)
            {
                cPart = Math.Exp((3.0 - averageCHPoints) * alpha);
            }

            return (double)totalSufficientlySalientConvexSegmentArea / numSufficientlySalientPixels * (1 - cPart);
        }

        // http://stackoverflow.com/questions/6996942/c-sharp-sort-list-of-x-y-coordinates-clockwise
        private static DefaultVertex reference;
        private static int SortCornersClockwise(DefaultVertex A, DefaultVertex B)
        {
            //  Fetch the atans
            var aTanA = Math.Atan2(A.Position[1] - reference.Position[1], A.Position[0] - reference.Position[0]);
            var aTanB = Math.Atan2(B.Position[1] - reference.Position[1], B.Position[0] - reference.Position[0]);

            //  Determine next point in Clockwise rotation
            if (aTanA < aTanB) return -1;
            if (aTanA > aTanB) return 1;
            return 0;
        }
    }
}
