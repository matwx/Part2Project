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
using Part2Project.Saliency;

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

            if (!btnGBIS.Visible)
            {
                btnGBIS.Visible = true;
                btnSegSaliency.Visible = true;
                btnHighlightSmall.Visible = true;
                btnRenormalise.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                txtK.Visible = true;
                txtSigma.Visible = true;
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

            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), 0.0);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, double.Parse(txtSigma.Text));

            viewer2.Image = ss.GetSaliencyMap();
        }

        private void btnSegSaliency_Click(object sender, EventArgs e)
        {
            // Do GBIS on our (resized) input image

            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), 0.0);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, double.Parse(txtSigma.Text));

            viewer2.Image = ss.GetSegmentSaliencyMap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dlgFolder.ShowDialog();
            if (dlgFolder.SelectedPath == "" || viewer.Image == null || viewer2.Image == null) return;

            viewer.Image.Save(dlgFolder.SelectedPath + "\\orig.png", ImageFormat.Png);
            viewer2.Image.Save(dlgFolder.SelectedPath + "\\salSeg.png", ImageFormat.Png);
        }

        private void btnRenormalise_Click(object sender, EventArgs e)
        {
            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), 0.0);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, double.Parse(txtSigma.Text));
            ss.RenormaliseToIgnore();

            viewer2.Image = ss.GetSegmentSaliencyMap();
        }

        private void btnHighlightSmall_Click(object sender, EventArgs e)
        {
            Segmentation s = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), 0.0);

            SaliencySegmentation ss = new SaliencySegmentation(s, bmp, double.Parse(txtSigma.Text));
            ss.RenormaliseToIgnore();

            viewer2.Image = ss.GetHighlightedSegmentSaliencyMap();
        }
    }
}
