using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Kaliko.ImageLibrary.ColorSpace;

namespace Part2Project.ImageSegmentation
{
    class GraphBasedImageSegmentation
    {
        private class GraphNode
        {
            private int x, y, rank, componentSize;
            private double internalDifference;
            private GraphNode parent;

            double totalL, totalA, totalB;

            public GraphNode(int xCoord, int yCoord, CIELab labColor)
            {
                x = xCoord;
                y = yCoord;

                rank = 0;
                componentSize = 1;
                internalDifference = 0;
                parent = this;

                totalL = labColor.L;
                totalA = labColor.A;
                totalB = labColor.B;
            }

            // Property wrappers
            public int X
            {
                get { return x; }
            } //R
            public int Y
            {
                get { return y; }
            } //R
            public int Rank
            {
                get { return rank; }
                set { rank = value; }
            } //RW
            public int ComponentSize
            {
                get { return componentSize; }
                set { componentSize = value; }
            } //RW
            public double InternalDifference
            {
                get { return internalDifference; }
                set { internalDifference = value; }
            } //RW
            public GraphNode Parent
            {
                get { return parent; }
                set { parent = value; }
            } //RW

            public double TotalL
            {
                get { return totalL; }
                set { totalL = value; }
            } //RW
            public double TotalA
            {
                get { return totalA; }
                set { totalA = value; }
            } //RW
            public double TotalB
            {
                get { return totalB; }
                set { totalB = value; }
            } //RW
        }
        private class GraphEdge : IComparable
        {
            private GraphNode v1, v2; // The two vertices that this edge connects
            private double weight; // Need to decide here whether to use integer or f.p. weights

            public GraphEdge(GraphNode node1, GraphNode node2, double w)
            {
                v1 = node1;
                v2 = node2;

                weight = w;
            }

            public int CompareTo(object obj)
            {
                GraphEdge otherEdge = obj as GraphEdge;
                if (otherEdge == null)
                {
                    throw new ArgumentException("Object is not a GraphEdge. You can only compare a GraphEdge to another GraphEdge.");
                }
                else
                {
                    return this.weight.CompareTo(otherEdge.weight);
                }
            }

            // Property Wrappers
            public double Weight
            {
                get { return weight; }
            } //R
            public GraphNode V1
            {
                get { return v1; }
            } //R
            public GraphNode V2
            {
                get { return v2; }
            } //R
        }

        GraphNode[][] V;
        List<GraphEdge> E;
        Bitmap image;
        double k, sigma;
        string displayType, edgeWeightType;

        public GraphBasedImageSegmentation(Bitmap inputImage, double kParam, double sigmaParam, string display = "Random", string edgeType = "Intensity Difference")
        { 
            k = kParam;
            sigma = sigmaParam;
            image = inputImage;
            displayType = display;
            edgeWeightType = edgeType;

            ScaleAndBlur();
            InitialiseDataStructures();
        }

        public void Start()
        {
            // Sort E by non-decreasing edge weight
            E.Sort();

            // For each edge e
            foreach (GraphEdge e in E)
            {
                // If the edge joins two discinct components
                if (FindSet(e.V1) != FindSet(e.V2))
                { 
                    // If the weight is small enough compared to the components
                    // w <= MInt(C1, C2)
                    if (e.Weight <= Math.Min(FindSet(e.V1).InternalDifference + k / FindSet(e.V1).ComponentSize,
                                             FindSet(e.V2).InternalDifference + k / FindSet(e.V2).ComponentSize))
                    { 
                        // Then merge the two components
                        Union(e.V1, e.V2, e.Weight);
                    }
                }
            }
        }

        private double ComputeEdgeWeight(int x1, int y1, int x2, int y2)
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
            else
            { 
                // Default to intensity

                // This just works out the intensity difference between two pixels
                double i1 = (double)c1.R * 0.21 + (double)c1.G * 0.72 + (double)c1.B * 0.07;
                double i2 = (double)c2.R * 0.21 + (double)c2.G * 0.72 + (double)c2.B * 0.07;
                return (int)Math.Abs(i1 - i2);
            }
        }

        private void InitialiseDataStructures()
        {
            // Create a node for each image pixel
            V = new GraphNode[image.Width][];
            for (int x = 0; x < image.Width; x++)
            {
                V[x] = new GraphNode[image.Height];

                for (int y = 0; y < image.Height; y++)
                {
                    V[x][y] = new GraphNode(x, y, ColorSpaceHelper.RGBtoLab(image.GetPixel(x,y)));
                }
            }

            // Create all the edges for an 8-connected grid graph
            E = new List<GraphEdge>();
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
                        E.Add(new GraphEdge(V[x][y], V[x - 1][y], ComputeEdgeWeight(x, y, x - 1, y)));
                    }

                    if (y > 0)
                    {
                        // Up
                        E.Add(new GraphEdge(V[x][y], V[x][y - 1], ComputeEdgeWeight(x, y, x, y - 1)));

                        if (x > 0)
                        {
                            // Up-Left
                            E.Add(new GraphEdge(V[x][y], V[x - 1][y - 1], ComputeEdgeWeight(x, y, x - 1, y - 1)));
                        }

                        if (x < image.Width - 1)
                        {
                            // Up-Right
                            E.Add(new GraphEdge(V[x][y], V[x + 1][y - 1], ComputeEdgeWeight(x, y, x + 1, y - 1)));
                        }
                    }
                }
            }
        }

        private void ScaleAndBlur()
        { 
            KalikoImage kImage = new KalikoImage(image);

            kImage.ApplyFilter(new GaussianBlurFilter((float) sigma));

            image = kImage.GetAsBitmap();
        }

        private GraphNode FindSet(GraphNode v)
        {
            if (v.Parent != v) v.Parent = FindSet(v.Parent);

            return v.Parent;
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

        private void Union(GraphNode v1, GraphNode v2, double joiningEdgeWeight)
        { 
            // Merges the sets containing v1 and v2
            Link(FindSet(v1), FindSet(v2), joiningEdgeWeight);
        }

        public Bitmap VisualiseSegmentation() 
        {
            // return image; // Uncomment to show blurred version

            Random rand = new Random();
            Bitmap outputImage = new Bitmap(image.Width, image.Height);

            Dictionary<GraphNode, Color> componentColours = new Dictionary<GraphNode,Color>();

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (componentColours.ContainsKey(FindSet(V[x][y])))
                    {

                    }
                    else
                    {
                        if (displayType.Equals("Average"))
                        {
                            // We like the average colour in CIELab space

                            CIELab labColor = new CIELab(FindSet(V[x][y]).TotalL / (double)FindSet(V[x][y]).ComponentSize,
                                                     FindSet(V[x][y]).TotalA / (double)FindSet(V[x][y]).ComponentSize,
                                                     FindSet(V[x][y]).TotalB / (double)FindSet(V[x][y]).ComponentSize);
                            RGB rgb = ColorSpaceHelper.LabtoRGB(labColor);

                            componentColours.Add(FindSet(V[x][y]), Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue));
                        }
                        else
                        { 
                            // Default to Random
                            componentColours.Add(FindSet(V[x][y]), Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)));
                        }
                    }

                    outputImage.SetPixel(x, y, componentColours[FindSet(V[x][y])]);
                }
            }

            return outputImage;
        }
    }
}
