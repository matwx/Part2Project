using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Part2Project_GUI.ViewModel
{
    class FourImageSelectorViewModel : BaseViewModel
    {
        private ImageSorting _imageSorting;
        private int _roundNum = 1; // The current round number
        private int[] _imageIndices = new int[4]; // The indicies into _imageSorting.ScoredImages that are currently being shown
        private double[] _featureMeans, _featureIncrementalVariances, _mins, _maxes;

        private const int 
            FEATURE_BRIGHTNESS = 0,
            FEATURE_INTENSITY_CONTRAST = 1,
            FEATURE_SATURATION = 2,
            FEATURE_BLURRINESS = 3,
            FEATURE_ROI_SIZE = 4,
            FEATURE_RULE_OF_THIRDS = 5,
            FEATURE_SHAPE_CONVEXITY = 6,
            FEATURE_BACKGROUND_DISTRACTION = 7;

        #region Properties

        private BitmapImage[] _imagesToChooseFrom = new BitmapImage[4];
        public BitmapImage Image1
        {
            get { return _imagesToChooseFrom[0]; }
            set
            {
                _imagesToChooseFrom[0] = value;
                OnPropertyChanged("Image1");
            }
        }
        public BitmapImage Image2
        {
            get { return _imagesToChooseFrom[1]; }
            set
            {
                _imagesToChooseFrom[1] = value;
                OnPropertyChanged("Image2");
            }
        }
        public BitmapImage Image3
        {
            get { return _imagesToChooseFrom[2]; }
            set
            {
                _imagesToChooseFrom[2] = value;
                OnPropertyChanged("Image3");
            }
        }
        public BitmapImage Image4
        {
            get { return _imagesToChooseFrom[3]; }
            set
            {
                _imagesToChooseFrom[3] = value;
                OnPropertyChanged("Image4");
            }
        }

        private ObservableCollection<string> _currentInferences;
        public IEnumerable<string> CurrentInferences // This is the current predictions of feature weights.
        {
            get { return _currentInferences; }
            set
            {
                _currentInferences = value as ObservableCollection<string>;
                OnPropertyChanged("CurrentInferences");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _chooseImageCommand;
        public RelayCommand ChooseImageCommand
        {
            get
            {
                if (_chooseImageCommand == null)
                {
                    _chooseImageCommand = new RelayCommand(ChooseImage, CanChooseImage);
                }
                return _chooseImageCommand;
            }
        }
        private void ChooseImage(Object input)
        {
            int imageNumber;

            if (input is string)
            {
                try
                {
                    imageNumber = int.Parse((string)input);

                    if (imageNumber < 1 || imageNumber > 4)
                    {
                        throw new Exception("ChooseImage was called with an invalid parameter '" + ((string) input) + "'.");
                    }
                }
                catch (FormatException e)
                {
                    throw new Exception("ChooseImage was called with a non-integer parameter '" + ((string)input) + "'.");
                }

            }
            else throw new Exception("ChooseImage was called with an invalid parameter.");

            // Update feature means and variances based on chosen image
            double[] oldMeans = (double[]) _featureMeans.Clone();
            int imageIndex = _imageIndices[imageNumber - 1];

            double chosenBrightness = _imageSorting.ScoredImages[imageIndex].Features.Brightness;
            _featureMeans[FEATURE_BRIGHTNESS] = oldMeans[FEATURE_BRIGHTNESS] + (1.0 / _roundNum) * (chosenBrightness - oldMeans[FEATURE_BRIGHTNESS]);
            _featureIncrementalVariances[FEATURE_BRIGHTNESS] += (chosenBrightness - oldMeans[FEATURE_BRIGHTNESS]) * (chosenBrightness - _featureMeans[FEATURE_BRIGHTNESS]);

            double chosenConstrast = _imageSorting.ScoredImages[imageIndex].Features.IntensityContrast;
            _featureMeans[FEATURE_INTENSITY_CONTRAST] = oldMeans[FEATURE_INTENSITY_CONTRAST] + (1.0 / _roundNum) * (chosenConstrast - oldMeans[FEATURE_INTENSITY_CONTRAST]);
            _featureIncrementalVariances[FEATURE_INTENSITY_CONTRAST] += (chosenConstrast - oldMeans[FEATURE_INTENSITY_CONTRAST]) * (chosenConstrast - _featureMeans[FEATURE_INTENSITY_CONTRAST]);

            double chosenSaturation = _imageSorting.ScoredImages[imageIndex].Features.Saturation;
            _featureMeans[FEATURE_SATURATION] = oldMeans[FEATURE_SATURATION] + (1.0 / _roundNum) * (chosenSaturation - oldMeans[FEATURE_SATURATION]);
            _featureIncrementalVariances[FEATURE_SATURATION] += (chosenSaturation - oldMeans[FEATURE_SATURATION]) * (chosenSaturation - _featureMeans[FEATURE_SATURATION]);

            double chosenBlurriness = _imageSorting.ScoredImages[imageIndex].Features.Blurriness;
            _featureMeans[FEATURE_BLURRINESS] = oldMeans[FEATURE_BLURRINESS] + (1.0 / _roundNum) * (chosenBlurriness - oldMeans[FEATURE_BLURRINESS]);
            _featureIncrementalVariances[FEATURE_BLURRINESS] += (chosenBlurriness - oldMeans[FEATURE_BLURRINESS]) * (chosenBlurriness - _featureMeans[FEATURE_BLURRINESS]);

            double chosenRoiSize = _imageSorting.ScoredImages[imageIndex].Features.RegionsOfInterestSize;
            _featureMeans[FEATURE_ROI_SIZE] = oldMeans[FEATURE_ROI_SIZE] + (1.0 / _roundNum) * (chosenRoiSize - oldMeans[FEATURE_ROI_SIZE]);
            _featureIncrementalVariances[FEATURE_ROI_SIZE] += (chosenRoiSize - oldMeans[FEATURE_ROI_SIZE]) * (chosenRoiSize - _featureMeans[FEATURE_ROI_SIZE]);

            double chosenROT = _imageSorting.ScoredImages[imageIndex].Features.RuleOfThirds;
            _featureMeans[FEATURE_RULE_OF_THIRDS] = oldMeans[FEATURE_RULE_OF_THIRDS] + (1.0 / _roundNum) * (chosenROT - oldMeans[FEATURE_RULE_OF_THIRDS]);
            _featureIncrementalVariances[FEATURE_RULE_OF_THIRDS] += (chosenROT - oldMeans[FEATURE_RULE_OF_THIRDS]) * (chosenROT - _featureMeans[FEATURE_RULE_OF_THIRDS]);

            double chosenConvexity = _imageSorting.ScoredImages[imageIndex].Features.ShapeConvexity;
            _featureMeans[FEATURE_SHAPE_CONVEXITY] = oldMeans[FEATURE_SHAPE_CONVEXITY] + (1.0 / _roundNum) * (chosenConvexity - oldMeans[FEATURE_SHAPE_CONVEXITY]);
            _featureIncrementalVariances[FEATURE_SHAPE_CONVEXITY] += (chosenConvexity - oldMeans[FEATURE_SHAPE_CONVEXITY]) * (chosenConvexity - _featureMeans[FEATURE_SHAPE_CONVEXITY]);

            double chosenDistract = _imageSorting.ScoredImages[imageIndex].Features.BackgroundDistraction;
            _featureMeans[FEATURE_BACKGROUND_DISTRACTION] = oldMeans[FEATURE_BACKGROUND_DISTRACTION] + (1.0 / _roundNum) * (chosenDistract - oldMeans[FEATURE_BACKGROUND_DISTRACTION]);
            _featureIncrementalVariances[FEATURE_BACKGROUND_DISTRACTION] += (chosenDistract - oldMeans[FEATURE_BACKGROUND_DISTRACTION]) * (chosenDistract - _featureMeans[FEATURE_BACKGROUND_DISTRACTION]);

            // Display a new set of images
            UpdateImages();

            // Display our updated weight predictions
            UpdateInferences();

            _roundNum++;
        }
        private bool CanChooseImage(Object input)
        {
            int imageNumber;

            if (input is string)
            {
                try
                {
                    imageNumber = int.Parse((string)input);

                    if (imageNumber < 1 || imageNumber > 4)
                    {
                        throw new Exception("ChooseImage was called with an invalid parameter '" + ((string)input) + "'.");
                    }
                }
                catch (FormatException e)
                {
                    throw new Exception("ChooseImage was called with a non-integer parameter '" + ((string)input) + "'.");
                }

            }
            else throw new Exception("ChooseImage was called with an invalid parameter.");

            return (_imagesToChooseFrom[imageNumber - 1] != null);
        }

        #endregion

        public FourImageSelectorViewModel(MainWindowViewModel w, BaseViewModel p, ImageSorting iS) : base(w, p)
        {
            _imageSorting = iS;

            // Initialise means and variances
            _featureMeans = new double[8];
            _featureIncrementalVariances = new double[8];
            for (int i = 0; i < 8; i++)
            {
                _featureMeans[i] = 0;
                _featureIncrementalVariances[i] = 0;
            }

            // Initialise boundaries
            _mins = new double[8];
            _maxes = new double[8];
            for (int i = 0; i < 8; i++)
            {
                _mins[i] = 1;
                _maxes[i] = 0;
            }

            UpdateImages();
        }

        private void UpdateImages()
        {
            Random rand = new Random();

            // Select the four images. Make sure that they're not the same
            _imageIndices[0] = rand.Next(0, _imageSorting.ScoredImages.Length);
            _imageIndices[1] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[1] == _imageIndices[0])
            {
                _imageIndices[1] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndices[2] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[2] == _imageIndices[1] || _imageIndices[2] == _imageIndices[0])
            {
                _imageIndices[2] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndices[3] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[3] == _imageIndices[2] || _imageIndices[3] == _imageIndices[1] || _imageIndices[3] == _imageIndices[0])
            {
                _imageIndices[3] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }

            for (int i = 0; i < 4; i++)
            {
                _maxes[FEATURE_BRIGHTNESS] = Math.Max(_maxes[FEATURE_BRIGHTNESS], _imageSorting.ScoredImages[_imageIndices[i]].Features.Brightness);
                _mins[FEATURE_BRIGHTNESS] = Math.Min(_mins[FEATURE_BRIGHTNESS], _imageSorting.ScoredImages[_imageIndices[i]].Features.Brightness);

                _maxes[FEATURE_INTENSITY_CONTRAST] = Math.Max(_maxes[FEATURE_INTENSITY_CONTRAST], _imageSorting.ScoredImages[_imageIndices[i]].Features.IntensityContrast);
                _mins[FEATURE_INTENSITY_CONTRAST] = Math.Min(_mins[FEATURE_INTENSITY_CONTRAST], _imageSorting.ScoredImages[_imageIndices[i]].Features.IntensityContrast);

                _maxes[FEATURE_SATURATION] = Math.Max(_maxes[FEATURE_SATURATION], _imageSorting.ScoredImages[_imageIndices[i]].Features.Saturation);
                _mins[FEATURE_SATURATION] = Math.Min(_mins[FEATURE_SATURATION], _imageSorting.ScoredImages[_imageIndices[i]].Features.Saturation);

                _maxes[FEATURE_BLURRINESS] = Math.Max(_maxes[FEATURE_BLURRINESS], _imageSorting.ScoredImages[_imageIndices[i]].Features.Blurriness);
                _mins[FEATURE_BLURRINESS] = Math.Min(_mins[FEATURE_BLURRINESS], _imageSorting.ScoredImages[_imageIndices[i]].Features.Blurriness);

                _maxes[FEATURE_ROI_SIZE] = Math.Max(_maxes[FEATURE_ROI_SIZE], _imageSorting.ScoredImages[_imageIndices[i]].Features.RegionsOfInterestSize);
                _mins[FEATURE_ROI_SIZE] = Math.Min(_mins[FEATURE_ROI_SIZE], _imageSorting.ScoredImages[_imageIndices[i]].Features.RegionsOfInterestSize);

                _maxes[FEATURE_RULE_OF_THIRDS] = Math.Max(_maxes[FEATURE_RULE_OF_THIRDS], _imageSorting.ScoredImages[_imageIndices[i]].Features.RuleOfThirds);
                _mins[FEATURE_RULE_OF_THIRDS] = Math.Min(_mins[FEATURE_RULE_OF_THIRDS], _imageSorting.ScoredImages[_imageIndices[i]].Features.RuleOfThirds);

                _maxes[FEATURE_SHAPE_CONVEXITY] = Math.Max(_maxes[FEATURE_SHAPE_CONVEXITY], _imageSorting.ScoredImages[_imageIndices[i]].Features.ShapeConvexity);
                _mins[FEATURE_SHAPE_CONVEXITY] = Math.Min(_mins[FEATURE_SHAPE_CONVEXITY], _imageSorting.ScoredImages[_imageIndices[i]].Features.ShapeConvexity);

                _maxes[FEATURE_BACKGROUND_DISTRACTION] = Math.Max(_maxes[FEATURE_BACKGROUND_DISTRACTION], _imageSorting.ScoredImages[_imageIndices[i]].Features.BackgroundDistraction);
                _mins[FEATURE_BACKGROUND_DISTRACTION] = Math.Min(_mins[FEATURE_BACKGROUND_DISTRACTION], _imageSorting.ScoredImages[_imageIndices[i]].Features.BackgroundDistraction);
            }

            Image1 = _imageSorting.ScoredImages[_imageIndices[0]].Image;
            Image2 = _imageSorting.ScoredImages[_imageIndices[1]].Image;
            Image3 = _imageSorting.ScoredImages[_imageIndices[2]].Image;
            Image4 = _imageSorting.ScoredImages[_imageIndices[3]].Image;
        }

        private void UpdateInferences()
        {
            var newInferences = new ObservableCollection<string>();

            _imageSorting.WBrightness = 2.0 * (_featureMeans[FEATURE_BRIGHTNESS] - _mins[FEATURE_BRIGHTNESS]) / (_maxes[FEATURE_BRIGHTNESS] - _mins[FEATURE_BRIGHTNESS]) - 1;
            _imageSorting.WIntensityContrast = 2.0 * (_featureMeans[FEATURE_INTENSITY_CONTRAST] - _mins[FEATURE_INTENSITY_CONTRAST]) / (_maxes[FEATURE_INTENSITY_CONTRAST] - _mins[FEATURE_INTENSITY_CONTRAST]) - 1;
            _imageSorting.WSaturation = 2.0 * (_featureMeans[FEATURE_SATURATION] - _mins[FEATURE_SATURATION]) / (_maxes[FEATURE_SATURATION] - _mins[FEATURE_SATURATION]) - 1;
            _imageSorting.WBlurriness = 2.0 * (_featureMeans[FEATURE_BLURRINESS] - _mins[FEATURE_BLURRINESS]) / (_maxes[FEATURE_BLURRINESS] - _mins[FEATURE_BLURRINESS]) - 1;
            _imageSorting.WRegionsOfInterestSize = 2.0 * (_featureMeans[FEATURE_ROI_SIZE] - _mins[FEATURE_ROI_SIZE]) / (_maxes[FEATURE_ROI_SIZE] - _mins[FEATURE_ROI_SIZE]) - 1;
            _imageSorting.WRuleOfThirds = 2.0 * (_featureMeans[FEATURE_RULE_OF_THIRDS] - _mins[FEATURE_RULE_OF_THIRDS]) / (_maxes[FEATURE_RULE_OF_THIRDS] - _mins[FEATURE_RULE_OF_THIRDS]) - 1;
            _imageSorting.WShapeConvexity = 2.0 * (_featureMeans[FEATURE_SHAPE_CONVEXITY] - _mins[FEATURE_SHAPE_CONVEXITY]) / (_maxes[FEATURE_SHAPE_CONVEXITY] - _mins[FEATURE_SHAPE_CONVEXITY]) - 1;
            _imageSorting.WBackgroundDistraction = 2.0 * (_featureMeans[FEATURE_BACKGROUND_DISTRACTION] - _mins[FEATURE_BACKGROUND_DISTRACTION]) / (_maxes[FEATURE_BACKGROUND_DISTRACTION] - _mins[FEATURE_BACKGROUND_DISTRACTION]) - 1;

            newInferences.Add("Current Feature Weight Predictions");
            newInferences.Add("Brightness: " + _imageSorting.WBrightness + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_BRIGHTNESS] / _roundNum));
            newInferences.Add("Contrast: " + _imageSorting.WIntensityContrast + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_INTENSITY_CONTRAST] / _roundNum));
            newInferences.Add("Saturation: " + _imageSorting.WSaturation + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_SATURATION] / _roundNum));
            newInferences.Add("Blurriness: " + _imageSorting.WBlurriness + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_BLURRINESS] / _roundNum));
            newInferences.Add("ROI Size: " + _imageSorting.WRegionsOfInterestSize + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_ROI_SIZE] / _roundNum));
            newInferences.Add("Rule of Thirds: " + _imageSorting.WRuleOfThirds + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_RULE_OF_THIRDS] / _roundNum));
            newInferences.Add("Shape Convexity: " + _imageSorting.WShapeConvexity + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_SHAPE_CONVEXITY] / _roundNum));
            newInferences.Add("Background Distraction: " + _imageSorting.WBackgroundDistraction + " +/- " + Math.Sqrt(_featureIncrementalVariances[FEATURE_BACKGROUND_DISTRACTION] / _roundNum));

            CurrentInferences = newInferences;
        }
    }
}
