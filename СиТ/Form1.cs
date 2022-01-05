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
using System.IO.Compression;

namespace СиТ
{
    public partial class Form1 : Form
    {
        private string Path = null;

        int source_size;
        int compressed_size;
        int decompressed_size;
        Form2 form2;

        string copyFile = "Copy.txt";
        string sourceFile; // исходный файл
        string compressedFile = "Compressed.txt"; // сжатый файл
        string decompressedFile = "Decompressed.txt"; // восстановленный файл

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            WindowState = FormWindowState.Maximized;            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(compressedFile);
            File.Delete(decompressedFile);
            File.Delete(copyFile);
        }

        //private void Open()
        //{

        //}

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)      // Загрузить
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();              // Открываем файл
                ofd.Filter = ".txt|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    File.Delete(compressedFile);
                    File.Delete(decompressedFile);
                    sourceFile = ofd.FileName;


                    string[] buf = File.ReadAllLines(sourceFile, Encoding.Default);  // Читаем файл и переписываем в textbox1
                    for (int i = 0; i < buf.Length; i++)
                        textBox1.Text += buf[i];
                    if (textBox1.Text != "")
                        source_size = textBox1.Text.Length;             // Считаем размер файла 
                    textBox4.Text = source_size.ToString();
                }
            }
            catch (Exception) { }
            
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void button2_Click(object sender, EventArgs e)      // Сжать
        {

            try
            {
                File.Copy(sourceFile, copyFile);
            }
            catch (Exception)
            {
                File.WriteAllText(copyFile, textBox1.Text);
            }
            

                textBox2.Clear();
            //if(textBox1.Text == "")
            // поток для чтения исходного файла
            try
            {
                
                using (FileStream sourceStream = new FileStream(copyFile, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        }
                    }
                }


                string[] buf = File.ReadAllLines(compressedFile, Encoding.Default); //Записываем в textbox2
                for (int i = 0; i < buf.Length; i++)
                    textBox2.Text += buf[i];

                if (textBox1.Text != "")
                        source_size = textBox1.Text.Length;             // Считаем размер файла 
                    textBox4.Text = source_size.ToString();
                if (textBox2.Text != "")
                    compressed_size = textBox2.Text.Length;             // Считаем размер файла
                textBox5.Text = compressed_size.ToString();
                textBox7.Text = (100 - 100 * compressed_size / source_size).ToString() + "%";
            }
            catch (Exception)
            {
                MessageBox.Show("Поле для исходного текста пустое!");
            }
        }

        private void button3_Click(object sender, EventArgs e)                  // Разжать
        {
            textBox3.Clear();
            try
            {
                // поток для чтения из сжатого файла
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create(decompressedFile))
                    {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Чтобы что-то разжать, нужно что-то сжать");
            }

            string[] buf = File.ReadAllLines(decompressedFile, Encoding.Default);     // Записываем в textbox3
            for (int i = 0; i < buf.Length; i++)
                textBox3.Text += buf[i];
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                MessageBox.Show("Чтобы что-то разжать, нужно что-то сжать");
                         // Считаем размер файла
            textBox3.Clear();
            textBox3.Text = textBox1.Text;
            if (textBox3.Text != "")
                textBox6.Text = textBox3.Text.Length.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            form2 = new Form2();
            form2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
