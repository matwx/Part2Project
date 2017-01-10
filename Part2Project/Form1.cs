using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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

        private void GenerateImageForParameterValue(string filename, string destFilename, int k, double sigma)
        {
            // Need to make sure that the files are already in dimesions 320x240
            DirectBitmap image = null;

            try
            {
                using (Bitmap selected = new Bitmap(filename))
                {
                    image = new DirectBitmap((int) ((double) selected.Width / (double) selected.Height * 240.0), 240);
                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int) ((double) selected.Width / (double) selected.Height * 240.0),
                            240);
                    }
                }

                Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
                
                // Create output segmentation image
                using (DirectBitmap outImage = new DirectBitmap(image.Width, image.Height))
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        for (int y = 0; y < image.Height; y++)
                        {
                            RGB pixelColour = ColorSpaceHelper.LabtoRGB(s.GetPixelsSegmentColour(x, y));
                            outImage.SetPixel(x, y, Color.FromArgb(pixelColour.Red, pixelColour.Green, pixelColour.Blue));
                        }
                    }
                
                    outImage.Bitmap.Save(destFilename);
                }
            }
            finally
            {
                if (image != null) image.Dispose();
            }
            


//            using (Bitmap selected = new Bitmap(filename))
//            {
//                using (DirectBitmap image = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240))
//                {
//                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
//                    {
//                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
//                    }
//
//                    Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
//
//                    // Create output segmentation image
//                    using (DirectBitmap outImage = new DirectBitmap(image.Width, image.Height))
//                    {
//                        for (int x = 0; x < image.Width; x++)
//                        {
//                            for (int y = 0; y < image.Height; y++)
//                            {
//                                RGB pixelColour = ColorSpaceHelper.LabtoRGB(s.GetPixelsSegmentColour(x, y));
//                                outImage.SetPixel(x, y, Color.FromArgb(pixelColour.Red, pixelColour.Green, pixelColour.Blue));
//                            }
//                        }
//
//                        outImage.Bitmap.Save(destFilename);
//                    }
//                }
//            }
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
                tasks[i] =
                    Task.Run(
                        () =>
                            GenerateImageForParameterValue(filename,
                                dName + dExt + "\\" + filename.Split('\\').Last(), k, sigma));
            }
            Task.WaitAll(tasks);
            
            foreach (Task task in tasks)
            {
                task.Dispose();
            }
        }

        private void DoSweep()
        {
            //            dlgFolder.ShowDialog();

            DateTime totalBefore = DateTime.Now;
            DateTime before, after;
            //            string dName = dlgFolder.SelectedPath;
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\Segmentation k and sigma";
            int[] kValues = { 25, 50, 75, 100, 125, 150, 175, 200, 250, 300, 400 };
            //int[] kValues = { 150, 175, 200, 250, 300, 400 };
            //            int[] kValues = {25};
            double[] sigmaValues = { 0.0, 0.4, 0.6, 0.8, 1.0, 1.4, 1.8, 2.5, 3.0, 4.0, 5.0 };

            box.Text = "";
            string nl = Environment.NewLine;

            int count = 0, k;
            double sigma;
            foreach (int kValue in kValues)
            {
                foreach (double sigmaValue in sigmaValues)
                {
                    k = kValue;
                    sigma = sigmaValue;

                    before = DateTime.Now;

                    GenerateFolderForParameterValue(dName, k, sigma);

                    after = DateTime.Now;
                    box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                    Application.DoEvents();

//                    GC.WaitForPendingFinalizers();
//                    GC.Collect();
//
//                    System.Threading.Thread.Sleep(2000);

                    count++;
                }
            }

            //            before = DateTime.Now;
            //            int k = int.Parse(txtK.Text);
            //            double sigma = double.Parse(txtSigma.Text);
            //            
            //            GenerateFolderForParameterValue(dName, k, sigma);
            //            
            //            after = DateTime.Now;
            //            box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;

            DateTime totalAfter = DateTime.Now;

            box.Text += nl + "In total took " + (totalAfter - totalBefore).TotalSeconds + " seconds to complete";
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            DoSweep();
        }

        private void btnResizeOriginals_Click(object sender, EventArgs e)
        {
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\Segmentation k and sigma";

            string[] filenames = Directory.GetFiles(dName + "\\originals");
            Directory.CreateDirectory(dName + "\\new");

            foreach (string filename in filenames)
            {
                using (Image selected = Image.FromFile(filename))
                {
                    using (DirectBitmap image = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240))
                    {
                        using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                        {
                            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                        }

                        image.Bitmap.Save(dName + "\\new\\" + filename.Split('\\').Last());
                    }
                }
            }
        }
    }
}
