using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2(Bitmap bitmap)
        {
            InitializeComponent();

            using (var foo = new Picture())
            {
                foo.bitmap = new Bitmap(bitmap);

                foo.SetAllPixelsToGrayLevel();

                pictureBox3.Image = foo.bitmap;

                var grayLevel = foo.grayLevel;

                var degrade = new Bitmap(256, 40);

                for (int i = 0; i < degrade.Width; i++)
                {
                    for (int j = 0; j < degrade.Height; j++)
                    {
                        degrade.SetPixel(i, j, Color.FromArgb(i, i, i));
                    }
                }

                pictureBox1.Image = degrade;

                var histograma = new Bitmap(256, 400);

                var grayLevelMax = grayLevel.Max();

                var bar = new int[256];

                for (int i = 0; i < 256; i++)
                {
                    bar[i] = (int)(Math.Log10(grayLevel[i]) * 100 / Math.Log10(grayLevelMax));
                }

                var blackColor = Color.FromArgb(0, 0, 0);

                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < bar[i]; j++)
                    {
                        histograma.SetPixel(i, 100 - j, blackColor);
                    }
                }

                pictureBox2.Image = histograma;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
