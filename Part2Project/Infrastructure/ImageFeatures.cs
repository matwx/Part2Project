﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Part2Project.Features;

namespace Part2Project.Infrastructure
{
    class ImageFeatures
    {
        // Retrieves/computes a list of all features for a particular image
        private string _filename;
        private Bitmap _image;
        public ImageFeatureList Features { get; private set; }

        public ImageFeatures(string filename)
        {
            _filename = filename;
        }

        public void ThreadPoolCallback()
        {
            Features = GetFeatures();
        }

        public ImageFeatureList GetFeatures()
        {
            ImageFeatureList result = new ImageFeatureList();

            string ext = _filename.Split('.').Last();
            byte[] makerNote = GetExifMakerNote(_filename);
            if (!(ext.Equals("jpg") || ext.Equals("jpeg")) || makerNote == null || !result.LoadFromByteArray(makerNote))
            {
                // If they can't be retrieved from Exif, we need to compute them
                // First read the image from file as a bitmap
                using (Image selected = Image.FromFile(_filename))
                {
                    using (_image = new Bitmap((int) ((double) selected.Width/(double) selected.Height*240.0), 240))
                    {
                        using (Graphics gfx = Graphics.FromImage(_image))
                        {
                            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                        } 
                        
                        // Then compute the features and store the results
                        result.Brightness = new FeatureBrightness().ComputeFeature(_image);
                        result.IntensityContrast = new FeatureIntensityContrast().ComputeFeature(_image);
                        result.Saturation = new FeatureSaturation().ComputeFeature(_image);
                        result.RuleOfThirds = new FeatureRuleOfThirds().ComputeFeature(_image);
                        result.Simplicity = new FeatureSimplicity().ComputeFeature(_image);
                    }
                }

                // Save the new computed features in the MakerNote
                SaveExifMakerNote(_filename, result.ToByteArray());
            }

            return result;
        }

        private void SaveExifMakerNote(string filename, byte[] data)
        {
            // Make a copy of the file 
            Random rand = new Random();
            string tmpFileName = Path.GetTempPath() + "\\" + rand.Next(10000, 99999) + ".jpg";
            byte[] imageBytes = File.ReadAllBytes(filename);
            File.WriteAllBytes(tmpFileName, imageBytes);

            using (Bitmap image = new Bitmap(tmpFileName))
            {
                PropertyItem pi = image.PropertyItems[0];
                pi.Type = 7; // Undefined
                pi.Len = data.Length;
                pi.Value = (byte[])data.Clone();
                pi.Id = 0x927C; // MakerNote field Id

                image.SetPropertyItem(pi);

                image.Save(filename);
            }

            File.Delete(tmpFileName);
        }

        private byte[] GetExifMakerNote(string filename) // Returns null if the MakerNote field doesn't exist
        {
            using (Bitmap image = new Bitmap(filename))
            {
                PropertyItem[] pis = image.PropertyItems;
                foreach (PropertyItem pi in pis)
                {
                    if (pi.Id == 0x927C) // Id of the MakerNote field
                    {
                        return (byte[])pi.Value.Clone();
                    }
                }

                return null;
            }
        }
    }
}
