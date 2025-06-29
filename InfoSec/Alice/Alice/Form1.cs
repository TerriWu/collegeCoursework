using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;

namespace Alice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void send_mac(string data,string ip, int port)
        {
            byte[] str = Encoding.ASCII.GetBytes(data);
            UdpClient udp = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            udp.Send(str, data.Length, ep);
        }

        byte[] alicekey={1,2,3,4,5,6,7,8};

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(textBox1.Text);
            HMACMD5 mac = new HMACMD5();
            mac.Key = alicekey;
            byte[] macva = mac.ComputeHash(data);
            label1.Text = BitConverter.ToString(macva);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            send_mac("1:" + textBox1.Text + ":" + alicekey, "127.0.0.1", 3000);
        }
    }
}
