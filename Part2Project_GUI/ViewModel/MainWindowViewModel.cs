using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;

namespace Part2Project_GUI.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {

        #region Properties

        private ObservableCollection<BitmapImage> _images;
        public IEnumerable<BitmapImage> Images
        {
            get { return _images; }
            set
            {
                _images = value as ObservableCollection<BitmapImage>;
                OnPropertyChanged("Images");
            }
        }

        #endregion

        public event EventHandler RequestClose;

        #region Commands

        private RelayCommand _closeCommand; 
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null) { _closeCommand = new RelayCommand(x => RequestClose(this, EventArgs.Empty)); }
                return _closeCommand;
            }
        }

        private RelayCommand _testCommand;
        public RelayCommand TestCommand
        {
            get
            {
                if (_testCommand == null) { _testCommand = new RelayCommand(x => TestCommandFunction()); }
                return _testCommand;
            }
        }
        private void TestCommandFunction()
        {
            
        }

        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand
        {
            get
            {
                if (_selectFolderCommand == null) { _selectFolderCommand = new RelayCommand(x => SelectFolder()); }
                return _selectFolderCommand;
            }
        }
        private void SelectFolder()
        {
            using (FolderBrowserDialog dlgFolder = new FolderBrowserDialog())
            {
                // Let the user choose a folder to sort
                dlgFolder.ShowDialog();
                if (dlgFolder.SelectedPath != "")
                {
                    // Initialise feature manager
                    ImageDirectoryFeatures featureManager = new ImageDirectoryFeatures(dlgFolder.SelectedPath);

                    // Load in images to display for the selected folder
                    var loadedImages = new ObservableCollection<BitmapImage>();
                    var filenames = featureManager.ImageFilenames;
                    foreach (var filename in filenames)
                    {
                        loadedImages.Add(new BitmapImage(new Uri(filename)));
                    }

                    Images = loadedImages;
                }
            }
        }

        #endregion
        
        private BitmapImage DirectBitmapToBitmapImage(DirectBitmap dBMP)
        {
            MemoryStream ms = new MemoryStream();
            dBMP.Bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
        
            return bi;
        }
    }
}
