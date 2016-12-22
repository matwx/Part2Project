using System;
using System.Collections.Generic;
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

        public ImageDirectoryFeatures(string dPath)
        {
            _dPath = dPath;
        }
    }
}
