using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public int[] histogram_array = new int[256];
        public Bitmap img;
        public Bitmap img_gray;
        public Bitmap img_otsu;
        public int Height ;
        public int Width;
        public string img_name = "sample_motorcycle.bmp";
        public UInt64 MaxHistogram = 0;
        private OpenFileDialog openfile = new OpenFileDialog();

        public Color GetGrayColor(Color input)
        {
            Color result = new Color();
            double graycolor = new double();
            graycolor = input.R * 0.299 + input.G * 0.587 + input.B * 0.114;
            result = Color.FromArgb((int)graycolor, (int)graycolor, (int)graycolor);
            return result;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openfile.Filter = "PNG(*.PNG)|*.png|" + "JPG(*.JPG)|*.jpg|" + "BMP(*.BMP)|*.bmp|" + "所有檔案|*.*";
         //   openfile.InitialDirectory = "C:";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                img_name = openfile.FileName;
            }
            img = new Bitmap(img_name);
            Height = img.Height;
            Width = img.Width;
            img_gray = new Bitmap(Width, Height);
            img_otsu = new Bitmap(Width, Height);
            pictureBox1.LoadAsync(img_name);
            textBox1.Text = img.PixelFormat.ToString();
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x, y;
            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    img_gray.SetPixel(x, y, GetGrayColor(img.GetPixel(x, y)));
                }
            }
            pictureBox1.Image = img_gray;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(this);
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double q1=0, q2=0;
            double[] p = new double[256];
            double u1=0, u2=0;
            double compare=0;
            double MaxBuffer=0;
            int i, itotal,T=0;
            for (itotal = 0; itotal <= 255; itotal++)
            {
                q1 = 0;
                q2 = 0;
                u1 = 0;
                u2 = 0;
                for (i = 0; i <= 255; i++)
                {
                    p[i] = (double)((double)histogram_array[i] / (double)MaxHistogram);
                }
                for (i = 0; i <= 255; i++)
                {
                    if (itotal >= i)
                    {
                        q1 += p[i];
                    }
                    q2 = 1 - q1;
                }
                for (i = 0; i <= 255; i++)
                {
                    if (itotal >= i)
                    {
                        u1 += (i * p[i] / q1);
                    }
                    else if (itotal <= i)
                    {
                        u2 += (i * p[i] / q2);
                    }
                }
                compare = q1 * q2 * Math.Pow(u1 - u2, 2);
                if (compare > MaxBuffer)
                {
                    MaxBuffer = compare;
                    T = itotal;
                    textBox2.Text = T.ToString();
                }
            }
            int x, y;
            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    if (img_gray.GetPixel(x, y).R >= T)
                    {
                        img_otsu.SetPixel(x, y, Color.White);
                    }
                    else if (img_gray.GetPixel(x, y).R < T)
                    {
                        img_otsu.SetPixel(x, y, Color.Black);
                    }

                }
            }
            pictureBox1.Image = img_otsu;

        }
        
    }
}
