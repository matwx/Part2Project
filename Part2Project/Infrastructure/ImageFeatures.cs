using System;
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
using Part2Project.ImageSegmentation;

namespace Part2Project.Infrastructure
{
    class ImageFeatures
    {
        // Retrieves/computes a list of all features for a particular image
        private string _filename;
        private DirectBitmap _image;
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
            ImageFeatureList result = new ImageFeatureList(_filename);

            string ext = _filename.Split('.').Last();
            byte[] makerNote = GetExifMakerNote(_filename);
            if (!(ext.Equals("jpg") || ext.Equals("jpeg")) || makerNote == null || !result.LoadFromByteArray(makerNote))
            {
                // If they can't be retrieved from Exif, we need to compute them
                // First read the image from file as a bitmap
                using (Image selected = Image.FromFile(_filename))
                {
                    using (_image = new DirectBitmap((int) ((double) selected.Width/(double) selected.Height*240.0), 240))
                    {
                        using (Graphics gfx = Graphics.FromImage(_image.Bitmap))
                        {
                            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                        } 
                        
                        // Then compute the features and store the results
                        // Low-level
                        result.Brightness = FeatureBrightness.ComputeFeature(_image);
                        result.IntensityContrast = FeatureIntensityContrast.ComputeFeature(_image);
                        result.Saturation = FeatureSaturation.ComputeFeature(_image);

                        // Segmentation-Derived
                        Segmentation s = GraphBasedImageSegmentation.Segment(_image, 150, 0.8);
                        result.RuleOfThirds = FeatureRuleOfThirds.ComputeFeature(_image, s);
                        result.Simplicity = FeatureSimplicity.ComputeFeature(_image, s);
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
            byte[] imageBytes = File.ReadAllBytes(filename);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Bitmap image = new Bitmap(ms))
                {
                    PropertyItem pi = image.PropertyItems[0];
                    pi.Type = 7; // Undefined
                    pi.Len = data.Length;
                    pi.Value = (byte[])data.Clone();
                    pi.Id = 0x927C; // MakerNote field Id

                    image.SetPropertyItem(pi);

                    image.Save(filename);
                }
            }
        }

        private byte[] GetExifMakerNote(string filename) // Returns null if the MakerNote field doesn't exist
        {
            using (Bitmap image = new Bitmap(filename))
            {
                try
                {
                    return image.GetPropertyItem(0x927C).Value;
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }
    }
}
