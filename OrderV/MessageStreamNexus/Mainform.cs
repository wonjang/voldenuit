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
using System.IO;
namespace MessageStreamNexux
{
    public partial class Mainform : Form
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Mainform));
        public static List<StreamObject> InStreamList = new List<StreamObject>();
        public static Dictionary<IPEndPoint, TcpListener> socketMap = new Dictionary<IPEndPoint, TcpListener>();
        public Queue<StremEntity> messageQueue;
        public Dictionary<string, DataTable> dataTableMap;
        public static readonly object lockObj = new object();
        public View1 view1;
        public View2 view2;

        public Mainform()
        {
            messageQueue = new Queue<StremEntity>();
            view1 = new View1(this);
            view2 = new View2(this);

            dataTableMap = new Dictionary<string, DataTable>();
            InitializeComponent();
            logger.Info("start MessageStreamnexus!");
        }
        public void createListerSocket()
        {
            System.Collections.Specialized.StringCollection InStreamArray = Properties.Settings.Default.INPUT_STREAM;

            foreach (string s in InStreamArray)
            {
                string[] parse = s.Split('|');
                if (parse.Length == 4)
                {
                    StreamObjectType type = 0;
                    StreamObjectThreadType threadType = 0;
                    if (parse[2].Equals("S"))
                    {
                        threadType = StreamObjectThreadType.SINGLE;
                    }
                    else if (parse[2].Equals("M"))
                    {
                        threadType = StreamObjectThreadType.MULTI;
                    }

                    if (parse[0].Equals("FILE"))
                    {
                        type = StreamObjectType.FILE;
                        InStreamList.Add(new StreamObject(type, parse[1], threadType, parse[3]));
                    }
                    else if (parse[0].Equals("SOCKET"))
                    {
                        type = StreamObjectType.SOCKET;
                        string[] parseAddr = parse[1].Split(':');
                        IPHostEntry ipHost = Dns.GetHostEntry(parseAddr[0]);
                        IPAddress ipAddr = ipHost.AddressList[1];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(parseAddr[1]));
                        //통신 소켓 생성
                        TcpListener listener = new TcpListener(ipEndPoint);
                        socketMap.Add(ipEndPoint, listener);
                        InStreamList.Add(new StreamObject(type, parse[1], threadType, parse[3], ipEndPoint));
                    }

                }
                else
                {
                    logger.Error("EXCEPTION : Configuration Wrong");
                }
            }
        }
        private void createListener_Click(object sender, EventArgs e)
        {
            cleanupSocket();
            createListerSocket();
            createlistener(InStreamList);
        }
        public void createlistener(List<StreamObject> stream)
        {
            foreach (StreamObject obj in stream)
            {
                if (obj.type == StreamObjectType.FILE)
                {
                    //To do FileWriter
                }
                else
                {
                    if (obj.threadType == StreamObjectThreadType.SINGLE)
                    {

                    }
                    else
                    {
                        Task socketListener = new Task(() => this.listener(obj.endPoint, obj.topic));
                        socketListener.Start();
                    }
                }
            }
        }
        public void listener(IPEndPoint endpoint, string listen_topic)
        {
            if (socketMap.ContainsKey(endpoint))
            {
                TcpListener listener = null;
                TcpClient client = null;
                NetworkStream networkStream = null;
                StreamReader streamReader = null;
                try
                {
                    listener = socketMap[endpoint];
                    listener.Start();
                    while (true)
                    {
                        client = listener.AcceptTcpClient();
                        logger.Info(string.Format("accept socket {0}", client.Client.RemoteEndPoint));
                        networkStream = client.GetStream();
                        streamReader = new StreamReader(networkStream);
                        while (client.Connected)
                        {
                            /* size[4]+topic[4]+header[4] */
                            string getData = streamReader.ReadLine();
                            if (getData.Length > 0)
                            {
                                string length = getData.Substring(0, 4);
                                string topic = getData.Substring(4, 4);
                                string header = getData.Substring(8, 4);
                                string content = getData.Substring(12, getData.Length - 12);
                                StremEntity entity = new StremEntity(topic, content);
                                view1.sendmessage(entity);
                               // messageQueue.Enqueue(entity);
                                logger.Info(string.Format("receve msg {0}", getData));
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    logger.Error(error.ToString());
                }
                finally
                {
                    if (streamReader != null) streamReader.Close();
                    if (client != null) client.Close();
                    if (networkStream != null) networkStream.Close();
                }
            }
            else
            {
                logger.Error(string.Format("Does not exist Socket in Map {0}", endpoint));
            }
        }
        public void cleanupSocket()
        {
            foreach (IPEndPoint endpoint in socketMap.Keys)
            {
                try
                {
                    logger.Info(string.Format("cleanup {0}", endpoint));
                    socketMap[endpoint].Stop();
                }
                catch (Exception error)
                {
                    logger.Error(error.ToString());
                }
                finally
                {
                    socketMap[endpoint].Stop();
                }
            }

            if (socketMap.Count != 0)
            {
                logger.Info("cleanupSocket");
                InStreamList.Clear();
                socketMap.Clear();
            }
        }

        private void clearSocket_Click(object sender, EventArgs e)
        {
            cleanupSocket();
        }
        bool showView = false;
        private void view_Click(object sender, EventArgs e)
        {
            if (showView == false)
            {
                view1.StartPosition = FormStartPosition.Manual;
                view1.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                view1.Show();
                view1.ControlBox = false;
                view2.Show();
                view2.ControlBox = false;
                showView = true;
                view.Text = "closeView";
            }
            else
            {
                view1.Hide();
                view2.Hide();
                showView = false;
                view.Text = "view";
            }
        }
    }
    public enum StreamObjectType
    {
        FILE = 0, SOCKET = 1
    }
    public enum StreamObjectThreadType
    {
        SINGLE = 0, MULTI = 1
    }

    public class StreamObject
    {
        public StreamObjectType type { get; set; }
        public string content { get; set; }
        public StreamObjectThreadType threadType { get; set; }
        public string topic { get; set; }
        public IPEndPoint endPoint { get; set; }
        public StreamObject(StreamObjectType type, string content, StreamObjectThreadType threadType, string topic, IPEndPoint endPoint = null)
        {
            this.type = type;
            this.content = content;
            this.threadType = threadType;
            this.topic = topic;
            this.endPoint = endPoint;
        }
    }
    public class MessageLength
    {
        public static int longest = 374;
        public static string T0374 = "0374";
        public static string T0022 = "0022";
        public static int shortest = 22;
    }

    public class StremEntity
    {
        public string size { get; set; } //char [4]
        public string topic { get; set; } //char [4]
        public string header { get; set; } //char [4]
        public string content { get; set; } //varchar
        public StremEntity(string topic, string content)
        {
            this.header = content.Substring(0, 4);
            if (header.Equals("0374"))
            {
                this.size = MessageLength.T0374;
            }
            else if (header.Equals("0022"))
            {
                this.size = MessageLength.T0022;
            }
            this.topic = topic;
            this.content = content;
        }
        public string toString()
        {
            return size + topic + header + content;
        }
    }
    public class STRUCT_0374
    {
        public string formatDateTime { get; set; }

        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUENCE_NUMBER { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string ME_GRP_NO { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string TRADING_NUMBER { get; set; }
        public string TRADING_PRICE { get; set; }
        public string TRADING_VOLUMN { get; set; }
        public string SESS_ID { get; set; }
        public string TRADING_DATE { get; set; }
        public string TRADING_TIME { get; set; }
        public string THE_NEARBY_MONTH_TRADING_PRICE { get; set; }
        public string THE_FUTURE_MONTH_TRADING_PRICE { get; set; }
        public string ASK_BID_TYPE_CODE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string MARKET_MAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string MEMBER_USE_AREA { get; set; }
        public string topic { get; set; }
        public DateTime dateTime { get; set; }
        public STRUCT_0374(string topic, string content)
        {
            this.topic = topic;
            int offset = 0;
            this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
            this.MSG_HEADER = content.Substring(offset, 46); offset += 46;
            this.MSG_DUMMY = content.Substring(offset, 82); offset += 82;
            this.MESSAGE_SEQUENCE_NUMBER = content.Substring(offset, 11); offset += 11;
            this.TRANSACTION_CODE = content.Substring(offset, 11); offset += 11;
            this.ME_GRP_NO = content.Substring(offset, 2); offset += 2;
            this.BOARD_ID = content.Substring(offset, 2); offset += 2;
            this.MEMBER_NUMBER = content.Substring(offset, 5); offset += 5;
            this.BRANCH_NUMBER = content.Substring(offset, 5); offset += 5;
            this.ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ORIGINAL_ORDER_IDENTIFICATION = content.Substring(offset, 10); offset += 10;
            this.ISSUE_CODE = content.Substring(offset, 12); offset += 12;
            this.TRADING_NUMBER = content.Substring(offset, 11); offset += 11;
            this.TRADING_PRICE = content.Substring(offset, 11); offset += 11;
            this.TRADING_VOLUMN = content.Substring(offset, 10); offset += 10;
            this.SESS_ID = content.Substring(offset, 2); offset += 2;
            this.TRADING_DATE = content.Substring(offset, 8); offset += 8;

            int year = Int32.Parse(TRADING_DATE.Substring(0, 4));
            int month = Int32.Parse(TRADING_DATE.Substring(4, 2));
            int day = Int32.Parse(TRADING_DATE.Substring(6, 2));

            this.TRADING_TIME = content.Substring(offset, 9); offset += 9;

            int hours = Int32.Parse(TRADING_TIME.Substring(0, 2));
            int minute = Int32.Parse(TRADING_TIME.Substring(2, 2));
            int sec = Int32.Parse(TRADING_TIME.Substring(4, 2));
            int milli = Int32.Parse(TRADING_TIME.Substring(6, 3));

            DateTime dateTime = new DateTime(year, month, day, hours, minute, sec, milli);
            this.dateTime = dateTime;
            this.formatDateTime = dateTime.ToString("HH:mm:ss.fff");

            this.THE_NEARBY_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
            this.THE_FUTURE_MONTH_TRADING_PRICE = content.Substring(offset, 11); offset += 11;
            this.ASK_BID_TYPE_CODE = content.Substring(offset, 1); offset += 1;
            this.ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.MARKET_MAKER_ORDER_TYPE_NUMBER = content.Substring(offset, 11); offset += 11;
            this.TRUST_COMPANY_NUMBER = content.Substring(offset, 5); offset += 5;
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = content.Substring(offset, 12); offset += 12;
            this.MEMBER_USE_AREA = content.Substring(offset, 60); offset += 60;

        }
    }

    public class STRUCT_0022
    {
        public string formatDateTime { get; set; }
        public DateTime dateTime { get; set; }
        public DateTime chartTime { get; set; }
        public string VALUE { get; set; }
        public string MSG_LENGTH { get; set; }
        public string MSG_TIME { get; set; }
        public string MSG_PLUS { get; set; }
        public string MSG_VALUE { get; set; }
        public string topic { get; set; }
        public STRUCT_0022(DataRow row)
        {
            this.MSG_LENGTH = row.Field<string>("MSG_LENGTH");
            this.MSG_TIME = row.Field<string>("MSG_TIME");
            this.MSG_PLUS = row.Field<string>("MSG_PLUS");
            this.MSG_VALUE = row.Field<string>("MSG_VALUE");
            this.dateTime = Convert.ToDateTime(row.Field<string>("dateTime"));
        }
        public STRUCT_0022(string topic, string content)
        {
            this.topic = topic;
            int offset = 0;
            this.MSG_LENGTH = content.Substring(offset, 4); offset += 4;
            this.MSG_TIME = content.Substring(offset, 11); offset += 11;
            this.MSG_PLUS = content.Substring(offset, 1); offset += 1;
            this.MSG_VALUE = content.Substring(offset, 6); offset += 6;
            int hours = Int32.Parse(MSG_TIME.Substring(0, 2));
            int minute = Int32.Parse(MSG_TIME.Substring(3, 2));
            int sec = Int32.Parse(MSG_TIME.Substring(6, 2));
            int milli = Int32.Parse(MSG_TIME.Substring(9, 2));

            DateTime dt = DateTime.Now;
            DateTime atTime = new DateTime(dt.Year, dt.Month, dt.Day, hours, minute, sec, milli);
            DateTime chartTime = new DateTime(dt.Year, dt.Month, dt.Day, hours, minute, 0, 0);

            this.dateTime = atTime;
            this.chartTime = chartTime;
            this.formatDateTime = chartTime.ToString("HH:mm:ss.fff");

            this.VALUE = MSG_PLUS + Double.Parse(MSG_VALUE).ToString();

        }
    }

    public class STRUCT_0374_0022
    {
        public int seq { get; set; }
        public string formatDateTime { get; set; }
        public string formatDateTime2 { get; set; }
        public string PRICE { get; set; }
        public string QTY { get; set; }
        public string POSITION { get; set; }
        public string netPROFIT { get; set; }
        public string VALUE { get; set; }

        public string TRADING_TIME { get; set; }
        public string MSG_LENGTH { get; set; }
        public string MSG_HEADER { get; set; }
        public string MSG_DUMMY { get; set; }
        public string MESSAGE_SEQUENCE_NUMBER { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string ME_GRP_NO { get; set; }
        public string BOARD_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string BRANCH_NUMBER { get; set; }
        public string ORDER_IDENTIFICATION { get; set; }
        public string ORIGINAL_ORDER_IDENTIFICATION { get; set; }
        public string ISSUE_CODE { get; set; }
        public string TRADING_NUMBER { get; set; }
        public string TRADING_PRICE { get; set; }
        public string ASK_BID_TYPE_CODE { get; set; }
        public string TRADING_VOLUMN { get; set; }
        public string SESS_ID { get; set; }
        public string TRADING_DATE { get; set; }
        public string THE_NEARBY_MONTH_TRADING_PRICE { get; set; }
        public string THE_FUTURE_MONTH_TRADING_PRICE { get; set; }
        public string ACCOUNT_NUMBER { get; set; }
        public string MARKET_MAKER_ORDER_TYPE_NUMBER { get; set; }
        public string TRUST_COMPANY_NUMBER { get; set; }
        public string SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER { get; set; }
        public string MEMBER_USE_AREA { get; set; }

        //0022
        public string MSG_TIME { get; set; }
        public string MSG_PLUS { get; set; }
        public string MSG_VALUE { get; set; }

        public DateTime dateTime { get; set; }
        public DateTime dateTime2 { get; set; }
        public DateTime chartTime { get; set; }
        public string topic { get; set; }

        //profit
        public string netfillCashByPointValue_commision { get; set; }

        public STRUCT_0374_0022(int seq, string topic, STRUCT_0374 s1, STRUCT_0022 s2)
        {
            this.seq = seq;
            this.topic = topic;
            this.MSG_LENGTH = s1.MSG_LENGTH;
            this.MSG_HEADER = s1.MSG_HEADER;
            this.MSG_DUMMY = s1.MSG_DUMMY;
            this.MESSAGE_SEQUENCE_NUMBER = s1.MESSAGE_SEQUENCE_NUMBER;
            this.TRANSACTION_CODE = s1.TRANSACTION_CODE;
            this.ME_GRP_NO = s1.ME_GRP_NO;
            this.BOARD_ID = s1.BOARD_ID;
            this.MEMBER_NUMBER = s1.MEMBER_NUMBER;
            this.BRANCH_NUMBER = s1.BRANCH_NUMBER;
            this.ORDER_IDENTIFICATION = s1.ORDER_IDENTIFICATION;
            this.ORIGINAL_ORDER_IDENTIFICATION = s1.ORIGINAL_ORDER_IDENTIFICATION;
            this.ISSUE_CODE = s1.ISSUE_CODE;
            this.TRADING_NUMBER = s1.TRADING_NUMBER;
            this.TRADING_PRICE = s1.TRADING_PRICE;
            this.TRADING_VOLUMN = s1.TRADING_VOLUMN;
            this.SESS_ID = s1.SESS_ID;
            this.TRADING_DATE = s1.TRADING_DATE;

            int year = Int32.Parse(TRADING_DATE.Substring(0, 4));
            int month = Int32.Parse(TRADING_DATE.Substring(4, 2));
            int day = Int32.Parse(TRADING_DATE.Substring(6, 2));

            this.TRADING_TIME = s1.TRADING_TIME;

            int hours = Int32.Parse(TRADING_TIME.Substring(0, 2));
            int minute = Int32.Parse(TRADING_TIME.Substring(2, 2));
            int sec = Int32.Parse(TRADING_TIME.Substring(4, 2));
            int milli = Int32.Parse(TRADING_TIME.Substring(6, 3));

            DateTime dateTime = new DateTime(year, month, day, hours, minute, sec, milli);
            DateTime chartTime = new DateTime(year, month, day, hours, minute, 0, 0);
            this.formatDateTime = dateTime.ToString("HH:mm:ss.fff");
            this.chartTime = chartTime;
            this.dateTime = dateTime;

            this.THE_NEARBY_MONTH_TRADING_PRICE = s1.THE_NEARBY_MONTH_TRADING_PRICE;
            this.THE_FUTURE_MONTH_TRADING_PRICE = s1.THE_FUTURE_MONTH_TRADING_PRICE;
            this.ASK_BID_TYPE_CODE = s1.ASK_BID_TYPE_CODE;
            this.ACCOUNT_NUMBER = s1.ACCOUNT_NUMBER;
            this.MARKET_MAKER_ORDER_TYPE_NUMBER = s1.MARKET_MAKER_ORDER_TYPE_NUMBER;
            this.TRUST_COMPANY_NUMBER = s1.TRUST_COMPANY_NUMBER;
            this.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER = s1.SUBSTITUTE_STOCK_CERTIFICATE_ACCOUNT_NUMBER;
            this.MEMBER_USE_AREA = s1.MEMBER_USE_AREA;

            this.MSG_TIME = s2.MSG_TIME;
            int hours2 = Int32.Parse(MSG_TIME.Substring(0, 2));
            int minute2 = Int32.Parse(MSG_TIME.Substring(3, 2));
            int sec2 = Int32.Parse(MSG_TIME.Substring(6, 2));
            int milli2 = Int32.Parse(MSG_TIME.Substring(9, 2));

            DateTime dateTime2 = new DateTime(year, month, day, hours2, minute2, sec2, milli2);
            this.dateTime2 = dateTime2;
            this.formatDateTime2 = dateTime.ToString("HH:mm:ss.fff");
            this.MSG_PLUS = s2.MSG_PLUS;
            this.MSG_VALUE = s2.MSG_VALUE;

            this.PRICE = (Double.Parse(TRADING_PRICE) * 100).ToString();
            if (ASK_BID_TYPE_CODE.Equals("1"))
            {
                QTY = "-" + Int32.Parse(TRADING_VOLUMN).ToString(); ;
            }
            else
            {
                QTY = "+" + Int32.Parse(TRADING_VOLUMN).ToString();
            }
            this.VALUE = MSG_PLUS + Double.Parse(MSG_VALUE).ToString();
        }
    }
}
