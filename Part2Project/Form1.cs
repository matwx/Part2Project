using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.ImageSegmentation;
using Part2Project.MyColor;

namespace Part2Project
{
    public partial class Form1 : Form
    {
        Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dlgChooseImage_FileOk(object sender, CancelEventArgs e)
        {
            // Resize the image so that it's height fits in the viewer
            Image selected = Image.FromFile(dlgChooseImage.FileName);
            bmp = new Bitmap((int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
            viewer.Image = bmp;

            // Image loaded
            viewer2.Image = null;

            if (!btnGBIS.Visible)
            {
                btnGBIS.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                txtK.Visible = true;
                txtSigma.Visible = true;
                cmboDisplayType.Visible = true;
                cmboEdgeWeights.Visible = true;
                cmboDisplayType.SelectedItem = "Random";
                cmboEdgeWeights.SelectedItem = "IntensityDiff";
            }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            // Choose an image
            dlgChooseImage.ShowDialog();
        }

        private void btnGBIS_Click(object sender, EventArgs e)
        {
            // Do GBIS on our (resized) input image

            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), double.Parse(txtSigma.Text), (string) cmboEdgeWeights.SelectedItem);

            // Create output segmentation image
            Bitmap outImage = new Bitmap(bmp.Width, bmp.Height);
            Color[] randomColours = new Color[s.NumSegments];
            Random rand = new Random();
            bool useRandomColours = ((string) (cmboDisplayType.SelectedItem)).Equals("Random");

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (useRandomColours)
                    {
                        if (randomColours[s.GetPixelsSegmentIndex(x, y)].IsEmpty)
                            randomColours[s.GetPixelsSegmentIndex(x, y)] = Color.FromArgb(rand.Next(0, 256),
                                rand.Next(0, 256), rand.Next(0, 256));
                        outImage.SetPixel(x, y, randomColours[s.GetPixelsSegmentIndex(x, y)]);
                    }
                    else
                    {
                        RGB pixelColour = ColorSpaceHelper.LabtoRGB(s.GetPixelsSegmentColour(x, y));
                        outImage.SetPixel(x, y, Color.FromArgb(pixelColour.Red, pixelColour.Green, pixelColour.Blue));
                    }
                }
            }

            viewer2.Image = outImage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dlgFolder.ShowDialog();
            if (dlgFolder.SelectedPath == "") return;

            if (viewer.Image != null && viewer2.Image != null)
            {
                viewer.Image.Save(dlgFolder.SelectedPath + "\\orig.png", ImageFormat.Png);
                if (((string)cmboDisplayType.SelectedItem).Equals("Random"))
                    viewer2.Image.Save(dlgFolder.SelectedPath + "\\segRand.png", ImageFormat.Png);
                else
                    viewer2.Image.Save(dlgFolder.SelectedPath + "\\segAve.png", ImageFormat.Png);
            }
        }
    }
}
