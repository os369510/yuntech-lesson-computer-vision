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
    public partial class Form2 : Form
    {
        Form1 f1 = null;

        public Form2(Form1 Temp)
        {
            InitializeComponent();
            f1 = Temp;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int x, y, i;
            f1.MaxHistogram = 0;
            for (i = 0; i <= 255; i++)
            {
                f1.histogram_array[i] = 0;
            }
            for (y = 0; y < f1.Height; y++)
            {
                for (x = 0; x < f1.Width; x++)
                {
                    f1.histogram_array[(int)(f1.img_gray.GetPixel(x, y).R)]++;
                }
            }
            for (i = 0; i <= 255; i++)
            {
                this.chart1.Series["Value"].Points.AddXY(i.ToString(), f1.histogram_array[i]);
                f1.MaxHistogram += (UInt64)f1.histogram_array[i];
            }                
        }
    }
}
