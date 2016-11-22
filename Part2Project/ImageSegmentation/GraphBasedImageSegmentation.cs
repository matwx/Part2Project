using System;
using System.Collections.Generic;
using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.MyColor;
using Part2Project.GraphBasedDataStructures;

namespace Part2Project.ImageSegmentation
{
    static class GraphBasedImageSegmentation
    {
        #region Initialisation

        private static double deltaE(CIELab c1, CIELab c2)
        {
            double k_L = 2, k_C = 1, k_H = 1;

            double deltaLp = c2.L - c1.L;
            double Lbar = (c1.L + c2.L) / 2;

            double C1 = Math.Sqrt(c1.A * c1.A + c1.B * c1.B);
            double C2 = Math.Sqrt(c2.A * c2.A + c2.B * c2.B);
            double Cbar = (C1 + C2) / 2;

            double a1p = c1.A + c1.A / 2 * (1 - Math.Sqrt(Math.Pow(Cbar, 7) / (Math.Pow(Cbar, 7) + Math.Pow(25, 7))));
            double a2p = c2.A + c2.A / 2 * (1 - Math.Sqrt(Math.Pow(Cbar, 7) / (Math.Pow(Cbar, 7) + Math.Pow(25, 7))));

            double C1p = Math.Sqrt(a1p * a1p + c1.B * c1.B);
            double C2p = Math.Sqrt(a2p * a2p + c2.B * c2.B);

            double Cbarp = (C1p + C2p) / 2;
            double deltaCp = C2p - C1p;

            double h1p, h2p;
            if (a1p == 0 && c1.B == 0) h1p = 0;
            else h1p = ((Math.Atan2(c1.B, a1p) + Math.PI) / Math.PI * 180); // Do we need a % 360 here?
            if (a2p == 0 && c2.B == 0) h2p = 0;
            else h2p = ((Math.Atan2(c2.B, a2p) + Math.PI) / Math.PI * 180); // Do we need a % 360 here?

            double deltahp;
            if (C1p == 0 || C2p == 0) deltahp = 0;
            else if (Math.Abs(h1p - h2p) <= 180) deltahp = h2p - h1p;
            else if (h2p <= h1p) deltahp = h2p - h1p + 360;
            else deltahp = h2p - h1p - 360;

            double deltaHp = 2 * Math.Sqrt(C1p * C2p) * Math.Sin(deltahp / 2 / 180 * Math.PI); // Perhaps it wants the angle in degrees here?
            double Hbarp;
            if (C1p == 0 || C2p == 0) Hbarp = h1p + h2p;
            else if (Math.Abs(h1p - h2p) <= 180) Hbarp = (h1p + h2p) / 2;
            else if (h1p + h2p < 360) Hbarp = (h1p + h2p + 360) / 2;
            else Hbarp = (h1p + h2p - 360) / 2;

            double T = 1
                       - 0.17 * Math.Cos((Hbarp - 30) / 180 * Math.PI)
                       + 0.24 * Math.Cos(2 * Hbarp / 180 * Math.PI)
                       + 0.32 * Math.Cos((3 * Hbarp + 6) / 180 * Math.PI)
                       - 0.20 * Math.Cos((4 * Hbarp - 63) / 180 * Math.PI); // Perhaps it wants the angle in degrees here?

            double S_L = 1 + 0.015 * (Lbar - 50) * (Lbar - 50) / Math.Sqrt(20 + (Lbar - 50) * (Lbar - 50));
            double S_C = 1 + 0.045 * Cbarp;
            double S_H = 1 + 0.015 * Cbarp * T;

            double R_T = -2 * Math.Sqrt(Math.Pow(Cbarp, 7) / (Math.Pow(Cbarp, 7) + Math.Pow(25, 7)))
                         * Math.Sin(60 * Math.Exp(-((Hbarp - 275) / 25) * ((Hbarp - 275) / 25)) / 180 * Math.PI); // Perhaps it wants the angle in degrees here?

            double deltaE = Math.Sqrt((deltaLp / k_L / S_L) * (deltaLp / k_L / S_L)
                                    + (deltaCp / k_C / S_C) * (deltaCp / k_C / S_C)
                                    + (deltaHp / k_H / S_H) * (deltaHp / k_H / S_H)
                                    + R_T * deltaCp * deltaHp / k_C / S_C / k_H / S_H);

            return deltaE;
        }

