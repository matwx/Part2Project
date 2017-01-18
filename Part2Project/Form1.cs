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
    public partial class Form1 : Form
    {
        public const int segK = 125;
        public const double segSigma = 0.6;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region RoT Sweep

        private void RoTRenameFolder(string dName, string[] filenames, DirectBitmap[] originals, double sigma)
        {
            Task<double>[] tasks = new Task<double>[originals.Length];

            string dExt = "\\sigma" + sigma;
            Directory.CreateDirectory(dName + dExt);

            for (int i = 0; i < originals.Length; i++)
            {
                var i1 = i;
                tasks[i1] = Task<double>.Factory.StartNew(() =>
                {
                    Segmentation s = GraphBasedImageSegmentation.Segment(originals[i1], segK, segSigma);
                    double result = FeatureRuleOfThirds.ComputeFeature(originals[i1], s, sigma);
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

        private void RoTRenameSigmaSweepSelective()
        {
            DateTime totalBefore = DateTime.Now;
            DateTime before, after;
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\Segmentation k and sigma - RoT 2";
            Tuple<int, double>[] pairs =
            {
                Tuple.Create(50, 1.4),
                Tuple.Create(50, 1.8),
                Tuple.Create(75, 0.8),
                Tuple.Create(75, 1.0),
                Tuple.Create(100, 0.0),
                Tuple.Create(100, 0.6),
                Tuple.Create(125, 0.0),
                Tuple.Create(125, 0.6),
                Tuple.Create(150, 0.8)
            };

            box.Text = "";
            string nl = Environment.NewLine;

            // Get the small images in memory
            string[] fileNames = Directory.GetFiles(dName + "\\originals");
            DirectBitmap[] originals = new DirectBitmap[fileNames.Length];

            box.Text += "Segmentation k and sigma parameter sweep using RoT calculation" + nl + nl +
                        "Loading original images" + nl;
            Application.DoEvents();
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
            Application.DoEvents();

            int k;
            double sigma;
            foreach (var pair in pairs)
            {
                k = pair.Item1;
                sigma = pair.Item2;

                before = DateTime.Now;

                RoTRenameFolder(dName, fileNames, originals, sigma);

                GC.WaitForPendingFinalizers();
                GC.Collect();

                Thread.Sleep(1000);

                after = DateTime.Now;
                box.Text += "k=" + k + ", sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                Application.DoEvents();
            }

            for (int i = 0; i < originals.Length; i++)
            {
                originals[i].Dispose();
            }

            DateTime totalAfter = DateTime.Now;

            box.Text += nl + "In total took " + (totalAfter - totalBefore).TotalSeconds + " seconds to complete";
        }

        private void RoTRenameSigmaSweep()
        {
            DateTime totalBefore = DateTime.Now;
            DateTime before, after;
            string dName =
                "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Parameter Sweeps\\RoT sigma";
            double[] sigmaValues = { 0.06, 0.07, 0.08, 0.09, 0.11, 0.12, 0.13, 0.14, 0.16, 0.17, 0.18, 0.19, 0.21, 0.22, 0.23, 0.24 };

            box.Text = "";
            string nl = Environment.NewLine;

            // Get the small images in memory
            string[] fileNames = Directory.GetFiles(dName + "\\originals");
            DirectBitmap[] originals = new DirectBitmap[fileNames.Length];

            box.Text += "RoT sigma parameter sweep" + nl + nl +
                        "Loading original images" + nl;
            Application.DoEvents();

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
            Application.DoEvents();

            int count = 0;
            double sigma;
            foreach (double sigmaValue in sigmaValues)
            {
                sigma = sigmaValue;

                before = DateTime.Now;

                RoTRenameFolder(dName, fileNames, originals, sigma);

                GC.WaitForPendingFinalizers();
                GC.Collect();

                Thread.Sleep(2000);

                after = DateTime.Now;
                box.Text += "sigma=" + sigma + ": " + (after - before).TotalSeconds + " seconds to complete" + nl;
                Application.DoEvents();

                count++;
            }

            for (int i = 0; i < originals.Length; i++)
            {
                originals[i].Dispose();
            }

            DateTime totalAfter = DateTime.Now;

            box.Text += nl + "In total took " + (totalAfter - totalBefore).TotalSeconds + " seconds to complete";
        }

        #endregion

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

        private void button1_Click(object sender, EventArgs e)
        {
            RoTRenameSigmaSweep();
        }
    }
}
