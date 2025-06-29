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
using System.Data.OleDb;

namespace land_tenant_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //接收
        UdpClient udp = new UdpClient(3000);
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

        public void send_to(string data_str, string ip, int port)
        {
            UdpClient udp = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            byte[] data = Encoding.UTF8.GetBytes(data_str);
            udp.Send(data, data.Length, ep);

        }

        

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (udp.Available > 0)
            {
                byte[] rsw_data = udp.Receive(ref ep);
                string[] token = Encoding.UTF8.GetString(rsw_data).Split(':');

                switch (token[0])
                {


                    case "rigister":
                        string name = token[1], id = token[2];
                        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand cmd = new OleDbCommand("select * from ITDB where landname=@landname", conn);//資料庫連接
                        cmd.Parameters.Add(new OleDbParameter("@landname", name));
                        conn.Open();
                        OleDbDataReader dr = cmd.ExecuteReader();//讀取
                        if (dr.Read())
                        {
                            label8.Text = (string)dr["landname"];
                            label9.Text = (string)dr["landidcard"];
                            ///label4.Text = "訊息提示：此身分已註冊";
                            send_to("user_land:" + "此身分已註冊", "127.0.0.1", 3001);
                            conn.Close();
                        }
                        else
                        {
                            send_to("user_land:" + "此身分未註冊", "127.0.0.1", 3001);
                        }
                        break;

                    case "register_tenant01":
                        string tenatname = token[1], tenantid = token[2];
                        OleDbConnection tenant_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand tenant_cmd = new OleDbCommand("select * from ITDB where tenantname=@tenantname", tenant_conn);//資料庫連接
                        tenant_cmd.Parameters.Add(new OleDbParameter("@tenantname", tenatname));
                        tenant_conn.Open();
                        OleDbDataReader tenant_dr = tenant_cmd.ExecuteReader();//讀取
                        if (tenant_dr.Read())
                        {
                            label8.Text = (string)tenant_dr["tenantname"];
                            label9.Text = (string)tenant_dr["tenantidcard"];
                            ///label4.Text = "訊息提示：此身分已註冊";
                            send_to("user_tenant:" + "此身分已註冊", "127.0.0.1", 3002);
                            tenant_conn.Close();
                        }
                        else
                        {
                            send_to("user_tenant:" + "此身分未註冊", "127.0.0.1", 3002);
                        }
                        break;



                    case "register_land": //註冊房東
                        string land_publickey = token[1], land_name = token[2], land_idcard = token[3];
                        label1.Text = land_name + land_idcard;
                        label2.Text = land_publickey;

                        //把房東公鑰 姓名 身分證存入資料庫 insert into
                        OleDbConnection register_land_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection register_land_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand register_land_cmd = new OleDbCommand("insert into ITDB ([landname],[landidcard],[landpublickey]) values(@landname,@landidcard,@landpublickey)", register_land_conn);//資料庫連接
                        register_land_cmd.Parameters.Add(new OleDbParameter("@landname", land_name));
                        register_land_cmd.Parameters.Add(new OleDbParameter("@landidcard", land_idcard));
                        register_land_cmd.Parameters.Add(new OleDbParameter("@landpublickey", land_publickey));
                        register_land_conn.Open();
                        register_land_cmd.ExecuteNonQuery(); // insert、delete、update：ExecuteNonQuery()
                        register_land_conn.Close();
                        break;

                    case "register_tenant": //註冊租戶
                        string tenant_name = token[1], tenant_idcard = token[2], tenant_landpublickey=token[3];
                        label3.Text = tenant_name + tenant_idcard;
                        label10.Text = tenant_landpublickey;

                        //把房客 姓名 身分證存入資料庫 insert into
                        OleDbConnection register_tenant_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection register_land_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand register_tenant_cmd = new OleDbCommand("update ITDB set tenantname=@tenantname,tenantidcard=@tenantidcard where landpublickey=@landpublickey", register_tenant_conn);//資料庫連接
                        register_tenant_cmd.Parameters.Add(new OleDbParameter("@tenantname", tenant_name));
                        register_tenant_cmd.Parameters.Add(new OleDbParameter("@tenantidcard", tenant_idcard));
                        register_tenant_cmd.Parameters.Add(new OleDbParameter("@landpublickey", tenant_landpublickey));
                        register_tenant_conn.Open();
                        register_tenant_cmd.ExecuteNonQuery(); // insert、delete、update：ExecuteNonQuery()
                        register_tenant_conn.Close();
                        break;

         
                    case "land_sign":
                        //Convert.FromBase64String(token[1]);
                        string land_sigdata = token[1], land_menomy = token[2], land_address = token[3], land_date = token[4]; string land_publickey_sigdata = "";
;                        label4.Text = land_sigdata;
                        label5.Text = land_menomy;
                        label6.Text = land_address;
                        label7.Text= land_date;

                        //與資料庫拿取房東的公鑰
                        OleDbConnection select_publickey_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection land_select_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand select_publickey_cmd = new OleDbCommand("select * from ITDB where landname=@landname", select_publickey_conn);//資料庫連接
                        select_publickey_cmd.Parameters.Add(new OleDbParameter("@landname", token[5]));
                        select_publickey_conn.Open();
                        OleDbDataReader select_publickey_dr = select_publickey_cmd.ExecuteReader();//讀取
                        if (select_publickey_dr.Read())
                        {
                            label8.Text = (string)select_publickey_dr["landname"];
                            label9.Text = (string)select_publickey_dr["landpublickey"];
                            ///label4.Text = "訊息提示：此身分已註冊";
                            land_publickey_sigdata = (string)select_publickey_dr["landpublickey"];
                            //send_to("user1:" + "此身分已註冊", "127.0.0.1", 3002);
                            select_publickey_conn.Close();
                        }

                        send_to("land_to_tenant_sign:" + land_publickey_sigdata + ":" + land_sigdata + ":" + land_menomy + ":" + land_address + ":" + land_date + ":出租人簽章成功", "127.0.0.1", 3002);

                        //傳房東私鑰 房東signdata 房東打的契約內容
                        string land_publicKey = label2.Text; //test
                        //send_to("land_to_tenant_sign:" + "test", "127.0.0.1", 3002);
                        
                        break;

                    case "land_sign_revise":
                        //與資料庫拿取房東的公鑰
                        string land_publicKey_revise = label2.Text;
                        string land_sigdata_revise = token[1], land_menomy_revise = token[2], land_address_revise = token[3], land_date_revise = token[4];
                        send_to("land_to_tenant_sign_revise:" + land_publicKey_revise + ":" + land_sigdata_revise + ":" + land_menomy_revise + ":" + land_address_revise + ":" + land_date_revise + ":出租人簽章成功", "127.0.0.1", 3002);
                        break;

                    case "tenant_insert_revise"://修改契約內容
                        OleDbConnection tenant_revise = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection tenant_revise = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand tenant_revise_cm = new OleDbCommand("update ITDB contractmoney=@contractmoney,contractaddress=@contractaddress,contractdate=@contractdate where tenantname=@tenantname", tenant_revise);
                        tenant_revise_cm.Parameters.Add(new OleDbParameter("@contractmoney", token[1]));
                        tenant_revise_cm.Parameters.Add(new OleDbParameter("@contractaddress", token[2]));
                        tenant_revise_cm.Parameters.Add(new OleDbParameter("@contractdate", token[3]));
                        tenant_revise_cm.Parameters.Add(new OleDbParameter("@tenantname", token[4]));
                        tenant_revise.Open();
                        tenant_revise_cm.ExecuteNonQuery();
                        tenant_revise.Close();
                        break;

                    //房客簽章後上傳資料tenant_insert
                    case "tenant_insert":
                        OleDbConnection tenant_update = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection tenant_update = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand tenant_update_cm = new OleDbCommand("update ITDB set contractmoney=@contractmoney,contractaddress=@contractaddress,contractdate=@contractdate where tenantname=@tenantname", tenant_update);
                        tenant_update_cm.Parameters.Add(new OleDbParameter("@contractmoney", token[1]));
                        tenant_update_cm.Parameters.Add(new OleDbParameter("@contractaddress", token[2]));
                        tenant_update_cm.Parameters.Add(new OleDbParameter("@contractdate", token[3]));
                        tenant_update_cm.Parameters.Add(new OleDbParameter("@tenantname", token[4]));
                        tenant_update.Open();
                        tenant_update_cm.ExecuteNonQuery();
                        tenant_update.Close();

                        send_to("tenant_insert_ok:" + "承租人已簽章", "127.0.0.1", 3001);
                        break;

                    case "land_select_con":
                        OleDbConnection land_select_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection land_select_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand land_select_cmd = new OleDbCommand("select * from ITDB where landname=@landname", land_select_conn);//資料庫連接
                        land_select_cmd.Parameters.Add(new OleDbParameter("@landname", token[1]));
                        land_select_conn.Open();
                        OleDbDataReader land_select_dr = land_select_cmd.ExecuteReader();//讀取
                        if (land_select_dr.Read())
                        {
                            label8.Text = (string)land_select_dr["landname"];
                            label9.Text = (string)land_select_dr["contractmoney"];
                            ///label4.Text = "訊息提示：此身分已註冊";
                            send_to("land_select_reult:" + land_select_dr["contractmoney"]+":"+land_select_dr["contractaddress"]+":"+land_select_dr["contractdate"] + ":" + land_select_dr["landname"] + ":" + land_select_dr["tenantname"], "127.0.0.1", 3001);
                            //send_to("user1:" + "此身分已註冊", "127.0.0.1", 3002);
                            land_select_conn.Close();
                        }
                        break;

                    case "tenant_select_con":
                        OleDbConnection tenant_select_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\大學\三上\資訊安全與應用\0107今日範圍\資安作業_0105_new\Database1.mdb");
                        //OleDbConnection tenant_select_conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Administrator\Desktop\資安作業_0105_new\Database1.mdb");
                        OleDbCommand tenant_select_cmd = new OleDbCommand("select * from ITDB where tenantname=@tenantname", tenant_select_conn);//資料庫連接
                        tenant_select_cmd.Parameters.Add(new OleDbParameter("@tenantname", token[1]));
                        tenant_select_conn.Open();
                        OleDbDataReader tenant_select_dr = tenant_select_cmd.ExecuteReader();//讀取
                        if (tenant_select_dr.Read())
                        {
                            label8.Text = (string)tenant_select_dr["tenantname"];
                            label9.Text = (string)tenant_select_dr["contractmoney"];
                            ///label4.Text = "訊息提示：此身分已註冊";
                            send_to("tenant_select_reult:" + tenant_select_dr["contractmoney"] + ":" + tenant_select_dr["contractaddress"] + ":" + tenant_select_dr["contractdate"] + ":" + tenant_select_dr["landname"] + ":" + tenant_select_dr["tenantname"], "127.0.0.1", 3002);
                            //send_to("user1:" + "此身分已註冊", "127.0.0.1", 3002);
                            tenant_select_conn.Close();
                        }
                        break;
                }
            }
        }
    }
}
