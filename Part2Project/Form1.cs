using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Part2Project.Features;
using Part2Project.ImageSegmentation;
using Part2Project.Infrastructure;

namespace Part2Project
{
    public partial class Form1 : Form
    {
        private DirectBitmap _image, _trueEdges;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Feature order:
        // 1. Brightness (L)
        // 2. Intensity Contrast (L)
        // 3. Saturation (L)
        // 4. Rule Of Thirds (H)
        // 5. RegionsOfInterestSize (Bounding box area) (H)
        // 6. Blurriness (L)
        // 7. Background Distraction (H)
        // 8. Shape Convexity (H)
        public string[] featureNames =
        {
            "Brightness",
            "Intensity_Contrast",
            "Saturation",
            "RoT",
            "RoI_Size",
            "Blurriness",
            "Background_Distraction",
            "Shape_Convexity"
        };

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            dlgFile.ShowDialog();
        }

        private ImageFeatureList result;
        private SaliencySegmentation ss;
        private DirectBitmap image;

        private void dlgFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string filename = dlgFile.FileName;

            result = new ImageFeatureList(filename);

            // If they can't be retrieved from Exif, we need to compute them
            // First read the image from file as a bitmap
            using (Image selected = Image.FromFile(filename))
            {
                image = new DirectBitmap(
                    (int) ((double) selected.Width / (double) selected.Height * 240.0), 240);
                // First compute blurriness
                using (DirectBitmap image512X512 = new DirectBitmap(512, 512))
                {
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(image512X512.Bitmap))
                    {
                        int originalWidth = (int)((double)selected.Width / (double)selected.Height * 512.0);
                        gfx.DrawImage(selected, 256 - originalWidth / 2, 0, originalWidth, 512);
                    }

                    // Then compute the features and store the results
                    // Low-level
//                    result.Blurriness = FeatureBlurriness.ComputeFeature(image512X512); // *** FEATURE ***
                }

                // Create the required resized image
                using (Graphics gfx = Graphics.FromImage(image.Bitmap))
                {
                    gfx.DrawImage(selected, 0, 0, (int)((double)selected.Width / (double)selected.Height * 240.0), 240);
                }

                // Then compute the features and store the results
                // Low-level
//                result.Brightness = FeatureBrightness.ComputeFeature(image); // *** FEATURE ***
//                result.IntensityContrast = FeatureIntensityContrast.ComputeFeature(image); // *** FEATURE ***
//                result.Saturation = FeatureSaturation.ComputeFeature(image); // *** FEATURE ***

                // Segmentation-Derived
                const int k = 125;
                const double sigma = 0.6;
//                Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
//                ss = new SaliencySegmentation(s, image, sigma);
                bool[][] boundedBinarySaliencyMap = new bool[image.Width][];
                for (int x = 0; x < image.Width; x++)
                {
                    boundedBinarySaliencyMap[x] = new bool[image.Height];
                }
//                result.RegionsOfInterestSize = FeatureRegionsOfInterestSize.ComputeFeature(ss, boundedBinarySaliencyMap); // Updates map for f_BD // *** FEATURE ***
//                result.BackgroundDistraction = FeatureBackgroundDistraction.ComputeFeature(image, boundedBinarySaliencyMap); // Uses map // *** FEATURE ***
            }

//            result.RuleOfThirds = FeatureRuleOfThirds.ComputeFeature(ss); // *** FEATURE ***
//            result.ShapeConvexity = FeatureShapeConvexity.ComputeFeature(ss); // *** FEATURE ***

            viewer1.Image = image.Bitmap;
        }

        private void btnBackDistractHist_Click(object sender, EventArgs e)
        {
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
            ss = new SaliencySegmentation(s, image, sigma);
            bool[][] boundedBinarySaliencyMap = new bool[image.Width][];
            for (int x = 0; x < image.Width; x++)
            {
                boundedBinarySaliencyMap[x] = new bool[image.Height];
            }
            FeatureRegionsOfInterestSize.ComputeFeature(ss, boundedBinarySaliencyMap); // Updates map for f_BD // *** FEATURE ***
            viewer2.Image = FeatureBackgroundDistraction.GetHistogram(image, boundedBinarySaliencyMap).Bitmap; // Uses map // *** FEATURE ***
        }
    }

    public class Pair : IComparable
    {
        public double score;
        public string filename;

        public int CompareTo(object obj)
        {
            return score.CompareTo(((Pair) obj).score);
        }
    }
}
