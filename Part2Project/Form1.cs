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
using Part2Project.Features;
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

        private void GenerateImageForParameterValue(DirectBitmap original, string destFilename, int k, double sigma)
        {
            Segmentation s = GraphBasedImageSegmentation.Segment(original, k, sigma);

            // Create output segmentation image
            using (DirectBitmap outImage = new DirectBitmap(original.Width, original.Height))
            {
                for (int x = 0; x < original.Width; x++)
                {
                    for (int y = 0; y < original.Height; y++)
                    {
                        RGB pixelColour = ColorSpaceHelper.LabtoRGB(s.GetPixelsSegmentColour(x, y));
                        outImage.SetPixel(x, y, Color.FromArgb(pixelColour.Red, pixelColour.Green, pixelColour.Blue));
                    }
                }

                outImage.Bitmap.Save(destFilename);
            }
        }

        private void GenerateFolderForParameterValue(string dName, string[] filenames, DirectBitmap[] originals, int k, double sigma)
        {
            Task[] tasks = new Task[originals.Length];

            string dExt = "\\k" + k + "sigma" + sigma;
            Directory.CreateDirectory(dName + dExt);

            for (int i = 0; i < originals.Length; i++)
            {
                var i1 = i;
                tasks[i] =
                    Task.Run(
                        () =>
                            GenerateImageForParameterValue(originals[i1], dName + dExt + "\\" + filenames[i1].Split('\\').Last(), k, sigma));
            }
            Task.WaitAll(tasks);
            
            foreach (Task task in tasks)
            {
                task.Dispose();
            }
        }

        private void DoSweep()
        {
            DateTime totalBefore = DateTime.Now;
            DateTime before, after;
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\Segmentation k and sigma";
            int[] kValues = { 25, 50, 75, 100, 125, 150, 175, 200, 250, 300, 400 };
            //int[] kValues = { 150, 175, 200, 250, 300, 400 };
            //            int[] kValues = {25};
            double[] sigmaValues = { 0.0, 0.4, 0.6, 0.8, 1.0, 1.4, 1.8, 2.5, 3.0, 4.0, 5.0 };

            box.Text = "";
            string nl = Environment.NewLine;

            // Get the small images in memory
            string[] fileNames = Directory.GetFiles(dName + "\\originals");
            DirectBitmap[] originals = new DirectBitmap[fileNames.Length];

            for (int i = 0; i < originals.Length; i++)
            {
                // Load image from file, and shrink it down
                using (Bitmap selected = new Bitmap(fileNames[i]))
                {
                    originals[i] = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    using (Graphics gfx = Graphics.FromImage(originals[i].Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0),
                            240);
                    }
                }
            }

            int count = 0, k;
            double sigma;
            foreach (int kValue in kValues)
            {
                foreach (double sigmaValue in sigmaValues)
                {
                    k = kValue;
                    sigma = sigmaValue;

                    before = DateTime.Now;

                    GenerateFolderForParameterValue(dName, fileNames, originals, k, sigma);

                    after = DateTime.Now;
                    box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                    Application.DoEvents();

                    count++;
                }
            }

            for (int i = 0; i < originals.Length; i++)
            {
                originals[i].Dispose();
            }

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

        private void RoTRenameFolder(string dName, string[] filenames, DirectBitmap[] originals, int k, double sigma)
        {
            Task<double>[] tasks = new Task<double>[originals.Length];

            string dExt = "\\k" + k + "sigma" + sigma;
            Directory.CreateDirectory(dName + dExt);

            for (int i = 0; i < originals.Length; i++)
            {
                var i1 = i;
                tasks[i1] = Task<double>.Factory.StartNew(() =>
                {
                    Segmentation s = GraphBasedImageSegmentation.Segment(originals[i1], k, sigma);
                    double result = FeatureRuleOfThirds.ComputeFeature(originals[i1], s);
                    return result;
                });
            }
            Task<double[]> resultsTask = Task.WhenAll(tasks);
            resultsTask.Wait();

            // Sort and save all of images in the right folder, named by the RoT value
            Dictionary<double, string> newNames = new Dictionary<double, string>();
            for (int i = 0; i < filenames.Length; i++)
            {
                newNames.Add(resultsTask.Result[i], filenames[i]);
            }
            List<double> keyList = new List<double>();
            foreach (double key in newNames.Keys)
            {
                keyList.Add(key);
            }
            keyList.Sort();
            keyList.Reverse();
            int current = 0;
            foreach (double key in keyList)
            {
                originals[Array.IndexOf(filenames, newNames[key])].Bitmap.Save(dName + dExt + "\\" + current + "--" +
                                                                               key + "." +
                                                                               newNames[key].Split('.').Last());
                current++;
            }

            foreach (Task<double> task in tasks)
            {
                task.Dispose();
            }
            resultsTask.Dispose();
        }

        private void RoTRenameSegmentationSweep()
        {
            DateTime totalBefore = DateTime.Now;
            DateTime before, after;
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\Segmentation k and sigma - RoT";
//            int[] kValues = { 25, 50, 75, 100, 125, 150, 175, 200, 250, 300, 400 };
            int[] kValues = { 125, 150, 175, 200, 250, 300, 400 };
            double[] sigmaValues = { 0.0, 0.4, 0.6, 0.8, 1.0, 1.4, 1.8, 2.5, 3.0, 4.0, 5.0 };

            box.Text = "";
            string nl = Environment.NewLine;

            // Get the small images in memory
            string[] fileNames = Directory.GetFiles(dName + "\\originals");
            DirectBitmap[] originals = new DirectBitmap[fileNames.Length];

            box.Text += "Segmentation k and sigma parameter sweep using RoT calculation" + nl + nl +
                        "Loading original images" + nl;
            for (int i = 0; i < originals.Length; i++)
            {
                // Load image from file, and shrink it down
                using (Bitmap selected = new Bitmap(fileNames[i]))
                {
                    originals[i] = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    using (Graphics gfx = Graphics.FromImage(originals[i].Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0),
                            240);
                    }
                }
            }

            box.Text += "Images loaded" + nl;

            int count = 0, k;
            double sigma;
            foreach (int kValue in kValues)
            {
                foreach (double sigmaValue in sigmaValues)
                {
                    k = kValue;
                    sigma = sigmaValue;

                    before = DateTime.Now;

                    RoTRenameFolder(dName, fileNames, originals, k, sigma);

                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    Thread.Sleep(2000);

                    after = DateTime.Now;
                    box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                    Application.DoEvents();

                    count++;
                }
            }

            for (int i = 0; i < originals.Length; i++)
            {
                originals[i].Dispose();
            }

            DateTime totalAfter = DateTime.Now;

            box.Text += nl + "In total took " + (totalAfter - totalBefore).TotalSeconds + " seconds to complete";
        }

        private void btnRoTRename_Click(object sender, EventArgs e)
        {
            RoTRenameSegmentationSweep();
        }
    }
}
