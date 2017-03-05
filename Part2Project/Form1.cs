using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Part2Project.Features;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project
{
    public partial class Form1 : Form
    {
        private DirectBitmap image;

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

        private void dlgImage_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgImage.FileName != "")
            {
                using (Image selected = Image.FromFile(dlgImage.FileName))
                {
                    image = new DirectBitmap((int) ((double) selected.Width / (double) selected.Height * 240.0), 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    }

                    
                }

                viewer1.Image = image.Bitmap;
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
            viewer2.Image = Edge1().Bitmap;
            viewer3.Image = Edge2().Bitmap;
        }
        private void btnEdge2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
