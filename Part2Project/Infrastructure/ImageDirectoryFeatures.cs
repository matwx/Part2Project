using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    class ImageDirectoryFeatures
    {
        // Given a directory path, configure and launch ImageFeatures threads for each image
        // using Threadpool, and gather together the list of features for each image in the 
        // directory.
        private string _dPath;
        private List<string> _imageFilenames;
        private List<List<double>> _imageFeatures;

        public ImageDirectoryFeatures(string dPath)
        {
            _dPath = dPath;

            // Get list of image filenames in the directory
            _imageFilenames = new List<string>();
            string[] filenames = Directory.GetFiles(_dPath);
            foreach (string filename in filenames)
            {
                string ext = filename.Split('.').Last();
                if (ext.Equals("jpg") || ext.Equals("jpeg") || ext.Equals("png"))
                {
                    // We'll accept these file extensions as images
                    _imageFilenames.Add(filename);
                }
            }
        }
    }
}

//            dlgChooseFolder.ShowDialog();
//            Dictionary<double, string> newNames = new Dictionary<double, string>();
//
//            var files = Directory.GetFiles(dlgChooseFolder.SelectedPath);
//            foreach (string filename in files)
//            {
//                using (Image selected = Image.FromFile(filename))
//                {
//                    bmp = new Bitmap((int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
//                    Graphics gfx = Graphics.FromImage(bmp);
//
//                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * (double)viewer.Height), viewer.Height);
//
//                    double value = new FeatureIntensityContrast().ComputeFeature(bmp);
//
//                    //newNames.Add(filename, dlgChooseFolder.SelectedPath + "\\" + value.ToString() + ".jpg");
//                    newNames.Add(value, filename);
//                }
//
//            }
//
//            List<double> keyList = new List<double>();
//            foreach (double key in newNames.Keys)
//            {
//                keyList.Add(key);
//            }
//            keyList.Sort();
//            keyList.Reverse();
//            int current = 0;
//            foreach (double key in keyList)
//            {
//                File.Move(newNames[key], dlgChooseFolder.SelectedPath + "\\" + current.ToString() + "--" + key.ToString() + "--IC.jpg");
//                current++;
//            }