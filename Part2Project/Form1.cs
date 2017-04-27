using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            dlgImage.ShowDialog();
        }

        private void btnSelectRealEdges_Click(object sender, EventArgs e)
        {
            dlgEdges.ShowDialog();
        }

        private void dlgImage_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgImage.FileName != "")
            {
                using (Image selected = Image.FromFile(dlgImage.FileName))
                {
                    _image = new DirectBitmap(320, 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(_image.Bitmap))
                    {
                        if (selected.Width / (double) selected.Height > 4.0 / 3.0)
                        {
                            // Too wide - crop left and right
                            int originalWidth = (int) ((double) selected.Width / (double) selected.Height * 240.0);
                            gfx.DrawImage(selected, 160 - originalWidth / 2, 0, originalWidth, 240);
                        }
                        else if (selected.Width / (double) selected.Height < 4.0 / 3.0)
                        {
                            // Too narrow - crop top and bottom
                            int originalHeight = (int) ((double) selected.Height / (double) selected.Width * 320.0);
                            gfx.DrawImage(selected, 0, 120 - originalHeight / 2, 320, originalHeight);
                        }
                        else
                        {
                            // Correct AR
                            gfx.DrawImage(selected, 0, 0, 320, 240);
                        }
                    }


                }

                viewer1.Image = _image.Bitmap;
            }
        }

        private void dlgEdges_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dlgEdges.FileName != "")
            {
                using (Image selected = Image.FromFile(dlgEdges.FileName))
                {
                    _trueEdges = new DirectBitmap(320, 240);
                    // Create the required resized image
                    using (Graphics gfx = Graphics.FromImage(_trueEdges.Bitmap))
                    {
                        if (selected.Width / (double) selected.Height > 4.0 / 3.0)
                        {
                            // Too wide - crop left and right
                            int originalWidth = (int) ((double) selected.Width / (double) selected.Height * 240.0);
                            gfx.DrawImage(selected, 160 - originalWidth / 2, 0, originalWidth, 240);
                        }
                        else if (selected.Width / (double) selected.Height < 4.0 / 3.0)
                        {
                            // Too narrow - crop top and bottom
                            int originalHeight = (int) ((double) selected.Height / (double) selected.Width * 320.0);
                            gfx.DrawImage(selected, 0, 120 - originalHeight / 2, 320, originalHeight);
                        }
                        else
                        {
                            // Correct AR
                            gfx.DrawImage(selected, 0, 0, 320, 240);
                        }
                    }
                }

                viewer4.Image = _trueEdges.Bitmap;
            }
        }

        private bool edgeMap1Helper(Segmentation s, int i, int x, int y)
        {
            int size = s.GetPixelsSegmentSize(x, y);
            if (size < 0.01 * s.Width * s.Height) return true;

            return i == s.GetPixelsSegmentIndex(x, y);
        }

        private double edgeMap2Helper(SaliencySegmentation ss, int x, int y)
        {
            double result = 0.0;

            // return the maximum saliency of the 3x3 neighbourhood
            if (x > 0 && x < ss.Width - 1 && y > 0 && y < ss.Height - 1)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    for (int k = y - 1; k <= y + 1; k++)
                    {
                        if (ss.GetPixelsSegmentSize(j, k) > 0.01 * ss.Width * ss.Height)
                            result = Math.Max(result, ss.GetSegmentsSaliency(ss.GetPixelsSegmentIndex(j, k)));
                    }
                }
            }

            return result;
        }

        private DirectBitmap Edge1()
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(_image, k, sigma);

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = s.GetPixelsSegmentIndex(x, y);
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x - 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y + 1);
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x + 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y + 1);
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y - 1);
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y + 1);
                    }

                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else edgeMap.SetPixel(x, y, Color.White);
                }
            }

            return edgeMap;
        }

        private DirectBitmap Edge2()
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(_image, k, sigma);
            SaliencySegmentation ss = new SaliencySegmentation(s, _image, sigma);

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = s.GetPixelsSegmentIndex(x, y);
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x - 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y + 1);
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x + 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y + 1);
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y - 1);
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y + 1);
                    }

                    // This time, make the edge colour a function of the segment saliencies
                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else
                    {
                        double sal = edgeMap2Helper(ss, x, y);
                        int val = (int) (sal * sal * 255.0);
                        edgeMap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
            }

            return edgeMap;
        }

        private void btnEdge1_Click(object sender, EventArgs e)
        {
            // Compute Edge maps
            DirectBitmap naiveEdges = Edge1();
            DirectBitmap salEdges = Edge2();
            DirectBitmap kMeansEdges = KMeansEdges(_image, 3);
            DirectBitmap naiveEdges_B, salEdges_B, trueEdges_B, kMeans_B;

            viewer9.Image = kMeansEdges.Bitmap;
            viewer2.Image = naiveEdges.Bitmap;
            viewer3.Image = salEdges.Bitmap;

            // Blur them
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(naiveEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                naiveEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }
            using (KalikoImage kImage = new KalikoImage(salEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                salEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }
            using (KalikoImage kImage = new KalikoImage(kMeansEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                kMeans_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            viewer5.Image = naiveEdges_B.Bitmap;
            viewer6.Image = salEdges_B.Bitmap;
            viewer10.Image = kMeans_B.Bitmap;

            // Then normalise true edge map
            DirectBitmap trueEdges = new DirectBitmap((Bitmap) viewer4.Image);
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int) ((double) trueEdges.GetPixel(x, y).R / (double) max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            viewer7.Image = trueEdges.Bitmap;

            // Then blur truth edge map
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            viewer8.Image = trueEdges_B.Bitmap;

            // Then compute Correlations
            // First, naive images
            double naiveResult = 0.0, totalNaiveValues = 0.0, totalTrueValues = 0.0, totalSalValues = 0.0, totalKMValues = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    totalNaiveValues += Math.Pow(naiveEdges_B.GetPixel(x, y).R / 255.0, 2);
                    totalSalValues += Math.Pow(salEdges_B.GetPixel(x, y).R / 255.0, 2);
                    totalTrueValues += Math.Pow(trueEdges_B.GetPixel(x, y).R / 255.0, 2);
                    totalKMValues += Math.Pow(kMeans_B.GetPixel(x, y).R / 255.0, 2);
                    naiveResult +=
                        Math.Pow((naiveEdges_B.GetPixel(x, y).R / 255.0) - (trueEdges_B.GetPixel(x, y).R / 255.0), 2);
                }
            }
            naiveResult /= (totalNaiveValues + totalTrueValues);
            naiveResult = (1 - naiveResult) * 100.0;
            blurredNaive.Text = naiveResult + "%";

            // Second, saliency images
            double salResult = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    salResult += Math.Pow(
                        (salEdges_B.GetPixel(x, y).R / 255.0) - (trueEdges_B.GetPixel(x, y).R / 255.0), 2);
                }
            }
            salResult /= (totalSalValues + totalTrueValues);
            salResult = (1 - salResult) * 100.0;
            blurredSaliency.Text = salResult + "%";

            // Then, K-Means images
            double kMResult = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    kMResult +=
                        Math.Pow((kMeans_B.GetPixel(x, y).R / 255.0) - (trueEdges_B.GetPixel(x, y).R / 255.0), 2);
                }
            }
            kMResult /= (totalKMValues + totalTrueValues);
            kMResult = (1 - kMResult) * 100.0;
            blurredKMeans.Text = kMResult + "%";
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private DirectBitmap GenerateNaiveEdgeMap(DirectBitmap image)
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = s.GetPixelsSegmentIndex(x, y);
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x - 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y + 1);
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x + 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y + 1);
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y - 1);
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y + 1);
                    }

                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else edgeMap.SetPixel(x, y, Color.White);
                }
            }

            return edgeMap;
        }
        private DirectBitmap GenerateSalEdgeMap(DirectBitmap image)
        {
            // Segmentation-Derived
            const int k = 125;
            const double sigma = 0.6;
            Segmentation s = GraphBasedImageSegmentation.Segment(image, k, sigma);
            SaliencySegmentation ss = new SaliencySegmentation(s, image, sigma);

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = s.GetPixelsSegmentIndex(x, y);
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x - 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x - 1, y + 1);
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y - 1);
                        }
                        notOnEdge &= edgeMap1Helper(s, i, x + 1, y);
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= edgeMap1Helper(s, i, x + 1, y + 1);
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y - 1);
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= edgeMap1Helper(s, i, x, y + 1);
                    }

                    // This time, make the edge colour a function of the segment saliencies
                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else
                    {
                        double sal = edgeMap2Helper(ss, x, y);
                        int val = (int)(sal * sal * 255.0);
                        edgeMap.SetPixel(x, y, Color.FromArgb(val, val, val));
                    }
                }
            }

            return edgeMap;
        }
        private DirectBitmap GetImageScaled(string filename)
        {
            DirectBitmap result;

            using (Image selected = Image.FromFile(filename))
            {
                result = new DirectBitmap(320, 240);
                // Create the required resized image
                using (Graphics gfx = Graphics.FromImage(result.Bitmap))
                {
                    if (selected.Width / (double)selected.Height > 4.0 / 3.0)
                    {
                        // Too wide - crop left and right
                        int originalWidth = (int)((double)selected.Width / (double)selected.Height * 240.0);
                        gfx.DrawImage(selected, 160 - originalWidth / 2, 0, originalWidth, 240);
                    }
                    else if (selected.Width / (double)selected.Height < 4.0 / 3.0)
                    {
                        // Too narrow - crop top and bottom
                        int originalHeight = (int)((double)selected.Height / (double)selected.Width * 320.0);
                        gfx.DrawImage(selected, 0, 120 - originalHeight / 2, 320, originalHeight);
                    }
                    else
                    {
                        // Correct AR
                        gfx.DrawImage(selected, 0, 0, 320, 240);
                    }
                }
            }

            return result;
        }

        private double ComputeEdgeMapAlignment(DirectBitmap original, DirectBitmap truth)
        {
            // Compute Correlations
            double result = 0.0;
            double totalTrueValues = 0.0, totalSalValues = 0.0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    totalSalValues += Math.Pow(original.GetPixel(x, y).R / 255.0, 2);
                    totalTrueValues += Math.Pow(truth.GetPixel(x, y).R / 255.0, 2);
                    result += Math.Pow((original.GetPixel(x, y).R / 255.0) - (truth.GetPixel(x, y).R / 255.0), 2);
                }
            }
            result /= (totalSalValues + totalTrueValues);
            result = 1 - result;

            return result;
        }
        private double ComputeSalAlignment(string originalFilename, string trueFilename, string folderPath, int id)
        {
            DirectBitmap bmp = GetImageScaled(originalFilename);
            DirectBitmap salEdges = GenerateSalEdgeMap(bmp);
            DirectBitmap trueEdges = GetImageScaled(trueFilename);

            DirectBitmap salEdges_B, trueEdges_B;

            // Blur them
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(salEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                salEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            // Then normalise true edge map
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Then blur truth edge map
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            double result = ComputeEdgeMapAlignment(salEdges_B, trueEdges_B);

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            output += originalFilename + nl + trueFilename + nl + result;
            File.WriteAllText(folderPath + "\\DeleteMe_" + id + ".txt", output);

            return result;
        }
        private void btnMyTestFolder_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Original") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Ground Truth")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Original");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Ground Truth");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\SalResults"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\SalResults");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeSalAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath, i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;

            List<Pair> pairs = new List<Pair>(); 
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;

                var newPair = new Pair();
                newPair.score = results[i];
                newPair.filename = originalFilenames[i];
                pairs.Add(newPair);
            }

            pairs.Sort();
            pairs.Reverse();
            for (int i = 0; i < pairs.Count; i++)
            {
                File.Copy(pairs[i].filename, dlgEdgesFolder.SelectedPath + "\\SalResults\\" + i + " - " + pairs[i].score + ".jpg");
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\SalResults\\SalResults.txt", output);
        }

        private long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
        private DirectBitmap KMeansEdges(DirectBitmap image, int k)
        {
            int numUpdates = -1;

            // Create a smaller image to actually find the cluster centres
            var centres = new Color[k];
            int[] counts = new int[k];
            int[] totalsR = new int[k];
            int[] totalsG = new int[k];
            int[] totalsB = new int[k];
            Random rand = new Random();
            int[][] assignments = new int[image.Width][];
            int[][] largeAssignments = new int[image.Width][];
            for (int i = 0; i < image.Width; i++)
            {
                largeAssignments[i] = new int[image.Height];
            }
            for (int i = 0; i < image.Width; i++)
            {
                assignments[i] = new int[image.Height];
            }

            // 1) Pick K clusters using the K++ algorithm

            // 1.1) Choose one center uniformly at random from among the data points.
            centres[0] = image.GetPixel(rand.Next(0, image.Width), rand.Next(0, image.Height));

            for (int l = 1; l < k; l++)
            {
                // 1.2) For each data point x, compute D(x), the distance between x and the nearest center that has already been chosen.
                //      Use the assignments 2D array for this purpose.
                long totalDistances = 0;
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {

                        Color pixel = image.GetPixel(i, j);
                        for (int p = 0; p < l; p++)
                        {
                            int val = (pixel.R - centres[l - 1].R) * (pixel.R - centres[l - 1].R) +
                                      (pixel.G - centres[l - 1].G) * (pixel.G - centres[l - 1].G) +
                                      (pixel.B - centres[l - 1].B) * (pixel.B - centres[l - 1].B);
                            //int val = colourDifference(pixel, centres[l - 1]);

                            val *= val;
                            if (val < assignments[i][j] || p == 0) assignments[i][j] = val;
                        }

                        totalDistances += assignments[i][j];
                    }
                }

                // 1.3) Choose one new data point at random as a new center, using a weighted probability distribution where a point x is 
                //      chosen with probability proportional to D(x)^2.
                long randomNumber = LongRandom(0, totalDistances, rand);
                bool done = false;
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        if (randomNumber < assignments[i][j])
                        {
                            centres[l] = image.GetPixel(i, j);
                            done = true;
                            break;
                        }
                    }
                    if (done) break;
                }

                // 1.4) Repeat Steps 2 and 3 until k centers have been chosen.
            }
            // 1.5) Now that the initial centers have been chosen, proceed using standard k-means clustering.


            bool updateMade = false;

            // 4) Repeat steps 2 and 3 until convergence is attained (i.e. no pixels change clusters)
            do
            {
                // Save the current state of the image
                numUpdates++;

                updateMade = false;
                for (int i = 0; i < k; i++)
                {
                    counts[i] = 0;
                    totalsR[i] = 0;
                    totalsG[i] = 0;
                    totalsB[i] = 0;
                }

                // 2) Assign each pixel in the image to the cluster that minimizes the distance between the pixel and the cluster center
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color pixel = image.GetPixel(i, j);
                        int minL = 0, minDifference = Int16.MaxValue;
                        for (int l = 0; l < k; l++)
                        {
                            // Calculate difference in colour between point (x,y) and centre l
                            int squareDifference = (pixel.R - centres[l].R) * (pixel.R - centres[l].R) +
                                                   (pixel.G - centres[l].G) * (pixel.G - centres[l].G) +
                                                   (pixel.B - centres[l].B) * (pixel.B - centres[l].B);

                            //squareDifference = colourDifference(pixel, centres[l]);

                            if (squareDifference < minDifference)
                            {
                                minL = l;
                                minDifference = squareDifference;
                            }
                        }
                        if (minL != assignments[i][j])
                        {
                            // Pixel changing cluster
                            updateMade = true;
                            assignments[i][j] = minL;
                        }

                        // Add to new totals and counts for new cluster centres
                        counts[minL]++;
                        totalsR[minL] += pixel.R;
                        totalsG[minL] += pixel.G;
                        totalsB[minL] += pixel.B;
                    }
                }

                // 3) Re-compute the cluster centers by averaging all of the pixels in the cluster
                for (int l = 0; l < k; l++)
                {
                    centres[l] = Color.FromArgb(totalsR[l] / (counts[l] + 1), totalsG[l] / (counts[l] + 1),
                        totalsB[l] / (counts[l] + 1));
                }

            } while (updateMade);


            // Now we have the cluster centres, assign each pixel in the larger image to the cluster that minimizes the 
            // distance between the pixel and the cluster center
            largeAssignments = new int[image.Width][];
            for (int i = 0; i < image.Width; i++)
            {
                largeAssignments[i] = new int[image.Height];
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    int minL = 0, minDifference = Int16.MaxValue;
                    for (int l = 0; l < k; l++)
                    {
                        // Calculate difference in colour between point (x,y) and centre l
                        int squareDifference = (pixel.R - centres[l].R) * (pixel.R - centres[l].R) +
                                               (pixel.G - centres[l].G) * (pixel.G - centres[l].G) +
                                               (pixel.B - centres[l].B) * (pixel.B - centres[l].B);

                        //squareDifference = colourDifference(pixel, centres[l]);

                        if (squareDifference < minDifference)
                        {
                            minL = l;
                            minDifference = squareDifference;
                        }
                    }
                    largeAssignments[i][j] = minL;
                }
            }

            // Create edge map
            DirectBitmap edgeMap = new DirectBitmap(320, 240);
            for (int x = 0; x < edgeMap.Width; x++)
            {
                for (int y = 0; y < edgeMap.Height; y++)
                {
                    int i = largeAssignments[x][y];
                    bool notOnEdge = true;

                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= i == largeAssignments[x - 1][y - 1];
                        }
                        notOnEdge &= i == largeAssignments[x - 1][y];
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= i == largeAssignments[x - 1][y + 1];
                        }
                    }
                    if (x < edgeMap.Width - 1)
                    {
                        if (y > 0)
                        {
                            notOnEdge &= i == largeAssignments[x + 1][y - 1];
                        }
                        notOnEdge &= i == largeAssignments[x + 1][y];
                        if (y < edgeMap.Height - 1)
                        {
                            notOnEdge &= i == largeAssignments[x + 1][y + 1];
                        }
                    }
                    if (y > 0)
                    {
                        notOnEdge &= i == largeAssignments[x][y - 1];
                    }
                    if (y < edgeMap.Height - 1)
                    {
                        notOnEdge &= i == largeAssignments[x][y + 1];
                    }

                    if (notOnEdge) edgeMap.SetPixel(x, y, Color.Black);
                    else edgeMap.SetPixel(x, y, Color.White);

                    //                    edgeMap.SetPixel(x, y, centres[largeAssignments[x][y]]);
                }
            }

            return edgeMap;
        }
        private double ComputeKMAlignment(string originalFilename, string trueFilename, string folderPath, int id, int k)
        {
            DirectBitmap bmp = GetImageScaled(originalFilename);
            DirectBitmap KMEdges = KMeansEdges(bmp, k);
            DirectBitmap trueEdges = GetImageScaled(trueFilename);

            DirectBitmap KMEdges_B, trueEdges_B;

            // Blur them
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(KMEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                KMEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            // Then normalise true edge map
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Then blur truth edge map
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            double result = ComputeEdgeMapAlignment(KMEdges_B, trueEdges_B);

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            output += originalFilename + nl + trueFilename + nl + result;
            File.WriteAllText(folderPath + "\\DeleteMe_" + id + ".txt", output);

            return result;
        }
        private void btnKMeansFolder_Click(object sender, EventArgs e)
        {
            int K_MEANS_K = 4;

            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Original") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Ground Truth")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Original");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Ground Truth");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\kMeansResults" + K_MEANS_K))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\kMeansResults" + K_MEANS_K);

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeKMAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath, i2, K_MEANS_K); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;

            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;

                var newPair = new Pair();
                newPair.score = results[i];
                newPair.filename = originalFilenames[i];
                pairs.Add(newPair);
            }

            pairs.Sort();
            pairs.Reverse();
            for (int i = 0; i < pairs.Count; i++)
            {
                File.Copy(pairs[i].filename, dlgEdgesFolder.SelectedPath + "\\kMeansResults" + K_MEANS_K + "\\" + i + " - " + pairs[i].score + ".jpg");
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\kMeansResults" + K_MEANS_K + "\\kMeansResults.txt", output);
        }

        private void ComputePRVectorsForAnImage(string originalFilename, string truthFilename, double[] recalls, double[] precisions)
        {
            // Compute Edge map
            DirectBitmap salEdges = GenerateSalEdgeMap(GetImageScaled(originalFilename));
            DirectBitmap trueEdges = GetImageScaled(truthFilename);
            precisions = new double[31];
            recalls = new double[31];
            
            // Normalise True Edge Map
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Compute the precision and recall vectors
            for (int i = 0; i < 31; i++)
            {
                int thresh = (i + 1) * 8;

                int totalTruePix = 0, totalGenAndTruePix = 0, totalGenPix = 0;
                // Precision is the probability that a machine-generated boundary pixel is a true boundary pixel.
                // Recall is the probability that a true boundary pixel is detected.
                for (int x = 0; x < 320; x++)
                {
                    for (int y = 0; y < 240; y++)
                    {
                        if (trueEdges.GetPixel(x, y).R >= thresh)
                        {
                            totalTruePix++;
                            if (salEdges.GetPixel(x, y).R >= thresh) totalGenAndTruePix++;
                        }
                        if (salEdges.GetPixel(x, y).R >= thresh) totalGenPix++;
                    }
                }

                precisions[i] = (totalGenPix == 0) ? 0 : (double)totalGenAndTruePix / totalGenPix;
                recalls[i] = (totalTruePix == 0) ? 0 : (double)totalGenAndTruePix / totalTruePix;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgPRFolder.ShowDialog();
            if (dlgPRFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgPRFolder.SelectedPath + "\\Original") || !Directory.Exists(dlgPRFolder.SelectedPath + "\\Ground Truth")) return;
            
            string[] originalFilenames = Directory.GetFiles(dlgPRFolder.SelectedPath + "\\Original");
            string[] truthFilenames = Directory.GetFiles(dlgPRFolder.SelectedPath + "\\Ground Truth");

            if (originalFilenames.Length != truthFilenames.Length) return;

            // Create a task for each image to test
            double[] recallMeans = new double[31], precisionMeans = new double[31];
            Task[] tasks = new Task[originalFilenames.Length];
            PRVectorComputer[] objects = new PRVectorComputer[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                var currentPRComputer = new PRVectorComputer(originalFilenames[i], truthFilenames[i], dlgPRFolder.SelectedPath);
                objects[i] = currentPRComputer;
                int i2 = i;
                tasks[i] = Task.Run(() => currentPRComputer.ComputeVectors(i2));
            }

            Task.WaitAll(tasks);

            // Compute means for each vector part
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                for (int j = 0; j < 31; j++)
                {
                    recallMeans[j] += objects[i].Recalls[j];
                    precisionMeans[j] += objects[i].Precisions[j];
                }
            }
            for (int i = 0; i < 31; i++)
            {
                recallMeans[i] /= originalFilenames.Length;
                precisionMeans[i] /= originalFilenames.Length;
            }

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < 31; i++)
            {
                output += recallMeans[i] + "," + precisionMeans[i] + nl;
            }

            File.WriteAllText(dlgPRFolder.SelectedPath + "\\PrValues.txt", output);

            // Produce a precison-recall curve
            using (DirectBitmap PRCurve = new DirectBitmap(500, 500))
            {
                using (var gfx = Graphics.FromImage(PRCurve.Bitmap))
                {
                    gfx.Clear(Color.White);

                    for (int i = 0; i < 500; i++)
                    {
                        gfx.DrawLine(new Pen(Color.DarkGray, 2f), 0, i * 50, 500, i * 50);
                        gfx.DrawLine(new Pen(Color.DarkGray, 2f), i * 50, 0, i * 50, 500);
                    }

                    for (int i = 0; i < 31; i++)
                    {
                        // Draw each point
                        int p = 500 - (int)(precisionMeans[i] * 500);
                        int r = (int)(recallMeans[i] * 500);

                        gfx.FillEllipse(Brushes.Red, r - 2, p - 2, 4, 4);

                        if (i > 0)
                        {
                            int p_prev = 500 - (int)(precisionMeans[i - 1] * 500);
                            int r_prev = (int)(recallMeans[i - 1] * 500);
                            gfx.DrawLine(Pens.Red, r_prev, p_prev, r, p);
                        }
                    }
                }

                PRCurve.Bitmap.Save(dlgPRFolder.SelectedPath + "\\PR.png", ImageFormat.Png);
            }
        }
        private void btnPR_Click(object sender, EventArgs e)
        {
            // Compute Edge map
            DirectBitmap salEdges = Edge2(), salEdges_B, trueEdges_B;
            double[] precisions = new double[31];
            double[] recalls = new double[31];

            float blurSigma = 0f;
            using (KalikoImage kImage = new KalikoImage(salEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                salEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            DirectBitmap trueEdges = new DirectBitmap(_trueEdges.Bitmap);
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            for (int i = 0; i < 31; i++)
            {
                int thresh = (i + 1) * 8;

                using (DirectBitmap outputIm = new DirectBitmap(320, 240))
                {
                    for (int x = 0; x < 320; x++)
                    {
                        for (int y = 0; y < 240; y++)
                        {
                            if (trueEdges_B.GetPixel(x, y).R >= thresh)
                            {
                                outputIm.SetPixel(x, y, Color.Blue);
                                if (salEdges_B.GetPixel(x, y).R >= thresh) outputIm.SetPixel(x, y, Color.Green);
                            }
                            else if (salEdges_B.GetPixel(x, y).R >= thresh) outputIm.SetPixel(x, y, Color.White);
                        }
                    }

                    outputIm.Bitmap.Save("D:\\Users\\Matt\\Downloads\\BSD\\" + i + ".png", ImageFormat.Png);
                }
                int totalTruePix = 0, totalGenAndTruePix = 0, totalGenPix = 0;
                // Precision is the probability that a machine-generated boundary pixel is a true boundary pixel.
                // Recall is the probability that a true boundary pixel is detected.
                for (int x = 0; x < 320; x++)
                {
                    for (int y = 0; y < 240; y++)
                    {
                        if (_trueEdges.GetPixel(x, y).R >= thresh)
                        {
                            totalTruePix++;
                            if (salEdges.GetPixel(x, y).R >= thresh) totalGenAndTruePix++;
                        }
                        if (salEdges.GetPixel(x, y).R >= thresh) totalGenPix++;
                    }
                }

                precisions[i] = (totalGenPix == 0) ? 0 : (double) totalGenAndTruePix / totalGenPix;
                recalls[i] = (totalTruePix == 0) ? 0 : (double)totalGenAndTruePix / totalTruePix;
            }

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < 31; i++)
            {
                output += recalls[i] + "," + precisions[i] + nl;
            }

            File.WriteAllText("D:\\Users\\Matt\\Downloads\\BSD\\temp.txt",output);

            // Produce a precison-recall curve
            using (DirectBitmap PRCurve = new DirectBitmap(500, 500))
            {
                using (var gfx = Graphics.FromImage(PRCurve.Bitmap))
                {
                    gfx.Clear(Color.White);

                    for (int i = 0; i < 500; i++)
                    {
                        gfx.DrawLine(new Pen(Color.DarkGray, 2f), 0, i * 50, 500, i * 50);
                        gfx.DrawLine(new Pen(Color.DarkGray, 2f), i * 50, 0, i * 50, 500);
                    }

                    for (int i = 0; i < 31; i++)
                    {
                        // Draw each point
                        int p = 500 - (int)(precisions[i] * 500);
                        int r = (int) (recalls[i] * 500);
                        
                        gfx.FillEllipse(Brushes.Red, r - 2, p - 2, 4 , 4);

                        if (i > 0)
                        {
                            int p_prev = 500 - (int)(precisions[i-1] * 500);
                            int r_prev = (int)(recalls[i-1] * 500);
                            gfx.DrawLine(Pens.Red, r_prev, p_prev, r, p);
                        }
                    }
                }

                PRCurve.Bitmap.Save("D:\\Users\\Matt\\Downloads\\BSD\\PR.png", ImageFormat.Png);
            }
        }

        private double ComputeNaiveAlignment(string originalFilename, string trueFilename, string folderPath, int id)
        {
            DirectBitmap bmp = GetImageScaled(originalFilename);
            DirectBitmap naiveEdgeMap = GenerateNaiveEdgeMap(bmp);
            DirectBitmap trueEdges = GetImageScaled(trueFilename);

            DirectBitmap naiveEdges_B, trueEdges_B;

            // Blur them
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(naiveEdgeMap.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                naiveEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            // Then normalise true edge map
            int max = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Then blur truth edge map
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            double result = ComputeEdgeMapAlignment(naiveEdges_B, trueEdges_B);

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            output += originalFilename + nl + trueFilename + nl + result;
            File.WriteAllText(folderPath + "\\DeleteMe_" + id + ".txt", output);

            return result;
        }
        private void btnNaiveFolder_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Original") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Ground Truth")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Original");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Ground Truth");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\NaiveResults"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\NaiveResults");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeNaiveAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath, i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;

            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;

                var newPair = new Pair();
                newPair.score = results[i];
                newPair.filename = originalFilenames[i];
                pairs.Add(newPair);
            }

            pairs.Sort();
            pairs.Reverse();
            for (int i = 0; i < pairs.Count; i++)
            {
                File.Copy(pairs[i].filename, dlgEdgesFolder.SelectedPath + "\\NaiveResults\\" + i + " - " + pairs[i].score + ".jpg");
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\NaiveResults\\NaiveResults.txt", output);
        }

        private double ComputeEdgeAlignment(string originalFilename, string trueFilename, string folderPath, int id)
        {
            DirectBitmap genEdges = GetImageScaled(originalFilename);
            DirectBitmap trueEdges = GetImageScaled(trueFilename);

            DirectBitmap genEdges_B, trueEdges_B;

            // Normalise edge maps
            int max = 0, genMax = 0;
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    max = Math.Max(max, trueEdges.GetPixel(x, y).R);
                    genMax = Math.Max(genMax, genEdges.GetPixel(x, y).R);
                }
            }
            for (int x = 0; x < 320; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    int val = (int)((double)trueEdges.GetPixel(x, y).R / (double)max * 255.0);
                    trueEdges.SetPixel(x, y, Color.FromArgb(val, val, val));

                    val = (int)((double)genEdges.GetPixel(x, y).R / (double)genMax * 255.0);
                    genEdges.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }

            // Then blur then
            float blurSigma = 4f;
            using (KalikoImage kImage = new KalikoImage(trueEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                trueEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }
            using (KalikoImage kImage = new KalikoImage(genEdges.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter(blurSigma));
                genEdges_B = new DirectBitmap(kImage.GetAsBitmap());
            }

            double result = ComputeEdgeMapAlignment(genEdges_B, trueEdges_B);

            // Save values in a text file
            string output = "";
            string nl = Environment.NewLine;
            output += originalFilename + nl + trueFilename + nl + result;
            File.WriteAllText(folderPath + "\\DeleteMe_" + id + ".txt", output);

            return result;
        }
        private void btnRandFolder_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\100Test") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Random")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\Random");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\100Test");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Results\\RandResults"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\RandResults");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeEdgeAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath + "\\Results", i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\Results\\RandResults\\RandResults.txt", output);
        }

        private void btnCGAccuracy_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\100Test") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\ColourGradient")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\ColourGradient");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\100Test");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Results\\CGresults"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\CGresults");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeEdgeAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath + "\\Results", i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\Results\\CGresults\\CGresults.txt", output);
        }

        private void btngPbucmcolorAccuracy_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\100Test") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\gPb_ucm_color")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\gPb_ucm_color");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\100Test");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Results\\gPb_ucm_color_Results"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\gPb_ucm_color_Results");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeEdgeAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath + "\\Results", i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\Results\\gPb_ucm_color_Results\\gPb_ucm_color_Results.txt", output);
        }

        private void btnBGTGFolder_Click(object sender, EventArgs e)
        {
            // Select the top level BSD folder
            dlgEdgesFolder.ShowDialog();
            if (dlgEdgesFolder.SelectedPath == "") return;
            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\100Test") || !Directory.Exists(dlgEdgesFolder.SelectedPath + "\\BGTG")) return;

            string[] originalFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\BGTG");
            string[] truthFilenames = Directory.GetFiles(dlgEdgesFolder.SelectedPath + "\\100Test");

            if (originalFilenames.Length != truthFilenames.Length) return;

            if (!Directory.Exists(dlgEdgesFolder.SelectedPath + "\\Results\\BGTGResults"))
                Directory.CreateDirectory(dlgEdgesFolder.SelectedPath + "\\Results\\BGTGResults");

            // Set up a task for each image
            double[] results = new double[originalFilenames.Length];
            Task[] tasks = new Task[originalFilenames.Length];
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                int i2 = i;
                tasks[i] = Task.Run(() => { results[i2] = ComputeEdgeAlignment(originalFilenames[i2], truthFilenames[i2], dlgEdgesFolder.SelectedPath + "\\Results", i2); });
            }

            Task.WaitAll(tasks);

            string output = "";
            string nl = Environment.NewLine;
            for (int i = 0; i < originalFilenames.Length; i++)
            {
                tasks[i].Dispose();
                output += results[i] + nl;
            }

            // Save values in a text file
            File.WriteAllText(dlgEdgesFolder.SelectedPath + "\\Results\\BGTGResults\\BGTGResults.txt", output);
        }


        private void label13_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dlgPRFolder.ShowDialog();
            if (dlgPRFolder.SelectedPath == "" || viewer2.Image == null) return;

            viewer1.Image.Save(dlgPRFolder.SelectedPath + "\\orig.png", ImageFormat.Png);
            viewer2.Image.Save(dlgPRFolder.SelectedPath + "\\naive.png", ImageFormat.Png);
            viewer3.Image.Save(dlgPRFolder.SelectedPath + "\\sal.png", ImageFormat.Png);
            viewer7.Image.Save(dlgPRFolder.SelectedPath + "\\true.png", ImageFormat.Png);
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
