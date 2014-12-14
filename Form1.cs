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
        String xbmpPath;
        Bitmap ximage;
        public Form1()
        {
            InitializeComponent();
        }
        private String string2bin(String wej)
        {
            byte[] btText;
            btText = System.Text.Encoding.ASCII.GetBytes(wej); 
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
        private String bit2string(int[] tab)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == 0)
                {
                    sb.Append(0);
                }
                else
                {
                    sb.Append(1);
                } 
            }
            return sb.ToString();
        }
        private Bitmap hide(Bitmap image, String text)
        {  
            String textBits = string2bin(textBox1.Text);
            int count = 0, k=textBits.Length;
            int R=0,G=0,B=0;
            progressBar1.Maximum = textBits.Length;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    progressBar1.Value = count;
                    progressBar1.Update();
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
        private String read(Bitmap image)
        {
            int[] tab = new int[image.Width*image.Height*3];
            BitArray bitsTab = new BitArray(tab.Length);
            String bitText;
            String Text;
            progressBar2.Maximum = image.Width * image.Height * 3;

            int R = 0, G = 0, B = 0;
            int count = 0;

            for (int i = 0; i < image.Width; i++)
            {
                progressBar2.Value = count;

                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;
                    if (R % 2 == 0)
                    {
                        tab[count] = 0;
                    }
                    if (R % 2 == 1)
                    {
                        tab[count] = 1;
                    }
                    if (G % 2 == 0)
                    {
                        tab[count+1] = 0;
                    }
                    if (G % 2 == 1)
                    {
                        tab[count+1] = 1;
                    }
                    if (B % 2 == 0)
                    {
                        tab[count+2] = 0;
                    }
                    if (B % 2 == 1)
                    {
                        tab[count+2] = 1;
                    }
                    count = count + 3;
                }
            }

            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == 0)
                {
                    bitsTab[i] = false;
                }
                else
                {
                    bitsTab[i] = true;
                }
            }

            bitText = bit2string(tab);



            return Text = binaryToString(bitText);
        }
        private String binaryToString(String binary)
        {
            byte[] bArr = binarytoBytes(binary);
            return Encoding.GetEncoding("windows-1250").GetString(bArr);
        }
        private byte[] binarytoBytes(string bitString)
        {
            return Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();
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

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Steganografia \nMarcin Gluza \n2014 \n\ngumball300@gmail.com", "Autor");
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "Image Files(*.bmp)| *.bmp";
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        textBox3.Text = open.FileName;
                        xbmpPath = open.FileName;
                        Bitmap bit = new Bitmap(open.FileName);
                        pictureBox2.Image = bit;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd. Nie można odczytać wskazenego pliku! " + ex.Message + "\n");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap ximage = new Bitmap(xbmpPath);
            String text = read(ximage);
            textBox4.Text = text;
        }

   
    }
}

