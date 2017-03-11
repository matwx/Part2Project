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
            
            for (int dataSetNum = 0; dataSetNum < 4; dataSetNum++)
            {
                // Create an output directory for Dataset i
                var currInDir = p + "\\Dataset " + (dataSetNum + 1);
                var currOutDir = sp + "\\Dataset" + (dataSetNum + 1);
                if (!Directory.Exists(currOutDir)) Directory.CreateDirectory(currOutDir);
                
                // Compute features for all images
                var featureComputer = new ImageDirectoryFeatures(currInDir);
                var featuresList = featureComputer.GetDirectoryFeatures();

                // Sort images by each feature, and save the ordering in a text file
                // (one file for each feature)
                sortedFeatureLists[dataSetNum] = new string[8][];
                for (int featureNum = 0; featureNum < 8; featureNum++)
                {
                    sortedFeatureLists[dataSetNum][featureNum] = new string[40];
                    List<Pair> jthFeatureSortingList = new List<Pair>();

                    // Construct the list to sort based on the jth feature
                    foreach (var imageFeatureList in featuresList)
                    {
                        Pair newPair = new Pair();
                        newPair.filename = imageFeatureList.ImageFilename;
                        newPair.score = imageFeatureList._features[featureNum];
                        jthFeatureSortingList.Add(newPair);
                    }

                    // Sort the list
                    jthFeatureSortingList.Sort();
                    jthFeatureSortingList.Reverse();

                    // Save the ordering in a file
                    string output = "";
                    string nl = Environment.NewLine;
                    for (var imageNum = 0; imageNum < jthFeatureSortingList.Count; imageNum++)
                    {
                        var pair = jthFeatureSortingList[imageNum];
                        sortedFeatureLists[dataSetNum][featureNum][imageNum] = pair.filename.Split('\\').Last().Split('.').First();
                        output += pair.filename.Split('\\').Last().Split('.').First() + nl;
                    }

                    File.WriteAllText(currOutDir + "\\" + featureNames[featureNum] + ".txt", output);
                }

                // Now we want to compute the Spearman's Rank Correlation Coefficient for all of the pairs of features
                currOutDir += "\\Spearmans";
                string toFile = "";
                if (!Directory.Exists(currOutDir)) Directory.CreateDirectory(currOutDir);
                for (int i = 0; i < 8; i++)
                {
                    for (int j = i + 1; j < 8; j++)
                    {
                        // We want to compute the correlation for the feature pair (i, j)
                        int sum_d_squared = 0;
                        for (int r = 0; r < 40; r++)
                        {
                            // The iRank is 'r'
                            string searchFor = sortedFeatureLists[dataSetNum][j][r];
                            // Search for 'searchFor' in the sorted feature list for feature i
                            int jRank = Array.IndexOf(sortedFeatureLists[dataSetNum][i], searchFor);
                            int d = jRank - r;
                            sum_d_squared += d * d;
                        }

                        double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));

                        toFile += featureNames[i] + " and " + featureNames[j] + "," + rho + Environment.NewLine;
                    }
                }

                File.WriteAllText(currOutDir + "\\correlations.txt", toFile);
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
