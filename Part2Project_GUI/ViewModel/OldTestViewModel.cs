using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;
using SystemFonts = System.Windows.SystemFonts;

namespace Part2Project_GUI.ViewModel
{
    class OldTestViewModel : BaseViewModel
    {
        //TODO: Put folder loading and feature extraction into a background worker

        private const bool DISPLAY_FEATURES = true;

        private ScoredBitmapImage[] _scoredImages;

        #region Properties

        private ObservableCollection<BitmapImage> _images;

        public OldTestViewModel(MainWindowViewModel window, BaseViewModel parent) : base(window, parent)
        {
        }

        public IEnumerable<BitmapImage> Images // This is the sorted set of thumbnail images to view.
        {
            get { return _images; }
            set
            {
                _images = value as ObservableCollection<BitmapImage>;
                OnPropertyChanged("Images");
            }
        }

        private double _wBrightness;
        public double BrightnessWeight
        {
            get { return _wBrightness; }
            set
            {
                _wBrightness = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("BrightnessWeight");
            }
        }
        private double _wIntensityContrast;
        public double IntensityContrastWeight
        {
            get { return _wIntensityContrast; }
            set
            {
                _wIntensityContrast = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("IntensityContrastWeight");
            }
        }
        private double _wSaturation;
        public double SaturationWeight
        {
            get { return _wSaturation; }
            set
            {
                _wSaturation = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("SaturationWeight");
            }
        }
        private double _wBlurriness;
        public double BlurrinessWeight
        {
            get { return _wBlurriness; }
            set
            {
                _wBlurriness = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("BlurrinessWeight");
            }
        }
        private double _wRegionsOfInterestSize;
        public double RegionsOfInterestSizeWeight
        {
            get { return _wRegionsOfInterestSize; }
            set
            {
                _wRegionsOfInterestSize = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("RegionsOfInterestSizeWeight");
            }
        }
        private double _wRuleOfThirds;
        public double RuleOfThirdsWeight
        {
            get { return _wRuleOfThirds; }
            set
            {
                _wRuleOfThirds = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("RuleOfThirdsWeight");
            }
        }
        private double _wShapeConvexity;
        public double ShapeConvexityWeight
        {
            get { return _wShapeConvexity; }
            set
            {
                _wShapeConvexity = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("ShapeConvexityWeight");
            }
        }
        private double _wBackgroundDistraction;
        public double BackgroundDistractionWeight
        {
            get { return _wBackgroundDistraction; }
            set
            {
                _wBackgroundDistraction = value;
                UpdateImageScores();
                SortViewableImagesFromScoredImages();
                OnPropertyChanged("BackgroundDistractionWeight");
            }
        }


