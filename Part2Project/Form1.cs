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

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            dlgFolder1.ShowDialog();

            if (dlgFolder1.SelectedPath == "") return;

            DateTime before = DateTime.Now;

            ImageDirectoryFeatures featureManager = new ImageDirectoryFeatures(dlgFolder1.SelectedPath);
            List<ImageFeatureList> features = featureManager.GetDirectoryFeatures();

            DateTime after = DateTime.Now;

            TimeSpan diff = after - before;

            txt.Text = "";
            string nl = Environment.NewLine;

            txt.Text += "Time taken to get features for " + features.Count + " image(s) was " + diff.TotalSeconds +
                        " seconds." + nl + nl;

            for (int i = 0; i < features.Count; i++)
            {
                txt.Text += "Image " + (i + 1) + " - \"" + features[i].ImageFilename + "\"" + nl;
                txt.Text += "  Brightness: " + features[i].Brightness + nl;
                txt.Text += "  IntensityContrast: " + features[i].IntensityContrast + nl;
                txt.Text += "  Saturation: " + features[i].Saturation + nl;
                txt.Text += "  RuleOfThirds: " + features[i].RuleOfThirds + nl;
                txt.Text += "  Simplicity: " + features[i].Simplicity + nl;
                txt.Text += nl;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dlgFolder1.ShowDialog();

            DateTime before = DateTime.Now;

            List<ImageFeatureList> features = new List<ImageFeatureList>();
            
            List<string> imageFilenames = new List<string>();
            int numImages = 0;
            string[] filenames = Directory.GetFiles(dlgFolder1.SelectedPath);
            foreach (string filename in filenames)
            {
                string ext = filename.Split('.').Last();
                if (ext.Equals("jpg") || ext.Equals("jpeg") || ext.Equals("png"))
                {
                    // We'll accept these file extensions as images
                    imageFilenames.Add(filename);
                    numImages++;
                }
            }
            
            for (int i = 0; i < numImages; i++)
            {
                ImageFeatures imFeat = new ImageFeatures(imageFilenames.ElementAt(i));
                imFeat.ThreadPoolCallback();
                features.Add(imFeat.Features);
            }

            DateTime after = DateTime.Now;

            TimeSpan diff = after - before;

            txt.Text = "";
            string nl = Environment.NewLine;

            txt.Text += "Time taken to get features for " + features.Count + " image(s) was " + diff.TotalSeconds +
                        " seconds." + nl + nl;

            for (int i = 0; i < features.Count; i++)
            {
                txt.Text += "Image " + (i + 1) + " - \"" + features[i].ImageFilename + "\"" + nl;
                txt.Text += "  Brightness: " + features[i].Brightness + nl;
                txt.Text += "  IntensityContrast: " + features[i].IntensityContrast + nl;
                txt.Text += "  Saturation: " + features[i].Saturation + nl;
                txt.Text += "  RuleOfThirds: " + features[i].RuleOfThirds + nl;
                txt.Text += "  Simplicity: " + features[i].Simplicity + nl;
                txt.Text += nl;
            }
        }

        private void AppendToTextBox(string toAppend)
        {
            txt.Text += toAppend;
        }

        private void btnTestBoth_Click(object sender, EventArgs e)
        {
            dlgFolder1.ShowDialog();

            if (dlgFolder1.SelectedPath == "") return;

            string nl = Environment.NewLine;
            txt.Text = "Multi-Threaded:" + nl;
            double totalSeconds = 0.0;

            for (int i = 0; i < 10; i++)
            {
                DateTime before = DateTime.Now;

                ImageDirectoryFeatures featureManager = new ImageDirectoryFeatures(dlgFolder1.SelectedPath);
                List<ImageFeatureList> features = featureManager.GetDirectoryFeatures();

                DateTime after = DateTime.Now;

                double seconds = (after - before).TotalSeconds;
                totalSeconds += seconds;

                string toAppend = (i + 1) + " - time taken to get features for " + features.Count + " image(s) was " + seconds + " seconds." + nl;
                AppendToTextBox(toAppend);
            }

            string ToAppend = "Average time taken was " + totalSeconds/10 + " seconds." + nl;
            AppendToTextBox(ToAppend);

            txt.Text += nl + "Single-Threaded:" + nl;
            totalSeconds = 0.0;
            for (int i = 0; i < 10; i++)
            {
                DateTime before = DateTime.Now;

                List<ImageFeatureList> features = new List<ImageFeatureList>();

                List<string> imageFilenames = new List<string>();
                int numImages = 0;
                string[] filenames = Directory.GetFiles(dlgFolder1.SelectedPath);
                foreach (string filename in filenames)
                {
                    string ext = filename.Split('.').Last();
                    if (ext.Equals("jpg") || ext.Equals("jpeg") || ext.Equals("png"))
                    {
                        // We'll accept these file extensions as images
                        imageFilenames.Add(filename);
                        numImages++;
                    }
                }

                for (int j = 0; j < numImages; j++)
                {
                    ImageFeatures imFeat = new ImageFeatures(imageFilenames.ElementAt(j));
                    imFeat.ThreadPoolCallback();
                    features.Add(imFeat.Features);
                }

                DateTime after = DateTime.Now;

                TimeSpan diff = after - before;

                string toAppend = (i + 1) + " - time taken to get features for " + features.Count + " image(s) was " + diff.TotalSeconds +
                            " seconds." + nl;
                AppendToTextBox(toAppend);
            }

            ToAppend = "Average time taken was " + totalSeconds / 10 + " seconds." + nl;
            AppendToTextBox(ToAppend);
        }
    }
}
