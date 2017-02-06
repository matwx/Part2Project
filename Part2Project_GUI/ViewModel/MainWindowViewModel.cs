using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;

namespace Part2Project_GUI.ViewModel
{
    

    class MainWindowViewModel : ObservableObject
    {
        private const bool DISPLAY_FEATURES = true;

        private ScoredBitmapImage[] _scoredImages;
        
        #region Properties

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

        #endregion

        public event EventHandler RequestClose;

        #region Commands

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(x => RequestClose(this, EventArgs.Empty));
                }
                return _closeCommand;
            }
        }

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
                                                SystemFonts.DefaultFont).Height;
                                        int count = 0;
                                        gfx.DrawString("Bright: " + Math.Round(allFeatures[i].Brightness, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Constrast: " + Math.Round(allFeatures[i].IntensityContrast, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Blurry: " + Math.Round(allFeatures[i].Blurriness, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Sat: " + Math.Round(allFeatures[i].Saturation, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("RoISize: " + Math.Round(allFeatures[i].RegionsOfInterestSize, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("RoT: " + Math.Round(allFeatures[i].RuleOfThirds, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("Convex: " + Math.Round(allFeatures[i].ShapeConvexity, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count); count++;
                                        gfx.DrawString("BackDistract: " + Math.Round(allFeatures[i].BackgroundDistraction, 5), SystemFonts.DefaultFont, Brushes.White, 5, 5 + (height + 2) * count);
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

    public class ScoredBitmapImage : IComparable
    {
        public BitmapImage Image { get; private set; }
        public ImageFeatureList Features { get; private set; }
        public double Score { get; set; }

        public ScoredBitmapImage(BitmapImage image, ImageFeatureList features, double score = 0.0)
        {
            Image = image;
            Features = features;
            Score = score;
        }

        public int CompareTo(object obj)
        {
            return Score.CompareTo(((ScoredBitmapImage) obj).Score);
        }
    }
}