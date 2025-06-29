using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Bob
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UdpClient udp = new UdpClient(3000);
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        byte[] bobkey = { 1, 2, 3, 4, 5, 6, 7, 8 };

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (udp.Available > 0)
            {
                byte[] rawdata = udp.Receive(ref ep);
                string rawstring = Encoding.ASCII.GetString(rawdata);
                string[] token = rawstring.Split(':');

                switch (token[0])
                {
                    case "0":
                        break;
                    case "1":
                        byte[] data = Encoding.ASCII.GetBytes(token[1]);
                        HMACMD5 mac = new HMACMD5();
                        mac.Key = bobkey;
                        byte[] macva = mac.ComputeHash(data);
                        if (BitConverter.ToString(macva) == token[2])
                        {
                            textBox1.Text += token[1] + Environment.NewLine;
                        }
                        break;
                }
            }
        }
    }
}
