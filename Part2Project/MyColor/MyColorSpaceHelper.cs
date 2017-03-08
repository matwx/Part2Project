using System;
using System.Drawing;
using Kaliko.ImageLibrary.ColorSpace;

namespace Part2Project.MyColor
{
    public sealed class MyColorSpaceHelper : ColorSpaceHelper
    {
        private MyColorSpaceHelper() {}

        public static double CIEDE2000(CIELab lab1, CIELab lab2)
        {
            const double kL = 2;
            const double kC = 1;
            const double kH = 1;

            double deltaLp = lab2.L - lab1.L;
            double lbar = (lab1.L + lab2.L) / 2;

            double c1 = Math.Sqrt(lab1.A * lab1.A + lab1.B * lab1.B);
            double c2 = Math.Sqrt(lab2.A * lab2.A + lab2.B * lab2.B);
            double cbar = (c1 + c2) / 2;

            double a1P = lab1.A + lab1.A / 2 * (1 - Math.Sqrt(Math.Pow(cbar, 7) / (Math.Pow(cbar, 7) + Math.Pow(25, 7))));
            double a2P = lab2.A + lab2.A / 2 * (1 - Math.Sqrt(Math.Pow(cbar, 7) / (Math.Pow(cbar, 7) + Math.Pow(25, 7))));

            double c1P = Math.Sqrt(a1P * a1P + lab1.B * lab1.B);
            double c2P = Math.Sqrt(a2P * a2P + lab2.B * lab2.B);

            double cbarp = (c1P + c2P) / 2;
            double deltaCp = c2P - c1P;

            double h1P, h2P;
            if (a1P == 0 && lab1.B == 0) h1P = 0;
            else h1P = ((Math.Atan2(lab1.B, a1P) + Math.PI) / Math.PI * 180); // Do we need a % 360 here?
            if (a2P == 0 && lab2.B == 0) h2P = 0;
            else h2P = ((Math.Atan2(lab2.B, a2P) + Math.PI) / Math.PI * 180); // Do we need a % 360 here?

            double deltahp;
            if (c1P == 0 || c2P == 0) deltahp = 0;
            else if (Math.Abs(h1P - h2P) <= 180) deltahp = h2P - h1P;
            else if (h2P <= h1P) deltahp = h2P - h1P + 360;
            else deltahp = h2P - h1P - 360;

            double deltaHp = 2 * Math.Sqrt(c1P * c2P) * Math.Sin(deltahp / 2 / 180 * Math.PI); // Perhaps it wants the angle in degrees here?
            double hbarp;
            if (c1P == 0 || c2P == 0) hbarp = h1P + h2P;
            else if (Math.Abs(h1P - h2P) <= 180) hbarp = (h1P + h2P) / 2;
            else if (h1P + h2P < 360) hbarp = (h1P + h2P + 360) / 2;
            else hbarp = (h1P + h2P - 360) / 2;

            double T = 1
                       - 0.17 * Math.Cos((hbarp - 30) / 180 * Math.PI)
                       + 0.24 * Math.Cos(2 * hbarp / 180 * Math.PI)
                       + 0.32 * Math.Cos((3 * hbarp + 6) / 180 * Math.PI)
                       - 0.20 * Math.Cos((4 * hbarp - 63) / 180 * Math.PI); // Perhaps it wants the angle in degrees here?

            double sL = 1 + 0.015 * (lbar - 50) * (lbar - 50) / Math.Sqrt(20 + (lbar - 50) * (lbar - 50));
            double sC = 1 + 0.045 * cbarp;
            double sH = 1 + 0.015 * cbarp * T;

            double rT = -2 * Math.Sqrt(Math.Pow(cbarp, 7) / (Math.Pow(cbarp, 7) + Math.Pow(25, 7)))
                         * Math.Sin(60 * Math.Exp(-((hbarp - 275) / 25) * ((hbarp - 275) / 25)) / 180 * Math.PI); // Perhaps it wants the angle in degrees here?

            double deltaE = Math.Sqrt((deltaLp / kL / sL) * (deltaLp / kL / sL)
                                    + (deltaCp / kC / sC) * (deltaCp / kC / sC)
                                    + (deltaHp / kH / sH) * (deltaHp / kH / sH)
                                    + rT * deltaCp * deltaHp / kC / sC / kH / sH);

            return deltaE;
        }

        public static double MyColourDifference(Color c1, Color c2)
        {
            // This is the same as CIEDE2000 except when the pixels are quite bright or dark,
            // in which case we revert back to intensity differences. This seems to help with
            // a problem I've been finding with the Lab distance metric in very light/dark
            // regions.

//            double i1 = c1.R * 0.21 + c1.G * 0.72 + c1.B * 0.07;
//            double i2 = c2.R * 0.21 + c2.G * 0.72 + c2.B * 0.07;
//
//            if (Math.Min(i1, i2) < 20 || Math.Max(i1, i2) > 210)
//            {
//                return Math.Abs(i1 - i2) / 255 * 10;
//            }
//            else
//            {
//                return CIEDE2000(RGBtoLab(c1), RGBtoLab(c2));
//            }

            return CIEDE2000(RGBtoLab(c1), RGBtoLab(c2));
        }

        public static double GetIntensityFromRGB(int r, int g, int b)
        {
            // https://en.wikipedia.org/wiki/Relative_luminance, since we're comparing intensities
            // rather than wanting an absolute luminance value.

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        public static double GetIntensityFromRGB(Color c)
        {
            return GetIntensityFromRGB(c.R, c.G, c.B);
        }
    }
}
