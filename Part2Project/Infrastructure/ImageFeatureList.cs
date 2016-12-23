using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    class ImageFeatureList
    {
        //TODO: Add conversions to/from the Exif byte array

        public const int VersionNumber = 1;
        public const int NumFeatures = 5;
        private bool[] _featuresSet;

        ImageFeatureList()
        {
            _featuresSet = new bool[NumFeatures];
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

        private double _brightness, _intensityContrast, _saturation, _ruleOfThirds, _simplicity;
        public double Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = value;
                _featuresSet[0] = true;
            }
        }
        public double IntensityContrast
        {
            get { return _intensityContrast; }
            set
            {
                _intensityContrast = value;
                _featuresSet[1] = true;
            }
        }
        public double Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
                _featuresSet[2] = true;
            }
        }
        public double RuleOfThirds
        {
            get { return _ruleOfThirds; }
            set
            {
                _ruleOfThirds = value;
                _featuresSet[3] = true;
            }
        }
        public double Simplicity
        {
            get { return _simplicity; }
            set
            {
                _simplicity = value;
                _featuresSet[4] = true;
            }
        }

        #endregion
    }
}
