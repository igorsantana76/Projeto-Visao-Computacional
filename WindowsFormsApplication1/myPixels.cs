using System;
using System.Drawing;
using System.Drawing.Imaging;

public unsafe class myPixels
{
    public Bitmap bmpOrigem;
    public Bitmap bmpDestino;

    public myPixels(Bitmap bmp)
    {
        bmpOrigem = new Bitmap(bmp);
        bmpDestino = new Bitmap(bmp);
    }

    public Color GetPixel(int x,int y)
    {
        int red, green, blue;

        BitmapData bmData = bmpDestino.LockBits(new Rectangle(0, 0, bmpDestino.Width, bmpDestino.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        int stride = bmData.Stride;
        System.IntPtr Scan0 = bmData.Scan0;
        unsafe
        {
            byte* p = (byte*)(void*)Scan0;
            int nOffset = stride - bmpDestino.Width * 3;
            int nWidth = bmpDestino.Width * 3;

            p += y * nWidth + (x * 3);

            red = p[2];
            green = p[1];
            blue = p[0];
        }
        bmpDestino.UnlockBits(bmData);
        return Color.FromArgb(red, green, blue);
    }

    public void SetPixel(int x, int y, Color k)
    {
        BitmapData bmData = bmpDestino.LockBits(new Rectangle(0, 0, bmpDestino.Width, bmpDestino.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        int stride = bmData.Stride;
        System.IntPtr Scan0 = bmData.Scan0;

        byte* p = (byte*)(void*)Scan0;
        int nOffset = stride - bmpDestino.Width * 3;
        int nWidth = bmpDestino.Width * 3;

        p += y * nWidth + (x * 3);

        p[2] = k.R;//Red
        p[1] = k.G;//Green
        p[0] = k.B;//Blue
        
        bmpDestino.UnlockBits(bmData);
        return;
    }

    public void SetGrayScaleCoefs(int pRed, int pGreen, int pBlue, bool keep)
    {
        if (!keep) bmpDestino = new Bitmap(bmpOrigem);

        BitmapData bmData = bmpDestino.LockBits(new Rectangle(0, 0, bmpDestino.Width, bmpDestino.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        int stride = bmData.Stride;
        System.IntPtr Scan0 = bmData.Scan0;

        int c = 0;
        byte[] cor = new byte[3];
        byte*[] pcor = new byte*[3];
        byte* p = (byte*)(void*)Scan0;
        int nOffset = stride - bmpDestino.Width * 3;
        int nWidth = bmpDestino.Width * 3;
        
        for (int y = 0; y < bmpDestino.Height; ++y)
        {
            for (int x = 0; x < nWidth; ++x)
            {
                cor[c] = p[0];
                pcor[c] = p;
                ++p;
                c++;
                if (c > 2)
                {
                    //coeficientes
                    byte gray = (byte)(cor[0] * (pRed / 100.0) + cor[1] * (pGreen / 100.0) + cor[2] * (pBlue / 100.0));

                    *pcor[0] = gray;
                    *pcor[1] = gray;
                    *pcor[2] = gray;

                    c = 0;
                }
            }
            p += nOffset;
        }
        bmpDestino.UnlockBits(bmData);
        return;
    }

    public void SetInverseColor(bool keep)
    {
        if (!keep) bmpDestino = new Bitmap(bmpOrigem);

        BitmapData bmData = bmpDestino.LockBits(new Rectangle(0, 0, bmpDestino.Width, bmpDestino.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        int stride = bmData.Stride;
        System.IntPtr Scan0 = bmData.Scan0;

        int c = 0;
        byte[] cor = new byte[3];
        byte*[] pcor = new byte*[3];
        byte* p = (byte*)(void*)Scan0;
        int nOffset = stride - bmpDestino.Width * 3;
        int nWidth = bmpDestino.Width * 3;

        for (int y = 0; y < bmpDestino.Height; ++y)
        {
            for (int x = 0; x < nWidth; ++x)
            {
                cor[c] = p[0];
                pcor[c] = p;
                ++p;
                c++;
                if (c > 2)
                {
                    *pcor[0] = (byte)(255 - cor[0]);
                    *pcor[1] = (byte)(255 - cor[1]);
                    *pcor[2] = (byte)(255 - cor[2]);

                    c = 0;
                }
            }
            p += nOffset;
        }
        bmpDestino.UnlockBits(bmData);
        return;
    }

}
