using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.GraphBasedDataStructures;

namespace Part2Project.ImageSegmentation
{
    // This base class is itself very similar to the 2-D array of Graph Nodes in the GraphBasedDisjointSet.
    // However, it is designed to be extended to add more segment charactersistics.
    class Segmentation
    {
        protected int[][] _pixelAssignments;
        protected Dictionary<int, int> _segmentSizes;
        protected Dictionary<int, CIELab> _segmentColours;
        public int NumSegments { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Segmentation(GraphBasedDisjointSet set)
        {
            // Need to enumerate each segment, computing the sizes and colour of each segment as we come across it

            _segmentSizes = new Dictionary<int, int>();
            _segmentColours = new Dictionary<int, CIELab>();

            Dictionary<GraphNode, int> segmentIndexes = new Dictionary<GraphNode, int>();

            Width = set.GetV().Length;
            Height = set.GetV()[0].Length;
            int currentSegment = 0;

            _pixelAssignments = new int[Width][];
            for (int x = 0; x < Width; x++)
            {
                _pixelAssignments[x] = new int[Height];
                for (int y = 0; y < Height; y++)
                {
                    GraphNode rep = set.FindSetOfPixel(x, y);

                    if (segmentIndexes.ContainsKey(rep))
                    {
                        // We've seen this segment before
                        _pixelAssignments[x][y] = segmentIndexes[rep];
                    }
                    else
                    {
                        // We haven't seen this segment before
                        segmentIndexes.Add(rep, currentSegment);

                        _pixelAssignments[x][y] = currentSegment;
                        _segmentSizes.Add(currentSegment, rep.ComponentSize);
                        _segmentColours.Add(currentSegment, new CIELab(rep.TotalL / rep.ComponentSize, rep.TotalA / rep.ComponentSize, rep.TotalB / rep.ComponentSize));
                        
                        currentSegment++;
                    }
                }
            }

            NumSegments = currentSegment;
        }

        protected Segmentation(Segmentation s)
        {
            NumSegments = s.NumSegments;
            Width = s.Width;
            Height = s.Height;

            _pixelAssignments = new int[Width][];
            for (int i = 0; i < Width; i++)
            {
                _pixelAssignments[i] = new int[Height];
                for (int j = 0; j < Height; j++)
                {
                    _pixelAssignments[i][j] = s._pixelAssignments[i][j];
                }
            }
            
            _segmentSizes = new Dictionary<int, int>();
            _segmentColours = new Dictionary<int, CIELab>();
            for (int i = 0; i < s.NumSegments; i++)
            {
                _segmentSizes.Add(i, s._segmentSizes[i]);
                _segmentColours.Add(i, s._segmentColours[i]);
            }
        }

        public int GetPixelsSegmentSize(int x, int y)
        {
            return _segmentSizes[_pixelAssignments[x][y]];
        }

        public CIELab GetPixelsSegmentColour(int x, int y)
        {
            return _segmentColours[_pixelAssignments[x][y]];
        }

        public int GetSegmentsSize(int i)
        {
            return _segmentSizes[i];
        }
    }
}
