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
using System.Runtime.InteropServices;

namespace OrderV
{
    public partial class MainForm : Form
    {
        bool showView = false;

        public static int seq0374 = 1;
        public static int seq0374_reply = 1;
        public static int seq0393 = 1;
        public static int seq0393_cancel = 1;
        public static int seq0419 = 1;
        public static int orderNumber = 72000;

        public Dictionary<string, DataTable> dataTableMap;
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MainForm));
        public static readonly log4net.ILog orderLogger = log4net.LogManager.GetLogger("OrderMsg");
        public static readonly log4net.ILog filledLogger = log4net.LogManager.GetLogger("FilledMsg");
        public static readonly log4net.ILog rejectLogger = log4net.LogManager.GetLogger("RejectMsg");
        public static readonly log4net.ILog socketLogger = log4net.LogManager.GetLogger("SocketMsg");

        public static Dictionary<IPEndPoint, TcpListener> socketMap = new Dictionary<IPEndPoint, TcpListener>();
        public static readonly object lockObj = new object();

        public View1 view1;
        public View2 view2;
        public MsgEventHandler msgEventHandler;
        public MainForm()
        {
            dataTableMap = new Dictionary<string, DataTable>();
            //logger check
            logger.Info("Start App...");
            orderLogger.Info("ORDER PROC !!!");
            filledLogger.Info("Filled PROC !!!");
            rejectLogger.Info("Reject PROC !!!");
            string ipMapPath = @"conf_iptable.conf";
            msgEventHandler = new MsgEventHandler(this);
            try
            {
                SocketManager sockMgr = SocketManager.getInstance(System.IO.File.ReadAllLines(ipMapPath, Encoding.Default), msgEventHandler);
            }
            catch(Exception e)
            {
                logger.Error(string.Format("file not found error {0} {1}", ipMapPath, e.ToString()));
            }
            view1 = new View1(this);
            view2 = new View2(this);
            InitializeComponent();
        }

        //create tcp listener
        private void button1_Click(object sender, EventArgs e)
        {
            string sockID1 = "SOCKET1";
            string sockID2 = "OMS";
         
            SocketManager.getInstance().createTcpListener(sockID1, SocketManager.iptable[sockID1]);
            SocketManager.getInstance().createTcpListener(sockID2, SocketManager.iptable[sockID2]);
        }

        private void view_click(object sender, EventArgs e)
        {
            if (showView == false)
            {
                view1.StartPosition = FormStartPosition.Manual;
                view1.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                view2.Location = new Point(this.Location.X ,this.Location.Y + this.Height);

                view1.Show();
                view1.ControlBox = false;
                view2.Show();
                view2.ControlBox = false;
                showView = true;
                button_view.Text = "closeView";
            }
            else
            {
                view1.Hide();
                view2.Hide();
                showView = false;
                button_view.Text = "view";
            }
        }

        private void connect_Click(object sender, EventArgs e)
        {
            SocketManager.getInstance().createTcpClient(SocketManager.iptable["SOCKET3"]);
            //test query
            var query1 = System.Linq.Enumerable.Where<신규주문>(msgEventHandler.신규주문테이블, n => n.수량 > n.체결 + n.취소확인 && n.매매구분 == "2" && n.취소요청 == 0);
            var query2 = System.Linq.Enumerable.Where<신규주문>(msgEventHandler.신규주문테이블, n => n.수량 > n.체결 + n.취소확인 && n.매매구분 == "1" && n.취소요청 == 0);
            var query3 = System.Linq.Enumerable.Where<신규주문>(msgEventHandler.신규주문테이블, n => n.주문번호 == "");
            var query4 = System.Linq.Enumerable.Where<신규주문>(msgEventHandler.신규주문테이블, n => n.주문번호 == "");

            StreamWriter sw = SocketManager.getInstance().clientSocketMap[SocketManager.iptable["SOCKET3"]]?.sw;
            sw.WriteLine("");
            sw.Flush();
            lock (MainForm.lockObj)
            {
                Monitor.PulseAll(MainForm.lockObj);
            }
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
