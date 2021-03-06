﻿using System;
using System.Drawing;
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.ColorSpace;
using Kaliko.ImageLibrary.Filters;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.ImageSegmentation
{
    class SaliencySegmentation : Segmentation
    {
        private double[] _segmentSaliencies;
        private double[][] sMap;

        public SaliencySegmentation(Segmentation s, DirectBitmap image, double sigma)
            : base(s)
        {
            double maxS = 0;

            // Calculate average colour of the image (we already have segment averages)
            double totalL = 0, totalA = 0, totalB = 0;
            for (int i = 0; i < NumSegments; i++)
            {
                totalL += _segmentColours[i].L * _segmentSizes[i];
                totalA += _segmentColours[i].A * _segmentSizes[i];
                totalB += _segmentColours[i].B * _segmentSizes[i];
            }
            CIELab averageLab = new CIELab(totalL / Width / Height, totalA / Width / Height, totalB / Width / Height);

            // Gaussian blur the image
            using (KalikoImage kImage = new KalikoImage(image.Bitmap))
            {
                kImage.ApplyFilter(new GaussianBlurFilter((float)sigma));
                using (DirectBitmap blurredImage = new DirectBitmap(kImage.GetAsBitmap()))
                {
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
                }
            }

            // Average across segments to get the segmentSaliencies
            _segmentSaliencies = new double[NumSegments];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    sMap[x][y] /= maxS; // Normalise sMap
                    _segmentSaliencies[_pixelAssignments[x][y]] += sMap[x][y];
                }
            }
            for (int i = 0; i < NumSegments; i++)
            {
                _segmentSaliencies[i] /= _segmentSizes[i];
            }

            // Renormalise the segment saliencies, to exclude segments smaller than 1% of the image area
            maxS = 0;
            for (int i = 0; i < NumSegments; i++)
            {
                if (_segmentSizes[i] > 0.01 * Width * Height && _segmentSaliencies[i] > maxS)
                    maxS = _segmentSaliencies[i];
            }
            for (int i = 0; i < NumSegments; i++)
            {
                _segmentSaliencies[i] = Math.Min(1.0, _segmentSaliencies[i] / maxS);
            }
        }

        public SaliencySegmentation(SaliencySegmentation ss): base(ss)
        {
            _segmentSaliencies = new double[NumSegments];
            for (int i = 0; i < NumSegments; i++)
            {
                _segmentSaliencies[i] = ss._segmentSaliencies[i];
            }

            sMap = new double[Width][];
            for (int x = 0; x < Width; x++)
            {
                sMap[x] = new double[Height];
                for (int y = 0; y < Height; y++)
                {
                    sMap[x][y] = ss.sMap[x][y];
                }
            }
        }

        public double GetSegmentsSaliency(int i)
        {
            return _segmentSaliencies[i];
        }

        public Bitmap GetSaliencyMap()
        {
            Bitmap image = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    image.SetPixel(x, y,
                        Color.FromArgb((int) (sMap[x][y]*255), (int) (sMap[x][y]*255), (int) (sMap[x][y]*255)));
                }
            }

            return image;
        }

        public Bitmap GetSegmentSaliencyMap()
        {
            Bitmap image = new Bitmap(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    image.SetPixel(x, y,
                        Color.FromArgb((int) (_segmentSaliencies[_pixelAssignments[x][y]]*255),
                            (int) (_segmentSaliencies[_pixelAssignments[x][y]]*255),
                            (int) (_segmentSaliencies[_pixelAssignments[x][y]]*255)));
                }
            }

            return image;
        }
    }
}
