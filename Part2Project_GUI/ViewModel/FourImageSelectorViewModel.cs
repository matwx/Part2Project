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
        }

        private void UpdateImages()
        {
            
        }
    }
}
