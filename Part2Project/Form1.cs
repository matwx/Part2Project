using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Part2Project.Features;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project
{
    public partial class Form1 : Form
    {
        private DirectBitmap image, edges;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            dlgImage.ShowDialog();
        }
        private void btnSelectRealEdges_Click(object sender, EventArgs e)
        {
            dlgEdges.ShowDialog();
        }

        private void dlgImage_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgImage.FileName != "")
            {
                using (Image selected = Image.FromFile(dlgImage.FileName))
                {
                    image = new DirectBitmap(320, 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
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

                viewer1.Image = image.Bitmap;
            }
        }
        private void dlgEdges_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgEdges.FileName != "")
            {
                using (Image selected = Image.FromFile(dlgEdges.FileName))
                {
                    edges = new DirectBitmap(320, 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(edges.Bitmap))
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

                viewer4.Image = edges.Bitmap;
            }
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
        private DirectBitmap Edge1()
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);

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

                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else edgeMap.SetPixel(x, y, Color.White);
                }
            }

            return edgeMap;
        }
        private DirectBitmap Edge2()
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
        private void btnEdge1_Click(object sender, EventArgs e)
        {
            // Compute Edge maps
            DirectBitmap naiveEdges = Edge1();
            DirectBitmap salEdges = Edge2();
            DirectBitmap naiveEdges_B, salEdges_B, trueEdges_B;

            viewer2.Image = naiveEdges.Bitmap;
            viewer3.Image = salEdges.Bitmap;

            // Blur them
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(naiveEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                naiveEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }
            using (KalikoImage kImage = new KalikoImage(salEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                salEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            viewer5.Image = naiveEdges_B.Bitmap;
            viewer6.Image = salEdges_B.Bitmap;

            // Then normalise true edge map
            DirectBitmap trueEdges = new DirectBitmap((Bitmap) viewer4.Image);
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
                    int val = (int) ((double) trueEdges.GetPixel(x, y).R / (double) max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            viewer7.Image = trueEdges.Bitmap;

            // Then blur truth edge map
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            viewer8.Image = trueEdges_B.Bitmap;

            // Then compute Correlations
            // First, naive images
            double naiveResult = 0.0, totalNaiveValues = 0.0, totalTrueValues = 0.0, totalSalValues = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    totalNaiveValues += Math.Pow(naiveEdges_B.GetPixel(x, y).R / 255.0, 2);
                    totalSalValues += Math.Pow(salEdges_B.GetPixel(x, y).R / 255.0, 2);
                    totalTrueValues += Math.Pow(trueEdges_B.GetPixel(x, y).R / 255.0, 2);
                    naiveResult += Math.Pow((naiveEdges_B.GetPixel(x, y).R / 255.0) - (trueEdges_B.GetPixel(x, y).R / 255.0), 2);
                }
            }
            naiveResult /= (totalNaiveValues + totalTrueValues);
            naiveResult = (1 - naiveResult) * 100.0;
            blurredNaive.Text = naiveResult + "%";

            // Second, saliency images
            double salResult = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    salResult += Math.Pow((salEdges_B.GetPixel(x, y).R / 255.0) - (trueEdges_B.GetPixel(x, y).R / 255.0), 2);
                }
            }
            salResult /= (totalSalValues + totalTrueValues);
            salResult = (1 - salResult) * 100.0;
            blurredSaliency.Text = salResult + "%";
        }
        
        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
