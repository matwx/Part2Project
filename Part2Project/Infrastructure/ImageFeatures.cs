using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    class ImageFeatures
    {
        // Retrieves/computes a list of all features for a particular image
        private Bitmap _image;

        public ImageFeatures(string filename)
        {
            // Read the image as a bitmap
            Image selected = Image.FromFile(filename);
            _image = new Bitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
            Graphics gfx = Graphics.FromImage(_image);
            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
        }

        List<double> GetFeatures()
        {
            List<double> result = new List<double>();



            return result;
        }
    }
}
