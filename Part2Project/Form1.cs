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
        private List<UserRecord> _records;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnReadRecords.PerformClick();
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

        private void btnReadRecords_Click(object sender, EventArgs e)
        {
//            dlgFolder.ShowDialog();
//            if (dlgFolder.SelectedPath == "") return;
            dlgFolder.SelectedPath =
                @"D:\Users\Matt\Dropbox\1 - University\1 - Work\Part II\Part II Project\Evaluation\Final User Result files";

            // Store the record filenames
            var filenames = Directory.GetFiles(dlgFolder.SelectedPath);
            List<string> recordFilenames = new List<string>();
            foreach (var filename in filenames)
            {
                if (filename.Split('\\').Last().Split('.').Last() == "txt")
                    recordFilenames.Add(filename);
            }
            if (recordFilenames.Count == 0) return;

            // Load the records into memory
            _records = new List<UserRecord>();
            foreach (var recordFilename in recordFilenames)
            {
                var newRecord = new UserRecord(recordFilename);
                _records.Add(newRecord);
            }
        }

        private string ManualCorrelation(int datasetNum)
        {
            double totalRho = 0, numPairs = 0;
            string result = "";

            // Compute spearmans for each pair of records in dataset
            for (int i = 0; i < _records.Count; i++)
            {
                for (int j = i; j < _records.Count; j++)
                {
                    // If we have a proper pair of records from dataset
                    if (i != j && _records[i].DatasetNum == datasetNum && _records[j].DatasetNum == datasetNum)
                    {
                        // We want to compute the correlation for the feature pair (i, j)
                        int sum_d_squared = 0;
                        for (int r = 0; r < 40; r++)
                        {
                            // The iRank is 'r'
                            string searchFor = _records[j].ManualSorting[r];
                            // Search for 'searchFor' in the sorted feature list for feature i
                            int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                            int d = jRank - r;
                            sum_d_squared += d * d;
                        }

                        double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                        totalRho += rho;
                        numPairs++;

                        result += "Pair " + numPairs + ": " + rho + Environment.NewLine;
                    }
                }
            }

            result += Environment.NewLine + "average rho: " + (totalRho / numPairs);
            return result;
        }
        private void btnAvManCorrel1_Click(object sender, EventArgs e)
        {
            txt.Text = "";
            double totalTotalRho = 0, totalTotalNum = 0;

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                double totalRho = 0, numPairs = 0;

                //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                // Compute spearmans for each pair of records in dataset
                for (int i = 0; i < _records.Count; i++)
                {
                    for (int j = i; j < _records.Count; j++)
                    {
                        // If we have a proper pair of records from dataset
                        if (i != j && _records[i].DatasetNum == datasetNum && _records[j].DatasetNum == datasetNum)
                        {
                            // We want to compute the correlation for the feature pair (i, j)
                            int sum_d_squared = 0;
                            for (int r = 0; r < 40; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[j].ManualSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                int d = jRank - r;
                                sum_d_squared += d * d;
                            }

                            double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
                        }
                    }
                }

                //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                txt.Text += Environment.NewLine;

                totalTotalRho += totalRho;
                totalTotalNum += numPairs;
            }

            txt.Text += Environment.NewLine + "average rho: " + (totalTotalRho / totalTotalNum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0, numPairs = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the correlation for the intuitive against manual
                            int sum_d_squared = 0;
                            for (int r = 0; r < 40; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].IntuitiveSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                int d = jRank - r;
                                sum_d_squared += d * d;
                            }

                            double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
//                            txt.Text += _records[i].Name + " intuitiveCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0, numPairs = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the correlation for the intuitive against manual
                            int sum_d_squared = 0;
                            for (int r = 0; r < 40; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].EfficientSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                int d = jRank - r;
                                sum_d_squared += d * d;
                            }

                            double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
//                            txt.Text += _records[i].Name + " efficientCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the fraction of intuitive top-10 that are in manual top-10
                            double numinBothTopTen = 0;
                            for (int r = 0; r < 10; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].IntuitiveSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                if (jRank < 10)
                                {
                                    // It is in that bottom-left square of the scatterGraph
                                    numinBothTopTen++;
                                }
                            }

                            double rho = numinBothTopTen / 10.0;
                            totalRho += rho;

                            txt.Text += rho + Environment.NewLine;
