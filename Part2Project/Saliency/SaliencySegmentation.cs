﻿using System;
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

            // Calculate average colour of the image (we already have segment averages)
            double totalL = 0, totalA = 0, totalB = 0;
            for (int i = 0; i < NumSegments; i++)
            {
                totalL += _segmentColours[i].L * _segmentSizes[i];
                totalA += _segmentColours[i].A * _segmentSizes[i];
                totalB += _segmentColours[i].B * _segmentSizes[i];
            }
            CIELab averageLab = new CIELab(totalL / Width / Height, totalA / Width / Height, totalB / Width / Height);

            // Calculate saliency map of the image
            sMap = new double[Width][];
            for (int x = 0; x < Width; x++)
            {
                sMap[x] = new double[Height];
                for (int y = 0; y < Height; y++)
                {
                    // I tried using my hybrid colour difference metric here, but, because it would
                    // require a few transformations between RGB, LAB and back, floating point errors
                    // became apparent, and ruined the result. So, CIEDE2000 is used.
                    sMap[x][y] = MyColorSpaceHelper.CIEDE2000(averageLab,
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
                        Color.FromArgb((int) (sMap[x][y]/maxS*255), (int) (sMap[x][y]/maxS*255),
                            (int) (sMap[x][y]/maxS*255)));
                }
            }

            return image;
        }
    }
}
