﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kaliko.ImageLibrary.ColorSpace;

namespace Part2Project.GraphBasedDataStructures
{
    class GraphBasedDisjointSet
    {
        private GraphNode[][] V;

        public GraphBasedDisjointSet(Bitmap image)
        {
            // Create a node for each image pixel
            V = new GraphNode[image.Width][];
            for (int x = 0; x < image.Width; x++)
            {
                V[x] = new GraphNode[image.Height];

                for (int y = 0; y < image.Height; y++)
                {
                    V[x][y] = new GraphNode(x, y, ColorSpaceHelper.RGBtoLab(image.GetPixel(x, y)));
                }
            }
        }

        public GraphNode FindSet(GraphNode v)
        {
            if (v.Parent != v) v.Parent = FindSet(v.Parent);

            return v.Parent;
        }

        public GraphNode FindSetOfPixel(int x, int y) {
            return FindSet(V[x][y]);
        }

        public void Union(GraphNode v1, GraphNode v2, double joiningEdgeWeight)
        {
            // Merges the sets containing v1 and v2
            Link(FindSet(v1), FindSet(v2), joiningEdgeWeight);
        }

        private void Link(GraphNode v1, GraphNode v2, double joiningEdgeWeight)
        {
            // Merges the sets represented by v1 and v2

            // The maximum weight edge in the MST of the new component is simple the
            // weight of the edge causing the merge. The new component size is the sum
            // of the size of the two components being merged

            if (v1.Rank > v2.Rank)
            {
                // v1 is the new representative of the merged set
                v2.Parent = v1;

                v1.ComponentSize += v2.ComponentSize;
                v1.InternalDifference = joiningEdgeWeight;

                v1.TotalL += v2.TotalL;
                v1.TotalA += v2.TotalA;
                v1.TotalB += v2.TotalB;
            }
            else
            {
                // v2 is the new representative of the merged set
                v1.Parent = v2;
                if (v1.Rank == v2.Rank) v2.Rank++;

                v2.ComponentSize += v1.ComponentSize;
                v2.InternalDifference = joiningEdgeWeight;

                v2.TotalL += v1.TotalL;
                v2.TotalA += v1.TotalA;
                v2.TotalB += v1.TotalB;
            }
        }

        public GraphNode[][] getV() { return V; }
    }
}
