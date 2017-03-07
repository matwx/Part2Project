using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project
{
    class PRVectorComputer
    {
        public double[] Precisions, Recalls;

        private string _originalFilename, _truthFilename, _outPath;

        public PRVectorComputer(string originalFilename, string truthFilename, string outputDirectoryPath)
        {
            _originalFilename = originalFilename;
            _truthFilename = truthFilename;
            _outPath = outputDirectoryPath;
        }

        private bool edgeMap1Helper(Segmentation s, int i, int x, int y)
        {
            int size = s.GetPixelsSegmentSize(x, y);
            if (size < 0.01 * s.Width * s.Height) return true;

            return i == s.GetPixelsSegmentIndex(x, y);
        }
        private double edgeMap2Helper(SaliencySegmentation ss, int x, int y)
        {
            double result = 0.0;

            // return the maximum saliency of the 3x3 neighbourhood
            if (x > 0 && x < ss.Width - 1 && y > 0 && y < ss.Height - 1)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    for (int k = y - 1; k <= y + 1; k++)
                    {
                        if (ss.GetPixelsSegmentSize(j, k) > 0.01 * ss.Width * ss.Height)
                            result = Math.Max(result, ss.GetSegmentsSaliency(ss.GetPixelsSegmentIndex(j, k)));
                    }
                }
            }

            return result;
        }
        private DirectBitmap GenerateEdgeMap(DirectBitmap image)
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
            SaliencySegmentation ss = new SaliencySegmentation(s, image, sigma);

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = s.GetPixelsSegmentIndex(x, y);
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x - 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y + 1);
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x + 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y + 1);
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y - 1);
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y + 1);
                    }

                    // This time, make the edge colour a function of the segment saliencies
                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else
                    {
                        double sal = edgeMap2Helper(ss, x, y);
                        int val = (int)(sal * sal * 255.0);
                        edgeMap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
            }

            return edgeMap;
        }
        private DirectBitmap GetImageScaled(string filename)
        {
            DirectBitmap result;

            using (Image selected = Image.FromFile(filename))
            {
                result = new DirectBitmap(320, 240);
                // Create the required resized image
                using (Graphics gfx = Graphics.FromImage(result.Bitmap))
                {
                    if (selected.Width / (double)selected.Height > 4.0 / 3.0)
                    {
                        // Too wide - crop left and right
                        int originalWidth = (int)((double)selected.Width / (double)selected.Height * 240.0);
                        gfx.DrawImage(selected, 160 - originalWidth / 2, 0, originalWidth, 240);
                    }
                    else if (selected.Width / (double)selected.Height < 4.0 / 3.0)
                    {
                        // Too narrow - crop top and bottom
                        int originalHeight = (int)((double)selected.Height / (double)selected.Width * 320.0);
                        gfx.DrawImage(selected, 0, 120 - originalHeight / 2, 320, originalHeight);
                    }
                    else
                    {
                        // Correct AR
                        gfx.DrawImage(selected, 0, 0, 320, 240);
                    }
                }
            }

            return result;
        }
        public void ComputeVectors(int id)
        {
            // Compute Edge map
            DirectBitmap salEdges = GenerateEdgeMap(GetImageScaled(_originalFilename));
            DirectBitmap trueEdges = GetImageScaled(_truthFilename);
            Precisions = new double[31];
            Recalls = new double[31];

            // Normalise True Edge Map
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Compute the precision and recall vectors
            for (int i = 0; i < 31; i++)
            {
                int thresh = (i + 1) * 8;

                int totalTruePix = 0, totalGenAndTruePix = 0, totalGenPix = 0;
                // Precision is the probability that a machine-generated boundary pixel is a true boundary pixel.
                // Recall is the probability that a true boundary pixel is detected.
                for (int x = 0; x < 320; x++)
                {
                    for (int y = 0; y < 240; y++)
                    {
                        if (trueEdges.GetPixel(x, y).R >= thresh)
                        {
                            totalTruePix++;
                            if (salEdges.GetPixel(x, y).R >= thresh) totalGenAndTruePix++;
                        }
                        if (salEdges.GetPixel(x, y).R >= thresh) totalGenPix++;
                    }
                }

                Precisions[i] = (totalGenPix == 0) ? 0 : (double)totalGenAndTruePix / totalGenPix;
                Recalls[i] = (totalTruePix == 0) ? 0 : (double)totalGenAndTruePix / totalTruePix;
            }

            // Save file for progress
            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < 31; i++)
            {
                output += Recalls[i] + "," + Precisions[i] + nl;
            }

            File.WriteAllText(_outPath + "\\" +  id + ".txt", output);
        }
    }
}
