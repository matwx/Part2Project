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

        private void ConvexityRenameFolder(string dName)
        {
            // Get the small images in memory
            string[] filenames = Directory.GetFiles(dName);
            DirectBitmap[] originals = new DirectBitmap[filenames.Length];

            for (int i = 0; i < originals.Length; i++)
            {
                // Load image from file, and shrink it down
                using (Bitmap selected = new Bitmap(filenames[i]))
                {
                    originals[i] = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    using (Graphics gfx = Graphics.FromImage(originals[i].Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    }
                }
            }

            Task<double>[] tasks = new Task<double>[originals.Length];

            for (int i = 0; i < originals.Length; i++)
            {
                var i1 = i;
                tasks[i1] = Task<double>.Factory.StartNew(() =>
                {
                    const int k = 125;
                    const double sigma = 0.6;
                    Segmentation s = GraphBasedImageSegmentation.Segment(originals[i1], k, sigma);
                    SaliencySegmentation ss = new SaliencySegmentation(s, originals[i1], sigma);

                    double result = FeatureShapeConvexity.ComputeFeature(ss, originals[i1]);
                    return result;
                });
            }
            Task<double[]> resultsTask = Task.WhenAll(tasks);
            resultsTask.Wait();

            // Sort and save all of images in the right folder, named by the RoT value
            Dictionary<double, string> newNames = new Dictionary<double, string>();
            Random rand = new Random();
            for (int i = 0; i < filenames.Length; i++)
            {
                if (newNames.ContainsKey(resultsTask.Result[i])) newNames.Add(resultsTask.Result[i] + rand.NextDouble() / 100000, filenames[i]);
                else newNames.Add(resultsTask.Result[i], filenames[i]);
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
                File.Delete(newNames[key]);
                originals[Array.IndexOf(filenames, newNames[key])].Bitmap.Save(dName + "\\" + current + "--" +
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

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {
            using (Image selected = Image.FromFile(openFileDialog1.FileName))
            {
                DirectBitmap image = new DirectBitmap(
                    (int) ((double) selected.Width / (double) selected.Height * 240.0), 240);
                // Create the required resized images
                using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                {
                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                }

                const int k = 125;
                const double sigma = 0.6;
                Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
                SaliencySegmentation ss = new SaliencySegmentation(s, image, sigma);
                button1.Text = FeatureShapeConvexity.ComputeFeature(ss, image).ToString();

                pictureBox1.Image = image.Bitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            ConvexityRenameFolder(folderBrowserDialog1.SelectedPath);
        }
    }
}
