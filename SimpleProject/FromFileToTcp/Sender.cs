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
using System.Threading;

namespace FromFileToTcp
{
    public partial class Sender : Form
    {
        string[] data;
        TcpClient client;

        public Sender ()
        {
            string confFilePath = @"FromFileToTcp.conf";
            string textValue = System.IO.File.ReadAllText(confFilePath, Encoding.Default);
            //read data file 
            data = System.IO.File.ReadAllLines(textValue);
            InitializeComponent();

        }


        private void connect_Click (object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                string ipconfigFilePath = @"ipsettings.conf";
                string ipstring = System.IO.File.ReadAllText(ipconfigFilePath, Encoding.Default);
                string[] parseAddr = ipstring.Split(':');
                IPHostEntry ipHost = Dns.GetHostEntry(parseAddr[0]);
                IPAddress ipAddr = ipHost.AddressList[1];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(parseAddr[1]));
                client.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
        private void send_Click (object sender, EventArgs e)
        {
            Task sendProc = new Task(() => this.SendPROC());
            sendProc.Start();
        }

        public void SendPROC()
        {
            try
            {
                NetworkStream ns = null;
                System.IO.StreamWriter sw = null;
                ns = client.GetStream();
                sw = new System.IO.StreamWriter(ns);
                for (int i = 0; i < data.Length; i++)
                {
                    sw.WriteLine(data[i]);
                    sw.Flush();
                    Thread.Sleep(100);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
        private void disconnect_Click (object sender, EventArgs e)
        {
            try
            {
                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
    }
}
