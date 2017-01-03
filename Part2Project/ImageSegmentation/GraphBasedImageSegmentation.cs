using System;
using System.Collections.Generic;
using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.MyColor;
using Part2Project.GraphBasedDataStructures;
using Part2Project.Infrastructure;

namespace Part2Project.ImageSegmentation
{
    static class GraphBasedImageSegmentation
    {
        #region Initialisation

        private static double ComputeEdgeWeight(DirectBitmap image, int x1, int y1, int x2, int y2)
        {
            Color c1 = image.GetPixel(x1, y1);
            Color c2 = image.GetPixel(x2, y2);

            return MyColorSpaceHelper.MyColourDifference(c1, c2);
        }

        private static void InitialiseEdges(DirectBitmap image, GraphBasedDisjointSet dSet, List<GraphEdge> eList)
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

        public static Segmentation Segment(DirectBitmap image, double k, double sigma)
        {
            // Transform the image as required
            image = new DirectBitmap(ScaleAndBlur(image.Bitmap, sigma));

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

            return new Segmentation(dSet);
        }
    }
}
