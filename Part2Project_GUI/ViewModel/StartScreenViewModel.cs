using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;

namespace Part2Project_GUI.ViewModel
{
    class StartScreenViewModel : BaseViewModel
    {
        private string _saveFolderName;
        private DateTime _startTime, _endTime;

        #region Properties

        private ImageSorting _imageSorting;
        private ObservableCollection<BitmapImage> _images;
        public IEnumerable<BitmapImage> Images // This is the sorted set of thumbnail images to view.
        {
            get { return _images; }
            set
            {
                _images = value as ObservableCollection<BitmapImage>;
                OnPropertyChanged("Images");
            }
        }

        private Visibility _questionVisibility = Visibility.Collapsed;
        public Visibility QuestionVisibility
        {
            get { return _questionVisibility; }
            set
            {
                _questionVisibility = value;
                OnPropertyChanged("QuestionVisibility");
            }
        }

        private Visibility _instructionVisibility = Visibility.Visible;
        public Visibility InstructionsVisibility
        {
            get { return _instructionVisibility; }
            set
            {
                _instructionVisibility = value;
                OnPropertyChanged("InstructionsVisibility");
            }
        }

        private string _happiness;
        public string Happiness
        {
            get { return _happiness; }
            set
            {
                _happiness = value;
                OnPropertyChanged("Happiness");
            }
        }

        private ComboBoxItem _hapCI;
        public ComboBoxItem HapCI
        {
            get { return _hapCI; }
            set
            {
                _hapCI = value;
                Happiness = (string)value.Content;
                OnPropertyChanged("HapCI");
            }
        }

        #endregion

        public StartScreenViewModel(MainWindowViewModel w, BaseViewModel p) : base(w, p)
        {
            _imageSorting = new ImageSorting();
        }

        #region Commands

        private RelayCommand _startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(x => StartCommandFunction(), x => QuestionVisibility == Visibility.Collapsed);
                }
                return _startCommand;
            }
        }
        private void StartCommandFunction()
        {
            // Let the user choose a folder to sort
            if (!(_saveFolderName = _imageSorting.SelectFolder()).Equals(""))
            {
                QuestionVisibility = Visibility.Visible;
                InstructionsVisibility = Visibility.Collapsed;
                if ((Images = _imageSorting.SortViewableImagesFromScoredImages()) != null)
                {
                    // Create a new viewmodel for the next page, and tell it I'm it's parent
                    var newVM = new FourImageSelectorViewModel(_window, this, _imageSorting);
                    // We want to sort the viewable images when it's done selecting parameters
                    newVM.RequestClose += delegate
                    {
                        Images = _imageSorting.SortViewableImagesFromScoredImages();
                        _endTime = DateTime.Now;
                    };
                    // then load the view
                    _window.ViewModel = newVM;

                    // and start the timer
                    _startTime = DateTime.Now;
                }
            }
        }

        private RelayCommand _stopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(x => StopCommandFunction(), x => Happiness != null);
                }
                return _stopCommand;
            }
        }
        private void StopCommandFunction()
        {
            // Record time taken
            TimeSpan timeTaken = (_endTime - _startTime);

            // Save answers and time in a text file
            string nl = Environment.NewLine;
            string output = "Segmentation " + (_window.SEG_FEATURES_ENABLED ? "Enabled" : "Disabled") + nl;
            output += Happiness + nl + timeTaken.TotalMilliseconds + nl;

            var sortedImages = _imageSorting.ScoredImages.ToList();
            sortedImages.Sort();
            sortedImages.Reverse();
            for (int i = 0; i < sortedImages.Count; i++)
            {
                output += sortedImages[i].Features.ImageFilename.Split('\\').Last().Split('.').First() + nl;
            }

            // Feature weights chosen
            output += _imageSorting.WBrightness + nl;
            output += _imageSorting.WIntensityContrast + nl;
            output += _imageSorting.WSaturation + nl;
            output += _imageSorting.WBlurriness + nl;
            output += _imageSorting.WRegionsOfInterestSize + nl;
            output += _imageSorting.WRuleOfThirds + nl;
            output += _imageSorting.WShapeConvexity + nl;
            output += _imageSorting.WBackgroundDistraction + nl;

            File.WriteAllText(_saveFolderName + "\\Image_Sorting_Stage2_Results.txt", output);

            // Then terminate
            CloseCommand.Execute(0);
        }

        #endregion
    }
}
