using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Picture picture = new Picture();
        myPixels myPixel;
        Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 0;
            label1.Text = "(X = 0, Y = 0)";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int workerThreads, complete;
            ThreadPool.GetMinThreads(out workerThreads, out complete);

            Console.WriteLine(workerThreads);
            ThreadPool.SetMinThreads(100, complete);
        }

        // https://docs.microsoft.com/pt-br/dotnet/csharp/async
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    picture.bitmap = await Task.Run(() => new Bitmap(openFileDialog1.FileName));
                    myPixel = new myPixels(picture.bitmap);
                }
            }
            catch (Exception) { }
            finally
            {
                pictureBox1.Image = picture.bitmap;
                pictureBox1.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var color = Color.FromArgb(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text));
            picture.bitmap.SetPixel(50, 50, color);
            pictureBox1.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var color = Color.FromArgb(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text));

            for (int w = 0; w < picture.bitmap.Width; w += 3)
            {
                for (int h = 0; h < picture.bitmap.Height; h += 3)
                {
                    if (h % 2 == 0 && w % 2 != 1)
                    {
                        picture.bitmap.SetPixel(w, h, color);
                    }

                    pictureBox1.Refresh();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                button5.Enabled = false;

                for (int w = 0; w < picture.bitmap.Width; w++)
                {
                    for (int h = 0; h < picture.bitmap.Height; h++)
                    {
                        var pixel = picture.bitmap.GetPixel(w, h);
                        int gray = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                        var color = Color.FromArgb(gray, gray, gray);

                        await Task.Run(() => picture.bitmap.SetPixel(w, h, color));
                    }

                    pictureBox1.Refresh();
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                button5.Enabled = true;
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Application.EnableVisualStyles();

                loadBarStart();

                var bitmap = await Task.Run(() => new Bitmap(picture.bitmap));
                var foo = await Task.Run(() => new Form2(bitmap));

                loadBarStop();

                foo.Show();
            }
            catch (Exception) { }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                label1.Text = string.Format("(X = {0}, Y = {1})", e.X.ToString(), e.Y.ToString());

                if (picture.bitmap != null && picture.bitmap.Width >= (int)e.X && picture.bitmap.Height >= (int)e.Y)
                {
                    var pixel = picture.bitmap.GetPixel((int)e.X, (int)e.Y);

                    panel1.BackColor = pixel;

                    textBox1.Text = pixel.R.ToString();
                    textBox2.Text = pixel.G.ToString();
                    textBox3.Text = pixel.B.ToString();
                }
            }
            catch (Exception) { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //picture.SetAllPixelsToGrayLevel();
            //pictureBox1.Refresh();

            for (int w = 0; w < picture.bitmap.Width; w++)
            {
                for (int h = 0; h < picture.bitmap.Height; h++)
                {
                    var pixel = picture.bitmap.GetPixel(w, h);
                    int gray = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                    var color = Color.FromArgb(gray, gray, gray);

                    picture.bitmap.SetPixel(w, h, color);
                }

                pictureBox1.Refresh();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            loadBarStart();
            picture.SetAllPixelsToGrayLevelParallel();
            pictureBox1.Refresh();
            loadBarStop();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        // https://stackoverflow.com/questions/312936/windows-forms-progressbar-easiest-way-to-start-stop-marquee
        private void loadBarStart()
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;
        }

        private void loadBarStop()
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void button8_Click(object sender, EventArgs e)
        {
            loadBarStart();

            for (int w = 0; w < picture.bitmap.Width; w++)
            {
                for (int h = 0; h < picture.bitmap.Height; h++)
                {
                    var pixel = picture.bitmap.GetPixel(w, h);
                    var color = Color.FromArgb(((int)pixel.R - 255) * -1, ((int)pixel.G - 255) * -1, ((int)pixel.B - 255) * -1);

                    await Task.Run(() => picture.bitmap.SetPixel(w, h, color));
                }
            }

            pictureBox1.Refresh();
            loadBarStop();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            loadBarStart();

            myPixel.SetInverseColor(true);

            pictureBox1.BackgroundImage = myPixel.bmpDestino;

            pictureBox1.Refresh();
            loadBarStop();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            loadBarStart();

            myPixel.SetGrayScaleCoefs(40, 40, 20, true);

            pictureBox1.BackgroundImage = myPixel.bmpDestino;

            pictureBox1.Refresh();
            loadBarStop();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var gray = new Grayscale(.3, .59, .11);

            pictureBox1.Image = gray.Apply(picture.bitmap);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SaltAndPepperNoise saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates = new SaltAndPepperNoise(50);
            saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates.ApplyInPlace(picture.bitmap);

            pictureBox1.Image = picture.bitmap;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Grayscale.CommonAlgorithms.BT709.Apply(picture.bitmap);
            pictureBox1.Image = picture.bitmap;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            SaltAndPepperNoise saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates = new SaltAndPepperNoise((int)numericUpDown1.Value);

            bmp = new Bitmap(picture.bitmap);

            saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates.ApplyInPlace(bmp);

            pictureBox1.Image = bmp;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SaltAndPepperNoise saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates = new SaltAndPepperNoise((int)trackBar1.Value);

            bmp = new Bitmap(picture.bitmap);

            saltAndPapperNoiseTransformerStarterPackLimitedEditionByBillGates.ApplyInPlace(bmp);

            pictureBox1.Image = bmp;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            var histogramEqualization = new HistogramEqualization();

            histogramEqualization.ApplyInPlace(picture.bitmap);

            pictureBox1.Image = picture.bitmap;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            button15_Click(sender, e);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int low =  trackBar2.Value,
                high = trackBar3.Value;
            var colorTable = new int[256];

            using (var newPicture = new Bitmap(picture.bitmap))
            {
                if (radioButton1.Checked)
                {
                    double a, b;
                    int range = high - low;
                    a = 255.0 / range;
                    b = -a * low;
                    int intVal;
                    for (int i = 0; i < 256; ++i)
                    {
                        if (i <= low)
                            colorTable[i] = 0;
                        else if (i > high)
                            colorTable[i] = 255;
                        else
                        {
                            intVal = Convert.ToInt32(a * i + b);
                            if (intVal > 255) intVal = 255;
                            if (intVal < 0) intVal = 0;
                            colorTable[i] = (byte)intVal;
                        }
                    }

                    for (int i = 0; i < newPicture.Width; i++)
                    {
                        for (int i2 = 0; i2 < newPicture.Height; i2++)
                        {
                            var pixel = newPicture.GetPixel(i, i2);
                            var pixelArgb = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                            var color = colorTable[pixelArgb];

                            newPicture.SetPixel(i, i2, Color.FromArgb(color, color, color));
                        }
                    }
                }
                else if (radioButton3.Checked)
                {
                    double scale1 = Math.Log10(256.0) / 255.0;
                    double a, b;
                    a = Math.Exp(scale1 * low) - 1.0;
                    b = Math.Exp(scale1 * high) - 1.0;
                    double scale2 = 255.0 / (b - a);
                    int intVal;
                    for (int i = 0; i < 256; ++i)
                    {
                        if (i <= low)
                            colorTable[i] = 0;
                        else if (i > high)
                            colorTable[i] = 255;
                        else
                        {
                            intVal = Convert.ToInt32(Math.Round(scale2 * (Math.Exp(scale1
                           * i) - 1.0 - a)));
                            if (intVal > 255) intVal = 255;
                            if (intVal < 0) intVal = 0;
                            colorTable[i] = (byte)intVal;
                        }
                    }

                    for (int i = 0; i < newPicture.Width; i++)
                    {
                        for (int i2 = 0; i2 < newPicture.Height; i2++)
                        {
                            var pixel = newPicture.GetPixel(i, i2);
                            var pixelArgb = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                            var color = colorTable[pixelArgb];

                            newPicture.SetPixel(i, i2, Color.FromArgb(color, color, color));
                        }
                    }
                }
                else if (radioButton4.Checked)
                {
                    double a, b;
                    double lHigh = Math.Log10(high + 1.0);
                    double lLow = Math.Log10(low + 1.0);
                    double range = lHigh - lLow;
                    a = 255.0 / range;
                    b = -a * lLow;
                    int intVal;
                    for (int i = 0; i < 256; ++i)
                    {
                        if (i <= low)
                            colorTable[i] = 0;
                        else if (i > high)
                            colorTable[i] = 255;
                        else
                        {
                            intVal = Convert.ToInt32(a * Math.Log10(i + 1) + b);
                            if (intVal > 255) intVal = 255;
                            if (intVal < 0) intVal = 0;
                            colorTable[i] = (byte)intVal;
                        }
                    }

                    for (int i = 0; i < newPicture.Width; i++)
                    {
                        for (int i2 = 0; i2 < newPicture.Height; i2++)
                        {
                            var pixel = newPicture.GetPixel(i, i2);
                            var pixelArgb = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                            var color = colorTable[pixelArgb];

                            newPicture.SetPixel(i, i2, Color.FromArgb(color, color, color));
                        }
                    }
                }
                else
                {
                    double gamma = Convert.ToDouble(comboBox1.Text);
                    double a, b;
                    a = Math.Pow(low, gamma);
                    b = Math.Pow(high, gamma);
                    double scale = 255.0 / (b - a);
                    int intVal;
                    for (int i = 0; i < 256; ++i)
                    {
                        if (i <= low)
                            colorTable[i] = 0;
                        else if (i > high)
                            colorTable[i] = 255;
                        else
                        {
                            intVal = Convert.ToInt32(Math.Round(scale * (Math.Pow(i,
                           gamma) - a)));
                            if (intVal > 255) intVal = 255;
                            if (intVal < 0) intVal = 0;
                            colorTable[i] = (byte)intVal;
                        }
                    }

                    for (int i = 0; i < newPicture.Width; i++)
                    {
                        for (int i2 = 0; i2 < newPicture.Height; i2++)
                        {
                            var pixel = newPicture.GetPixel(i, i2);
                            var pixelArgb = ((int)pixel.R + (int)pixel.G + (int)pixel.B) / 3;
                            var color = colorTable[pixelArgb];

                            newPicture.SetPixel(i, i2, Color.FromArgb(color, color, color));
                        }
                    }
                }

                pictureBox1.Image = newPicture;
                pictureBox1.Refresh();
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            button15_Click(sender, e);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(radioButton5.Checked)
                button15_Click(sender, e);
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
