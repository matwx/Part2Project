using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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
            Image selected = Image.FromFile(_filename);

            // TODO: First try to check the Exif metadata for feature values

            // If they can't be retrieved from there, we need to compute them

            // Read the image from file as a bitmap
            _image = new Bitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
            Graphics gfx = Graphics.FromImage(_image);
            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);

            // Compute the features and store the results
            result.Brightness = new FeatureBrightness().ComputeFeature(_image);
            result.IntensityContrast = new FeatureIntensityContrast().ComputeFeature(_image);
            result.Saturation = new FeatureSaturation().ComputeFeature(_image);
            result.RuleOfThirds = new FeatureRuleOfThirds().ComputeFeature(_image);
            result.Simplicity = new FeatureSimplicity().ComputeFeature(_image);

            return result;
        }
    }
}
