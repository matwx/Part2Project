using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.Infrastructure
{
    // Mostly from stack overflow http://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public DirectBitmap(Bitmap bmp)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            Bits = new Int32[bmp.Width * bmp.Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(bmp.Width, bmp.Height, bmp.Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());

            using (var gfx = Graphics.FromImage(Bitmap))
            {
                gfx.DrawImageUnscaled(bmp, 0, 0);
            }
        }

        public Color GetPixel(int x, int y)
        {
            return Color.FromArgb(Bits[x + y * Width]);
        }

        public void SetPixel(int x, int y, Color color)
        {
            Bits[x + y*Width] = color.ToArgb();
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
