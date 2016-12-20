using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.Net.Sockets;
namespace MessageStreamManager
{
    public partial class Mainform : Form
    {
        //효율적인 로깅을 위한 logger 설정정보는 app.config 에있음
        //관련 정보 더 알고 싶으시면 https://logging.apache.org/log4net/ 참조
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Mainform));
        public static List<StreamObject> InStreamList = new List<StreamObject>();
        public static List<StreamObject> OutStreamList = new List<StreamObject>();
        public static Dictionary<IPEndPoint, TcpClient> socketMap = new Dictionary<IPEndPoint, TcpClient>();
        public Queue<StremEntity> messageQueue;

        public delegate void DrawGridDelegate();
        
        private Object thisLock = new Object();
        public Mainform()
        {
            messageQueue = new Queue<StremEntity>();
            logger.Debug("Create Message Queue for Multi Thread");
            readConfigurationFromAppConfig();
            logger.Debug("read config file");
            InitializeComponent();
            //inputStreamGrid.DataSource = messageQueue.ToList();
        }
        public void drawGrid()
        {
            inputStreamGrid.DataSource= messageQueue.ToList();
        }

        public void readConfigurationFromAppConfig()
        {
            try
            {
                System.Collections.Specialized.StringCollection InStreamArray = Properties.Settings.Default.INPUT_STREAM;
                System.Collections.Specialized.StringCollection OutStreamSrray = Properties.Settings.Default.OUTPUT_STREAM;
                createStreamMapByConfig(InStreamArray, InStreamList);
                createStreamMapByConfig(OutStreamSrray, OutStreamList);
            }
            catch (Exception e)
            {
                logger.Error(string.Format("EXCEPTION : {0}", e.ToString()));
            }
        }
        public void createOnlySocketStream(List<StreamObject> dest)
        {
            foreach (StreamObject obj in dest)
            {
                if(obj.type == StreamObjectType.SOCKET)
                {
                    if (!socketMap.ContainsKey(obj.endPoint))
                    {
                        new TcpClient();
                        socketMap.Add(obj.endPoint, new TcpClient());
                    }
                }
            }
        }
        public void createStreamMapByConfig(System.Collections.Specialized.StringCollection source, List<StreamObject> dest)
        {
            foreach (String s in source)
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

                    if(parse[0].Equals("FILE"))
                    {
                        type = StreamObjectType.FILE;
                        dest.Add(new StreamObject(type, parse[1], threadType, parse[3]));
                    }
                    else if (parse[0].Equals("SOCKET"))
                    {
                        type = StreamObjectType.SOCKET;
                        string[] parseAddr = parse[1].Split(':');
                        IPHostEntry ipHost = Dns.GetHostEntry(parseAddr[0]);
                        IPAddress ipAddr = ipHost.AddressList[1];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(parseAddr[1]));
                        //통신 소켓 생성
                        TcpClient sender = new TcpClient();
                        socketMap.Add(ipEndPoint, sender);
                        dest.Add(new StreamObject(type, parse[1], threadType,parse[3], ipEndPoint));
                    }

                }
                else
                {
                    logger.Error("EXCEPTION : Configuration Wrong");
                }
            }
        }

        public void fileRead(string topic ,string fileName)
        {
            string line;
            //파일 스트림 객체 생성
            System.IO.StreamReader file = new System.IO.StreamReader(@fileName);
            while ((line = file.ReadLine()) != null)
            {
                lock (thisLock)
                {
                    StremEntity entity = new StremEntity(topic, line);
                    messageQueue.Enqueue(entity);
                    logger.Info(string.Format("topic : {0}  message : {1} queueCnt : {2} ", topic, line, messageQueue.Count));
                    if (line == null)
                        break;
                }
            }
            file.Close();
        }
        public void fileWrite(string topic, string fileName)
        {
           //To do 
        }
        public void socketWrite(IPEndPoint ipEndPoint, string addr)
        {
            TcpClient sender = null;
            NetworkStream networkStream = null;
            System.IO.StreamWriter streamWriter = null;
            try
            {
                if (socketMap.ContainsKey(ipEndPoint))
                {
                    sender = socketMap[ipEndPoint];
                    networkStream = sender.GetStream();
                    streamWriter = new System.IO.StreamWriter(networkStream);
                }

                StremEntity entity = null;

                while (true)
                {
                    lock (thisLock)
                    {
                        if (messageQueue.Count > 0)
                        {
                            entity = messageQueue.Dequeue();
                            if (entity != null)
                            {
                                if (sender != null)
                                {
                                    if (messageQueue.Count >= 0)
                                    {

                                        streamWriter.WriteLine(entity.toString());
                                        streamWriter.Flush();
                                        logger.Info(string.Format("send  topic : {0}  message : {1}", entity.topic, entity.content));
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error(string.Format("TCP SOCKET ERROR : {0}", e.ToString()));
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (networkStream != null) networkStream.Close();
                if (sender != null) sender.Close();
            }
        }

        public void readStream(List<StreamObject> stream)
        {
            foreach(StreamObject obj in stream)
            {
                if(obj.type==StreamObjectType.FILE)
                {
                    if(obj.threadType == StreamObjectThreadType.SINGLE)
                    {
                        //파일 한개씩 읽어서 큐에 넣는다
                        fileRead(obj.topic, obj.content);
                    }
                    else
                    {
                        //파일 갯수당 TASK(스레드풀) 에서 할당해서 그 스레드가 큐에 넣는다.
                        Task fileReadTask = new Task(() => this.fileRead(obj.topic, obj.content));
                        fileReadTask.Start();
                    }
                }
                else
                {
                    //TO DO SOCKET STREAM
                }
            }
        }

        public void sendStream(List<StreamObject> stream)
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
                        Task socketSend = new Task(() => this.socketWrite(obj.endPoint, obj.content));
                        socketSend.Start();
                    }
                }
            }
        }
        private void read_Click(object sender, EventArgs e)
        {
            readStream(InStreamList);
        }

        private void send_Click(object sender, EventArgs e)
        {
            sendStream(OutStreamList);
        }

        private void connect_Click(object sender, EventArgs e)
        {
            createOnlySocketStream(OutStreamList);
            foreach (IPEndPoint ipEndPoint in socketMap.Keys)
            {
                try
                {
                    socketMap[ipEndPoint].Connect(ipEndPoint);
                    logger.Info(string.Format("conneted {0}", ipEndPoint));
                    socketGrid.DataSource = socketMap.ToList<KeyValuePair<IPEndPoint, TcpClient>>();
                }
                catch (Exception error)
                {
                    logger.Equals(error.ToString());
                }
            }
        }

        private void socketGrid_rowContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewSelectedRowCollection rows = socketGrid.SelectedRows;
            foreach(DataGridViewRow row in rows)
            {
                Socket s = (Socket)row.Cells[1].Value;
                s.Disconnect(false);
                s.Close();
                socketMap.Remove((IPEndPoint)row.Cells[0].Value);
            }
            socketGrid.DataSource = socketMap.ToList<KeyValuePair<IPEndPoint, TcpClient>>();
        }

        private void socketGrid_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {

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
        public StreamObject(StreamObjectType type, string content, StreamObjectThreadType threadType, string topic,IPEndPoint endPoint = null)
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
}
