using System;
using System.Collections.Generic;
using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.GraphBasedDataStructures;

namespace Part2Project.ImageSegmentation
{
    static class GraphBasedImageSegmentation
    {
        #region Initialisation

        private static double ComputeEdgeWeight(Bitmap image, int x1, int y1, int x2, int y2)
        {
            Color c1 = image.GetPixel(x1, y1);
            Color c2 = image.GetPixel(x2, y2);

            // This just works out the intensity difference between two pixels
            //double i1 = (double)c1.R * 0.21 + (double)c1.G * 0.72 + (double)c1.B * 0.07;
            //double i2 = (double)c2.R * 0.21 + (double)c2.G * 0.72 + (double)c2.B * 0.07;
            //return (int) Math.Abs(i1 - i2);

            // This converts the pixels to CIE L*A*B* color space and computes 
            CIELab lab1 = ColorSpaceHelper.RGBtoLab(c1);
            CIELab lab2 = ColorSpaceHelper.RGBtoLab(c2);
            return Math.Sqrt((lab1.A - lab2.A) * (lab1.A - lab2.A) + (lab1.B - lab2.B) * (lab1.B - lab2.B) + (lab1.L - lab2.L) * (lab1.L - lab2.L));
        }

        private static void InitialiseEdges(Bitmap image, GraphBasedDisjointSet dSet, List<GraphEdge> eList)
        {
            GraphNode[][] v = dSet.getV();

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
                        eList.Add(new GraphEdge(v[x][y], v[x - 1][y], ComputeEdgeWeight(image, x, y, x - 1, y)));
                    }

                    if (y <= 0) continue;
                    // So, y > 0

                    // Up
                    eList.Add(new GraphEdge(v[x][y], v[x][y - 1], ComputeEdgeWeight(image, x, y, x, y - 1)));

                    if (x > 0)
                    {
                        // Up-Left
                        eList.Add(new GraphEdge(v[x][y], v[x - 1][y - 1], ComputeEdgeWeight(image, x, y, x - 1, y - 1)));
                    }

                    if (x < image.Width - 1)
                    {
                        // Up-Right
                        eList.Add(new GraphEdge(v[x][y], v[x + 1][y - 1], ComputeEdgeWeight(image, x, y, x + 1, y - 1)));
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

        public static GraphBasedDisjointSet Segment(Bitmap image, double k, double sigma)
        {
            // Transform the image as required
            image = ScaleAndBlur(image, sigma);

            // Set up Data Structures
            GraphBasedDisjointSet dSet = new GraphBasedDisjointSet(image);
            List<GraphEdge> eList = new List<GraphEdge>();
            InitialiseEdges(image, dSet, eList); // Create all the edges for an 8-connected grid graph

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

            return dSet;
        }

        public static Bitmap VisualiseSegmentation(GraphBasedDisjointSet dSet) 
        {
            int width = dSet.getV().Length;
            int height = dSet.getV()[0].Length;

            Random rand = new Random();
            Bitmap outputImage = new Bitmap(width, height);

            Dictionary<GraphNode, Color> componentColours = new Dictionary<GraphNode,Color>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!componentColours.ContainsKey(dSet.FindSetOfPixel(x,y)))
                    {
                        //componentColours.Add(FindSet(V[x][y]), Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)));

                        CIELab labColor = new CIELab(dSet.FindSetOfPixel(x, y).TotalL / dSet.FindSetOfPixel(x, y).ComponentSize,
                                                     dSet.FindSetOfPixel(x, y).TotalA / dSet.FindSetOfPixel(x, y).ComponentSize,
                                                     dSet.FindSetOfPixel(x, y).TotalB / dSet.FindSetOfPixel(x, y).ComponentSize);
                        RGB rgb = ColorSpaceHelper.LabtoRGB(labColor);

                        componentColours.Add(dSet.FindSetOfPixel(x, y), Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue));
                    }

                    outputImage.SetPixel(x, y, componentColours[dSet.FindSetOfPixel(x, y)]);
                }
            }

            return outputImage;
        }
    }
}
