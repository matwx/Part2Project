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

        private void btnMultiThreaded_Click(object sender, EventArgs e)
        {
            string p = "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Speed tests";

            List<int> folderSizesToTest = new List<int>();
            for (int i = 28; i < 30; i++)
            {
                // 1 to 30
                folderSizesToTest.Add(i+1);
            }

            //File.WriteAllText(p + "\\temp\\results.txt", "");

            string[] filenames = Directory.GetFiles(p + "\\50 Original Images\\");

            foreach (int folderSize in folderSizesToTest)
            {
                while (Directory.EnumerateFileSystemEntries(p + "\\temp").Count() > 1){} // Wait for directory to be empty

                GC.WaitForPendingFinalizers();
                GC.Collect();
                System.Threading.Thread.Sleep(2000);

                for (int i = 0; i < folderSize; i++)
                {
                    File.Copy(filenames[i], p + "\\temp\\image" + i + ".jpg");
                }

                DateTime start = DateTime.Now;
                var featureComputer = new ImageDirectoryFeatures(p + "\\temp");
                featureComputer.GetDirectoryFeatures();
                double milliseconds = (DateTime.Now - start).TotalMilliseconds;

                File.AppendAllText(p + "\\temp\\results.txt", folderSize + ", " + milliseconds + Environment.NewLine);

                for (int i = 0; i < folderSize; i++)
                {
                    File.Delete(p + "\\temp\\image" + i + ".jpg");
                }
            }
        }

        private void btnSingleThreaded_Click(object sender, EventArgs e)
        {
            string p = "D:\\Users\\Matt\\Documents\\1 - Part II Project Tests\\Speed tests";

            List<int> folderSizesToTest = new List<int>();
            for (int i = 0; i < 30; i++)
            {
                // 1 to 30
                folderSizesToTest.Add(i + 1);
            }

            //File.WriteAllText(p + "\\temp\\results.txt", "");

            string[] filenames = Directory.GetFiles(p + "\\50 Original Images\\");

            foreach (int folderSize in folderSizesToTest)
            {
                while (Directory.EnumerateFileSystemEntries(p + "\\temp").Count() > 1) { } // Wait for directory to be empty

                GC.WaitForPendingFinalizers();
                GC.Collect();
                System.Threading.Thread.Sleep(2000);

                for (int i = 0; i < folderSize; i++)
                {
                    File.Copy(filenames[i], p + "\\temp\\image" + i + ".jpg");
                }

                DateTime start = DateTime.Now;
                for (int i = 0; i < folderSize; i++)
                {
                    ImageFeatures imFeat = new ImageFeatures(p + "\\temp\\image" + i + ".jpg");
                    imFeat.ThreadPoolCallback();
                }
                double milliseconds = (DateTime.Now - start).TotalMilliseconds;

                File.AppendAllText(p + "\\temp\\results.txt", folderSize + ", " + milliseconds + Environment.NewLine);

                for (int i = 0; i < folderSize; i++)
                {
                    File.Delete(p + "\\temp\\image" + i + ".jpg");
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
}