        private static double ComputeEdgeWeight(Bitmap image, int x1, int y1, int x2, int y2, string edgeWeightType)
        {
            Color c1 = image.GetPixel(x1, y1);
            Color c2 = image.GetPixel(x2, y2);

            if (edgeWeightType.Equals("CIELabDist"))
            {
                // This converts the pixels to CIE L*A*B* color space and computes 
                CIELab lab1 = ColorSpaceHelper.RGBtoLab(c1);
                CIELab lab2 = ColorSpaceHelper.RGBtoLab(c2);

                return Math.Sqrt((lab1.A - lab2.A) * (lab1.A - lab2.A) + (lab1.B - lab2.B) * (lab1.B - lab2.B) + (lab1.L - lab2.L) * (lab1.L - lab2.L));
            }
            else if (edgeWeightType.Equals("Hybrid"))
            {
                // This is the same as CIEDE2000 except when the pixels are quite bright or dark,
                // in which case we revert back to intensity differences. This seems to help with
                // a problem I've been finding with the Lab distance metric in very light/dark
                // regions.

                double i1 = (double)c1.R * 0.21 + (double)c1.G * 0.72 + (double)c1.B * 0.07;
                double i2 = (double)c2.R * 0.21 + (double)c2.G * 0.72 + (double)c2.B * 0.07;

                if (Math.Min(i1, i2) < 20 || Math.Max(i1, i2) > 210)
                {
                    return Math.Abs(i1 - i2) / 255 * 10;
                }
                else
                {
                    return deltaE(ColorSpaceHelper.RGBtoLab(c1), ColorSpaceHelper.RGBtoLab(c2));
                }
            }
            else if (edgeWeightType.Equals("HybridInterpolation"))
            {
                // This is the same as CIEDE2000 except when the pixels are quite bright or dark,
                // in which case we revert back to intensity differences. This seems to help with
                // a problem I've been finding with the Lab distance metric in very light/dark
                // regions.

                double i1 = (double)c1.R * 0.21 + (double)c1.G * 0.72 + (double)c1.B * 0.07;
                double i2 = (double)c2.R * 0.21 + (double)c2.G * 0.72 + (double)c2.B * 0.07;

                CIELab lab1 = ColorSpaceHelper.RGBtoLab(c1);
                CIELab lab2 = ColorSpaceHelper.RGBtoLab(c2);

                if (Math.Min(i1, i2) < 10 || Math.Max(i1, i2) > 230)
                {
                    return Math.Abs(i1 - i2) / 255;
                }
                if (Math.Min(i1, i2) < 20 || Math.Max(i1, i2) > 210)
                {
                    double iVal = Math.Abs(i1 - i2) / 255;
                    double eVal = deltaE(lab1, lab2);
                    double t;

                    if (Math.Min(i1, i2) < 35) t = (Math.Min(i1, i2) - 10) / 10;
                    else t = (230 - Math.Max(i1, i2)) / 20;

                    return eVal * t + iVal * (1 - t);
                }
                else
                {
                    return deltaE(lab1, lab2);
                }
            }
            else if (edgeWeightType.Equals("CIEDE2000"))
            {
                return deltaE(ColorSpaceHelper.RGBtoLab(c1), ColorSpaceHelper.RGBtoLab(c2));
            }
            else
            {
                // Default to intensity

                // This just works out the intensity difference between two pixels
                double i1 = (double)c1.R * 0.21 + (double)c1.G * 0.72 + (double)c1.B * 0.07;
                double i2 = (double)c2.R * 0.21 + (double)c2.G * 0.72 + (double)c2.B * 0.07;
                return (int)Math.Abs(i1 - i2);
            }
        }

        private static void InitialiseEdges(Bitmap image, GraphBasedDisjointSet dSet, List<GraphEdge> eList, string edgeWeightType)
        {
            GraphNode[][] v = dSet.GetV();

            // Create all the edges for an 8-connected grid graph
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // \    |    /
                    //   \  |  /
                    //     \|/
                    // -----o
                    // Only add an edge for the 'left', 'up-left', 'up' and 'up-right' neighbours
                    // for each vertex. This will stop the problem of adding the 'same' edge twice.


                    if (x > 0)
                    {
                        // Left
                        eList.Add(new GraphEdge(v[x][y], v[x - 1][y], ComputeEdgeWeight(image, x, y, x - 1, y, edgeWeightType)));
                    }

                    if (y <= 0) continue;
                    // So, y > 0

                    // Up
                    eList.Add(new GraphEdge(v[x][y], v[x][y - 1], ComputeEdgeWeight(image, x, y, x, y - 1, edgeWeightType)));

                    if (x > 0)
                    {
                        // Up-Left
                        eList.Add(new GraphEdge(v[x][y], v[x - 1][y - 1], ComputeEdgeWeight(image, x, y, x - 1, y - 1, edgeWeightType)));
                    }

                    if (x < image.Width - 1)
                    {
                        // Up-Right
                        eList.Add(new GraphEdge(v[x][y], v[x + 1][y - 1], ComputeEdgeWeight(image, x, y, x + 1, y - 1, edgeWeightType)));
                    }
                }
            }
        }

        private static Bitmap ScaleAndBlur(Bitmap image, double sigma)
        {
            KalikoImage kImage = new KalikoImage(image);

            kImage.ApplyFilter(new GaussianBlurFilter((float)sigma));

            return kImage.GetAsBitmap();
        }

        #endregion

        public static Segmentation Segment(Bitmap image, double k, double sigma, string edgeWeightType)
        {
            // Transform the image as required
            image = ScaleAndBlur(image, sigma);

            // Set up Data Structures
            GraphBasedDisjointSet dSet = new GraphBasedDisjointSet(image);
            List<GraphEdge> eList = new List<GraphEdge>();
            InitialiseEdges(image, dSet, eList, edgeWeightType); // Create all the edges for an 8-connected grid graph

            // Sort E by non-decreasing edge weight
            eList.Sort();

            // For each edge eList
            foreach (GraphEdge e in eList)
            {
                // If the edge joins two discinct components
                if (dSet.FindSet(e.V1) != dSet.FindSet(e.V2))
                { 
                    // If the weight is small enough compared to the components
                    // w <= MInt(C1, C2)
                    if (e.Weight <= Math.Min(dSet.FindSet(e.V1).InternalDifference + k / dSet.FindSet(e.V1).ComponentSize,
                                             dSet.FindSet(e.V2).InternalDifference + k / dSet.FindSet(e.V2).ComponentSize))
                    { 
                        // Then merge the two components
                        dSet.Union(e.V1, e.V2, e.Weight);
                    }
                }
            }

            return new Segmentation(dSet);
        }
    }
}
