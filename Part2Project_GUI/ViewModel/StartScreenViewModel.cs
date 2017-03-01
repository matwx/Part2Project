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

        #endregion

        public StartScreenViewModel(MainWindowViewModel w, BaseViewModel p) : base(w, p)
        {
            _imageSorting = new ImageSorting();
        }

        #region Commands

        private RelayCommand _beginCommand;
        public RelayCommand BeginCommand
        {
            get
            {
                if (_beginCommand == null)
                {
                    _beginCommand = new RelayCommand(
                        x =>
                        {
                            // Create a new viewmodel for the next page, and tell it I'm it's parent
                            var newVM = new FourImageSelectorViewModel(_window, this, _imageSorting);
                            // We want to sort the viewable images when it's done selecting parameters
                            newVM.RequestClose += delegate
                            {
                                Images = _imageSorting.SortViewableImagesFromScoredImages();
                            };
                            // then load the view
                            _window.ViewModel = newVM;
                        }, 
                        x => Images != null);
                }
                return _beginCommand;
            }
        }

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
                    _stopCommand = new RelayCommand(x => StopCommandFunction(), x => Images != null);
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
            string output = LevelOfPhotography + nl + timeTaken.TotalMilliseconds + nl;
            foreach (var question in QuestionsToAnswer)
            {
                output += question.AnswerText + nl;
            }

            File.WriteAllText(_saveFolderName + "\\Image_Sorting_Results.txt", output);

            // Then terminate
            CloseCommand.Execute(0);
        }

        #endregion
    }
}
