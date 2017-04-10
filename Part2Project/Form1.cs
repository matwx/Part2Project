﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kaliko.ImageLibrary.ColorSpace;
using Part2Project.Features;
using Part2Project.ImageSegmentation;

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
            btnROT.Text = @"Rule Of Thirds";
            btnIC.Text = @"Intensity Contrast";

            if (!btnROT.Visible)
            {
                btnROT.Visible = true;
                btnIC.Visible = true;
                btnICDiffMap.Visible = true;
            }
        }

        private void btnChooseImage_Click_1(object sender, EventArgs e)
        {
            // Choose an image
            dlgChooseImage.ShowDialog();
        }

        private void btnROT_Click(object sender, EventArgs e)
        {
            double value = new FeatureRuleOfThirds().ComputeFeature(bmp);
            btnROT.Text = value.ToString(CultureInfo.InvariantCulture);

            viewer2.Image = new Bitmap(bmp);
            DrawThirdLines();
        }

        private void DrawThirdLines()
        {
            Graphics gfx = Graphics.FromImage(viewer2.Image);

            gfx.DrawLine(Pens.Red, bmp.Width / 3, 0, bmp.Width / 3, bmp.Height);
            gfx.DrawLine(Pens.Red, bmp.Width * 2 / 3, 0, bmp.Width * 2 / 3, bmp.Height);
            gfx.DrawLine(Pens.Red, 0, bmp.Height / 3, bmp.Width, bmp.Height / 3);
            gfx.DrawLine(Pens.Red, 0, bmp.Height * 2 / 3, bmp.Width, bmp.Height * 2 / 3);
        }

        private void btnBatchROT_Click(object sender, EventArgs e)
        {
            dlgChooseFolder.ShowDialog();
            Dictionary<double, string> newNames = new Dictionary<double, string>();

            var files = Directory.GetFiles(dlgChooseFolder.SelectedPath);
            foreach (string filename in files)
            {
                using (Image selected = Image.FromFile(filename))
                {
                    bmp = new Bitmap((int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
                    Graphics gfx = Graphics.FromImage(bmp);

                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);

                    double value = new FeatureRuleOfThirds().ComputeFeature(bmp);

                    //newNames.Add(filename, dlgChooseFolder.SelectedPath + "\\" + value.ToString() + ".jpg");
                    newNames.Add(value, filename);
                }
                
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
                File.Move(newNames[key], dlgChooseFolder.SelectedPath + "\\" + current.ToString() + "--" + key.ToString() + "--RoT.jpg");
                current++;
            }
        }

        private void btnIC_Click(object sender, EventArgs e)
        {
            FeatureIntensityContrast f = new FeatureIntensityContrast();
            double value = f.ComputeFeature(bmp);
            btnIC.Text = value.ToString(CultureInfo.InvariantCulture);

            viewer2.Image = f.GetDifferenceMap(bmp);
        }

        private void btnICFolder_Click(object sender, EventArgs e)
        {
            dlgChooseFolder.ShowDialog();
            Dictionary<double, string> newNames = new Dictionary<double, string>();

            var files = Directory.GetFiles(dlgChooseFolder.SelectedPath);
            foreach (string filename in files)
            {
                using (Image selected = Image.FromFile(filename))
                {
                    bmp = new Bitmap((int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
                    Graphics gfx = Graphics.FromImage(bmp);

                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);

                    double value = new FeatureIntensityContrast().ComputeFeature(bmp);

                    //newNames.Add(filename, dlgChooseFolder.SelectedPath + "\\" + value.ToString() + ".jpg");
                    newNames.Add(value, filename);
                }

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
                File.Move(newNames[key], dlgChooseFolder.SelectedPath + "\\" + current.ToString() + "--" + key.ToString() + "--IC.jpg");
                current++;
            }
        }

        private void btnICDiffMap_Click(object sender, EventArgs e)
        {
            viewer2.Image = new FeatureIntensityContrast().GetWeberContrastMap(bmp);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dlgChooseFolder.ShowDialog();
            if (dlgChooseFolder.SelectedPath == "" || viewer.Image == null || viewer2.Image == null) return;

            viewer.Image.Save(dlgChooseFolder.SelectedPath + "\\orig.png", ImageFormat.Png);
            viewer2.Image.Save(dlgChooseFolder.SelectedPath + "\\intCont.png", ImageFormat.Png);
        }
    }
}
