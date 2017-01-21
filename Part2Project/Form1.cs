﻿using System;
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
                using (DirectBitmap image = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240))
                {
                    // Create the required resized images
                    using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                    {
                        gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                    }

                    Segmentation s = GraphBasedImageSegmentation.Segment(image, 125, 0.6);
                    button1.Text = FeatureColourContrast.ComputeFeature(image, s).ToString();
                }
            }
        }

        private void ColourContrastRenameFolder(string dName)
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
                    Segmentation s = GraphBasedImageSegmentation.Segment(originals[i1], 125, 0.6);
                    double result = FeatureColourContrast.ComputeFeature(originals[i1], s);
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

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            ColourContrastRenameFolder(folderBrowserDialog1.SelectedPath);
        }
    }
}
