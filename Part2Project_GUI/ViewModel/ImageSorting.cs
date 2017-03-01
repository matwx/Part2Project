using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class ImageSorting
    {
        private const bool DISPLAY_FEATURES = false;

        public ScoredBitmapImage[] ScoredImages { get; private set; }

        public double WBrightness,
            WIntensityContrast,
            WSaturation,
            WBlurriness,
            WRegionsOfInterestSize,
            WRuleOfThirds,
            WShapeConvexity,
            WBackgroundDistraction;

        public void SetWeightByIndex(int i, double value)
        {
            switch (i)
            {
                case 0:
                    WBrightness = value;
                    break;
                case 1:
                    WIntensityContrast = value;
                    break;
                case 2:
                    WSaturation = value;
                    break;
                case 3:
                    WBlurriness = value;
                    break;
                case 4:
                    WRegionsOfInterestSize = value;
                    break;
                case 5:
                    WRuleOfThirds = value;
                    break;
                case 6:
                    WShapeConvexity = value;
                    break;
                case 7:
                    WBackgroundDistraction = value;
                    break;
            }
        }
        public double GetWeightByIndex(int i)
        {
            switch (i)
            {
                case 0:
                    return WBrightness;
                case 1:
                    return WIntensityContrast;
                case 2:
                    return WSaturation;
                case 3:
                    return WBlurriness;
                case 4:
                    return WRegionsOfInterestSize;
                case 5:
                    return WRuleOfThirds;
                case 6:
                    return WShapeConvexity;
                case 7:
                    return WBackgroundDistraction;
            }
            return 0;
        }

        public void SelectFolder()
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
                    ScoredImages = new ScoredBitmapImage[filenames.Count];
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

                                ScoredImages[i] = new ScoredBitmapImage(DirectBitmapToBitmapImage(image), allFeatures[i]);
                            }
                        }
                    }

                    UpdateImageScores();
                    SortViewableImagesFromScoredImages();
                }
            }
        }

        public ObservableCollection<BitmapImage> SortViewableImagesFromScoredImages()
        {
            if (ScoredImages == null) return null;

            UpdateImageScores();

            ObservableCollection<BitmapImage> newImages = new ObservableCollection<BitmapImage>();

            List<ScoredBitmapImage> sortedImages = ScoredImages.ToList();
            sortedImages.Sort();
            sortedImages.Reverse();
            foreach (ScoredBitmapImage scoredImage in sortedImages)
            {
                newImages.Add(scoredImage.Image);
            }

            return newImages;
        }
        private void UpdateImageScores()
        {
            if (ScoredImages == null) return;
            foreach (var image in ScoredImages)
            {
                image.Score = 0.0;
                image.Score += WBrightness * image.Features.Brightness;
                image.Score += WSaturation * image.Features.Saturation;
                image.Score += WIntensityContrast * image.Features.IntensityContrast;
                image.Score += WBlurriness * image.Features.Blurriness;
                image.Score += WRegionsOfInterestSize * image.Features.RegionsOfInterestSize;
                image.Score += WRuleOfThirds * image.Features.RuleOfThirds;
                image.Score += WShapeConvexity * image.Features.ShapeConvexity;
                image.Score += WBackgroundDistraction * image.Features.BackgroundDistraction;
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
            return Score.CompareTo(((ScoredBitmapImage)obj).Score);
        }
    }
}
