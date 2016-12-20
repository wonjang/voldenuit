using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using Microsoft.VisualBasic;  // Mid, Left, Right
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Concurrent; // ConcurrentQueue
using System.Runtime.CompilerServices;  // DataGridView IPropertyChanged.

namespace SocketServer
{
    public partial class Form1 : Form
    {
        delegate void SetTextCallback(string text);
        ConcurrentQueue<string> qDataGridView1 = new ConcurrentQueue<string>();
        ConcurrentQueue<string> qDataGridView2 = new ConcurrentQueue<string>();

        private BindingSource customersBindingSource1 = new BindingSource();
        private BindingSource customersBindingSource2 = new BindingSource();

        public int 주문번호 = 700000;
        //public Socket client;
        //public bool _OnClient = false;  // 클라이언트의 연결상태를 저장한다.
        ClientSocket _myClient;

        //전략1[] my전략 = new 전략1[2];
        //Form2 chartDlg;

        public Form1()
        {
            InitializeComponent();

            BindingList<DemoCustomer> customerList1 = new BindingList<DemoCustomer>();
            BindingList<DemoCustomer> customerList2 = new BindingList<DemoCustomer>();

            this.customersBindingSource1.DataSource = customerList1;
            this.customersBindingSource2.DataSource = customerList2;

            dataGridView1.DataSource = this.customersBindingSource1;
            dataGridView2.DataSource = this.customersBindingSource2;

            DataGridViewColumn column;
            column = dataGridView1.Columns[0]; column.Width = 80; // 주문
            column = dataGridView1.Columns[1]; column.Width = 80; // 원주문
            column = dataGridView1.Columns[2]; column.Width = 40; //신규/정정/취소
            column = dataGridView1.Columns[3]; column.Width = 40; //매수'매도
            column = dataGridView1.Columns[4]; column.Width = 80; //가격
            column = dataGridView1.Columns[5]; column.Width = 80; //수량

            column = dataGridView2.Columns[0]; column.Width = 80;
            column = dataGridView2.Columns[1]; column.Width = 80;
            column = dataGridView2.Columns[2]; column.Width = 40;
            column = dataGridView2.Columns[3]; column.Width = 40;
            column = dataGridView2.Columns[4]; column.Width = 80;
            column = dataGridView2.Columns[5]; column.Width = 80;
            
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; // 100ms   60 * 60 * 1000; // 1 시간
            timer.Tick += new EventHandler(timer_Tick2); //주기마다 실행되는 이벤트 등록
            timer.Start();

        }
       
        public static DemoCustomer AddNewCustomer(string i주문번호, string i원주문번호, string i신규정정취소, string i매수매도, string i가격, string i수량)
        {
            return new DemoCustomer(i주문번호, i원주문번호, i신규정정취소, i매수매도, i가격, i수량);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread ctThread1 = new Thread(DoAcceptTcpClient1); ctThread1.Start();
        }

        public void DoAcceptTcpClient1()
        {
            string iMSG;
            TcpListener serverSocket = new TcpListener(Int32.Parse(textBox3.Text));
            TcpClient clientSocket = null;
            int counter = 0;

            serverSocket.Start();

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();   //<--- 클라이언트가 접속할때까지 기다린다.

                iMSG = " >> " + "Client No:" + Convert.ToString(counter) + " started!";
                //SetText1(iMSG); // listBox1.Items.Add(iMSG);
                
                handleClinet client = new handleClinet(this, 1);   //<--- 새로운 클라이언트 소켓을 만든다.

                //client.ReturnToText += SetText1; // (UpdateText_set);//이벤트 핸들러 연결
                client.ReturnToText += SetListView1;

                client.startClient(clientSocket, Convert.ToString(counter));    //<---- 클라이언트와 통신한다. 
            }

            clientSocket.Close();
            serverSocket.Stop();
            // Console.WriteLine(" >> " + "exit");
            // Console.ReadLine();
        }
        /*
        private void SetText1(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox1.Items.Add(text);
            }
            return;
        }
        */
        private void SetListView1(string iLINE)
        {
            try
            {
                if (iLINE.Substring(0, 4) == "0374")  //<--- 체결 메시지
                {
                    //HDL_0374(0, iLINE);
                }
                else if (iLINE.Substring(0, 4) == "0022")//<--- 간격 메시지
                {
                    //HDL_0022(0, iLINE);
                }
                else if (iLINE.Substring(0, 4) == "0393")//<--- 주문 메시지
                {
                    HDL_0393(0, iLINE);
                }
                else if (iLINE.Substring(0, 4) == "0419")//<--- 주문/취소/정정 응답 메시지
                {
                    //HDL_0419(0, iLINE);
                }
            }catch(Exception e)
            {

            }
        }
        /*
        private void SetText2(string text)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox2.Items.Add(text);
            }

            //updateLIstBox2(text);
        }
        */
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            // 2. 구조체 데이타를 바이트 배열로 변환
            DataPacket packet = new DataPacket();
            packet.Name = "Name___";
            packet.Subject = "Subject";
            packet.Grade = 1;
            packet.Memo = "Memo";

            byte[] buffer = packet.Serialize();
            * /
            string iLINE = "0000001111112222222222222333";
            ClsMyMsg msg = new ClsMyMsg(iLINE);
            string mMSG = string.Format("[{0}] [{1}] [{2}] [{3}]", msg.NAME, msg.PHONE, msg.LENGTH, msg.MEMO);

            msg.NAME = "name";
            msg.PHONE = "02-2677-2234";
            mMSG = msg.ToString();

            //NetworkStream stream = client.GetStream();

            // 4. 데이타 전송
            //stream.Write(buffer, 0, buffer.Length);
            //Console.WriteLine("{0} data sent", buffer.Length);
            //Console.WriteLine("===============================\n");

