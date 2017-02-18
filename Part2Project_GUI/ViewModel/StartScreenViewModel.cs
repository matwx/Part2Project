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
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;

namespace Part2Project_GUI.ViewModel
{
    class StartScreenViewModel : BaseViewModel
    {
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

        #endregion

        public StartScreenViewModel(MainWindowViewModel w, BaseViewModel p) : base(w, p)
        {
            _imageSorting = new ImageSorting();
        }

        #region Commands

        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand
        {
            get
            {
                if (_selectFolderCommand == null)
                {
                    _selectFolderCommand = new RelayCommand(x => { 
                        _imageSorting.SelectFolder();
                        Images = _imageSorting.SortViewableImagesFromScoredImages();
                    });
                }
                return _selectFolderCommand;
            }
        }

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

        #endregion
    }
}
