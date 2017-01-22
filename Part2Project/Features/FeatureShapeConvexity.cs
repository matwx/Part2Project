using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIConvexHull;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project.Features
{
    static class FeatureShapeConvexity
    {
        //https://www.researchgate.net/profile/Jia_Li87/publication/221304720_Studying_Aesthetics_in_Photographic_Images_Using_a_Computational_Approach/links/55a71b9b08ae51639c5762ed.pdf
        //https://designengrlab.github.io/MIConvexHull/

        public static double ComputeFeature(DirectBitmap image, Segmentation s)
        {
            // Re-organise data into lists of points for each segment
            List<DefaultVertex>[] segments = new List<DefaultVertex>[s.NumSegments];
            for (int i = 0; i < s.NumSegments; i++)
            {
                if (s.GetSegmentsSize(i) >= 0.01 * s.Width * s.Height)
                    segments[i] = new List<DefaultVertex>();
            }
            for (int x = 0; x < s.Width; x++)
            {
                for (int y = 0; y < s.Height; y++)
                {
                    if (s.GetPixelsSegmentSize(x, y) >= 0.01 * s.Width * s.Height)
                    {
                        DefaultVertex v = new DefaultVertex();
                        v.Position = new double[] {x, y};
                        segments[s.GetPixelsSegmentIndex(x, y)].Add(v);
                    }
                }
            }

//            // For each segment larger than 1% of image,
//            //  - Compute its convex hull
//            //  - Compute the proportion of segment pixels in its convex hull
//            //  - Compute proportion of image that is taken up by >80% convex segments
//
            IEnumerable<DefaultVertex> points = new List<DefaultVertex>();
            for (int i = 0; i < s.NumSegments; i++)
            {
                if (s.GetSegmentsSize(i) >= 0.01 * s.Width * s.Height)
                {
                    // Compute its convex hull

                    var ch = ConvexHull.Create(segments[i]).Points;
                    if (ch.Count() > points.Count()) points = ch;
                }
            }

//            points = points.OrderBy(x => Math.Atan2(x.Position[0], x.Position[1])).ToList();

            using (var gfx = Graphics.FromImage(image.Bitmap))
            {

                for (int i = 0; i < points.Count() - 1; i++)
                {
                    gfx.DrawLine(Pens.Red, (int) points.ElementAt(i).Position[0], (int) points.ElementAt(i).Position[1],
                        (int) points.ElementAt(i+1).Position[0], (int) points.ElementAt(i+1).Position[1]);
                }
            }

            return 0.0;
        }

//        private static DefaultVertex reference;

//        public static int SortCornersClockwise(DefaultVertex A, DefaultVertex B)
//        {
//            //  Variables to Store the atans
//            double aTanA, aTanB;
//
//            //  Reference Point
//            Pixels reference = Program.main.reference;
//
//            //  Fetch the atans
//            aTanA = Math.Atan2(A.Pixel.Y - reference.Y, A.Pixel.X - reference.X);
//            aTanB = Math.Atan2(B.Pixel.Y - reference.Y, B.Pixel.X - reference.X);
//
//            //  Determine next point in Clockwise rotation
//            if (aTanA < aTanB) return -1;
//            else if (aTanB > aTanA) return 1;
//            return 0;
//        }
    }
}
