using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Part2Project_GUI.ViewModel
{
    class FourImageSelectorViewModel : BaseViewModel
    {
        private ImageSorting _imageSorting;
        private int _round = 0; // The number of rounds completed
        private int _imageIndex1, _imageIndex2, _imageIndex3, _imageIndex4;

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

            // Do something with our chosen image
            CloseCommand.Execute(0);
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

            UpdateImages();
        }

        private void UpdateImages()
        {
            Random rand = new Random();

            // Select the four images. Make sure that they're not the same
            _imageIndex1 = rand.Next(0, _imageSorting.ScoredImages.Length);
            _imageIndex2 = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndex2 == _imageIndex1)
            {
                _imageIndex2 = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndex3 = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndex3 == _imageIndex2 || _imageIndex3 == _imageIndex1)
            {
                _imageIndex3 = rand.Next(0, _imageSorting.ScoredImages.Length);
            }
            _imageIndex4 = rand.Next(0, _imageSorting.ScoredImages.Length);
            while (_imageIndex4 == _imageIndex3 || _imageIndex4 == _imageIndex2 || _imageIndex4 == _imageIndex1)
            {
                _imageIndex4 = rand.Next(0, _imageSorting.ScoredImages.Length);
            }

            Image1 = _imageSorting.ScoredImages[_imageIndex1].Image;
            Image2 = _imageSorting.ScoredImages[_imageIndex2].Image;
            Image3 = _imageSorting.ScoredImages[_imageIndex3].Image;
            Image4 = _imageSorting.ScoredImages[_imageIndex4].Image;
        }
    }
}