            //string str = Encoding.Default.GetString(buffer);
            listBox1.Items.Add(mMSG);
            */

        }

        private void button4_Click(object sender, EventArgs e)
        {            //ADD
            BindingList<DemoCustomer> customerList1 = this.customersBindingSource1.DataSource as BindingList<DemoCustomer>;
            //customerList.Add(DemoCustomer.CreateNewCustomer());
            customerList1.Add(DemoCustomer.AddNewCustomer("0","1","2","3","4","5"));
        }
        // string -> byte[]
        public static byte[] StringToBytes(string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        // byte[] -> string
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            button5.Text = DateTime.Now.ToLongTimeString();

            // Queue 에서 메시지를 꺼내서, 리스트뷰에 출력한다.
            //UpdateListView();
        }
        void timer_Tick2(object sender, EventArgs e)
        {
            //button5.Text = DateTime.Now.ToLongTimeString();

            // Queue 에서 메시지를 꺼내서, 리스트뷰에 출력한다.
            UpdateDataGridView();

        }
        public void UpdateButtonText(int n, string str)
        {
            /*
            switch (n)
            {
                case 0:
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((Action)delegate
                        {
                            tbDebug.Text = str;
                        });
                    }
                    else
                    {
                        tbDebug.Text = str;
                    }
                    break;
                case 1:
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((Action)delegate
                        {
                            button1.Text = str;
                        });
                    }
                    else
                    {
                        button1.Text = str;
                    }
                    break;

                case 2:
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((Action)delegate
                        {
                            button2.Text = str;
                        });
                    }
                    else
                    {
                        button2.Text = str;
                    }
                    break;
                default:
                    break;
            }
            */
        }
        private void UpdateDataGridView()
        {
            string[] keys = null;            
            string iLINE = "";

            // Queue 에서 메시지를 꺼내서, 리스트뷰에 출력한다.
            BindingList<DemoCustomer> customerList1 = this.customersBindingSource1.DataSource as BindingList<DemoCustomer>;
            while (qDataGridView1.TryDequeue(out iLINE)) // my전략[0].TradeQueue.TryDequeue(out iLINE))
            {
                keys = iLINE.Split('^');
                customerList1.Add(DemoCustomer.AddNewCustomer(keys[0], keys[1], keys[2], keys[3], string.Format("{0:N2}", Convert.ToInt32(keys[4])), string.Format("{0}", Convert.ToInt32(keys[5])))); // mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드, mt.호가가격, mt.호가수량)); //  "0001", "0000", "2", "1", "123400", "123"));
            }

            BindingList<DemoCustomer> customerList2 = this.customersBindingSource1.DataSource as BindingList<DemoCustomer>;
            while (qDataGridView2.TryDequeue(out iLINE)) // my전략[0].TradeQueue.TryDequeue(out iLINE))
            {

                keys = iLINE.Split('^');
                //string.Format("{0:N2}", Convert.ToInt32(keys[0]));
                customerList2.Add(DemoCustomer.AddNewCustomer(keys[0], keys[1], keys[2], keys[3], string.Format("{0:N2}", Convert.ToInt32(keys[4])), string.Format("{0}", Convert.ToInt32(keys[5])))); // mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드, mt.호가가격, mt.호가수량)); //  "0001", "0000", "2", "1", "123400", "123"));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        public void SendTrade(int i주문번호, string i신규정정취소, string i매도매수, int price, int cnt, int OrgNumber)
        {
            // 체결메시지를 보낸다.

            string buffer = "0374" + new string(' ', 374 - 4); //MessageBox.Show(buffer.Length.ToString()); //XC1120TRDATA000000010000000000000081309098                                                                                      00000000000TCHODR10002G100088001010007900001          KR4101LC000622000101302346000000000000000000.002000000000000          11     00                         9193100140013000F532B7CF120161201081309098                                                           1 ";
            byte[] enc = StringToBytes(buffer);
            byte[] chg;

            int r = 0, Len = 0;
            r = r + 4 + 46 + 82;  // 증권사 헤더
            r = r + 11 + 11 + 2 + 2 + 5;

            string 지점번호 = new string('5', 5); Len = 지점번호.Length;
            chg = StringToBytes(지점번호);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 5;

            //string 주문ID = 주문번호.ToString("D10"); 주문번호++; Len = 주문ID.Length;
            string 주문ID = i주문번호.ToString("D10"); Len = 주문ID.Length;
            chg = StringToBytes(주문ID);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;

            string 원주문ID = OrgNumber.ToString("D10"); Len = 원주문ID.Length;
            chg = StringToBytes(원주문ID);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;

            r = r + 12; r = r + 11;

            //int price = 123450;
            double dPrice = (double)price / 100;
            string 체결가격 = string.Format("{0:00000000.00}", dPrice); Len = 체결가격.Length;
            chg = StringToBytes(체결가격);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 11;

            string 체결수량 = cnt.ToString("D10"); Len = 체결수량.Length;
            chg = StringToBytes(체결수량);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;

            r = r + 2; r = r + 8; r = r + 9; r = r + 11; r = r + 11;

            string 매도매수;
            if (i매도매수 == "매도" || i매도매수 == "1")
            {
                매도매수 = "1"; Len = 매도매수.Length;
            }
            else
            {
                매도매수 = "2"; Len = 매도매수.Length;
            }
            chg = StringToBytes(매도매수);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 1;

            try
            {
                string dec_ = BytesToString(enc);    // byte[] --> string 
                _myClient.BeginSend(dec_);           // 소켓을 통해서 보낸다. 
            } 
            catch(Exception ex)
            {

            }
            string dec = BytesToString(enc);    // byte[] --> string 
            ClsFILE.SaveLog(dec);               //---> 파일로 저장
        }
        public void SendResponse(int i주문번호, string i신규정정취소, string i매도매수, int price, int cnt, int OrgNumber, int i실정정취소수량,string i에러코드="0000") //string i주문번호)// i신규정정취소, string i매도매수, int price, int cnt, int OrgNumber)
        {
     
            // 응답메시지를 보낸다.
            string buffer = "0419" + new string(' ', 419 - 4); //MessageBox.Show(buffer.Length.ToString()); //XC1120TRDATA000000010000000000000081309098                                                                                      00000000000TCHODR10002G100088001010007900001          KR4101LC000622000101302346000000000000000000.002000000000000          11     00                         9193100140013000F532B7CF120161201081309098                                                           1 ";
            byte[] enc = StringToBytes(buffer);
            byte[] chg;

            int r = 0, Len = 0;
            r = r + 4 + 46 + 82;  // 증권사 헤더
            r = r + 11 + 11 + 2 + 2 + 5;
            
            string 지점번호 = new string('5', 5); Len = 지점번호.Length;
            chg = StringToBytes(지점번호);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 5;

            string 주문ID = i주문번호.ToString("D10"); Len = 주문ID.Length;
            chg = StringToBytes(주문ID);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;


            string 원주문ID = OrgNumber.ToString("D10"); Len = 원주문ID.Length;
            chg = StringToBytes(원주문ID);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;

            r = r + 12;

            string 매도매수;
            if (i매도매수 == "매도")
            {
                매도매수 = "1"; Len = 매도매수.Length;
            }
            else
            {
                매도매수 = "2"; Len = 매도매수.Length;
            }
            chg = StringToBytes(매도매수);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 1;

            string 정정취소;
            if (i신규정정취소 == "신규" || i신규정정취소 == "1")
            {
                정정취소 = "1"; Len = 정정취소.Length;
            }
            else if (i신규정정취소 == "정정" || i신규정정취소 == "2")
            {
                정정취소 = "2"; Len = 정정취소.Length;
            }
            else
            {
                정정취소 = "3"; Len = 정정취소.Length;
            }
            chg = StringToBytes(정정취소);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 1;

            r = r + 12;

            string 호가수량 = cnt.ToString("D10"); Len = 호가수량.Length;
            chg = StringToBytes(호가수량);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 10;

            //int price = 123450;
            double dPrice = (double)price / 100;
            string 호가가격 = string.Format("{0:00000000.00}", dPrice); Len = 호가가격.Length;
            chg = StringToBytes(호가가격);
            Buffer.BlockCopy(chg, 0, enc, r, Len); r = r + 11;

            r = r + 168;

            string 실정정취소호가수량;
            if (i에러코드 == "0000")
            {
                실정정취소호가수량 = i실정정취소수량.ToString("D10"); Len = 실정정취소호가수량.Length;
            } else
            {
                실정정취소호가수량 = new string('0', 10);                    
            }            
            chg = StringToBytes(실정정취소호가수량);
            Buffer.BlockCopy(chg, 0, enc, r, 10); r = r + 10;

            r = r + 1;
            
            chg = StringToBytes(i에러코드);
            Buffer.BlockCopy(chg, 0, enc, r, 4); r = r + 4;

            try
            {
                string dec_ = BytesToString(enc);    // byte[] --> string 
                _myClient.BeginSend(dec_);
            }
            catch(Exception ex)
            {

            }
            string dec = BytesToString(enc);

            ClsFILE.SaveLog(dec);
            
        }
        
        public void HDL_0393(int index, string buffer)
        {
            ClsOrderMsg mt = new ClsOrderMsg(buffer);  // 받은 패킷을 클래스에 넣어서 항목별로 분리한다.

            string iMSG;
            if (mt.정정취소구분코드 == "1") // 신규주문일때..
            {
                iMSG = mt.tostring();
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((Action)delegate
                    {
                        BindingList<DemoCustomer> customerList1 = this.customersBindingSource1.DataSource as BindingList<DemoCustomer>;
                        customerList1.Add(DemoCustomer.AddNewCustomer(mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드,
                            string.Format("{0}", (int)(100 * Convert.ToDouble(mt.호가가격))), string.Format("{0}", Convert.ToInt32(mt.호가수량))));
                    });
                }
                else
                {
                    BindingList<DemoCustomer> customerList1 = this.customersBindingSource1.DataSource as BindingList<DemoCustomer>;
                    customerList1.Add(DemoCustomer.AddNewCustomer(mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드,
                        string.Format("{0}", (int)(100 * Convert.ToDouble(mt.호가가격))), string.Format("{0}", Convert.ToInt32(mt.호가수량))));
                }
            }
            else if (mt.정정취소구분코드 == "2") // 정정주문일때..
            {
                MessageBox.Show("정정주문 접수.. 처리하지 않습니다.");
            }
            else if (mt.정정취소구분코드 == "3") // 취소주문일때..
            {
                iMSG = mt.tostring();
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((Action)delegate
                    {
                        BindingList<DemoCustomer> customerList2 = this.customersBindingSource2.DataSource as BindingList<DemoCustomer>;
                        customerList2.Add(DemoCustomer.AddNewCustomer(mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드,
                            string.Format("{0}", (int)(100 * Convert.ToDouble(mt.호가가격))), string.Format("{0}", Convert.ToInt32(mt.호가수량))));
                    });
                }
                else
                {
                    BindingList<DemoCustomer> customerList2 = this.customersBindingSource2.DataSource as BindingList<DemoCustomer>;
                    customerList2.Add(DemoCustomer.AddNewCustomer(mt.주문ID, mt.원주문ID, mt.정정취소구분코드, mt.매도매수구분코드,
                        string.Format("{0}", (int)(100 * Convert.ToDouble(mt.호가가격))), string.Format("{0}", Convert.ToInt32(mt.호가수량))));
                }
            }
        }

        public string ToMin(string iLINE)
        {
            string hh = iLINE.Substring(0, 2);
            string mm = iLINE.Substring(2, 2);
            string ss = iLINE.Substring(4, 2);
            string ff = iLINE.Substring(6, 2);
            string sLINE = string.Format("{0}:{1}:{2}.{3}", hh, mm, ss, ff);
            return sLINE;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int price = 123450;
            double dPrice = (double) price / 100;
            string imsg = string.Format("{0:000000000000000.00}", dPrice);

            //MessageBox.Show(imsg);

            string iPrice = "000001234.56";
            //dPrice = 100 * Convert.ToDouble(iPrice);
            //price = (int)dPrice;

            price = (int)(100 * Convert.ToDouble(iPrice));
            MessageBox.Show(price.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void button11_Click(object sender, EventArgs e)
        {
        }

        private void button12_Click(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
        }

        private void button17_Click(object sender, EventArgs e)
        {   // 클라이언트 ---> 서버로 연결버튼
            // 클라이언트 소켓을 만들고 연결을 시도한다.
            _myClient = new ClientSocket(this, textBox1.Text, Convert.ToInt32(textBox2.Text));
            button17.Enabled = false;

            if (_myClient.OnCONNECT == false)
            {
                _myClient.DoInit();
            }
            else
            {
                //tbDebug.Text += "\r\n이미 접속중입니다. : ";
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // 서버실행 버튼을 클릭했을때..
            button19.Enabled = false;
            Thread ctThread1 = new Thread(DoAcceptTcpClient1); ctThread1.Start();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            // 체결버튼을 눌렀을때  텍스트4의 주문번호를 검색해서
            // 체결메시지를 보낸다.

            string 주문ID = textBox4.Text;

            // 텍스트박스의 주문번호를 이용해서 리스트박스에서 해당주문을 찾는다.
            // dataGridView1 에서 찾는다. 
            int i = 0;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(dataGridView1.Rows[i].Cells[0].Value.ToString() == 주문ID)
                {
                    int k = 1;
                    string 원주문ID = dataGridView1.Rows[i].Cells[k].Value.ToString(); k++;
                    string 신규정정취소구분코드 = dataGridView1.Rows[i].Cells[k].Value.ToString(); k++;
                    string 매도매수구분코드 = dataGridView1.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가가격 = dataGridView1.Rows[i].Cells[k].Value.ToString(); k++;
                    string 체결수량 = textBox5.Text;  k++;  

                    SendTrade(Convert.ToInt32(주문ID), 신규정정취소구분코드, 매도매수구분코드,   Convert.ToInt32(호가가격), Convert.ToInt32(체결수량), Convert.ToInt32(원주문ID));
                    return;
               }
            }
            MessageBox.Show("해당 주문을 찾을 수 없습니다.");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            // 전량취소 확인 메시지를 보낸다.

            string 주문ID = textBox6.Text;

            // 텍스트박스의 주문번호를 이용해서 리스트박스에서 해당주문을 찾는다.
            int i = 0;
            for (i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == 주문ID)
                {
                    int k = 1;
                    string 원주문ID = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 신규정정취소구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 매도매수구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가가격 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가수량 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    int i실정정취소수량 = Convert.ToInt32(textBox7.Text);

                    if (i실정정취소수량 > Convert.ToInt32(호가수량))
                    {
                        MessageBox.Show("실정정취소수량이 호가수량보다 많습니다.");
                        return;
                    }

                    SendResponse( Convert.ToInt32(주문ID), 신규정정취소구분코드, 매도매수구분코드, Convert.ToInt32(호가가격), Convert.ToInt32(호가수량), Convert.ToInt32(원주문ID), i실정정취소수량, "0000");
                    return;
                }
            }
            MessageBox.Show("해당 주문을 찾을 수 없습니다.");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            // 0803 취소에러 메시지를 보낸다.

            string 주문ID = textBox8.Text;

            // 텍스트박스의 주문번호를 이용해서 리스트박스에서 해당주문을 찾는다.
            int i = 0;
            for (i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == 주문ID)
                {
                    int k = 1;
                    string 원주문ID = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 신규정정취소구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 매도매수구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가가격 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가수량 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    int i실정정취소수량 = 0;
                    SendResponse(Convert.ToInt32(주문ID), 신규정정취소구분코드, 매도매수구분코드, Convert.ToInt32(호가가격), Convert.ToInt32(호가수량), Convert.ToInt32(원주문ID), i실정정취소수량, "0803");
                    return;
                }
            }
            MessageBox.Show("해당 주문을 찾을 수 없습니다.");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            // 0804 취소에러 응답 메시지를 보낸다.

            string 주문ID = textBox10.Text;

            // 텍스트박스의 주문번호를 이용해서 리스트박스에서 해당주문을 찾는다.
            int i = 0;
            for (i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == 주문ID)
                {
                    int k = 1;
                    string 원주문ID = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 신규정정취소구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 매도매수구분코드 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가가격 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    string 호가수량 = dataGridView2.Rows[i].Cells[k].Value.ToString(); k++;
                    int i실정정취소수량 = 0;
                    SendResponse(Convert.ToInt32(주문ID), 신규정정취소구분코드, 매도매수구분코드, Convert.ToInt32(호가가격), Convert.ToInt32(호가수량), Convert.ToInt32(원주문ID), i실정정취소수량, "0804");
                    return;
                }
            }
            MessageBox.Show("해당 주문을 찾을 수 없습니다.");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            // 주문응답을 보낸다.
            // 주문ID + "^" + 원주문ID + "^" + 매도매수구분코드 + "^" + 정정취소구분코드 + "^" + 호가수량 + "^" + 호가가격;

            string 주문ID = textBox12.Text;
            string iLINE = "";

            // 텍스트박스의 주문번호를 이용해서 리스트박스에서 해당주문을 찾는다.
            int i = 0;
            for (i = 0; i < listBox1.Items.Count; i++)
            {
                iLINE = listBox1.Items[i].ToString();

                if (iLINE.Substring(0, 주문ID.Length + 1) == 주문ID + "^")
                {
                    string[] keys = null;
                    keys = iLINE.Split('^');

                    int k = 1;
                    //string 주문ID = keys[0];
                    string 원주문ID = keys[k]; k++;
                    string 매도매수구분코드 = keys[k]; k++;
                    string 정정취소구분코드 = keys[k]; k++;
                    string 호가수량 = keys[k]; k++;
                    string 호가가격 = keys[k]; k++;

                    SendResponse(Convert.ToInt32(주문ID), 정정취소구분코드, 매도매수구분코드, Convert.ToInt32(호가가격), Convert.ToInt32(호가수량), Convert.ToInt32(원주문ID), Convert.ToInt32(호가수량), "0000");
                    return;
                }
            }
            MessageBox.Show("해당 주문을 찾을 수 없습니다.");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 주문ID + "^" + 원주문ID + "^" + 매도매수구분코드 + "^" + 정정취소구분코드 + "^" + 호가수량 + "^" + 호가가격;
            string iLINE = listBox1.Text;
            string[] keys = null;

            keys = iLINE.Split('^');
            textBox4.Text = keys[0];    //<--- 체결 처리를 위해서
            textBox12.Text = keys[0];   //<--- 주문응답 처리를 위해서..

        }

        private void button18_Click(object sender, EventArgs e)
        {
            //client.Close();
            //_OnClient = false;
            button17.Enabled = true;
            //tbDebug.Text += "\r\n 클라이언트 연결종료 : ";
            _myClient.CloseClient();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
             * // 주문ID + "^" + 원주문ID + "^" + 매도매수구분코드 + "^" + 정정취소구분코드 + "^" + 호가수량 + "^" + 호가가격;
            string iLINE = listBox1.Text;
            string[] keys = null;

            keys = iLINE.Split('^');
            textBox4.Text = keys[0];    //<--- 체결 처리를 위해서
            textBox12.Text = keys[0];   //<--- 주문응답 처리를 위해서..
            */
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                //MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();    //<--- 체결 처리를 위해서
                textBox12.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();   //<--- 주문응답 처리를 위해서..
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                //MessageBox.Show(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();    //<--- 체결 처리를 위해서
                textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();   //<--- 주문응답 처리를 위해서..
                textBox10.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();    //<--- 체결 처리를 위해서
            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            // 취소확인
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            // 0803 에러메시지
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            // 0804 에러메시지
        }

        private void button31_Click_1(object sender, EventArgs e)
        {
            // 주문응답
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            // 체결메시지
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            // button19.Enabled = true;
        }
    }
    public class DemoCustomer : INotifyPropertyChanged
    {
        // These fields hold the values for the public properties.
        //private Guid idValue = Guid.NewGuid();
        //private string customerNameValue = String.Empty;
        //private string phoneNumberValue = String.Empty;
        private string _주문번호 = String.Empty;
        private string _원주문번호 = String.Empty;
        private string _신규정정취소 = String.Empty;
        private string _매수매도 = String.Empty;
        private string _가격 = String.Empty;
        private string _수량 = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        // The constructor is private to enforce the factory pattern.
        private DemoCustomer()
        {
            //customerNameValue = "Customer";
            //phoneNumberValue = "(312)555-0100";
            _주문번호 = String.Empty;
            _원주문번호 = String.Empty;
            _신규정정취소 = String.Empty;
            _매수매도 = String.Empty;
            _가격 = String.Empty;
            _수량 = String.Empty;

    }
        public DemoCustomer(string i주문번호, string i원주문번호, string i신규정정취소, string i매수매도, string i가격, string i수량)
        {
            //customerNameValue = Name;
            //phoneNumberValue = Phone;
            _주문번호 = i주문번호;
            _원주문번호 = i원주문번호;
            _신규정정취소 = i신규정정취소;
            _매수매도 = i매수매도;
            _가격 = i가격;
            _수량 = i수량;
        }
        // This is the public factory method.
        public static DemoCustomer CreateNewCustomer()
        {
            return new DemoCustomer();
        }
        public static DemoCustomer AddNewCustomer(string i주문번호, string i원주문번호, string i신규정정취소, string i매수매도, string i가격, string i수량)
        {
            return new DemoCustomer(i주문번호, i원주문번호, i신규정정취소, i매수매도, i가격, i수량);
        }

        public string 주문번호
        {
            get
            {
                return this._주문번호;
            }

            set
            {
                if (value != this._주문번호)
                {
                    this._주문번호 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string 원주문번호
        {
            get
            {
                return this._원주문번호;
            }

            set
            {
                if (value != this._원주문번호)
                {
                    this._원주문번호 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string 신규정정취소
        {
            get
            {
                return this._신규정정취소;
            }

            set
            {
                if (value != this._신규정정취소)
                {
                    this._신규정정취소 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string 매수매도
        {
            get
            {
                return this._매수매도;
            }

            set
            {
                if (value != this._매수매도)
                {
                    this._매수매도 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string 가격
        {
            get
            {
                return this._가격;
            }

            set
            {
                if (value != this._가격)
                {
                    this._가격 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string 수량
        {
            get
            {
                return this._수량;
            }

            set
            {
                if (value != this._수량)
                {
                    this._수량 = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }

    //Class to handle each client request separatly
    public class handleClinet
    {
        //메인 폼에 있는 텍스트라벨을 갱신하기 위해 델리게이트 사용
        public delegate void uptext(string upLabelText); //델리게이트 선언
        public event uptext ReturnToText; //델리게이트 이벤트 선언

        int Index = 0;
        public handleClinet(object sender, int idx)
        {
            Index = idx;
        }
        private void labelToText()
        {
            ReturnToText("연결이 잘 되나 보자");//이벤트 호출
        }

        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[70025];
            requestCount = 0;
            int MsgLength = 0;
            string pkt = "";
            int rSIZE = 0;

            while ((true))
            {
                //try
                //{

                string Tail = "", rMSG = "", myMSG = "", tMSG = "";
                requestCount = requestCount + 1;
                NetworkStream networkStream = clientSocket.GetStream();
                rSIZE = networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                tMSG = System.Text.Encoding.ASCII.GetString(bytesFrom);
                rMSG = tMSG.Substring(0, rSIZE);
                myMSG = Tail + rMSG;

                int S = 0;
                while (true)
                {
                    if (myMSG.Length >= S + 4)
                    {
                        MsgLength = Convert.ToInt32(myMSG.Substring(S, 4));

                    }
                    else
                    {
                        break;
                    }

                    if (S + MsgLength <= myMSG.Length)
                    {
                        pkt = myMSG.Substring(S, MsgLength);

                        string iMSG = pkt; ReturnToText?.Invoke(iMSG); // ---> SetText1, SetListView1

                        S = S + MsgLength;
                    }
                    else
                    {
                        break;
                    }
                }
                if (S < myMSG.Length)
                {
                    Tail = myMSG.Substring(S);
                }
                else
                {
                    Tail = "";
                }
            }
        }
    }
    public class ClsMyMessage
    {
        public string msg_length_;
        public string msg_time_;
        public string msg_plus_;
        public string msg_value_;

        public ClsMyMessage(string rMSG)
        {
            int r = 0;

            msg_length_ = rMSG.Substring(r, 4); r = r + 4;
            msg_time_ = rMSG.Substring(r, 11); r = r + 11;
            msg_plus_ = rMSG.Substring(r, 1); r = r + 1;
            msg_value_ = rMSG.Substring(r, 6); r = r + 6;

        }
        public override string ToString()
        {
            return msg_length_ + "^" + msg_time_ + "^" + msg_plus_ + "^" + msg_value_ + "^";
        }
        public static int ToMin(string buff)
        {
            string sMIN = buff.Substring(0, 2) + buff.Substring(3, 2);
            return Convert.ToInt32(sMIN);
        }
    }
    public class ClsMyTradeMsg
    {
        public string msg_length_;
        public string trading_price_;
        public string trading_volume_;
        public string trading_time_;
        public string ask_bid_type_code_;
        public string 주문ID;
        public string 원주문ID;

        public ClsMyTradeMsg(string rMSG)
        {
            int r = 0;

            msg_length_ = rMSG.Substring(r, 4); r = r + 4;
            r = r + 46;
            r = r + 82;
            r = r + 11;
            r = r + 11;
            r = r + 2;
            r = r + 2;
            r = r + 5;
            r = r + 5;
            주문ID = rMSG.Substring(r, 10); r = r + 10;
            원주문ID = rMSG.Substring(r, 10); r = r + 10;
            r = r + 12;
            r = r + 11;
            trading_price_ = rMSG.Substring(r, 11); r = r + 11;
            trading_volume_ = rMSG.Substring(r, 10); r = r + 10;
            r = r + 2;
            r = r + 8;
            trading_time_ = rMSG.Substring(r, 9); r = r + 9;
            r = r + 11;
            r = r + 11;
            ask_bid_type_code_ = rMSG.Substring(r, 1); r = r + 1;

        }

        public override string ToString()
        {
            return 주문ID + "^" + 원주문ID + "^" + msg_length_ + "^" + trading_price_ + "^" + trading_volume_ + "^" + trading_time_ + "^" + ask_bid_type_code_ + "^";
        }
        //Right
        /// <summary>
        /// 문자열 원본에서 오른쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Right(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(sString.Length - nLength, nLength);

            return sReturn;
        }

        //Left
        /// <summary>
        /// 문자열 원본에서 왼쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Left(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(0, nLength);

            return sReturn;
        }
        //Mid
        /// <summary>
        /// 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nStart">추출을 시작할 위치</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Mid(string sString, int nStart, int nLength)
        {
            string sReturn;

            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면 
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.
            --nStart;

            //시작위치가 데이터의 범위를 안넘겼는지?
            if (nStart <= sString.Length)
            {
                //안넘겼다.

                //필요한 부분이 데이터를 넘겼는지?
                if ((nStart + nLength) <= sString.Length)
                {
                    //안넘겼다.
                    sReturn = sString.Substring(nStart, nLength);
                }
                else
                {
                    //넘겼다.

                    //데이터 끝까지 출력
                    sReturn = sString.Substring(nStart);
                }

            }
            else
            {
                //넘겼다.

                //그렇다는 것은 데이터가 없음을 의미한다.
                sReturn = string.Empty;
            }

            return sReturn;
        }


    }
    public class ClsOrderMsg
    {
        public string msg_length_;
        public string 트랜잭션코드; //	"TRANSACTION_CODE"	String	 11 
        public string 주문ID; //	"ORDER_IDENTIFICATION"	String	 10 
        public string 원주문ID; //	"ORIGINAL_ORDER_IDENTIFICATION"	String	 10 
        public string 매도매수구분코드; //	"ASK_BID_TYPE_CODE"	String	 1 
        public string 정정취소구분코드; //	"MODIFY OR CANCEL_TYPE_CODE"	String	 1 
        public string 호가수량; //	"ORDER_QUANTITY"	Long	 10 
        public string 호가가격; // ORDER_PRICE Float	        
        public ClsOrderMsg(string rMSG)
        {
            int r = 0;

            msg_length_ = rMSG.Substring(r, 4); r = r + 4;
            r = r + 46;
            r = r + 82;
            r = r + 11; //메세지일련번호	"MESSAGE_SEQUENCENUMBER"	Long	 11 
            트랜잭션코드 = rMSG.Substring(r, 11); r = r + 11; //	"TRANSACTION_CODE"	String	 11 
            r = r + 2;                                          //public string 보드ID BOARD_ID    String	 2 
            r = r + 5;                                                     //public string 회원번호	"MEMBER_NUMBER"	String	 5 
            r = r + 5;                                                                //public string 지점번호	"BRANCH_NUMBER"	String	 5 
            주문ID = rMSG.Substring(r, 10); r = r + 10; ; //	"ORDER_IDENTIFICATION"	String	 10 
            원주문ID = rMSG.Substring(r, 10); r = r + 10; ; //	"ORIGINAL_ORDER_IDENTIFICATION"	String	 10 
            r = r + 12; //종목코드 ISSUE_CODE  String	 12 
            매도매수구분코드 = rMSG.Substring(r, 1); r = r + 1; ; //	"ASK_BID_TYPE_CODE"	String	 1 
            정정취소구분코드 = rMSG.Substring(r, 1); r = r + 1; ; //	"MODIFY OR CANCEL_TYPE_CODE"	String	 1 
            r = r + 12; //계좌번호	"ACCOUNT_NUMBER"	String	 12 
            호가수량 = rMSG.Substring(r, 10); r = r + 10; ; //	"ORDER_QUANTITY"	Long	 10 
            호가가격 = rMSG.Substring(r, 11); r = r + 11; ; // ORDER_PRICE Float	 11 
        }
        public string tostring()
        {
            return 주문ID + "^" + 원주문ID + "^" + 매도매수구분코드 + "^" + 정정취소구분코드 + "^" + 호가수량 + "^" + 호가가격;
        }
    }
    public class ClsResponseMsg
    {
        public string msg_length_;
        public string 트랜잭션코드;
        public string 주문ID; //	"ORDER_IDENTIFICATION"	String	 10 
        public string 원주문ID; //	"ORIGINAL_ORDER_IDENTIFICATION"	String	 10 
        public string 매도매수구분코드; //	"ASK_BID_TYPE_CODE"	String	 1 
        public string 정정취소구분코드; //	"MODIFY OR CANCEL_TYPE_CODE"	String	 1 
        public string 호가수량; //	"ORDER_QUANTITY"	Long	 10 
        public string 호가가격; // ORDER_PRICE Float	

        public string 실정정취소호가수량;
        public string 호가거부사유코드;
                
        public ClsResponseMsg(string rMSG)
        {
            int r = 0;

            int j = rMSG.Length;

            msg_length_ = rMSG.Substring(r, 4); r = r + 4;
            r = r + 46;
            r = r + 82;
            r = r + 11; //메세지일련번호	"MESSAGE_SEQUENCENUMBER"	Long	 11 
            트랜잭션코드 = rMSG.Substring(r, 11); r = r + 11; //	"TRANSACTION_CODE"	String	 11 
            r = r + 2;
            r = r + 2; //public string 보드ID BOARD_ID    String	 2 
            r = r + 5;                                                     //public string 회원번호	"MEMBER_NUMBER"	String	 5 
            r = r + 5;                                                                //public string 지점번호	"BRANCH_NUMBER"	String	 5 
            주문ID = rMSG.Substring(r, 10); r = r + 10; ; //	"ORDER_IDENTIFICATION"	String	 10 
            원주문ID = rMSG.Substring(r, 10); r = r + 10; ; //	"ORIGINAL_ORDER_IDENTIFICATION"	String	 10 
            r = r + 12; //종목코드 ISSUE_CODE  String	 12 
            매도매수구분코드 = rMSG.Substring(r, 1); r = r + 1; ; //	"ASK_BID_TYPE_CODE"	String	 1 
            정정취소구분코드 = rMSG.Substring(r, 1); r = r + 1; ; //	"MODIFY OR CANCEL_TYPE_CODE"	String	 1 
            r = r + 12; //계좌번호	"ACCOUNT_NUMBER"	String	 12 
            호가수량 = rMSG.Substring(r, 10); r = r + 10; ; //	"ORDER_QUANTITY"	Long	 10 
            호가가격 = rMSG.Substring(r, 11); r = r + 11; ; // ORDER_PRICE Float	 11 
            r = r + 168; //
            실정정취소호가수량 = rMSG.Substring(r, 10); r = r + 10;
            r = r + 1;
            호가거부사유코드 = rMSG.Substring(r, 4); r = r + 4;
        }
        public string tostring()
        {
            return 주문ID + "^" + 원주문ID + "^" + 매도매수구분코드 + "^" + 정정취소구분코드 + "^" + 호가수량 + "^" + 호가가격 + "^" + 실정정취소호가수량 + "^" + 호가거부사유코드;
        }
    }
    public class ClsMyMsg
    {
        string name_;
        string length_;
        string phone_;
        string memo_;


        public string NAME
        {
            get { return name_; }
            set
            {
                name_ = Left(value + "      ", 6);
            }
        }
        public string LENGTH
        {
            get { return length_; }
            set
            {
                length_ = Right("000000" + value, 6);
            }
        }
        public string PHONE
        {
            get { return phone_; }
            set
            {
                phone_ = Right("             " + value, 13);
            }
        }
        public string MEMO
        {
            get { return memo_; }
            set { memo_ = value; }
        }

        public ClsMyMsg(string rMSG)
        {
            int r = 0;

            name_ = rMSG.Substring(r, 6); r = r + 6;
            length_ = rMSG.Substring(r, 6); r = r + 6;
            phone_ = rMSG.Substring(r, 13); r = r + 13;
            memo_ = rMSG.Substring(r);
        }

        public override string ToString()
        {
            /*
            string rMSG = Right("      " + name_, 6)
                + Right("      " + 길이, 6)
                + Right("             " + phone, 13)
                + memo;
            return rMSG;
            */
            return name_ + length_ + phone_ + memo_;
        }
        //Right
        /// <summary>
        /// 문자열 원본에서 오른쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Right(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(sString.Length - nLength, nLength);

            return sReturn;
        }

        //Left
        /// <summary>
        /// 문자열 원본에서 왼쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Left(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(0, nLength);

            return sReturn;
        }
        //Mid
        /// <summary>
        /// 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nStart">추출을 시작할 위치</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Mid(string sString, int nStart, int nLength)
        {
            string sReturn;

            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면 
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.
            --nStart;

            //시작위치가 데이터의 범위를 안넘겼는지?
            if (nStart <= sString.Length)
            {
                //안넘겼다.

                //필요한 부분이 데이터를 넘겼는지?
                if ((nStart + nLength) <= sString.Length)
                {
                    //안넘겼다.
                    sReturn = sString.Substring(nStart, nLength);
                }
                else
                {
                    //넘겼다.

                    //데이터 끝까지 출력
                    sReturn = sString.Substring(nStart);
                }

            }
            else
            {
                //넘겼다.

                //그렇다는 것은 데이터가 없음을 의미한다.
                sReturn = string.Empty;
            }

            return sReturn;
        }


    }
    public class ClientSocket
    {
        private Socket clientSock;  /* client Socket */
        private Socket cbSock;      /* client Async Callback Socket */
        private byte[] recvBuffer;

        private const int MAXSIZE = 4096;		/* 4096  */
        private string HOST = "";
        private int PORT = 0;

        public bool OnCONNECT = false;
        Form1 _MainWin;

        public ClientSocket(Form1 MainWin, string rHOST, int rPORT)
        {
            _MainWin = MainWin;
            HOST = rHOST; PORT = rPORT;
            recvBuffer = new byte[MAXSIZE];
        }
        public void DoInit()
        {
            clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.BeginConnect();
        }

        /*----------------------*
		 *		Connection		*
		 *----------------------*/
        public void BeginConnect()
        {
            _MainWin.UpdateButtonText(0, "서버 접속 대기 중");

            try
            {
                clientSock.BeginConnect(HOST, PORT, new AsyncCallback(ConnectCallBack), clientSock);
            }
            catch (SocketException se)
            {
                /*서버 접속 실패 */
                _MainWin.UpdateButtonText(0, "\r\n서버접속 실패하였습니다. " + se.NativeErrorCode);
                this.DoInit();

            }
            catch (Exception ex)
            {
                _MainWin.UpdateButtonText(0, "\r\n서버접속 실패하였습니다.  ");
            }

        }

        /*----------------------*
		 * ##### CallBack #####	*
		 *		Connection		*
		 *----------------------*/
        private void ConnectCallBack(IAsyncResult IAR)
        {
            try
            {
                // 보류중인 연결을 완성
                Socket tempSock = (Socket)IAR.AsyncState;
                IPEndPoint svrEP = (IPEndPoint)tempSock.RemoteEndPoint;
                _MainWin.UpdateButtonText(0, "\r\n 서버로 접속 성공 : " + svrEP.Address);
                OnCONNECT = true;
                tempSock.EndConnect(IAR);
                cbSock = tempSock;
                cbSock.BeginReceive(this.recvBuffer, 0, recvBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallBack), cbSock);
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.NotConnected)
                {
                    _MainWin.UpdateButtonText(0, "\r\n 서버 접속 실패 CallBack " + se.Message);
                    this.BeginConnect();
                }
            }
        }

        BufferedStream bs;
        StreamWriter sw;

        /*----------------------*
		 *		   Send 		*
		 *----------------------*/
        public void BeginSend(string message)
        {
            try
            {
                /* 연결 성공시 */
                if (clientSock.Connected)
                {

                    byte[] buffer = new ASCIIEncoding().GetBytes(message);
                    clientSock.BeginSend(buffer,0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), message);
                }
            }
            catch (SocketException e)
            {
                _MainWin.UpdateButtonText(0, "\r\n 전송 에러 : " + e.Message);
            }
        }

        /*----------------------*
		 * ##### CallBack #####	*
		 *		   Send 		*
		 *----------------------*/
        private void SendCallBack(IAsyncResult IAR)
        {
            string message = (string)IAR.AsyncState;
            _MainWin.UpdateButtonText(0, "\r\n 전송 완료 CallBack : " + message);
        }

        /*----------------------*
		 *		  Receive 		*
		 *----------------------*/
        public void Receive()
        {
            if (OnCONNECT == true)
            {
                cbSock.BeginReceive(this.recvBuffer, 0, recvBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallBack), cbSock);
            }
        }

        public void CloseClient()
        {
            clientSock.Close();
            OnCONNECT = false;
        }
        /*----------------------*
		 * ##### CallBack #####	*
		 *		  Receive 		*
		 *----------------------*/
        private void OnReceiveCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSock = (Socket)IAR.AsyncState;
                int nReadSize = tempSock.EndReceive(IAR);
                if (nReadSize != 0)
                {
                    string message = new UTF8Encoding().GetString(recvBuffer, 0, nReadSize);
                    _MainWin.UpdateButtonText(0, "\r\n 서버로 데이터 수신 : " + message);
                }
                else
                {
                    _MainWin.UpdateButtonText(0, "\r\n 서버로 부터 연결종료 : ");
                    OnCONNECT = false;
                    cbSock.Close();
                }
                this.Receive();
            }

            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionReset)
                {
                    this.BeginConnect();
                }

                //OnCONNECT = false;
                //cbSock.Close();
            }
            catch (Exception ex) // )
            {
                OnCONNECT = false;
                cbSock.Close();
            }
        }
    }

    public class ClsFILE
    {
        public List<string> list = new List<string>();
        public int nC = 0;

        public ClsFILE()
        {
            list.Clear();
        }
        public void LoadFile(string strFile)
        {
            using (FileStream fs = new FileStream(strFile, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8, false))
                {

                    string strLineValue = null;

                    while ((strLineValue = sr.ReadLine()) != null)
                    {
                        list.Add(strLineValue); nC = nC + 1;
                    }
                }
            }
        }
        static public void SaveLog(string sLINE) //, bool overWrite)
        {
            /*
            // 큐에 저장한다.
            LogQueue.Enqueue(sLINE);

            // 저장된 갯수가 1000 을 넘어가면 파일로 저장한다.
            if (LogQueue.Count > 1000)
            {
                DumpLog();
            }
            */
            
            string filePath = Application.StartupPath + @"\log.txt";

            FileStream fs;
            fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(sLINE);
            }
            
        }
    }    
}
