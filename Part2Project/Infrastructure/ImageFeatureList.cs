using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Part2Project.Infrastructure
{
    class ImageFeatureList
    {
        // Feature order:
        // 1. Brightness
        // 2. Intensity Contrast
        // 3. Saturation
        // 4. Rule Of Thirds
        // 5. Simplicity (Bounding box area)

        public const int VersionNumber = 2;
        public const int NumFeatures = 5;
        private bool[] _featuresSet;
        private double[] _features;
        public string ImageFilename { get; private set; }

        public ImageFeatureList(string filename)
        {
            _featuresSet = new bool[NumFeatures];
            _features = new double[NumFeatures];
            ImageFilename = filename;
        }

        public bool LoadFromByteArray(byte[] data)
        {
            // Returns boolean indicating if it was successful in parsing the correct number of doubles
            // from the array, and that the version was up-to-date
            
            // Byte format:
            // 0     :  byte  : Version number
            // 1     :  byte  : Number of Features ((size - 2) / 8) Not really needed, but extra protection from error
            // 2-9   : double : Feature 1
            // 10-17 : double : Feature 2
            // ...
            int length = data.Length;

            if (length != NumFeatures*8 + 2) return false;

            // Check that version is up-to-date
            int dVersionNumber = data[0];
            if (dVersionNumber != VersionNumber) return false;

            // Check that number of features is consistent with current version
            int dNumFeatures = data[1];
            if (dNumFeatures != NumFeatures) return false;

            // Read all of the features
            for (int i = 0; i < NumFeatures; i++)
            {
                _features[i] = BitConverter.ToDouble(data, 8*i + 2);
                _featuresSet[i] = true;
            }

            return true;
        }

        public byte[] ToByteArray() // Returns null if not all of the features have been set
        {
            if (!AllFeaturesSet()) return null;

            byte[] array = new byte[NumFeatures * 8 + 2];
            array[0] = VersionNumber;
            array[1] = NumFeatures;

            for (int i = 0; i < NumFeatures; i++)
            {
                byte[] featureBytes = BitConverter.GetBytes(_features[i]);

                for (int j = 0; j < 8; j++)
                {
                    array[2 + 8*i + j] = featureBytes[j];
                }
            }

            return array;
        }

        public bool AllFeaturesSet()
        {
            bool result = _featuresSet[0];

            for (int i = 1; i < _featuresSet.Length; i++)
            {
                result = result && _featuresSet[i];
            }

            return result;
        }

        #region Features

        public double Brightness
        {
            get { return _features[0]; }
            set
            {
                _features[0] = value;
                _featuresSet[0] = true;
            }
        }
        public double IntensityContrast
        {
            get { return _features[1]; }
            set
            {
                _features[1] = value;
                _featuresSet[1] = true;
            }
        }
        public double Saturation
        {
            get { return _features[2]; }
            set
            {
                _features[2] = value;
                _featuresSet[2] = true;
            }
        }
        public double RuleOfThirds
        {
            get { return _features[3]; }
            set
            {
                _features[3] = value;
                _featuresSet[3] = true;
            }
        }
        public double Simplicity
        {
            get { return _features[4]; }
            set
            {
                _features[4] = value;
                _featuresSet[4] = true;
            }
        }

        #endregion
    }
}
