using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MsgSend
{

    public partial class MsgSender : Form
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MsgSender));
        public static readonly string dummy0 = "0374MCCFN0TRDATA000000220905007650000090535093                                                                                      00000000000TTRODP11301  G10008800101";
        public static readonly string dummy2 = "          KR4175L9000400000000001";
        public static readonly string dummy5 = "2020161130";
        public static readonly string dummy7 = "00000000.0000000000.00";
        public static readonly string dummy9 = "00010130234600000000000";

        public int count;
        public IPEndPoint ipEndPoint;
        TcpClient client;
        NetworkStream ns ;
        StreamWriter sw ;

        public MsgSender()
        {
            InitializeComponent();

            if (!(File.Exists("ipsettings.txt")))
            {
                MessageBox.Show("파일없음");
            }
            else
            {

                StreamReader sr = new StreamReader("ipsettings.txt");
                string ipsettings = sr.ReadLine();
                string[] parseipsettings = ipsettings.Split(':');
                tbxip.Text = parseipsettings[0];
                tbxPort.Text = parseipsettings[1];
                sr.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logger.Info("button4_Click start ");
            DateTime now = HRDateTime.Now;
            if (tbxType.Text == "" || !(tbxType.Text == "1" || tbxType.Text == "2"))
            {
                MessageBox.Show("타입 잘못입력하셨습니다");
            }
            else if (tbxPrice.Text == "" || !(tbxPrice.Text.Length == 7) || !(tbxPrice.Text.Substring(4,1) == "."))
            {
                MessageBox.Show("가격을 잘못입력하셨습니다 XXXX.XX 로 입력하세요");
            }
            else if (tbxVolumn.Text == "" || Convert.ToInt32(tbxVolumn.Text) < 1 || Convert.ToInt32(tbxVolumn.Text) > 999)
            {
                MessageBox.Show("수량을 잘못입력하셨습니다");
            }
            else
            {

                string price = String.Format("{0:00000000.00}", Convert.ToDouble(tbxPrice.Text));
                string volumn = String.Format("{0:0000000000}", Convert.ToInt32(tbxVolumn.Text));
                string type = tbxType.Text;
                Queue<string> queue = new Queue<string>();
                //0더미, 1주문, 2더미,3가격,4수량,5더미,6시간,7더미,8타입,9더미
                // 0374MCCFN0TRDATA000000220905007650000090535093                                                                                      00000000000TTRODP11301  G100088001010000000002          KR4175L900040000000000100001117.900000000015202016113009053509000000000.0000000000.00100010130234600000000000                                                                             
                for (int i = 0; i < Convert.ToInt32(tbxCount.Text); i++)
                {
                    string time = now.ToString("HHmmssfff");
                    string order_identification = String.Format("{0:0000000000}", ++count);
                    string msg = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", dummy0, order_identification, dummy2, price, volumn, dummy5, time, dummy7, type, dummy9);
                    queue.Enqueue(msg);
                }
                while (queue.Count > 0)
                {
                    string order = queue.Dequeue();
                    if (order != null)
                    {
                        try
                        {
                            sw.WriteLine(order);
                            sw.Flush();
                        }catch(Exception f)
                        {

                        }
                    } 
                }
            }
        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            string ipAddress = tbxip.Text;
            int ipPort = int.Parse(tbxPort.Text);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            try
            {
                client = new TcpClient();

                client.Connect(ipEndPoint);
                ns = client.GetStream();
                sw = new StreamWriter(ns);
                MessageBox.Show("연결되었습니다.");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            try
            {
                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
                MessageBox.Show("Connection Close");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
        }
        private void btnOrder_Click(object sender, EventArgs e)
        {
            logger.Info("button4_Click start ");
            DateTime now = HRDateTime.Now;
            if (tbxType.Text == "" || !(tbxType.Text == "1" || tbxType.Text == "2"))
            {
                MessageBox.Show("유형을 잘못입력하셨습니다");
            }
            else if (tbxPrice.Text == "" || !(tbxPrice.Text.Length == 7) || !(tbxPrice.Text.Substring(4, 1) == "."))
            {
                MessageBox.Show("가격을 잘못입력하셨습니다 XXXX.XX 로 입력하세요");
            }
            else if (tbxVolumn.Text == "" || Convert.ToInt32(tbxVolumn.Text) < 1 || Convert.ToInt32(tbxVolumn.Text) > 999)
            {
                MessageBox.Show("수량을 잘못입력하셨습니다");
            }
            else
            {
                string order_identification = String.Format("{0:0000000000}", ++count);
                string price = String.Format("{0:00000000.00}", Convert.ToDouble(tbxPrice.Text));
                string volumn = String.Format("{0:0000000000}", Convert.ToInt32(tbxVolumn.Text));
                string time = now.ToString("HHmmssfff");                  // "090535090";
                string type = tbxType.Text;
                //0더미, 1주문, 2더미,3가격,4수량,5더미,6시간,7더미,8타입,9더미
                string msg = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", dummy0, order_identification, dummy2, price, volumn, dummy5, time, dummy7, type, dummy9);
                sw.WriteLine(msg);
                sw.Flush();
            }
        }

        private void btnipSave_Click(object sender, EventArgs e)
        {
            string ipAddress = tbxip.Text;
            string port = tbxPort.Text;
            string msgsave = string.Format("{0}:{1}", ipAddress, port);
            StreamWriter sw = new StreamWriter(new FileStream("ipsettings.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            sw.WriteLine(msgsave);
            sw.AutoFlush = true;
            sw.Close();
        }
    }

    public static class HRDateTime
    {
        static bool isAvailable;

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        public static DateTime Now
        {
            get
            {
                if (!isAvailable)
                {
                    return DateTime.Now;
                }

                long filetime;
                GetSystemTimePreciseAsFileTime(out filetime);

                return DateTime.FromFileTime(filetime);
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                if (!isAvailable)
                {
                    return DateTime.UtcNow;
                }

                long filetime;
                GetSystemTimePreciseAsFileTime(out filetime);

                return DateTime.FromFileTimeUtc(filetime);
            }
        }

        static HRDateTime()
        {
            try
            {
                long filetime;
                GetSystemTimePreciseAsFileTime(out filetime);
                isAvailable = true;
            }
            catch (EntryPointNotFoundException)
            {
                // Not running Windows 8 or higher.
                isAvailable = false;
            }
        }
    }
}
