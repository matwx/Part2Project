using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.ColorSpace;
using Kaliko.ImageLibrary.Filters;
using Part2Project.GraphBasedDataStructures;
using Part2Project.ImageSegmentation;
using Part2Project.MyColor;

namespace Part2Project.Saliency
{
    class SaliencySegmentation : Segmentation
    {
        private Dictionary<int, double> _segmentSaliencies;
        private double[][] sMap;
        private double maxS = 0;

        public SaliencySegmentation(Segmentation s, Bitmap image, double sigma) : base(s)
        {
            // Gaussian blur the image
            KalikoImage kImage = new KalikoImage(image);
            kImage.ApplyFilter(new GaussianBlurFilter((float)sigma));
            Bitmap blurredImage = kImage.GetAsBitmap();

            // Calculate average colour of the image
            double totalL = 0, totalA = 0, totalB = 0;
            for (int i = 0; i < NumSegments; i++)
            {
                totalL += _segmentColours[i].L;
                totalA += _segmentColours[i].A;
                totalB += _segmentColours[i].B;
            }
            CIELab averageLab = new CIELab(totalL / NumSegments, totalA / NumSegments, totalB / NumSegments);

            // Calculate saliency map of the image
            sMap = new double[Width][];
            for (int x = 0; x < Width; x++)
            {
                sMap[x] = new double[Height];
                for (int y = 0; y < Height; y++)
                {
                    sMap[x][y] = MyColorSpaceHelper.MyColourDifference(averageLab,
                        ColorSpaceHelper.RGBtoLab(blurredImage.GetPixel(x, y)));
                    if (sMap[x][y] > maxS) maxS = sMap[x][y];
                }
            }

            // Average across segments to get the segmentSaliencies
        }

        public Bitmap GetSaliencyMap()
        {
            Bitmap image = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    image.SetPixel(x, y,
                        Color.FromArgb((int) (sMap[x][y]/maxS)*255, (int) (sMap[x][y]/maxS)*255,
                            (int) (sMap[x][y]/maxS)*255));
                }
            }

            return image;
        }
    }
}
