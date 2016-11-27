using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.Features;
using Part2Project.ImageSegmentation;

namespace Part2Project
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void dlgChooseImage_FileOk_1(object sender, CancelEventArgs e)
        {
            // Resize the image so that it's height fits in the viewer
            Image selected = Image.FromFile(dlgChooseImage.FileName);
            bmp = new Bitmap((int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
            viewer.Image = bmp;

            // Image loaded
            viewer2.Image = null;
            btnROT.Text = @"Rule Of Thirds";

            if (!btnGBIS.Visible)
            {
                btnGBIS.Visible = true;
                btnSegSaliency.Visible = true;
                btnROT.Visible = true;
            }
        }

        private void btnChooseImage_Click_1(object sender, EventArgs e)
        {
            // Choose an image
            dlgChooseImage.ShowDialog();
        }

        private void btnGBIS_Click_1(object sender, EventArgs e)
        {
            // Do GBIS on our (resized) input image

            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, 150, 0.8);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, 0.8);

            viewer2.Image = ss.GetSaliencyMap();
            DrawThirdLines();
        }

        private void btnSegSaliency_Click(object sender, EventArgs e)
        {
            // Do GBIS on our (resized) input image

            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, 150, 0.8);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, 0.8);

            viewer2.Image = ss.GetSegmentSaliencyMap();
            DrawThirdLines();
        }

        private void btnROT_Click(object sender, EventArgs e)
        {
            double value = new FeatureRuleOfThirds().ComputeFeature(bmp);
            btnROT.Text = value.ToString(CultureInfo.InvariantCulture);

            viewer2.Image = new Bitmap(bmp);
            DrawThirdLines();
        }

        private void DrawThirdLines()
        {
            Graphics gfx = Graphics.FromImage(viewer2.Image);

            gfx.DrawLine(Pens.Red, bmp.Width / 3, 0, bmp.Width / 3, bmp.Height);
            gfx.DrawLine(Pens.Red, bmp.Width * 2 / 3, 0, bmp.Width * 2 / 3, bmp.Height);
            gfx.DrawLine(Pens.Red, 0, bmp.Height / 3, bmp.Width, bmp.Height / 3);
            gfx.DrawLine(Pens.Red, 0, bmp.Height * 2 / 3, bmp.Width, bmp.Height * 2 / 3);
        }
    }
}
