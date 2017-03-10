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

        private void btnDoTest_Click(object sender, EventArgs e)
        {
            dlgFolder.ShowDialog();
            if (dlgFolder.SelectedPath == "") return;
            var p = dlgFolder.SelectedPath;

            // Load in all the filenames
            for (int i = 1; i <= 4; i++)
            {
                if (!Directory.Exists(p + "\\Dataset " + i)) return;
            }

            // Create output directories
            if (!Directory.Exists(p + "\\Sortings")) Directory.CreateDirectory(p + "\\Sortings");
            var sp = p + "\\Sortings";

            string[][][] sortedFeatureLists = new string[4][][];
            
            for (int dataSetNum = 1; dataSetNum <= 4; dataSetNum++)
            {
                // Create an output directory for Dataset i
                var currInDir = p + "\\Dataset " + dataSetNum;
                var currOutDir = sp + "\\Dataset" + dataSetNum;
                if (!Directory.Exists(currOutDir)) Directory.CreateDirectory(currOutDir);
                
                // Compute features for all images
                var featureComputer = new ImageDirectoryFeatures(currInDir);
                var featuresList = featureComputer.GetDirectoryFeatures();

                // Sort images by each feature, and save the ordering in a text file
                // (one file for each feature)
                sortedFeatureLists[dataSetNum]
                for (int j = 0; j < 8; j++)
                {
                    List<Pair> jthFeatureSortingList = new List<Pair>();

                    // Construct the list to sort based on the jth feature
                    foreach (var imageFeatureList in featuresList)
                    {
                        Pair newPair = new Pair();
                        newPair.filename = imageFeatureList.ImageFilename;
                        newPair.score = imageFeatureList._features[j];
                        jthFeatureSortingList.Add(newPair);
                    }

                    // Sort the list
                    jthFeatureSortingList.Sort();
                    jthFeatureSortingList.Reverse();

                    // Save the ordering in a file
                    string output = "";
                    string nl = Environment.NewLine;
                    foreach (var pair in jthFeatureSortingList)
                    {
                        output += pair.filename.Split('\\').Last().Split('.').First() + nl;
                    }

                    File.WriteAllText(currOutDir + "\\" + featureNames[j] + ".txt", output);
                }

                // Now we want to compute the Spearman's Rank Correlation Coefficient for all of the 
            }
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
