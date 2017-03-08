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

        private void label7_Click(object sender, EventArgs e)
        {

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



        private void label13_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
