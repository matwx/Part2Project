using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    class FeatureIntensityContrast : IFeature // Uses Weber Constrast
    {
        public double ComputeFeature(Bitmap image)
        {
            // Compute the average intensity of the image
            double total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y));
                }
            }
            double I_average = total/image.Width/image.Height;

            // Compute the average pixel intensity difference from that average
            total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += Math.Abs(MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y)) - I_average);
                }
            }
            total = total / I_average / image.Width / image.Height;

            return total;
        }

        public Bitmap GetWeberContrastMap(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            // Compute the average intensity of the image
            double total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y));
                }
            }
            double I_average = total / image.Width / image.Height;

            // Set each pixel based on the difference from the average
            double[][] values = new double[image.Width][];
            double maxVal = 0;
            for (int x = 0; x < image.Width; x++)
            {
                values[x] = new double[image.Height];
                for (int y = 0; y < image.Height; y++)
                {
                    values[x][y] = MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y)) - I_average;
                    if (Math.Abs(values[x][y]) > maxVal) maxVal = Math.Abs(values[x][y]);
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (values[x][y] < 0)
                    {
                        result.SetPixel(x, y, Color.FromArgb(0, -(int)(values[x][y] / maxVal * 255), -(int)(values[x][y] / maxVal * 255)));
                    }
                    else
                    {
                        result.SetPixel(x, y, Color.FromArgb((int)(values[x][y] / maxVal * 255), (int)(values[x][y] / maxVal * 255), 0));
                    }
                    
                }
            }

            return result;
        }

        public Bitmap GetDifferenceMap(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            // Compute the average intensity of the image
            double total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y));
                }
            }
            double I_average = total / image.Width / image.Height;

            // Set each pixel based on the difference from the average
            double[][] values = new double[image.Width][];
            double maxVal = 0;
            for (int x = 0; x < image.Width; x++)
            {
                values[x] = new double[image.Height];
                for (int y = 0; y < image.Height; y++)
                {
                    values[x][y] = Math.Abs(MyColorSpaceHelper.GetIntensityFromRGB(image.GetPixel(x, y)) - I_average);
                    if (values[x][y] > maxVal) maxVal = values[x][y];
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    result.SetPixel(x, y,
                        Color.FromArgb((int) (values[x][y]/maxVal*255), (int) (values[x][y]/maxVal*255),
                            (int) (values[x][y]/maxVal*255)));
                }
            }

            return result;
        }
    }
}
