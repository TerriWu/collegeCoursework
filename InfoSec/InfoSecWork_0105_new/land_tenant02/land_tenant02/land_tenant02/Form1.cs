using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Data.OleDb;

namespace land_tenant02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //接收    
        UdpClient udp = new UdpClient(3002);
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

        string land_publickey;
        byte[] land_signdata;
        Boolean rigister_ft, revise=false;

        public void send_to(string data_str, string ip, int port)
        {
            UdpClient udp = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            byte[] data = Encoding.UTF8.GetBytes(data_str);
            udp.Send(data, data.Length, ep);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //與資料庫比對使用者是否註冊過了 如果已註冊label4顯示出已註冊 沒註冊label4顯示註冊成功並顯示groupbox2
            //如果是房客註冊登入groupbox2不會顯示上傳紐

            //房東要生成金鑰，並把公鑰傳給伺服器，伺服器在傳到資料庫，私鑰則存入作業系統中

            if (radioButton1.Checked) //房東
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("使用者名稱或密碼不能為空！");
                }

               


                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                //私鑰存入電腦中
                //StreamWriter sw = new StreamWriter("c:/private.txt");
                StreamWriter sw = new StreamWriter("C:/Users/s0979/OneDrive/桌面/private.txt");
                sw.Write(rsa.ToXmlString(true));
                sw.Close();

                //公鑰存入資料庫中 先傳給伺服器
                send_to("register_land:" + rsa.ToXmlString(false) + ":" + textBox1.Text + ":" + textBox2.Text, "127.0.0.1", 3000);

                //顯示契約內容groupbox 不顯示簽章確認 
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = false;


            }

            if (radioButton2.Checked)//房客
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("使用者名稱或密碼不能為空！");
                }

                //姓名 身分證傳入資料庫 先傳給伺服器
                send_to("register_tenant01:" + textBox1.Text + ":" + textBox2.Text, "127.0.0.1", 3000);

                
                if (rigister_ft == false)
                {
                    send_to("register_tenant01:" + textBox1.Text + ":" + textBox2.Text, "127.0.0.1", 3000);
                }

                while (rigister_ft == true)
                {
                    //公鑰存入資料庫中 先傳給伺服器
                    send_to("register_tenant:" + textBox1.Text + ":" + textBox2.Text+":"+textBox10.Text, "127.0.0.1", 3000);
                    //this.Text = "ok";
                    //顯示契約內容groupbox 不顯示簽章確認sign 上傳button
                    groupBox4.Visible = true;
                    groupBox3.Visible = true;
                    groupBox2.Visible = false;
                    button5.Visible = false;
                    label4.Visible = false;
                    return;
                }

                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (udp.Available > 0)
            {
                byte[] rsw_data = udp.Receive(ref ep);
                string[] token = Encoding.UTF8.GetString(rsw_data).Split(':');

                switch (token[0])
                {
                    case "land_to_tenant_sign":
                        land_signdata =Convert.FromBase64String(token[2]);
                        land_publickey = token[1]; //land_signdata = token[2]; 
                        string land_menomy = token[3], land_address = token[4], land_date = token[5], msg = token[6];
                        textBox9.Text = land_menomy; textBox7.Text = land_address; textBox6.Text = land_date; label21.Text = msg;
                         // test textBox7.Text = token[1];
                        break;

                    case "user_tenant":
                        if (token[1] == "此身分已註冊")
                        {
                            label4.Visible = true;
                            label4.Text = token[1];
                            rigister_ft = false;
                        }
                        if (token[1] == "此身分未註冊")
                        {
                            rigister_ft = true;
                            label4.Visible = false;
                        }
                        break;

                    case "tenant_select_reult":
                        textBox8.Text = "每月租金：" + token[1] + "台幣" + Environment.NewLine + "承租地址："
                            + token[2] + Environment.NewLine + "簽約時間：" + token[3] + Environment.NewLine + "承租人：" + token[4] + Environment.NewLine + "出租人：" + token[5];
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//登入
        {
           
            if (radioButton1.Checked) //房東
            {
                //顯示契約內容groupbox 不顯示簽章確認 
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = false;

            }

            if (radioButton2.Checked)//房客
            {

                //姓名 身分證傳入資料庫 先傳給伺服器
                send_to("register_tenant01:" + textBox1.Text + ":" + textBox2.Text, "127.0.0.1", 3000);


                if (rigister_ft == true)
                {
                    send_to("register_tenant01:" + textBox1.Text + ":" + textBox2.Text, "127.0.0.1", 3000);
                }

                while (rigister_ft == false)
                {
                    //顯示契約內容groupbox 不顯示簽章確認sign 上傳button
                    groupBox4.Visible = true;
                    groupBox3.Visible = true;
                    groupBox2.Visible = false;
                    button5.Visible = false;
                    label4.Visible = false;
                    return;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //拿取房東私鑰做簽章
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            StreamReader sr = new StreamReader("c:/private.txt");
            rsa.FromXmlString(sr.ReadLine());
            sr.Close();

            byte[] msg = Encoding.UTF8.GetBytes(textBox3.Text + textBox4 + textBox5.Text);
            byte[] signature = rsa.SignData(msg, "SHA1");
            String signature_str = Convert.ToBase64String(signature);

            //signature_str傳伺服器再做房客簽章
            send_to("land_sign:" + signature_str + ":" + textBox3.Text + ":" + textBox4.Text + ":" + textBox5.Text, "127.0.0.1", 3000);
        }

        private void button6_Click(object sender, EventArgs e)//查詢
        {
            send_to("tenant_select_con:"+textBox1.Text, "127.0.0.1", 3000);
        }

        private void button5_Click(object sender, EventArgs e)//修改
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //簽章確認，假如沒有填資料，label顯示提示字，否則執行.......???
            if (textBox9.Text == "" || textBox7.Text == "" || textBox6.Text == "")
            {
                label27.Text = "訊息提示：未填寫租金";
                label27.Visible = true;

                label26.Text = "訊息提示：未填寫地址";
                label26.Visible = true;

                label25.Text = "訊息提示：未填寫時間";
                label25.Visible = true;
            }



            string land_menomy = textBox9.Text, land_address = textBox7.Text, land_date = textBox6.Text;
            //byte[] new_land_signdata = Encoding.UTF8.GetBytes(land_signdata);
            byte[] contract = Encoding.UTF8.GetBytes(land_menomy + land_address + land_date);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(land_publickey);
            bool result = rsa.VerifyData(contract, "SHA1", land_signdata);
            label22.Text = result.ToString();

            if (revise == true)//修改
            {
                if (result == true)
                {
                    send_to("tenant_insert_revise:" + land_menomy + ":" + land_address + ":" + land_date + ":" + textBox1.Text, "127.0.0.1", 3000);
                    result = false;
                }
                revise = false;
            }

            if (result == true)//上傳前的驗證
            {
                send_to("tenant_insert:" + land_menomy + ":" + land_address + ":" + land_date+":"+textBox1.Text, "127.0.0.1", 3000);
                result = false;

            }
            else
            {
                label9.Visible = true;
            }
        }
    }
}