//                              txt.Text += _records[i].Name + " intCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the fraction of efficient top-10 that are in manual top-10
                            double numinBothTopTen = 0;
                            for (int r = 0; r < 10; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].EfficientSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                if (jRank < 10)
                                {
                                    // It is in that bottom-left square of the scatterGraph
                                    numinBothTopTen++;
                                }
                            }

                            double rho = numinBothTopTen / 10.0;
                            totalRho += rho;

                            txt.Text += rho + Environment.NewLine;
//                            txt.Text += _records[i].Name + " effCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txt.Text = "";
            double totalTotalRho = 0, totalTotalNum = 0;

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                double totalRho = 0, numPairs = 0;

                //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                // Compute spearmans for each pair of records in dataset
                for (int i = 0; i < _records.Count; i++)
                {
                    for (int j = i; j < _records.Count; j++)
                    {
                        // If we have a proper pair of records from dataset
                        if (i != j && _records[i].DatasetNum == datasetNum && _records[j].DatasetNum == datasetNum)
                        {
                            // We want to compute the correlation for the feature pair (i, j)
                            double numinBothTopTen = 0;
                            for (int r = 0; r < 10; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[j].ManualSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                if (jRank < 10)
                                {
                                    // It is in that bottom-left square of the scatterGraph
                                    numinBothTopTen++;
                                }
                            }

                            double rho = numinBothTopTen / 10.0;
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
                        }
                    }
                }

                //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                txt.Text += Environment.NewLine;

                totalTotalRho += totalRho;
                totalTotalNum += numPairs;
            }

            txt.Text += Environment.NewLine + "average rho: " + (totalTotalRho / totalTotalNum);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txt.Text = "";
            Random rand = new Random();

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the fraction of efficient top-10 that are in manual top-10
                            double numinBothTopTen = 0;
                            for (int j = 0; j < 100000; j++)
                            {
                                List<int> chosenRanks = new List<int>();
                                for (int r = 0; r < 10; r++)
                                {
                                    // The iRank is random
                                    int iRank = rand.Next(0, 40);
                                    while (chosenRanks.Contains(iRank))
                                    {
                                        iRank = rand.Next(0, 40);
                                    }
                                    chosenRanks.Add(iRank);
                                    if (iRank < 10)
                                    {
                                        // It is in that bottom-left square of the scatterGraph
                                        numinBothTopTen++;
                                    }
                                }
                            }

                            double rho = numinBothTopTen / 1000000.0;
                            totalRho += rho;

                            txt.Text += rho + Environment.NewLine;
                            //                            txt.Text += _records[i].Name + " effCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0, numPairs = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1))
                        {
                            // We want to compute the correlation for the alphabetical against manual
                            int sum_d_squared = 0;
                            for (int r = 0; r < 40; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].OriginalSorting[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                int d = jRank - r;
                                sum_d_squared += d * d;
                            }

                            double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
                            //                            txt.Text += _records[i].Name + " intuitiveCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1) && _records[i].ManualSorting2[0].Length > 0)
                        {
                            // We want to compute the fraction of efficient top-10 that are in manual top-10
                            double numinBothTopTen = 0;
                            for (int r = 0; r < 10; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].ManualSorting2[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                if (jRank < 10)
                                {
                                    // It is in that bottom-left square of the scatterGraph
                                    numinBothTopTen++;
                                }
                            }

                            double rho = numinBothTopTen / 10.0;
                            totalRho += rho;

                            txt.Text += rho + Environment.NewLine;
                            //                            txt.Text += _records[i].Name + " effCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txt.Text = "";

            for (int datasetNum = 1; datasetNum <= 4; datasetNum++)
            {
                for (int seg = 1; seg >= 0; seg--)
                {
                    double totalRho = 0, numPairs = 0;

                    //txt.Text += "Dataset " + datasetNum + Environment.NewLine + Environment.NewLine;

                    // Compute spearmans for each record in dataset
                    for (int i = 0; i < _records.Count; i++)
                    {
                        // If we have a record from dataset with the correct seg bool
                        if (_records[i].DatasetNum == datasetNum && _records[i].SegFeaturesEnabled == (seg == 1) && _records[i].ManualSorting2[0].Length > 0)
                        {
                            // We want to compute the correlation for the intuitive against manual
                            int sum_d_squared = 0;
                            for (int r = 0; r < 40; r++)
                            {
                                // The iRank is 'r'
                                string searchFor = _records[i].ManualSorting2[r];
                                // Search for 'searchFor' in the sorted feature list for feature i
                                int jRank = Array.IndexOf(_records[i].ManualSorting, searchFor);
                                int d = jRank - r;
                                sum_d_squared += d * d;
                            }

                            double rho = 1.0 - 6.0 * sum_d_squared / (40.0 * (40.0 * 40.0 - 1.0));
                            totalRho += rho;
                            numPairs++;

                            txt.Text += rho + Environment.NewLine;
                            //                            txt.Text += _records[i].Name + " efficientCorrel: " + rho + Environment.NewLine;
                        }
                    }

                    //txt.Text += Environment.NewLine + "average rho for dataset " + datasetNum + ": " + (totalRho / numPairs);
                    //txt.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    txt.Text += Environment.NewLine;
                }
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

    public class UserRecord
    {
       
        #region Properties

        public enum LoP
        {
            AlmostNever,
            Occasionally,
            Hobby,
            Professional
        }
        public enum Happiness
        {
            AllWrong,
            MostlyWrong,
            SlightlyWrong,
            Random,
            SlightlyCorrect,
            MostlyCorrect,
            AllCorrect
        }
        public enum Method
        {
            Manual,
            PictureBased,
            SliderBased
        }

        public string Name { get; private set; }
        public int DatasetNum { get; private set; }
        public bool SegFeaturesEnabled { get; private set; }
        public string[] DatasetMapping { get; private set; }
        public string[] OriginalSorting { get; private set; }
        public string[] ManualSorting { get; private set; }
        public string[] ManualSorting2 { get; private set; }
        public string[] IntuitiveSorting { get; private set; }
        public string[] EfficientSorting { get; private set; }
        public LoP LevelOfPhotography { get; private set; }
        public double ManualSortingTime { get; private set; }
        public double IntuitiveSortingTime { get; private set; }
        public double EfficientSortingTime { get; private set; }
        public Happiness IntuitiveHappiness { get; private set; }
        public Happiness EfficientHappiness { get; private set; }
        public double[] IntuitiveWeights { get; private set; }
        public double[] EfficientWeights { get; private set; }
        public int NumIntuitiveIterations { get; private set; }
        public Method FavouriteMethod { get; private set; }

        #endregion

        private void NormaliseRankings()
        {
            string[] canonicalNames;

            // Rename all of the rankings so that they are in terms of the canonical dataset names
            if (DatasetNum == 2)
            {
                // The dataset is numbered differently
                canonicalNames = new[]
                {
                    "Birds_1", "Birds_2", "Birds_3", "Birds_4", "Birds_5", "Birds_6", "Birds_7", "Birds_8", "Birds_9", "Birds_10",
                    "Butterflies_1", "Butterflies_2", "Butterflies_3", "Butterflies_4", "Butterflies_5", 
                    "Butterflies_6", "Butterflies_7", "Butterflies_8", "Butterflies_9", "Butterflies_10",
                    "Goats_1", "Goats_2", "Goats_3", "Goats_4", "Goats_5", "Goats_6", "Goats_7", "Goats_8", "Goats_9", "Goats_10",
                    "Reindeer_1", "Reindeer_2", "Reindeer_3", "Reindeer_4", "Reindeer_5", "Reindeer_6", "Reindeer_7", "Reindeer_8", "Reindeer_9", "Reindeer_10"
                };
            }
            else
            {
                // The dataset is numbered "01" to "40"
                canonicalNames = new []
                {
                    "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                    "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                    "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                    "31", "32", "33", "34", "35", "36", "37", "38", "39", "40"
                };
            }

            for (int i = 0; i < 40; i++)
            {
                OriginalSorting[i] = canonicalNames[Array.IndexOf(DatasetMapping, OriginalSorting[i])];
                ManualSorting[i] = canonicalNames[Array.IndexOf(DatasetMapping, ManualSorting[i])];
                if (ManualSorting2[0].Length > 0) ManualSorting2[i] = canonicalNames[Array.IndexOf(DatasetMapping, ManualSorting2[i])];
                IntuitiveSorting[i] = canonicalNames[Array.IndexOf(DatasetMapping, IntuitiveSorting[i])];
                EfficientSorting[i] = canonicalNames[Array.IndexOf(DatasetMapping, EfficientSorting[i])];
            }
        }

        public UserRecord(string filename)
        {
            // Open the file and save all of the data
            StreamReader file = new StreamReader(filename);

            file.ReadLine();
            Name = file.ReadLine();
            file.ReadLine();
            DatasetNum = Convert.ToInt32(file.ReadLine());
            SegFeaturesEnabled = file.ReadLine().Equals("Yes");
            file.ReadLine();
            file.ReadLine();
            file.ReadLine();
            file.ReadLine();
            file.ReadLine();
            DatasetMapping = new string[40];
            for (int i = 0; i < 40; i++)
            {
                DatasetMapping[i] = file.ReadLine();
            }
            switch (file.ReadLine())
            {
                case "Almost never use a camera":
                    LevelOfPhotography = LoP.AlmostNever;
                    break;
                case "Occasionally take photographs":
                    LevelOfPhotography = LoP.Occasionally;
                    break;
                case "Photography is a hobby":
                    LevelOfPhotography = LoP.Hobby;
                    break;
                case "Professional Photographer":
                    LevelOfPhotography = LoP.Professional;
                    break;
            }
            ManualSortingTime = Convert.ToDouble(file.ReadLine());
            ManualSorting = new string[40];
            for (int i = 0; i < 40; i++)
            {
                ManualSorting[i] = file.ReadLine();
            }
            file.ReadLine();
            switch (file.ReadLine())
            {
                case "(-3) - Nearly completely sorted the wrong way":
                    IntuitiveHappiness = Happiness.AllWrong;
                    break;
                case "(-2) - Mostly sorted the wrong way":
                    IntuitiveHappiness = Happiness.MostlyWrong;
                    break;
                case "(-1) - Slightly sorted the wrong way":
                    IntuitiveHappiness = Happiness.SlightlyWrong;
                    break;
                case "(0) - Apparenty random":
                    IntuitiveHappiness = Happiness.Random;
                    break;
                case "(+1) - Slightly sorted correctly":
                    IntuitiveHappiness = Happiness.SlightlyCorrect;
                    break;
                case "(+2) - Mostly sorted correctly":
                    IntuitiveHappiness = Happiness.MostlyCorrect;
                    break;
                case "(+3) - Nearly completely sorted correctly":
                    IntuitiveHappiness = Happiness.AllCorrect;
                    break;
            }
            IntuitiveSortingTime = Convert.ToDouble(file.ReadLine());
            IntuitiveSorting = new string[40];
            for (int i = 0; i < 40; i++)
            {
                IntuitiveSorting[i] = file.ReadLine();
            }
            IntuitiveWeights = new double[8];
            for (int i = 0; i < 8; i++)
            {
                IntuitiveWeights[i] = Convert.ToDouble(file.ReadLine());
            }
            NumIntuitiveIterations = Convert.ToInt32(file.ReadLine());
            file.ReadLine();
            EfficientSortingTime = Convert.ToDouble(file.ReadLine());
            EfficientSorting = new string[40];
            for (int i = 0; i < 40; i++)
            {
                EfficientSorting[i] = file.ReadLine();
            }
            EfficientWeights = new double[8];
            for (int i = 0; i < 8; i++)
            {
                EfficientWeights[i] = Convert.ToDouble(file.ReadLine());
            }
            switch (file.ReadLine())
            {
                case "(-3) - Nearly completely sorted the wrong way":
                    EfficientHappiness = Happiness.AllWrong;
                    break;
                case "(-2) - Mostly sorted the wrong way":
                    EfficientHappiness = Happiness.MostlyWrong;
                    break;
                case "(-1) - Slightly sorted the wrong way":
                    EfficientHappiness = Happiness.SlightlyWrong;
                    break;
                case "(0) - Apparenty random":
                    EfficientHappiness = Happiness.Random;
                    break;
                case "(+1) - Slightly sorted correctly":
                    EfficientHappiness = Happiness.SlightlyCorrect;
                    break;
                case "(+2) - Mostly sorted correctly":
                    EfficientHappiness = Happiness.MostlyCorrect;
                    break;
                case "(+3) - Nearly completely sorted correctly":
                    EfficientHappiness = Happiness.AllCorrect;
                    break;
            }
            switch (file.ReadLine())
            {
                case "Manual Sorting (Stage 1)":
                    FavouriteMethod = Method.Manual;
                    break;
                case "Picture-based Assisted Sorting (Stage 2 Part 1)":
                    FavouriteMethod = Method.PictureBased;
                    break;
                case "Slider-based Assisted Sorting (Stage 2 Part 2)":
                    FavouriteMethod = Method.SliderBased;
                    break;
            }
            file.ReadLine();
            file.ReadLine();
            file.ReadLine();
            ManualSorting2 = new string[40];
            for (int i = 0; i < 40; i++)
            {
                ManualSorting2[i] = file.ReadLine();
            }

            // Close the file
            file.Close();
            file.Dispose();

            // Get original sorting (alphabetical)
            List<string> sorted = DatasetMapping.ToList();
            sorted.Sort();
            OriginalSorting = sorted.ToArray();
                
            NormaliseRankings();
        }
    }
}
