using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.FFTLibrary;
using Part2Project.Infrastructure;
using Part2Project.MyColor;

namespace Part2Project.Features
{
    static class FeatureBlurriness
    {
        public static double ComputeFeature(DirectBitmap image512X512)
        {
            double totalfreq = 0.0;
            int num = 0;
            using (var fft = new FFT(image512X512))
            {
                fft.ForwardFFT();
                fft.FFTPlot();

                for (int x = 271; x < 512; x++)
                {
                    for (int y = 0; y < 241; y++)
                    {
                        totalfreq += fft.FourierPlot.GetPixel(x, y).R / 255.0;
                        num++;
                    }
                }
            }

            return totalfreq / num;
        }
    }
}
