using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Part2Project.Features;
using Part2Project.Infrastructure;

namespace Part2Project
{
//        ** Single threaded feature computer **
//        dlgFolder.ShowDialog();
//
//        List<ImageFeatureList> features = new List<ImageFeatureList>();
//
//        List<string> imageFilenames = new List<string>();
//        int numImages = 0;
//        string[] filenames = Directory.GetFiles(dlgFolder.SelectedPath);
//        foreach (string filename in filenames)
//        {
//            string ext = filename.Split('.').Last();
//            if (ext.Equals("jpg") || ext.Equals("jpeg") || ext.Equals("png"))
//            {
//                // We'll accept these file extensions as images
//                imageFilenames.Add(filename);
//                numImages++;
//            }
//        }
//
//        for (int i = 0; i < numImages; i++)
//        {
//            ImageFeatures imFeat = new ImageFeatures(imageFilenames.ElementAt(i));
//            imFeat.ThreadPoolCallback();
//            features.Add(imFeat.Features);
//        }
//
//        Console.Write(features[0].Brightness);

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            using (Image selected = Image.FromFile(openFileDialog1.FileName))
            {
//                DirectBitmap image = new DirectBitmap((int) ((double) selected.Width / (double) selected.Height * 512.0), 512);
                DirectBitmap image = new DirectBitmap(512, 512);
                using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                {
                    int originalWidth = (int) ((double) selected.Width / (double) selected.Height * 512.0);
//                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 512.0), 512);
                    gfx.DrawImage(selected, 256 -originalWidth / 2, 0, originalWidth, 512);
                }

                label1.Text = FeatureBlurriness.ComputeFeature(image).ToString();
                pictureBox1.Image = image.Bitmap;
                pictureBox2.Image = FeatureBlurriness.Get2DFFT(image).Bitmap;
            }
        }
    }
}
