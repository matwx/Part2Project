using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    total += GetIntensityFromRGB(image.GetPixel(x, y));
                }
            }
            double I_average = total/image.Width/image.Height;

            // Compute the average pixel intensity difference from that average
            total = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    total += Math.Abs(GetIntensityFromRGB(image.GetPixel(x, y)) - I_average);
                }
            }
            total = total / I_average / image.Width / image.Height;

            return total;
        }

        private double GetIntensityFromRGB(int r, int g, int b)
        {
            // https://en.wikipedia.org/wiki/Relative_luminance, since we're comparing intensities

            return (0.2126*r + 0.7152*g + 0.0722*b);
        }

        private double GetIntensityFromRGB(Color c)
        {
            return GetIntensityFromRGB(c.R, c.G, c.B);
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
                    total += GetIntensityFromRGB(image.GetPixel(x, y));
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
                    values[x][y] = GetIntensityFromRGB(image.GetPixel(x, y)) - I_average;
                    if (Math.Abs(values[x][y]) > maxVal) maxVal = Math.Abs(values[x][y]);
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    values[x][y] /= 180.08;
                    // blue, cyan, green, yellow, red
                    // -1  , -0.5,  0   ,  0.5  ,  1
                    var blu = Color.DarkBlue;
                    var cya = Color.Cyan;
                    var gre = Color.ForestGreen;
                    var yel = Color.Yellow;
                    var red = Color.Red;

                    if (values[x][y] < -0.5)
                    {
                        double val = (values[x][y] + 1) * 2;
                        result.SetPixel(x, y, Color.FromArgb(
                            (int)(blu.R * (1 - val) + cya.R * val),
                            (int)(blu.G * (1 - val) + cya.G * val),
                            (int)(blu.B * (1 - val) + cya.B * val)));
                    }
                    else if (values[x][y] < 0)
                    {
                        double val = (values[x][y] + 0.5) * 2;
                        result.SetPixel(x, y, Color.FromArgb(
                            (int)(cya.R * (1 - val) + gre.R * val),
                            (int)(cya.G * (1 - val) + gre.G * val),
                            (int)(cya.B * (1 - val) + gre.B * val)));
                    }
                    else if (values[x][y] < 0.5)
                    {
                        double val = values[x][y] * 2;
                        result.SetPixel(x, y, Color.FromArgb(
                            (int)(gre.R * (1 - val) + yel.R * val),
                            (int)(gre.G * (1 - val) + yel.G * val),
                            (int)(gre.B * (1 - val) + yel.B * val)));
                    }
                    else
                    {
                        double val = (values[x][y] - 0.5) * 2;
                        result.SetPixel(x, y, Color.FromArgb(
                            (int)(yel.R * (1 - val) + red.R * val),
                            (int)(yel.G * (1 - val) + red.G * val),
                            (int)(yel.B * (1 - val) + red.B * val)));
                    }

//                    if (values[x][y] < 0)
//                    {
//                        result.SetPixel(x, y, Color.FromArgb(0, -(int)(values[x][y] / maxVal * 255), -(int)(values[x][y] / maxVal * 255)));
//                    }
//                    else
//                    {
//                        result.SetPixel(x, y, Color.FromArgb((int)(values[x][y] / maxVal * 255), (int)(values[x][y] / maxVal * 255), 0));
//                    }
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
                    total += GetIntensityFromRGB(image.GetPixel(x, y));
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
                    values[x][y] = Math.Abs(GetIntensityFromRGB(image.GetPixel(x, y)) - I_average);
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
