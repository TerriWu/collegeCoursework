using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace certificate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            X509Store store = new X509Store("my", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection;
            collection = X509Certificate2UI.SelectFromCollection(store.Certificates, "憑證", "請選擇一張憑證", X509SelectionFlag.SingleSelection);
            X509Certificate2 x509 = collection[0];
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PrivateKey;
            byte[] msg = Encoding.ASCII.GetBytes("hello");
            byte[] signture = rsa.SignData(msg, "SHA1");
            RSACryptoServiceProvider rsa1 = (RSACryptoServiceProvider)x509.PublicKey.Key;
            bool signture1 = rsa1.VerifyData(msg, "SHA1", signture);
            MessageBox.Show(signture1.ToString());
        }
    }
}
