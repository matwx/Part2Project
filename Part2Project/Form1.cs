using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Part2Project.ImageSegmentation;

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

            GraphBasedDataStructures.GraphBasedDisjointSet S = GraphBasedImageSegmentation.Segment(bmp, int.Parse(txtK.Text), double.Parse(txtSigma.Text), (string) cmboEdgeWeights.SelectedItem);

            viewer2.Image = GraphBasedImageSegmentation.VisualiseSegmentation(S, (string) cmboDisplayType.SelectedItem);
        }
    }
}
