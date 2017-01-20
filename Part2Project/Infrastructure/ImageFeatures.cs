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
                    using (DirectBitmap image = new DirectBitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240))
                    {
                        using (DirectBitmap image512X512 = new DirectBitmap(512, 512))
                        {
                            // Create the required resized images
                            using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                            {
                                gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                            }
                            using (Graphics gfx = Graphics.FromImage(image512X512.Bitmap))
                            {
                                int originalWidth = (int)((double)selected.Width / (double)selected.Height * 512.0);
                                gfx.DrawImage(selected, 256 - originalWidth / 2, 0, originalWidth, 512);
                            }

                            // Then compute the features and store the results
                            // Low-level
                            result.Blurriness = FeatureBlurriness.ComputeFeature(image512X512);
                            result.Brightness = FeatureBrightness.ComputeFeature(image);
                            result.IntensityContrast = FeatureIntensityContrast.ComputeFeature(image);
                            result.Saturation = FeatureSaturation.ComputeFeature(image);

                            // Segmentation-Derived
                            const int k = 125;
                            const double sigma = 0.6;
                            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
                            result.RuleOfThirds = FeatureRuleOfThirds.ComputeFeature(image, s, sigma);
                            result.Simplicity = FeatureSimplicity.ComputeFeature(image, s, sigma);
                        }
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
