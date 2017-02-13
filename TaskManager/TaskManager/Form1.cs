using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        public static readonly object _mutex = new object();
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger("default");
        public Form1()
        {
            logger.Debug("Start!!!");
            InitializeComponent();
        }
        public void mainTask(string s)
        {
            Task<int> t1 = new Task<int>(() => Logic1(s));
            Task<int> t2 = new Task<int>(() => Logic2(s));
            Task<int> t3 = new Task<int>(() => Logic3(s));
            Task<int> t4 = new Task<int>(() => Logic4(s));
            Task<int> t5 = new Task<int>(() => Logic5(s));
            Task<int> t6 = new Task<int>(() => Logic6(s));
            Task<int> t7 = new Task<int>(() => Logic7(s));
            Task<int> t8 = new Task<int>(() => Logic8(s));
            Task<int> t9 = new Task<int>(() => Logic9(s));
            Task<int> t10 = new Task<int>(() => Logic10(s));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();

            Task all = Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            TaskAwaiter awaiter = all.GetAwaiter();
            awaiter.OnCompleted(() => todoAll(s));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<string> fileContents = File.ReadAllLines(@"../data/inputData.csv");
            foreach (string s in fileContents)
            {
                Task t0 = new Task(() => mainTask(s));
                t0.Start();
                lock(_mutex)
                {
                    Monitor.Wait(_mutex);
                    //일단 기다리자 10가지 로직 의 잡이 다 끝날때까지 
                }
            }
        }

        public void todoAll(string s)
        {
            logger.Debug("todoAll: All done : "+s);
            lock (_mutex)
            {
                Monitor.PulseAll(_mutex);
                //모든 로직의 잡이 다 끝났다 이제 깨우자
            }
        }

        public int Logic1(string msg)
        {
            int value = Int32.Parse(msg);
            value = value + 1;
            logger.Debug("["+msg +"]"+"Logic1:" +value);
            return value;
        }
        public int Logic2(string msg)
        {
            int value = Int32.Parse(msg);
            int sum = 0;
            for(int i= 1; i<=value;i++)
            {
                sum = sum + i;
            }
            logger.Debug("[" + msg + "]"+"Logic2:" + sum);
            return value;
        }
        public int Logic3(string msg)
        {
            int value = Int32.Parse(msg);
            Thread.Sleep(10);
            logger.Debug("[" + msg + "]"+"Logic3:" + value);
            return value;
        }
        public int Logic4(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic4:" + value);
            return value;
        }
        public int Logic5(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic5:" + value);
            return value;
        }
        public int Logic6(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic6:" + value);
            return value;
        }
        public int Logic7(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic7:" + value);
            return value;
        }
        public int Logic8(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic8:" + value);
            return value;
        }
        public int Logic9(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic9:" + value);
            return value;
        }
        public int Logic10(string msg)
        {
            int value = Int32.Parse(msg);
            logger.Debug("[" + msg + "]" + "Logic10:" + value);
            return value;
        }
    }
}
