using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA_KEYTO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = textBox1.Text;
            byte[] data = Encoding.ASCII.GetBytes(msg);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipher = rsa.Encrypt(data, true);
            string cipher_str = Convert.ToBase64String(cipher);

            StreamWriter sw = new StreamWriter("d:/cipher.txt");
            sw.WriteLine(cipher_str);
            sw.Close();

            StreamWriter sw_key = new StreamWriter("d:/key.txt");
            sw_key.Write(rsa.ToXmlString(true));
            sw_key.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            StreamReader sr_key = new StreamReader("d:/key.txt");
            rsa1.FromXmlString(sr_key.ReadLine());
            sr_key.Close();

            StreamReader sr_c = new StreamReader("d:/cipher.txt");
            String cipher_flie = sr_c.ReadLine();
            sr_c.Close();

            byte[] cipher1 = Convert.FromBase64String(cipher_flie);
            byte[] palin1 = rsa1.Decrypt(cipher1, true);
            string palin = Encoding.ASCII.GetString(palin1);
            MessageBox.Show(palin);
        }
    }
}
