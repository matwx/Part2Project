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
        private int _roundNum = 0; // The current round number
        private int[] _imageIndices = new int[4]; // The indicies into _imageSorting.ScoredImages that are currently being shown
        private double[] _xBar, _S_n, _a, _b, _mu, _aAll, _bAll, _muAll, _SAll;
        private List<int> _indicesSeen;
        private double _totalImagesSeen = 0.0;

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
            double[] oldMeans = (double[]) _xBar.Clone();
            int imageIndex = _imageIndices[imageNumber - 1];

            _roundNum++;
            _indicesSeen.Add(imageIndex);

            double chosenBrightness = _imageSorting.ScoredImages[imageIndex].Features.Brightness;
            _xBar[FEATURE_BRIGHTNESS] = oldMeans[FEATURE_BRIGHTNESS] + (1.0 / _roundNum) * (chosenBrightness - oldMeans[FEATURE_BRIGHTNESS]);
            _S_n[FEATURE_BRIGHTNESS] += (chosenBrightness - oldMeans[FEATURE_BRIGHTNESS]) * (chosenBrightness - _xBar[FEATURE_BRIGHTNESS]);

            double chosenConstrast = _imageSorting.ScoredImages[imageIndex].Features.IntensityContrast;
            _xBar[FEATURE_INTENSITY_CONTRAST] = oldMeans[FEATURE_INTENSITY_CONTRAST] + (1.0 / _roundNum) * (chosenConstrast - oldMeans[FEATURE_INTENSITY_CONTRAST]);
            _S_n[FEATURE_INTENSITY_CONTRAST] += (chosenConstrast - oldMeans[FEATURE_INTENSITY_CONTRAST]) * (chosenConstrast - _xBar[FEATURE_INTENSITY_CONTRAST]);

            double chosenSaturation = _imageSorting.ScoredImages[imageIndex].Features.Saturation;
            _xBar[FEATURE_SATURATION] = oldMeans[FEATURE_SATURATION] + (1.0 / _roundNum) * (chosenSaturation - oldMeans[FEATURE_SATURATION]);
            _S_n[FEATURE_SATURATION] += (chosenSaturation - oldMeans[FEATURE_SATURATION]) * (chosenSaturation - _xBar[FEATURE_SATURATION]);

            double chosenBlurriness = _imageSorting.ScoredImages[imageIndex].Features.Blurriness;
            _xBar[FEATURE_BLURRINESS] = oldMeans[FEATURE_BLURRINESS] + (1.0 / _roundNum) * (chosenBlurriness - oldMeans[FEATURE_BLURRINESS]);
            _S_n[FEATURE_BLURRINESS] += (chosenBlurriness - oldMeans[FEATURE_BLURRINESS]) * (chosenBlurriness - _xBar[FEATURE_BLURRINESS]);

            if (_window.SEG_FEATURES_ENABLED)
            {
                double chosenRoiSize = _imageSorting.ScoredImages[imageIndex].Features.RegionsOfInterestSize;
                _xBar[FEATURE_ROI_SIZE] = oldMeans[FEATURE_ROI_SIZE] + (1.0 / _roundNum) * (chosenRoiSize - oldMeans[FEATURE_ROI_SIZE]);
                _S_n[FEATURE_ROI_SIZE] += (chosenRoiSize - oldMeans[FEATURE_ROI_SIZE]) * (chosenRoiSize - _xBar[FEATURE_ROI_SIZE]);

                double chosenROT = _imageSorting.ScoredImages[imageIndex].Features.RuleOfThirds;
                _xBar[FEATURE_RULE_OF_THIRDS] = oldMeans[FEATURE_RULE_OF_THIRDS] + (1.0 / _roundNum) * (chosenROT - oldMeans[FEATURE_RULE_OF_THIRDS]);
                _S_n[FEATURE_RULE_OF_THIRDS] += (chosenROT - oldMeans[FEATURE_RULE_OF_THIRDS]) * (chosenROT - _xBar[FEATURE_RULE_OF_THIRDS]);

                double chosenConvexity = _imageSorting.ScoredImages[imageIndex].Features.ShapeConvexity;
                _xBar[FEATURE_SHAPE_CONVEXITY] = oldMeans[FEATURE_SHAPE_CONVEXITY] + (1.0 / _roundNum) * (chosenConvexity - oldMeans[FEATURE_SHAPE_CONVEXITY]);
                _S_n[FEATURE_SHAPE_CONVEXITY] += (chosenConvexity - oldMeans[FEATURE_SHAPE_CONVEXITY]) * (chosenConvexity - _xBar[FEATURE_SHAPE_CONVEXITY]);

                double chosenDistract = _imageSorting.ScoredImages[imageIndex].Features.BackgroundDistraction;
                _xBar[FEATURE_BACKGROUND_DISTRACTION] = oldMeans[FEATURE_BACKGROUND_DISTRACTION] + (1.0 / _roundNum) * (chosenDistract - oldMeans[FEATURE_BACKGROUND_DISTRACTION]);
                _S_n[FEATURE_BACKGROUND_DISTRACTION] += (chosenDistract - oldMeans[FEATURE_BACKGROUND_DISTRACTION]) * (chosenDistract - _xBar[FEATURE_BACKGROUND_DISTRACTION]);
            }

            // Display a new set of images
            UpdateImages();

            // Display our updated weight predictions
            UpdateInferences();
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

        private RelayCommand _skipCommand;
        public RelayCommand SkipCommand
        {
            get
            {
                if (_skipCommand == null)
                {
                    _skipCommand = new RelayCommand(x => UpdateImages(), x => CanChooseImage("1"));
                }
                return _skipCommand;
            }
        }

        #endregion

        public FourImageSelectorViewModel(MainWindowViewModel w, BaseViewModel p, ImageSorting iS) : base(w, p)
        {
            _imageSorting = iS;

            // Initialise means and variances
            _xBar = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];
            _S_n = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];
            for (int i = 0; i < ((_window.SEG_FEATURES_ENABLED) ? 8 : 4); i++)
            {
                _xBar[i] = 0;
                _S_n[i] = 0;
            }

            // Initialise seen indices
            _indicesSeen = new List<int>();

            ComputeAllFolderMinsMaxesMus();

            UpdateImages();
        }

        private void ComputeAllFolderMinsMaxesMus()
        {
            _aAll = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];
            _bAll = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];
            _muAll = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];
            _SAll = new double[((_window.SEG_FEATURES_ENABLED) ? 8 : 4)];

            for (int i = 0; i < _imageSorting.ScoredImages.Length; i++)
            {
                var Brightness = _imageSorting.ScoredImages[i].Features.Brightness;
                var oldmu = _muAll[FEATURE_BRIGHTNESS];
                _bAll[FEATURE_BRIGHTNESS] = Math.Max(_bAll[FEATURE_BRIGHTNESS], Brightness);
                _aAll[FEATURE_BRIGHTNESS] = Math.Min(_aAll[FEATURE_BRIGHTNESS], Brightness);
                _muAll[FEATURE_BRIGHTNESS] = oldmu + (1.0 / (i + 1)) * (Brightness - oldmu);
                _SAll[FEATURE_BRIGHTNESS] += (Brightness - oldmu) * (Brightness - _muAll[FEATURE_BRIGHTNESS]);

                var IntensityContrast = _imageSorting.ScoredImages[i].Features.IntensityContrast;
                oldmu = _muAll[FEATURE_INTENSITY_CONTRAST];
                _bAll[FEATURE_INTENSITY_CONTRAST] = Math.Max(_bAll[FEATURE_INTENSITY_CONTRAST], IntensityContrast);
                _aAll[FEATURE_INTENSITY_CONTRAST] = Math.Min(_aAll[FEATURE_INTENSITY_CONTRAST], IntensityContrast);
                _muAll[FEATURE_INTENSITY_CONTRAST] = _muAll[FEATURE_INTENSITY_CONTRAST] + (1.0 / (i + 1)) * (IntensityContrast - _muAll[FEATURE_INTENSITY_CONTRAST]);
                _SAll[FEATURE_INTENSITY_CONTRAST] += (IntensityContrast - oldmu) * (IntensityContrast - _muAll[FEATURE_INTENSITY_CONTRAST]);

                var Saturation = _imageSorting.ScoredImages[i].Features.Saturation;
                oldmu = _muAll[FEATURE_SATURATION];
                _bAll[FEATURE_SATURATION] = Math.Max(_bAll[FEATURE_SATURATION], Saturation);
                _aAll[FEATURE_SATURATION] = Math.Min(_aAll[FEATURE_SATURATION], Saturation);
                _muAll[FEATURE_SATURATION] = _muAll[FEATURE_SATURATION] + (1.0 / (i + 1)) * (Saturation - _muAll[FEATURE_SATURATION]);
                _SAll[FEATURE_SATURATION] += (Saturation - oldmu) * (Saturation - _muAll[FEATURE_SATURATION]);

                var Blurriness = _imageSorting.ScoredImages[i].Features.Blurriness;
                oldmu = _muAll[FEATURE_BLURRINESS];
                _bAll[FEATURE_BLURRINESS] = Math.Max(_bAll[FEATURE_BLURRINESS], Blurriness);
                _aAll[FEATURE_BLURRINESS] = Math.Min(_aAll[FEATURE_BLURRINESS], Blurriness);
                _muAll[FEATURE_BLURRINESS] = _muAll[FEATURE_BLURRINESS] + (1.0 / (i + 1)) * (Blurriness - _muAll[FEATURE_BLURRINESS]);
                _SAll[FEATURE_BLURRINESS] += (Blurriness - oldmu) * (Blurriness - _muAll[FEATURE_BLURRINESS]);

                if (_window.SEG_FEATURES_ENABLED)
                {
                    var RegionsOfInterestSize = _imageSorting.ScoredImages[i].Features.RegionsOfInterestSize;
                    oldmu = _muAll[FEATURE_ROI_SIZE];
                    _bAll[FEATURE_ROI_SIZE] = Math.Max(_bAll[FEATURE_ROI_SIZE], RegionsOfInterestSize);
                    _aAll[FEATURE_ROI_SIZE] = Math.Min(_aAll[FEATURE_ROI_SIZE], RegionsOfInterestSize);
                    _muAll[FEATURE_ROI_SIZE] = _muAll[FEATURE_ROI_SIZE] + (1.0 / (i + 1)) * (RegionsOfInterestSize - _muAll[FEATURE_ROI_SIZE]);
                    _SAll[FEATURE_ROI_SIZE] += (RegionsOfInterestSize - oldmu) * (RegionsOfInterestSize - _muAll[FEATURE_ROI_SIZE]);

                    var RuleOfThirds = _imageSorting.ScoredImages[i].Features.RuleOfThirds;
                    oldmu = _muAll[FEATURE_RULE_OF_THIRDS];
                    _bAll[FEATURE_RULE_OF_THIRDS] = Math.Max(_bAll[FEATURE_RULE_OF_THIRDS], RuleOfThirds);
                    _aAll[FEATURE_RULE_OF_THIRDS] = Math.Min(_aAll[FEATURE_RULE_OF_THIRDS], RuleOfThirds);
                    _muAll[FEATURE_RULE_OF_THIRDS] = _muAll[FEATURE_RULE_OF_THIRDS] + (1.0 / (i + 1)) * (RuleOfThirds - _muAll[FEATURE_RULE_OF_THIRDS]);
                    _SAll[FEATURE_RULE_OF_THIRDS] += (RuleOfThirds - oldmu) * (RuleOfThirds - _muAll[FEATURE_RULE_OF_THIRDS]);
                
                    var ShapeConvexity = _imageSorting.ScoredImages[i].Features.ShapeConvexity;
                    oldmu = _muAll[FEATURE_SHAPE_CONVEXITY];
                    _bAll[FEATURE_SHAPE_CONVEXITY] = Math.Max(_bAll[FEATURE_SHAPE_CONVEXITY], ShapeConvexity);
                    _aAll[FEATURE_SHAPE_CONVEXITY] = Math.Min(_aAll[FEATURE_SHAPE_CONVEXITY], ShapeConvexity);
                    _muAll[FEATURE_SHAPE_CONVEXITY] = _muAll[FEATURE_SHAPE_CONVEXITY] + (1.0 / (i + 1)) * (ShapeConvexity - _muAll[FEATURE_SHAPE_CONVEXITY]);
                    _SAll[FEATURE_SHAPE_CONVEXITY] += (ShapeConvexity - oldmu) * (ShapeConvexity - _muAll[FEATURE_SHAPE_CONVEXITY]);

                    var BackgroundDistraction = _imageSorting.ScoredImages[i].Features.BackgroundDistraction;
                    oldmu = _muAll[FEATURE_BACKGROUND_DISTRACTION];
                    _bAll[FEATURE_BACKGROUND_DISTRACTION] = Math.Max(_bAll[FEATURE_BACKGROUND_DISTRACTION], BackgroundDistraction);
                    _aAll[FEATURE_BACKGROUND_DISTRACTION] = Math.Min(_aAll[FEATURE_BACKGROUND_DISTRACTION], BackgroundDistraction);
                    _muAll[FEATURE_BACKGROUND_DISTRACTION] = _muAll[FEATURE_BACKGROUND_DISTRACTION] + (1.0 / (i + 1)) * (BackgroundDistraction - _muAll[FEATURE_BACKGROUND_DISTRACTION]);
                    _SAll[FEATURE_BACKGROUND_DISTRACTION] += (BackgroundDistraction - oldmu) * (BackgroundDistraction - _muAll[FEATURE_BACKGROUND_DISTRACTION]);
                }
            }
        }

        private void UpdateImages()
        {
            Random rand = new Random();

            // Select the four images. Make sure that they're not the same
            _imageIndices[0] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_indicesSeen.Contains(_imageIndices[0]))
            {
                _imageIndices[0] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndices[1] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[1] == _imageIndices[0] || _indicesSeen.Contains(_imageIndices[1]))
            {
                _imageIndices[1] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndices[2] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[2] == _imageIndices[1] || _imageIndices[2] == _imageIndices[0] || _indicesSeen.Contains(_imageIndices[2]))
            {
                _imageIndices[2] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndices[3] = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndices[3] == _imageIndices[2] || _imageIndices[3] == _imageIndices[1] || _imageIndices[3] == _imageIndices[0] || _indicesSeen.Contains(_imageIndices[3]))
            {
                _imageIndices[3] = rand.Next(0, _imageSorting.ScoredImages.Length);
            }

            Image1 = _imageSorting.ScoredImages[_imageIndices[0]].Image;
            Image2 = _imageSorting.ScoredImages[_imageIndices[1]].Image;
            Image3 = _imageSorting.ScoredImages[_imageIndices[2]].Image;
            Image4 = _imageSorting.ScoredImages[_imageIndices[3]].Image;
        }

        private void UpdateInferences()
        {
            int numConverged = 0;
            for (int i = 0; i < ((_window.SEG_FEATURES_ENABLED) ? 8 : 4); i++)
            {
                if (_xBar[i] <= _muAll[i])
                {
                    _imageSorting.SetWeightByIndex(i, (_xBar[i] - _muAll[i]) / (_muAll[i] - _aAll[i]));
                    var confIntWidth = 2.0 * 1.96 * Math.Sqrt(_S_n[i] / _roundNum) / (_muAll[i] - _aAll[i]) / Math.Sqrt(_roundNum);
                    var thresh = Math.Sqrt(_SAll[i] / _imageSorting.ScoredImages.Length) / (_muAll[i] - _aAll[i]);
                    if (confIntWidth < thresh) numConverged++;
                }
                else
                {
                    _imageSorting.SetWeightByIndex(i, (_xBar[i] - _muAll[i]) / (_bAll[i] - _muAll[i]));
                    var confIntWidth = 2.0 * 1.96 * Math.Sqrt(_S_n[i] / _roundNum) / (_bAll[i] - _muAll[i]) / Math.Sqrt(_roundNum);
                    var thresh = Math.Sqrt(_SAll[i] / _imageSorting.ScoredImages.Length) / (_bAll[i] - _muAll[i]);
                    if (confIntWidth < thresh) numConverged++;
                }
            }

            if (_roundNum > 5 && numConverged == ((_window.SEG_FEATURES_ENABLED) ? 8 : 4))
            {
                // We've "converged", get outta there
                if (CloseCommand.CanExecute(0)) CloseCommand.Execute(0);
            }

            if (_roundNum == _imageSorting.ScoredImages.Length - 5)
            {
                // We're about to run out of images, ABORT!
                if (CloseCommand.CanExecute(0)) CloseCommand.Execute(0);
            }

//            double maxWeight = 0;
//            for (int i = 0; i < 8; i++)
//            {
//                maxWeight = Math.Max(maxWeight, Math.Abs(_imageSorting.GetWeightByIndex(i)));
//            }
//            for (int i = 0; i < 8; i++)
//            {
//                _imageSorting.SetWeightByIndex(i, _imageSorting.GetWeightByIndex(i) / maxWeight);
//            }
        }
    }
}
