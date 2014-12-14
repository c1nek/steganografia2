using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace steganografia2
{
    public partial class Form1 : Form
    {
        String bmpPath;
        Bitmap imageText;
        String xbmpPatch;
        Bitmap ximage;
        public Form1()
        {
            InitializeComponent();
        }
        private String string2bin(String wej)
        {
            byte[] btText;
            btText = System.Text.Encoding.UTF8.GetBytes(wej); 
            Array.Reverse(btText); 
            BitArray bit = new BitArray(btText); 
            StringBuilder sb = new StringBuilder(); 
        
            for (int i = bit.Length - 1; i >= 0; i--) 
                { 
                if (bit[i] == true) 
                    { 
                        sb.Append(1); 
                    } 
                else 
                    { 
                        sb.Append(0); 
                    } 
                }

        return sb.ToString();

        }
        private Bitmap hide(Bitmap image, String text)
        {  
            String textBits = string2bin(textBox1.Text);
            int count = 0, k=textBits.Length;
            int R=0,G=0,B=0;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (textBits.Length == count)
                    {
                        break;
                    }
                    Color pixel = image.GetPixel(i, j);
                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;
                    if (textBits.Length > count+2)
                    {
                        if (R % 2 == 0 && textBits[count] == '1')
                        {
                            R = R + 1;
                        }
                        if (R % 2 == 1 && textBits[count] == '0')
                        {
                            R = R - 1;
                        }
                        if (G % 2 == 0 && textBits[count + 1] == '1')
                        {
                            G = G + 1;
                        }
                        if (G % 2 == 1 && textBits[count + 1] == '0')
                        {
                            G = G - 1;
                        }
                        if (B % 2 == 0 && textBits[count + 2] == '1')
                        {
                            B = B + 1;
                        }
                        if (B % 2 == 1 && textBits[count + 2] == '0')
                        {
                            B = B - 1;
                        }
                    }
                    else if (textBits.Length == count +2)
                    {
                        if (R % 2 == 0 && textBits[count] == '1')
                        {
                            R = R + 1;
                        }
                        if (R % 2 == 1 && textBits[count] == '0')
                        {
                            R = R - 1;
                        }
                        if (G % 2 == 0 && textBits[count + 1] == '1')
                        {
                            G = G + 1;
                        }
                        if (G % 2 == 1 && textBits[count + 1] == '0')
                        {
                            G = G - 1;
                        }
                    }
                    else if (textBits.Length == count +1)
                    {
                        if (R % 2 == 0 && textBits[count] == '1')
                        {
                            R = R + 1;
                        }
                        if (R % 2 == 1 && textBits[count] == '0')
                        {
                            R = R - 1;
                        }
                    }
                    
                    Color temp = Color.FromArgb(R,G,B);
                    image.SetPixel(i, j, temp);
                    count = count + 3;
                    if (textBits.Length == count || textBits.Length < count)
                    {
                        break;
                    }
                    Console.WriteLine(temp);
                }
                if (textBits.Length < count)
                {
                    break;
                }      
            }
            MessageBox.Show("done!");
            pictureBox1.Image = image;
            return image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "Image Files(*.bmp)| *.bmp";
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        textBox2.Text = open.FileName;
                        bmpPath = open.FileName;
                        Bitmap bit = new Bitmap(open.FileName);
                        pictureBox1.Image = bit;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd. Nie można odczytać wskazenego pliku! " + ex.Message + "\n");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            try
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    {
                        pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    MessageBox.Show("Zapisano!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd. Nie można zapisać wskazenego obrazu! \n " + ex.Message);
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(bmpPath);
            imageText = hide(image, textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            textBox1.Text = File.ReadAllText(openFileDialog1.FileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd. Nie można odczytać wskazenego pliku! \n " + ex.Message);
                }
            }
        }
    }
}

