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



            return result;
        }
    }
}