        private WindowState _windowState = WindowState.Normal;
        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                SetPicListWidthForWindowSize();
                OnPropertyChanged("WindowState");
            }
        }
        private int _windowWidth;
        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                _windowWidth = value;
                SetPicListWidthForWindowSize();
                OnPropertyChanged("WindowWidth");
            }
        }
        private int _picListWidth;
        public int PicListWidth
        {
            get { return _picListWidth; }
            set
            {
                _picListWidth = value;
                OnPropertyChanged("PicListWidth");
            }
        }
        private void SetPicListWidthForWindowSize()
        {
            int spaceLeftForPics = 0;
            if (_windowState == WindowState.Maximized)
            {
                var interopHelper = new WindowInteropHelper(System.Windows.Application.Current.MainWindow);
                var activeScreen = Screen.FromHandle(interopHelper.Handle);
                spaceLeftForPics = activeScreen.Bounds.Width - 475;
            }
            else
            {
                spaceLeftForPics = _windowWidth - 475;
            }

            if (spaceLeftForPics < 330) PicListWidth = 1;
            else PicListWidth = spaceLeftForPics / 330;
        }

        #endregion

        #region Commands

        private RelayCommand _testCommand;
        public RelayCommand TestCommand
        {
            get
            {
                if (_testCommand == null)
                {
                    _testCommand = new RelayCommand(x => TestCommandFunction());
                }
                return _testCommand;
            }
        }
        private void TestCommandFunction()
        {
//            BlurrinessWeight = -1;
        }

        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand
        {
            get
            {
                if (_selectFolderCommand == null)
                {
                    _selectFolderCommand = new RelayCommand(x => SelectFolder());
                }
                return _selectFolderCommand;
            }
        }
        private void SelectFolder()
        {
            // We need to let the user select a folder and, if they do, load all of the images from that
            // folder. We then need to set up our internal list of images bound with their feature values.
            // Finally, we need to update our ViewModel of images to be displayed after sorting the list.

            using (FolderBrowserDialog dlgFolder = new FolderBrowserDialog())
            {
                // Let the user choose a folder to sort
                dlgFolder.ShowDialog();
                if (dlgFolder.SelectedPath != "")
                {
                    // Initialise feature manager and extract all features
                    ImageDirectoryFeatures featureManager = new ImageDirectoryFeatures(dlgFolder.SelectedPath);
                    var allFeatures = featureManager.GetDirectoryFeatures();

                    // Load in images to display for the selected folder
                    var filenames = featureManager.ImageFilenames;
                    _scoredImages = new ScoredBitmapImage[filenames.Count];
                    for (int i = 0; i < filenames.Count; i++)
                    {
                        // Load thumbnail to view
                        using (Bitmap inBitmap = new Bitmap(filenames[i]))
                        {
                            using (
                                DirectBitmap image =
                                    new DirectBitmap(
                                        (int)((double)inBitmap.Width / (double)inBitmap.Height * 240.0), 240))
                            {
                                using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                                {
                                    gfx.DrawImage(inBitmap, 0, 0,
                                        (int)((double)inBitmap.Width / (double)inBitmap.Height * 240.0), 240);

                                    if (DISPLAY_FEATURES)
                                    {
                                        // Draw feature values over the bitmap
                                        gfx.FillRectangle(new SolidBrush(Color.FromArgb(120, 0, 0, 0)), 0, 0, 320, 240);
                                        float height =
                                            gfx.MeasureString(
                                                "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz",
                                                System.Drawing.SystemFonts.DefaultFont).Height;
                                        int count = 0;
                                        gfx.DrawString("Bright: " + Math.Round(allFeatures[i].Brightness, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Constrast: " + Math.Round(allFeatures[i].IntensityContrast, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Blurry: " + Math.Round(allFeatures[i].Blurriness, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Sat: " + Math.Round(allFeatures[i].Saturation, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("RoISize: " + Math.Round(allFeatures[i].RegionsOfInterestSize, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("RoT: " + Math.Round(allFeatures[i].RuleOfThirds, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Convex: " + Math.Round(allFeatures[i].ShapeConvexity, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("BackDistract: " + Math.Round(allFeatures[i].BackgroundDistraction, 5), System.Drawing.SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count);
                                    }
                                }

                                _scoredImages[i] = new ScoredBitmapImage(DirectBitmapToBitmapImage(image), allFeatures[i]);
                            }
                        }
                    }

                    UpdateImageScores();
                    SortViewableImagesFromScoredImages();
                }
            }
        }

        private RelayCommand _resetWeightCommand;
        public RelayCommand ResetWeightCommand
        {
            get
            {
                if (_resetWeightCommand == null)
                {
                    _resetWeightCommand = new RelayCommand(ResetWeight, IsWeightZero);
                }
                return _resetWeightCommand;
            }
        }
        private bool IsWeightZero(Object weightNumber)
        {
            if (weightNumber is string)
            {
                try
                {
                    weightNumber = int.Parse((string)weightNumber);
                }
                catch (FormatException e)
                {
                    throw new Exception("IsWeightZero was called with a non-integer parameter.");
                }
                
            }
            else throw new Exception("IsWeightZero was called with an invalid parameter.");

            switch ((int) weightNumber)
            {
                case 0:
                    return BrightnessWeight != 0;
                case 1:
                    return IntensityContrastWeight != 0;
                case 2:
                    return SaturationWeight != 0;
                case 3:
                    return BlurrinessWeight != 0;
                case 4:
                    return RegionsOfInterestSizeWeight != 0;
                case 5:
                    return RuleOfThirdsWeight != 0;
                case 6:
                    return ShapeConvexityWeight != 0;
                case 7:
                    return BackgroundDistractionWeight != 0;
            }

            return false;
        }
        private void ResetWeight(Object weightNumber)
        {
            if (weightNumber is string)
            {
                try
                {
                    weightNumber = int.Parse((string)weightNumber);
                }
                catch (FormatException e)
                {
                    throw new Exception("IsWeightZero was called with a non-integer parameter.");
                }

            }
            else throw new Exception("IsWeightZero was called with an invalid parameter.");

            switch ((int) weightNumber)
            {
                case 0:
                    BrightnessWeight = 0;
                    return;
                case 1:
                    IntensityContrastWeight = 0;
                    return;
                case 2:
                    SaturationWeight = 0;
                    return;
                case 3:
                    BlurrinessWeight = 0;
                    return;
                case 4:
                    RegionsOfInterestSizeWeight = 0;
                    return;
                case 5:
                    RuleOfThirdsWeight = 0;
                    return;
                case 6:
                    ShapeConvexityWeight = 0;
                    return;
                case 7:
                    BackgroundDistractionWeight = 0;
                    return;
            }
        }

        #endregion

        
        private void SortViewableImagesFromScoredImages()
        {
            if (_scoredImages == null) return;

            ObservableCollection<BitmapImage> newImages = new ObservableCollection<BitmapImage>();

            List<ScoredBitmapImage> sortedImages = _scoredImages.ToList();
            sortedImages.Sort();
            sortedImages.Reverse();
            foreach (ScoredBitmapImage scoredImage in sortedImages)
            {
                newImages.Add(scoredImage.Image);
            }

            Images = newImages;
        }
        private void UpdateImageScores()
        {
            if (_scoredImages == null) return;
            foreach (var image in _scoredImages)
            {
                image.Score = 0.0;
                image.Score += _wBrightness * image.Features.Brightness;
                image.Score += _wSaturation * image.Features.Saturation;
                image.Score += _wIntensityContrast * image.Features.IntensityContrast;
                image.Score += _wBlurriness * image.Features.Blurriness;
                image.Score += _wRegionsOfInterestSize * image.Features.RegionsOfInterestSize;
                image.Score += _wRuleOfThirds * image.Features.RuleOfThirds;
                image.Score += _wShapeConvexity * image.Features.ShapeConvexity;
                image.Score += _wBackgroundDistraction * image.Features.BackgroundDistraction;
            }
        }

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
