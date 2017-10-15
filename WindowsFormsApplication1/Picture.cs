using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Picture : IDisposable
    {
        private Bitmap _bitmap;
        private int[] _grayLevel = new int[256];

        public int[] grayLevel
        {
            get
            {
                _grayLevel = new int[256];

                for (int i = 0; i < bitmap.Width - 1; i++)
                {
                    for (int j = 0; j < bitmap.Height - 1; j++)
                    {
                        var pixel = bitmap.GetPixel(i, j);
                        int R = (int)pixel.R;
                        int G = (int)pixel.G;
                        int B = (int)pixel.B;

                        _grayLevel[(R + G + B) / 3]++;
                    }
                }

                return _grayLevel;
            }
        }

        public Bitmap bitmap { get { return _bitmap; } set { _bitmap = value; } }

        public void SetAllPixelsToGrayLevel()
        {
            for (int w = 0; w < bitmap.Width; w++)
            {
                for (int h = 0; h < bitmap.Height; h++)
                {
                    var pixel = bitmap.GetPixel(w, h);
                    int gray = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                    var color = Color.FromArgb(gray, gray, gray);

                    bitmap.SetPixel(w, h, color);
                }
            }
        }

        public async Task SetAllPixelsToGrayLevelAsync()
        {
            var list = new List<Task>();

            using (var foo = new Bitmap(bitmap))
            {
                for (int w = 0; w < foo.Width; w++)
                {
                    for (int h = 0; h < foo.Height; h++)
                    {
                        var pixel = foo.GetPixel(w, h);
                        int gray = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                        var color = Color.FromArgb(gray, gray, gray);

                        await Task.Run(() => bitmap.SetPixel(w, h, color));
                    }
                }
            }
        }

        public void SetAsNegative()
        {
            for (int w = 0; w < bitmap.Width; w++)
            {
                for (int h = 0; h < bitmap.Height; h++)
                {
                    var pixel = bitmap.GetPixel(w, h);
                    var color = Color.FromArgb(((int)pixel.R - 255) * -1, ((int)pixel.G - 255) * -1, ((int)pixel.B - 255) * -1);

                    bitmap.SetPixel(w, h, color);
                }
            }
        }

        //Allow an Image to be accessed by several threads
        // O intutito disso foi saciar a minha curiosidade de como fazer esse mesmo processo porém de forma mais rapida
        // Muito boa a explicação e da até uma introdução de como trabalhar com uma imagens de uma forma paralela
        // Segue o link abaixo
        //https://stackoverflow.com/questions/21497537/allow-an-image-to-be-accessed-by-several-threads
        public void SetAllPixelsToGrayLevelParallel()
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel

            var buffer = new byte[data.Width * data.Height * depth];

            //copy pixels to buffer
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            Parallel.Invoke(
                () => {
                    //upper-left
                    Process(buffer, 0, 0, data.Width, data.Height / 2, data.Width, depth);
                        },
                () => {
                    //lower-right
                    Process(buffer, 0, data.Height / 2, data.Width, data.Height, data.Width, depth);
                }
            );

            //Copy the buffer back to image
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

            bitmap.UnlockBits(data);
        }

        private void Process(byte[] buffer, int x, int y, int endx, int endy, int width, int depth)
        {
            for (int i = x; i < endx; i++)
            {
                for (int j = y; j < endy; j++)
                {
                    var offset = ((j * width) + i) * depth;
                    // Dummy work    
                    // To grayscale (0.2126 R + 0.7152 G + 0.0722 B)
                    var b = 0.2126 * buffer[offset + 0] + 0.7152 * buffer[offset + 1] + 0.0722 * buffer[offset + 2];
                    buffer[offset + 0] = buffer[offset + 1] = buffer[offset + 2] = (byte)b;
                }
            }
        }

        public void Dispose()
        {
            _bitmap = null;
            _grayLevel = new int[0];
        }
    }
}
