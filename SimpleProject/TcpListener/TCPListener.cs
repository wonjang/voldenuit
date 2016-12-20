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
namespace TcpListener
{
    public partial class TCPListener : Form
    {
        System.Net.Sockets.TcpListener listener;
        IPEndPoint ipEndPoint;
        public TCPListener()
        {
            string ipconfigFilePath = @"ipsettings.conf";
            string ipstring = System.IO.File.ReadAllText(ipconfigFilePath, Encoding.Default);
            string[] parseAddr = ipstring.Split(':');
            IPHostEntry ipHost = Dns.GetHostEntry(parseAddr[0]);
            IPAddress ipAddr = ipHost.AddressList[1];
            ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(parseAddr[1]));

            InitializeComponent();
        }
        public void CreateTcpListener()
        {
            TcpClient client = null;
            NetworkStream ns = null;
            System.IO.StreamReader sr = null;
            try
            {
                listener = new System.Net.Sockets.TcpListener(ipEndPoint);
                listener.Start();
                while(true)
                {
                    client = listener.AcceptTcpClient();
                    ns = client.GetStream();
                    sr = new System.IO.StreamReader(ns);
                    while(client.Connected)
                    {
                        string data = sr.ReadLine();
                        if (data != null)
                        {
                           textBox1.AppendText(data + "\r\n");
                        }
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateTcpListener();
        }
    }
}
