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
            const double windowSize = 0.9;
            double maxRadius = Math.Sqrt(2 * 256 * 256);
            double totalWeight = 0.0, total = 0.0;
            double totalfreq = 0.0;
            int num = 0;
            using (var fft = new FFT(image512X512))
            {
                fft.ForwardFFT();
                fft.FFTShift();
                fft.FFTPlot(fft.FFTShifted);

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

        public static DirectBitmap Get2DFFT(DirectBitmap image512X512)
        {
            DirectBitmap result, result2 = new DirectBitmap(128, 128);
            using (var fft = new FFT(image512X512))
            {
                fft.ForwardFFT();
                fft.FFTShift();
                fft.FFTPlot(fft.FFTShifted);

                result = new DirectBitmap(fft.FourierPlot.Bitmap);

//                for (int x = 26; x < 486; x++)
//                {
//                    for (int y = 26; y < 486; y++)
//                    {
//                        result.SetPixel(x, y, Color.Red);
//                    }
//                }
            }

            return result;
        }

        public static DirectBitmap GetInverseFFT(DirectBitmap image)
        {
            DirectBitmap result;

            COMPLEX[,] Fourier = new COMPLEX[image.Width, image.Width];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    var val = image.GetPixel(x, y).R;
                    Fourier[x,y] = new COMPLEX(val, val);
                }
            }


            using (var fft = new FFT(Fourier))
            {
                fft.RemoveFFTShift();
                fft.InverseFFT();
                result = new DirectBitmap(fft.Obj.Bitmap);
            }

            int max = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    if (result.GetPixel(x, y).R > max) max = result.GetPixel(x, y).R;
                }
            }

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Width; y++)
                {
                    var val = (int)(result.GetPixel(x, y).R / (double) max * 255.0);
                    result.SetPixel(x, y, Color.FromArgb(val, val ,val));
                }
            }

            return result;
        }
    }
}
