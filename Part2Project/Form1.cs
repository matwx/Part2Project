using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.ImageSegmentation;
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

        private void GenerateFolderForParameterValue(string dName, int k, double sigma)
        {
            string[] fileNames = Directory.GetFiles(dName + "\\originals");
            Task[] tasks = new Task[fileNames.Length];

            string dExt = "\\k" + k + "sigma" + sigma;
            Directory.CreateDirectory(dName + dExt);

            for (int i = 0; i < fileNames.Length; i++)
            {
                string filename = fileNames[i];
                tasks[i] = Task.Run(() =>
                {
                    using (Image selected = Image.FromFile(filename))
                    {
                        using (DirectBitmap image = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240))
                        {
                            using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                            {
                                gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                            }

                            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);

                            // Create output segmentation image
                            DirectBitmap outImage = new DirectBitmap(image.Width, image.Height);

                            for (int x = 0; x < image.Width; x++)
                            {
                                for (int y = 0; y < image.Height; y++)
                                {
                                    RGB pixelColour = ColorSpaceHelper.LabtoRGB(s.GetPixelsSegmentColour(x, y));
                                    outImage.SetPixel(x, y, Color.FromArgb(pixelColour.Red, pixelColour.Green, pixelColour.Blue));

                                }
                            }

                            outImage.Bitmap.Save(dName + dExt + "\\" + filename.Split('\\').Last());
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            dlgFolder.ShowDialog();

            DateTime totalBefore = DateTime.Now;

            string dName = dlgFolder.SelectedPath;
            int[] kValues = {150};
            double[] sigmaValues = {0.8, 1.2};

            box.Text = "";
            string nl = Environment.NewLine;

            int count = 0;
            foreach (int kValue in kValues)
            {
                foreach (double sigmaValue in sigmaValues)
                {
                    int k = kValue;
                    double sigma = sigmaValue;

                    DateTime before = DateTime.Now;

                    GenerateFolderForParameterValue(dName, k, sigma);

                    DateTime after = DateTime.Now;
                    box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                    Application.DoEvents();

                    count++;
                }
            }

            DateTime totalAfter = DateTime.Now;

            box.Text += nl + "In total took " + (totalAfter - totalBefore).TotalSeconds + " seconds to complete";
        }
    }
}
