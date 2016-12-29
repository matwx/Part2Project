using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    class ImageDirectoryFeatures
    {
        // Given a directory path, configure and launch ImageFeatures threads for each image
        // using Tasks, and gather together the list of features for each image in the 
        // directory.
        private string _dPath;
        private List<string> _imageFilenames;
        private int _numImages = 0;

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
                    _numImages++;
                }
            }
        }

        public List<ImageFeatureList> GetDirectoryFeatures()
        {
            List<ImageFeatureList> result = new List<ImageFeatureList>();

            Task[] tasks = new Task[_numImages];
            ImageFeatures[] imFeatArray = new ImageFeatures[_numImages];

            // Configure and launch threads using ThreadPool
            for (int i = 0; i < _numImages; i++)
            {
                ImageFeatures imFeat = new ImageFeatures(_imageFilenames.ElementAt(i));
                imFeatArray[i] = imFeat;
                tasks[i] = Task.Run(() => imFeat.ThreadPoolCallback());
            }

            // Wait for all threads in the pool to finish computing image features
            Task.WaitAll(tasks);

            for (int i = 0; i < _numImages; i++)
            {
                result.Add(imFeatArray[i].Features);
            }

            return result;
        }
    }
}