using System;
using System.Drawing;
using System.Windows.Forms;
using Part2Project.Features;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project
{
    public partial class Form1 : Form
    {
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

        private bool edgeMapHelper(Segmentation s, int i, int x, int y)
        {
            int size = s.GetPixelsSegmentSize(x, y);
            if (size < 0.01 * s.Width * s.Height) return true;

            return i == s.GetPixelsSegmentIndex(x, y);
        }
        private void dlgImage_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgImage.FileName != "")
            {

                // Get segmentation
                Segmentation s;
                DirectBitmap image;
                using (Image selected = Image.FromFile(dlgImage.FileName))
                {
                    image = new DirectBitmap((int) ((double) selected.Width / (double) selected.Height * 240.0), 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    }

                    // Segmentation-Derived
                    const int k = 125;
                    const double sigma = 0.6;
                    s = GraphBasedImageSegmentation.Segment(image, k, sigma);
                }

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
                                notOnEdge &= edgeMapHelper(s, i, x - 1, y - 1);
                            }
                            notOnEdge &= edgeMapHelper(s, i, x - 1, y);
                            if (y < edgeMap.Height - 1)
                            {
                                notOnEdge &= edgeMapHelper(s, i, x - 1, y + 1);
                            }
                        }
                        if (x < edgeMap.Width - 1)
                        {
                            if (y > 0)
                            {
                                notOnEdge &= edgeMapHelper(s, i, x + 1, y - 1);
                            }
                            notOnEdge &= edgeMapHelper(s, i, x + 1, y);
                            if (y < edgeMap.Height - 1)
                            {
                                notOnEdge &= edgeMapHelper(s, i, x + 1, y + 1);
                            }
                        }
                        if (y > 0)
                        {
                            notOnEdge &= edgeMapHelper(s, i, x, y - 1);
                        }
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMapHelper(s, i, x, y + 1);
                        }

                        if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                        else edgeMap.SetPixel(x, y, Color.White);
                    }
                }

                viewer1.Image = image.Bitmap;
                viewer2.Image = edgeMap.Bitmap;
            }
        }
    }
}
