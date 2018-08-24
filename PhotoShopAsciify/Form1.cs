using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace PhotoShopAsciify
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        
        Bitmap bmp;
        double normalized;
        //byte [] read;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.ShowDialog();
        }

        private void openFile_FileOk(object sender, CancelEventArgs e)
        {
            bmp = new Bitmap(openFile.FileName);

            StreamReader file = new StreamReader(openFile.FileName);

            file.ReadLine();
            file.Close();

            pictureBox1.Image = new Bitmap(openFile.FileName);


            Color c;
            int result;

            //Greyscale image to second picture box
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {

                    //Grab pixel values and store in C
                    c = bmp.GetPixel(x, y);

                    //Assign color values
                    int r = c.R;
                    int g = c.G;
                    int b = c.B;

                    result = (r + g + b) / 3;

                    
                    //Use average to greyscale the pixels and create a new image
                    bmp.SetPixel(x, y, Color.FromArgb(255, result, result, result));

                    
                }
            }
            //Store greyscale image in the second picture box
            pictureBox2.Image = bmp;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Asciitize Method to show ascii art
            Asciitize(bmp);

            //Store greyscale image in the second picture box
            //pictureBox2.Image = bmp;
        }

        //Asciitize Method reads the bmp, converts to gray scale, and creates ascii art
        public void Asciitize (Bitmap bmp)
        {
            //Builds the ascii art string
            StringBuilder asciitext = new StringBuilder();

            //Holds the normalized value
            double kernel;

            int xSTART = 0;
            int ySTART = 0;

            //Size
            int xS = Convert.ToInt32(xBox.Value);
            int yS = Convert.ToInt32(xBox.Value);

            int columns = bmp.Height / yS;
            int rows = bmp.Width / xS;
            
            //Loops through the bitmap & (x, y)
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    //Calls kernel method
                    kernel = AverageKernel(xSTART, ySTART, xS, yS, bmp);

                    //Appends ascii characters by using the GrayToString Method
                    asciitext.Append(GreyToString(kernel));

                    xSTART += xS;
                }

                xSTART = 0;
                ySTART += yS;
                asciitext.Append("\r\n");
            }

            //Show ascii art in textbox
            richTextBox1.Text = asciitext.ToString();

        }

        //Average Method returns the normalized value (0-1)
        public double AverageColor (Color c)
        {
            //Assign color values
            int a = c.A;
            int r = c.R;
            int g = c.G;
            int b = c.B;

            int avg = (r + g + b) / 3;
            normalized = avg / 255.0;

            //GreyToString(normalized);

            return normalized;
        }


        public double AverageKernel (int xstart, int ystart, int xSize, int ySize, Bitmap bmp)
        {
            //Color variable
            Color c;

           
            double norm;
            double runTotal = 0.0;
         


            //Loops through the bitmap & (x, y)
            for (int j = 0; j < ySize; j++)
            {
                for (int i = 0; i < xSize; i++)
                {
                    //Grab pixel values and store in C
                    c = bmp.GetPixel(xstart, ystart);

                    //Assign color values
                    int r = c.R;
                    int g = c.G;
                    int b = c.B;

                    int result = (r + g + b) / 3;

                    norm = AverageColor(c);

                    //Running total
                    runTotal += norm;

                    xstart++;
                }

                xstart -= xSize;
                ystart ++;
            }

            
            return runTotal / (xSize * ySize);
        }

        //Reads the normalized value and returns the associated string
        public string GreyToString(double value)
        {
            string ascii = "";

            if (value >= 0.90)
            {
                ascii = " ";
            }
            else if(value >= 0.80)
            {
                ascii = ".";
            }
            else if (value >= 0.70)
            {
                ascii = "*";
            }
            else if (value >= 0.60)
            {
                ascii = ":";
            }
            else if (value >= 0.50)
            {
                ascii = "&";
            }
            else if (value >= 0.40)
            {
                ascii = "8";
            }
            else if (value >= 0.30)
            {
                ascii = "#";
            }
            else
            {
                ascii = "@";
            }
            return ascii;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFile.FileName);
                writer.Write(richTextBox1.Text);
                saveFile.Filter = "Text Files|*.txt"; 
                writer.Close();

            }
        }
    }
}
