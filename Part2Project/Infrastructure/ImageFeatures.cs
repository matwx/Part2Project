using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    class ImageFeatures
    {
        // Retrieves/computes a list of all features for a particular image
        private string _filename;
        private Bitmap _image;
        private ManualResetEvent _doneEvent;
        public List<double> Features { get; private set; }

        public ImageFeatures(string filename, ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
            _filename = filename;
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            Features = GetFeatures();
            _doneEvent.Set();
        }

        public List<double> GetFeatures()
        {
            List<double> result = new List<double>();

            // Read the image as a bitmap
            Image selected = Image.FromFile(_filename);
            _image = new Bitmap((int)((double)selected.Width / (double)selected.Height * 240.0), 240);
            Graphics gfx = Graphics.FromImage(_image);
            gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);

            //Todo: Fill this in with feature retrieval or extraction.

            return result;
        }
    }
}
