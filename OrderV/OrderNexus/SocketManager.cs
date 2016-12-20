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

namespace OrderV
{
    //통신 관리 객체
    public class SocketManager
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MainForm));
        public static readonly log4net.ILog orderLogger = log4net.LogManager.GetLogger("OrderMsg");
        public static readonly log4net.ILog filledLogger = log4net.LogManager.GetLogger("FilledMsg");
        public static readonly log4net.ILog rejectLogger = log4net.LogManager.GetLogger("RejectMsg");
        public static readonly log4net.ILog socketLogger = log4net.LogManager.GetLogger("SocketMsg");
        public static readonly log4net.ILog socketLogger1 = log4net.LogManager.GetLogger("SocketMsg1");
        public static readonly log4net.ILog socketLogger2 = log4net.LogManager.GetLogger("SocketMsg2");

        public Queue<STRUCT_0374> messageQueue_0374;
        public Queue<STRUCT_0374_REPLY> messageQueue_0374_reply;
        public Queue<STRUCT_0393> messageQueue_0393;
        public Queue<STRUCT_0419> messageQueue_0419;
        public MsgEventHandler msgHandler;
        public class TCPClientSet
        {
            public TcpClient client;
            public NetworkStream ns;
            public StreamWriter sw;
            public byte[] readBuffer = new byte[2048];
            public byte[] sendBuffer = new byte[2048];

            public TCPClientSet(TcpClient client, NetworkStream ns,StreamWriter sw)
            {
                this.client = client;
                this.ns = ns;
                this.sw = sw;
            }
        }
        public class TCPListenerSet
        {
            public TcpListener listener;
            public NetworkStream ns;
            public byte[] readBuffer = new byte[2048];
            public byte[] sendBuffer = new byte[2048];
            public TCPListenerSet(TcpListener listener, NetworkStream ns)
            {
                this.listener = listener;
                this.ns = ns;
            }
        }
        public Dictionary<IPEndPoint, TCPListenerSet> listenerSocketMap = new Dictionary<IPEndPoint, TCPListenerSet>();
        public Dictionary<IPEndPoint, TCPClientSet> clientSocketMap = new Dictionary<IPEndPoint, TCPClientSet>();
        public static Dictionary<string, IPEndPoint> iptable = new Dictionary<string, IPEndPoint>();
        private static volatile SocketManager instance;
        private static object syncRoot = new Object();
        private SocketManager(string[] data,MsgEventHandler msgHandler)
        {
            this.msgHandler = msgHandler;
            messageQueue_0374 = new Queue<STRUCT_0374>();
            messageQueue_0374_reply = new Queue<STRUCT_0374_REPLY>();
            messageQueue_0393 = new Queue<STRUCT_0393>();
            messageQueue_0419 = new Queue<STRUCT_0419>();

            socketLogger.Info("Create SocketManager singleton instance...");
            socketLogger.Info("ip table loading");
            foreach(string s in data)
            {
                try
                {
                    string[] temp = s.Split('|');
                    if (!iptable.ContainsKey(temp[0]))
                    {
                        string sockID = temp[0];
                        string addr = temp[1];
                        string[] parseAddr = addr.Split(':');
                        IPHostEntry ipHost = Dns.GetHostEntry(parseAddr[0]);
                        IPAddress ipAddr = ipHost.AddressList[1];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Int32.Parse(parseAddr[1]));
                        iptable.Add(sockID, ipEndPoint);
                    }
                    else
                    {
                        logger.Error("socket id duplicate");
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.ToString());
                }

            }
        }
        public static SocketManager getInstance(string [] data ,MsgEventHandler msgHandler)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new SocketManager(data, msgHandler);
                }
            }
            else
            {
                logger.Info("already exist SocketManager singleton instance...");
            }
            return instance;
        }
        public static SocketManager getInstance ()
        {
            if (iptable.Count == 0)
            {
                logger.Error("first time you must load conf_iptable.conf");
                return instance;
            }
            else
            {
                return instance;
            }
        }

        public void createTcpListener(string sockID ,IPEndPoint ipEndPoint)
        {
            if (listenerSocketMap.ContainsKey(ipEndPoint))
            {
                logger.Info(string.Format("already exist socket :{0}", ipEndPoint.ToString()));
            }
            else
            {
                TcpListener listener = new TcpListener(ipEndPoint);
                NetworkStream ns = null;
                TCPListenerSet listenerSet = new TCPListenerSet(listener, ns);
                listenerSocketMap.Add(ipEndPoint, listenerSet);
                logger.Info(string.Format("create success socket :{0}", ipEndPoint.ToString()));
                Task socketListener = new Task(() => this.listener(sockID, listenerSet));
                socketListener.Start();
            }
        }

        public void createTcpClient(IPEndPoint ipEndPoint)
        {
            if (clientSocketMap.ContainsKey(ipEndPoint))
            {
                logger.Info(string.Format("already exist socket :{0}", ipEndPoint.ToString()));
            }
            else
            {
                try
                {
                    TcpClient client = new TcpClient();
                    logger.Info(string.Format("create success socket :{0}", ipEndPoint.ToString()));
                    client.Connect(ipEndPoint.Address, ipEndPoint.Port);
                    logger.Info(string.Format("create success connect socket :{0}", ipEndPoint.ToString()));
                    NetworkStream ns = new NetworkStream(client.Client);
                    StreamWriter sw = new StreamWriter(ns);
                    TCPClientSet clientSet = new TCPClientSet(client, ns,sw);
                    clientSocketMap.Add(ipEndPoint, clientSet);
                }
                catch (Exception e)
                {

                }
            }
        }

    
        public void listener (string socketID, TCPListenerSet listenerSet)
        {
            TcpClient client = null;
            StreamReader sr;
            string getData = "";
            string header = "";
            try
            {
                listenerSet.listener.Start();
                while (true)
                {
                    socketLogger.Info(string.Format("socket id {0} listen... socket {1}", socketID, listenerSet.listener.LocalEndpoint));
                    client = listenerSet.listener.AcceptTcpClient();
                    socketLogger.Info(string.Format("socket id {0} accept socket {1}", socketID,client.Client.RemoteEndPoint));
                    listenerSet.ns = client.GetStream();
                    sr = new StreamReader(client.GetStream());
                    while (client.Connected)
                    {
                        int size = 0;
                        if (socketID.Equals("OMS"))
                        {
                            size = listenerSet.ns.Read(listenerSet.readBuffer, 0, listenerSet.readBuffer.Length);
                            getData = System.Text.Encoding.ASCII.GetString(listenerSet.readBuffer);
                        }
                        else
                        {
                            getData = sr.ReadLine();
                        }

                        if (getData != null)
                        {
                            header = getData.Substring(0, 4);
                            //lock (MainForm.lockObj)
                            {
                                switch(header)
                                {
                                    case "0374":
                                        if(socketID.Equals("OMS"))
                                        {
                                            STRUCT_0374_REPLY msg_oms = new STRUCT_0374_REPLY(socketID, getData);
                                            Task FilledThread = new Task(() => msgHandler.FilledMessageProc(msg_oms));
                                            Task positionCalcThread = new Task(() => msgHandler.PositionCalcFilled(msg_oms));
                                            FilledThread.Start();
                                            positionCalcThread.Start();

                                        }
                                        else
                                        {
                                            STRUCT_0374 msg_ticket = new STRUCT_0374(socketID, getData);
                                            Task CancelCheckThread = new Task(() => msgHandler.CancelCheck(msg_ticket));
                                            Task positionCalcThread = new Task(() => msgHandler.PositionCalcTicket(msg_ticket));
                                            CancelCheckThread.Start();
                                            positionCalcThread.Start();
                                        }
                                        break;
                                    case "0393":
                                        messageQueue_0393.Enqueue(new STRUCT_0393(socketID, getData));
                                        break;
                                    case "0419":
                                        STRUCT_0419 msg_confirm = new STRUCT_0419(socketID, getData);
                                        Task ConfirmThread = new Task(() => msgHandler.ReceiveConfirm(msg_confirm));
                                        ConfirmThread.Start();
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (listenerSet.ns != null) listenerSet.ns.Close();
                            if (client != null) client.Close();
                            if (client.GetStream() != null) client.GetStream().Close();
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
                if (listenerSet.ns != null) listenerSet.ns.Close();
                if (client.Connected == true)
                {
                    if (client?.GetStream() != null) client.GetStream().Close();
                }
                if (client != null) client.Close();
            }
        }
    }
}
