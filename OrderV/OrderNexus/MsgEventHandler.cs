using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.IO;
using System.Net.Sockets;

namespace OrderV
{
    public class MsgEventHandler
    {
        public delegate void SocketEventHadler();
        public event SocketEventHadler GETDATA;

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MsgEventHandler));
        public static readonly log4net.ILog orderLogger = log4net.LogManager.GetLogger("OrderMsg");
        public static readonly log4net.ILog filledLogger = log4net.LogManager.GetLogger("FilledMsg");
        public static readonly log4net.ILog rejectLogger = log4net.LogManager.GetLogger("RejectMsg");
        public static readonly log4net.ILog socketLogger = log4net.LogManager.GetLogger("SocketMsg");
        public static readonly log4net.ILog socketLogger1 = log4net.LogManager.GetLogger("SocketMsg1");
        public static readonly log4net.ILog socketLogger2 = log4net.LogManager.GetLogger("SocketMsg2");

        public MainForm mainForm;

        public List<로직> 로직테이블 = new List<로직>();
        public List<운용> 운용테이블 = new List<운용>();
        public List<클라이언트> 클라이언트테이블 = new List<클라이언트>();
        public List<신규주문> 신규주문테이블 = new List<신규주문>();
        public List<취소주문> 취소주문테이블 = new List<취소주문>();
        double commission = 0.01;
        double pointValue = 1;
        ParallelOptions taskOption;
        Dictionary<string, Dictionary<string, string>> calcMap = new Dictionary<string, Dictionary<string, string>>();

        public MsgEventHandler(MainForm mainForm)
        {
            this.mainForm = mainForm;
            taskOption = new ParallelOptions();
            taskOption.MaxDegreeOfParallelism = Environment.ProcessorCount;

            STRUCT_0374 init1 = new STRUCT_0374();
            STRUCT_0374_REPLY init2 = new STRUCT_0374_REPLY();
            STRUCT_0393 init3 = new STRUCT_0393();
            STRUCT_0393_CANCEL init4 = new STRUCT_0393_CANCEL();
            STRUCT_0419 init5 = new STRUCT_0419();
        }


        /*
        public void GetDataFromQueue_3074_reply()
        {
            messageQueue_0374_reply = SocketManager.getInstance().messageQueue_0374_reply;
            while (true)
            {
                lock (MainForm.lockObj)
                {
                    while (messageQueue_0374_reply.Count > 0)
                    {
                        STRUCT_0374_REPLY msg = messageQueue_0374_reply.Dequeue();
                        if (msg != null)
                        {
                            FilledMessageProc(msg);
                            Task positionCalcThread = new Task(() => this.PositionCalcFilled(msg));
                            positionCalcThread.Start();
                            orderLogger.Info(string.Format("Fill       : {0}", JsonConvert.SerializeObject(msg)));
                        }

                        if (messageQueue_0374_reply.Count == 0)
                        {
                            break;
                        }
                    }
                    //Monitor.Wait(MainForm.lockObj);
                }
            }
        }
        public void GetDataFromQueue_0393()
        {
            messageQueue_0393 = SocketManager.getInstance().messageQueue_0393;
            while (true)
            {
                lock (MainForm.lockObj)
                {
                    while (messageQueue_0393.Count > 0)
                    {
                        STRUCT_0393 msg = messageQueue_0393.Dequeue();
                        if (msg != null)
                        {
                        }
                        if (messageQueue_0393.Count == 0)
                        {
                            break;
                        }
                    }
                    //Monitor.Wait(MainForm.lockObj);
                }
            }
        }

        public void GetDataFromQueue_0419()
        {
            messageQueue_0419 = SocketManager.getInstance().messageQueue_0419;
            while (true)
            {
                lock (MainForm.lockObj)
                {
                    while (messageQueue_0419.Count > 0)
                    {
                        STRUCT_0419 msg = messageQueue_0419.Dequeue();
                        if (msg != null)
                        {
                            ReceiveConfirm(msg);
                            GETDATA();
                        }

                        if (messageQueue_0419.Count == 0)
                        {
                            break;
                        }
                    }
                    //Monitor.Wait(MainForm.lockObj);
                }
            }
        }
        */

        public void createLogicTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "로직테이블";
            dt.Columns.Add("시퀀스");
            dt.Columns.Add("토픽");
            dt.Columns.Add("시간");
            dt.Columns.Add("가격");
            dt.Columns.Add("수량");
            dt.Columns.Add("포지션");
            dt.Columns.Add("순손익");
            dt.Columns.Add("자료1");
            mainForm.dataTableMap.Add(dt.TableName, dt);
        }


        public void createFillTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "운용테이블";
            dt.Columns.Add("시퀀스");
            dt.Columns.Add("토픽");
            dt.Columns.Add("시간");
            dt.Columns.Add("가격");
            dt.Columns.Add("수량");
            dt.Columns.Add("포지션");
            dt.Columns.Add("순손익");
            dt.Columns.Add("자료1");
            mainForm.dataTableMap.Add(dt.TableName, dt);
        }

        public void createClientTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "클라이언트";
            dt.Columns.Add("시퀀스");
            dt.Columns.Add("토픽");
            dt.Columns.Add("시간");
            dt.Columns.Add("주문번호");
            dt.Columns.Add("종목");
            dt.Columns.Add("매매구분");
            dt.Columns.Add("가격");
            dt.Columns.Add("체결");
            dt.Columns.Add("시간1");
            mainForm.dataTableMap.Add(dt.TableName, dt);
        }

        public void createNewOrderTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "신규주문";
            dt.Columns.Add("시퀀스");
            dt.Columns.Add("토픽");
            dt.Columns.Add("시퀀스1");
            dt.Columns.Add("시간");
            dt.Columns.Add("주문번호");
            dt.Columns.Add("원주문번호");
            dt.Columns.Add("종목");
            dt.Columns.Add("매매구분");
            dt.Columns.Add("가격");
            dt.Columns.Add("수량");
            dt.Columns.Add("체결");
            dt.Columns.Add("취소요청");
            dt.Columns.Add("취소확인");
            mainForm.dataTableMap.Add(dt.TableName, dt);
        }

        public void createCancelTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "취소주문";
            dt.Columns.Add("시퀀스");
            dt.Columns.Add("토픽");
            dt.Columns.Add("시퀀스1");
            dt.Columns.Add("시간");
            dt.Columns.Add("주문번호");
            dt.Columns.Add("원주문번호");
            dt.Columns.Add("종목");
            dt.Columns.Add("매매구분");
            dt.Columns.Add("가격");
            dt.Columns.Add("수량");
            dt.Columns.Add("취소확인");
            dt.Columns.Add("취소거부");
            mainForm.dataTableMap.Add(dt.TableName, dt);
        }


        public void insertLogicTable(STRUCT_0374 message)
        {
            DataTable dt = mainForm.dataTableMap["로직테이블"];
            DataRow row = dt.NewRow();
            row[0] = message.seq;
            row[1] = message.topic;
            row[2] = message.formatDateTime;
            row[3] = message.TRADING_PRICE;
            row[4] = message.TRADING_VOLUMN;
            row[5] = message.POSITION;
            row[6] = message.netfillCashByPointValue_commision;
            row[7] = "";
            dt.Rows.Add(row);
        }

        public void insertFilledTable(STRUCT_0374_REPLY message)
        {
            DataTable dt = mainForm.dataTableMap["운용테이블"];
            DataRow row = dt.NewRow();
            row[0] = message.seq;
            row[1] = message.topic;
            row[2] = message.formatDateTime;
            row[3] = message.TRADING_PRICE;
            row[4] = message.TRADING_VOLUMN;
            row[5] = message.POSITION;
            row[6] = message.netfillCashByPointValue_commision;
            row[7] = "";
            dt.Rows.Add(row);
        }

        public void insertClientTable(STRUCT_0374 message)
        {
            DataTable dt = mainForm.dataTableMap["클라이언트"];
            DataRow row = dt.NewRow();
            row[0] = message.seq;
            row[1] = message.topic;
            row[2] = message.formatDateTime;
            row[3] = message.ORDER_IDENTIFICATION;
            row[4] = message.ISSUE_CODE;
            row[5] = message.ASK_BID_TYPE_CODE;
            row[6] = message.TRADING_PRICE;
            row[7] = message.TRADING_VOLUMN;
            row[8] = message.formatLocalTime;
            dt.Rows.Add(row);
        }

        public void insertNewOrderTable(STRUCT_0393 message)
        {
            DataTable dt = mainForm.dataTableMap["신규주문"];
            DataRow row = dt.NewRow();
            row[0] = message.seq;
            row[1] = message.topic;
            row[2] = message.ticketSeq;
            row[3] = message.formatLocalTime;
            row[4] = message.ORDER_IDENTIFICATION;
            row[5] = message.ORIGINAL_ORDER_IDENTIFICATION;
            row[6] = message.ISSUE_CODE;
            row[7] = message.ASK_BIDTYPE_CODE;
            row[8] = message.ORDER_PRICE;
            row[9] = message.ORDER_QUANTITY;
            row[10] = message.FILLED_QUANTITY;
            row[11] = message.CANCELE_QUANTITY;
            row[12] = message.CANCELED_QUANTITY;

            dt.Rows.Add(row);
        }

        public void insertCancelOrder(STRUCT_0393_CANCEL message)
        {
            DataTable dt = mainForm.dataTableMap["취소주문"];
            DataRow row = dt.NewRow();
            row[0] = message.seq;
            row[1] = message.topic;
            row[2] = message.ticketSeq;
            row[3] = message.formatLocalTime;
            row[4] = message.ORDER_IDENTIFICATION;
            row[5] = message.ORIGINAL_ORDER_IDENTIFICATION;
            row[6] = message.ISSUE_CODE;
            row[7] = message.ASK_BIDTYPE_CODE;
            row[8] = message.ORDER_PRICE;
            row[9] = message.ORDER_QUANTITY;
            row[10] = message.CANCELED_QUANTITY;
            row[11] = message.REJECT_CANCEL_QUANTITY;
            dt.Rows.Add(row);
        }


        public void insert로직(STRUCT_0374 message)
        {
            로직 data = new 로직();
            data.시퀀스 = message.seq;
            data.토픽 = message.topic;
            data.시간 = message.formatDateTime;
            data.매수매도 = message.ASK_BID_TYPE_CODE;
            data.가격 = message.TRADING_PRICE;
            data.수량 = Int32.Parse(message.TRADING_VOLUMN);
            data.포지션 = Int32.Parse(message.POSITION);
            data.순손익 = Double.Parse(message.netfillCashByPointValue_commision);
            lock (MainForm.lockObj)
            {
                로직테이블.Add(data);
            }
        }

        public void insert운용(STRUCT_0374_REPLY message)
        {
            운용 data = new 운용();
            data.시퀀스1 = message.ticketSeq;
            data.시퀀스 = message.seq;
            data.토픽 = message.topic;
            data.시간 = message.formatDateTime;
            data.매수매도 = message.ASK_BID_TYPE_CODE;
            data.가격 = message.TRADING_PRICE;
            data.수량 = Int32.Parse(message.TRADING_VOLUMN);
            data.포지션 = Int32.Parse(message.POSITION);
            data.순손익 = Double.Parse(message.netfillCashByPointValue_commision);
            lock (MainForm.lockObj)
            {
                운용테이블.Add(data);
            }
        }

        public void insert클라이언트(STRUCT_0374 message)
        {
            클라이언트 data = new 클라이언트();
            data.시퀀스 = message.seq;
            data.토픽 = message.topic;
            data.시간 = message.formatDateTime;
            data.주문번호 = message.ORDER_IDENTIFICATION;
            data.종목 = message.ISSUE_CODE;
            data.매매구분 = message.ASK_BID_TYPE_CODE;
            data.가격 = message.TRADING_PRICE;
            data.체결 = Int32.Parse(message.TRADING_VOLUMN);
            data.시간1 = message.formatLocalTime;
            lock (MainForm.lockObj)
            {
                클라이언트테이블.Add(data);
            }
        }

        public void insert신규주문(STRUCT_0393 message)
        {
            신규주문 data = new 신규주문();
            data.시퀀스 = message.seq;
            data.토픽 = message.topic;
            data.시퀀스1 = message.ticketSeq;
            data.시간 = message.formatLocalTime;
            data.주문번호 = message.ORDER_IDENTIFICATION;
            data.원주문번호 = message.ORIGINAL_ORDER_IDENTIFICATION;
            data.종목 = message.ISSUE_CODE;
            data.매매구분 = message.ASK_BIDTYPE_CODE;
            data.가격 = message.ORDER_PRICE;
            data.수량 = Int32.Parse(message.ORDER_QUANTITY);
            data.체결 = Int32.Parse(message.FILLED_QUANTITY);
            data.취소요청 = Int32.Parse(message.CANCELE_QUANTITY);
            data.취소확인 = Int32.Parse(message.CANCELED_QUANTITY);
            lock (MainForm.lockObj)
            {
                신규주문테이블.Add(data);
            }
        }

        public void insert취소주문(STRUCT_0393_CANCEL message)
        {
            취소주문 data = new 취소주문();
            data.시퀀스 = message.seq;
            data.토픽 = message.topic;
            data.시퀀스1 = message.ticketSeq;
            data.시간 = message.formatLocalTime;
            data.주문번호 = message.ORDER_IDENTIFICATION;
            data.원주문번호 = message.ORIGINAL_ORDER_IDENTIFICATION;
            data.종목 = message.ISSUE_CODE;
            data.매매구분 = message.ASK_BIDTYPE_CODE;
            data.가격 = message.ORDER_PRICE;
            data.수량 = Int32.Parse(message.ORDER_QUANTITY);
            data.취소확인 = Int32.Parse(message.CANCELED_QUANTITY);
            data.취소거부 = Int32.Parse(message.REJECT_CANCEL_QUANTITY);
            lock (MainForm.lockObj)
            {
                취소주문테이블.Add(data);
            }
        }

        public void on_message<T>(T message)
        {
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string tableName = typeof(T).ToString();
            DataTable dt = mainForm.dataTableMap[tableName];

            DataRow row = dt.NewRow();
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                row[i] = Props[i].GetValue(message, null);
            }
            dt.Rows.Add(row);
            GETDATA();
        }

        public void ReceiveConfirm(STRUCT_0419 message)
        {
            string OrgOrderNumber = message.ORIGINAL_ORDER_IDENTIFICATION;
            string OrderNumber = message.ORDER_IDENTIFICATION;
            int cancelQty = Int32.Parse(message.REAL_MODIFY_OR_CANCEL_ORDER_QUANTITY);
            int orderQty = Int32.Parse(message.ORDER_QUANTITY);
            int rejectQty = orderQty - cancelQty;
            IEnumerable<신규주문> query = System.Linq.Enumerable.Where<신규주문>(신규주문테이블, n => n.주문번호 == OrgOrderNumber);
            IEnumerable<취소주문> query2 = System.Linq.Enumerable.Where<취소주문>(취소주문테이블, n => n.주문번호 == OrderNumber);
            int ticketSeq = 0;
            if (query2.Any<취소주문>())
            {
                foreach (취소주문 find in query2)
                {
                    lock (MainForm.lockObj)
                    {
                        find.취소거부 = rejectQty;
                        find.취소확인 = cancelQty;
                        ticketSeq = find.시퀀스1;
                    }
                }
            }

            if (message.ORDER_REJECTED_REASON_CODE.Equals("0803") || message.ORDER_REJECTED_REASON_CODE.Equals("0804"))
            {
                if (query.Any<신규주문>())
                {
                    STRUCT_0393 order = null;
                    foreach (신규주문 find in query)
                    {
                        find.취소확인 = 0;
                        if (find.매매구분.Equals("1"))
                        {
                            //매수 주문하기
                            order = new STRUCT_0393(find.토픽, ticketSeq, find.종목,
                                            MainForm.orderNumber.ToString("0000000000"), "0000000000", "2", OrderParam.NewOrder,
                                            rejectQty.ToString("0000000000"), find.가격);
                        }
                        else
                        {
                            //매도 주문하기 
                            order = new STRUCT_0393(find.토픽, ticketSeq, find.종목,
                                            MainForm.orderNumber.ToString("0000000000"), "0000000000", "1", OrderParam.NewOrder,
                                            rejectQty.ToString("0000000000"), find.가격);
                        }
                    }

                    if (order != null)
                    {

                        Task NewOrderThread = new Task(() => this.NewOrder(order));
                        NewOrderThread.Start();
                    }
                }
            }
            else
            {
                if (query.Any<신규주문>())
                {
                    STRUCT_0393 order = null;
                    foreach (신규주문 find in query)
                    {
                        find.취소확인 = find.취소확인 + cancelQty;
                        if (cancelQty < orderQty)
                        {
                            if (find.매매구분.Equals("1"))
                            {
                                //매수 주문하기
                                order = new STRUCT_0393(find.토픽, find.시퀀스1, find.종목,
                                               MainForm.orderNumber.ToString("0000000000"), "0000000000", "2", OrderParam.NewOrder,
                                               rejectQty.ToString("0000000000"), find.가격);
                            }
                            else
                            {
                                //매도 주문하기 
                                order = new STRUCT_0393(find.토픽, find.시퀀스1, find.종목,
                                               MainForm.orderNumber.ToString("0000000000"), "0000000000", "1", OrderParam.NewOrder,
                                               rejectQty.ToString("0000000000"), find.가격);
                            }
                        }
                        else
                        {
                            logger.Equals("NEVER canceled qty bigger then cancel qty");
                        }
                    }
                    if (order != null && cancelQty != orderQty) // 실제 취소된 수량과 취소요청 수량이 같을 경우 전량 취소 성공이라 주문을 안낸다 
                    {
                        Task NewOrderThread = new Task(() => this.NewOrder(order));
                        NewOrderThread.Start();
                    }
                }
            }
            GETDATA();
        }

        public void CancelCheck(STRUCT_0374 message)
        {
            int ticketOrderQty = Int32.Parse(message.TRADING_VOLUMN);
            List<신규주문> query = null;
            if (message.ASK_BID_TYPE_CODE.Equals("1"))
            {
                query = 신규주문테이블.Where(n => n.수량 > n.체결 + n.취소요청 && n.매매구분 == "2" /*&& n.취소요청 == 0*/).ToList<신규주문>();
            }
            else
            {
                query = 신규주문테이블.Where(n => n.수량 > n.체결 + n.취소요청 && n.매매구분 == "1"/* && n.취소요청 == 0*/).ToList<신규주문>();
            }

            if (query.Any<신규주문>())
            {
                foreach (신규주문 find in query)
                {
                    int remainQty = find.수량 - find.체결 - find.취소요청;
                    if (remainQty > 0)
                    {
                        if (ticketOrderQty >= remainQty)
                        {
                            //전량취소
                            STRUCT_0393_CANCEL order = new STRUCT_0393_CANCEL(find.토픽, message.seq, find.종목,
                                                                              MainForm.orderNumber.ToString("0000000000"), find.주문번호, find.매매구분, OrderParam.CancelOrder,
                                                                              remainQty.ToString("0000000000"), find.가격);
                            lock (MainForm.lockObj)
                            {
                                find.취소요청 += remainQty;
                            }
                            Task CancelOrderThread = new Task(() => this.CancelOrder(order));
                            CancelOrderThread.Start();
                            ticketOrderQty -= remainQty;
                        }
                        else if (ticketOrderQty < remainQty)
                        {
                            if (ticketOrderQty > 0)
                            {
                                //부분취소
                                STRUCT_0393_CANCEL order = new STRUCT_0393_CANCEL(find.토픽, message.seq, find.종목,
                                                                                  MainForm.orderNumber.ToString("0000000000"), find.주문번호, find.매매구분, OrderParam.CancelOrder,
                                                                                  ticketOrderQty.ToString("0000000000"), find.가격);
                                lock (MainForm.lockObj)
                                {
                                    find.취소요청 += ticketOrderQty;
                                }
                                Task CancelOrderThread = new Task(() => this.CancelOrder(order));
                                CancelOrderThread.Start();
                                ticketOrderQty = 0;
                            }
                            break;
                        }
                    }
                    else
                    {
                        logger.Error(string.Format("never remainQty < 0 : {0}", message.ORDER_IDENTIFICATION));
                    }
                }
            }

            if (ticketOrderQty > 0)
            {
                Task NewOrderThread = new Task(() => this.NewOrder(message, ticketOrderQty), TaskCreationOptions.PreferFairness);
                NewOrderThread.Start();
            }
        }

        public void NewOrder(STRUCT_0374 message, int orderQty)
        {
            STRUCT_0393 order = new STRUCT_0393(message.topic, message.seq, message.ISSUE_CODE, MainForm.orderNumber.ToString("0000000000"), "0000000000", message.ASK_BID_TYPE_CODE,
                                                OrderParam.NewOrder, orderQty.ToString("0000000000"), message.TRADING_PRICE);

            StreamWriter sw = SocketManager.getInstance().clientSocketMap[SocketManager.iptable["SOCKET3"]].sw;
            if (sw != null)
            {
                sw.WriteLine(order.toKRXFORMAT());
                sw.Flush();
                insert신규주문(order);
                GETDATA();
            }
            orderLogger.Info(string.Format("NewOrder     : {0}", JsonConvert.SerializeObject(order)));
        }

        public void NewOrder(STRUCT_0393 message)
        {
            StreamWriter sw = SocketManager.getInstance().clientSocketMap[SocketManager.iptable["SOCKET3"]].sw;
            if (sw != null)
            {
                sw.WriteLine(message.toKRXFORMAT());
                sw.Flush();
                insert신규주문(message);
                GETDATA();
            }
            orderLogger.Info(string.Format("NewOrder     : {0}", JsonConvert.SerializeObject(message)));
        }

        public void CancelOrder(STRUCT_0393_CANCEL message)
        {
            string sockID3 = "SOCKET3";
            StreamWriter sw = SocketManager.getInstance().clientSocketMap[SocketManager.iptable[sockID3]].sw;
            if (sw != null)
            {
                sw.WriteLine(message.toKRXFORMAT());
                sw.Flush();
                insert취소주문(message);
                GETDATA();
            }
            orderLogger.Info(string.Format("CancelOrder  : {0}", JsonConvert.SerializeObject(message)));
        }

        public void FilledMessageProc(STRUCT_0374_REPLY msg)
        {
            IEnumerable<신규주문> query = System.Linq.Enumerable.Where<신규주문>(신규주문테이블, n => n.주문번호 == msg.ORDER_IDENTIFICATION);
            if (query.Any<신규주문>())
            {
                foreach (신규주문 find in query)
                {
                    lock (MainForm.lockObj)
                    {
                        find.체결 = find.체결 + Int32.Parse(msg.TRADING_VOLUMN);
                    }
                    msg.ticketSeq = find.시퀀스1;
                    msg.orderSeq = find.시퀀스;
                }
            }
            else
            {
                logger.Error(string.Format("does not exist ticketSeq in filled msg ORRDER_IDENTIFICATION : {0}", msg.ORDER_IDENTIFICATION));
            }
            filledLogger.Info(string.Format("Filled       : {0}", JsonConvert.SerializeObject(msg)));
        }

        public void PositionCalcTicket(STRUCT_0374 data)
        {
            double askFillCash = 0;
            double avgAskFillPrice = 0;
            int askQty = 0;

            double bidFillCash = 0;
            double avgBidFillPrice = 0;
            int bidQty = 0;
            int netQty = 0;

            double fillCash = 0;
            double netfillCash = 0;
            string dateTime = "";
            double netfillCashByPointValue = 0;
            double netfillCashByPointValue_commision = 0;
            double totalCommision = 0;
            double openProfit = 0;
            if (calcMap.ContainsKey(data.topic))
            {
                Dictionary<string, string> subCalcMap = calcMap[data.topic];
                double filledPrice = double.Parse(data.TRADING_PRICE);
                int filledCount = int.Parse(data.TRADING_VOLUMN);
                int side = int.Parse(data.ASK_BID_TYPE_CODE);
                dateTime = data.dateTime.ToString("HH:mm:ss.fff");
                string topic = data.topic;

                askFillCash = Double.Parse(subCalcMap["askFillCash"]);
                bidFillCash = Double.Parse(subCalcMap["bidFillCash"]);
                avgAskFillPrice = Double.Parse(subCalcMap["avgAskFillPrice"]);
                avgBidFillPrice = Double.Parse(subCalcMap["avgBidFillPrice"]);
                askQty = Int32.Parse(subCalcMap["askQty"]);
                bidQty = Int32.Parse(subCalcMap["bidQty"]);
                netQty = Int32.Parse(subCalcMap["netQty"]);
                fillCash = Double.Parse(subCalcMap["fillCash"]);
                netfillCash = Double.Parse(subCalcMap["netfillCash"]);
                totalCommision = Double.Parse(subCalcMap["totalCommision"]);
                netfillCashByPointValue = Double.Parse(subCalcMap["netfillCashByPointValue"]);
                netfillCashByPointValue_commision = Double.Parse(subCalcMap["netfillCashByPointValue_commision"]);

                if ((SIDE)side == SIDE.ASK)
                {
                    askFillCash += filledPrice * filledCount;
                    askQty += filledCount;
                    fillCash += filledPrice * filledCount;
                    avgAskFillPrice = askFillCash / askQty;
                }
                else
                {
                    bidFillCash += filledPrice * filledCount;
                    bidQty += filledCount;
                    fillCash -= filledPrice * filledCount;
                    avgBidFillPrice = bidFillCash / bidQty;
                }

                netQty = bidQty - askQty;
                if (netQty > 0)
                {
                    netfillCash = netQty * avgBidFillPrice + fillCash;
                    openProfit = netQty * (filledPrice - avgBidFillPrice);
                }
                else if (netQty < 0)
                {
                    netfillCash = netQty * avgAskFillPrice + fillCash;
                    openProfit = netQty * (avgAskFillPrice - filledPrice);

                }
                else
                {
                    netfillCash = fillCash;
                }
                totalCommision += commission * filledCount;
                netfillCashByPointValue = netfillCash * pointValue + openProfit;
                netfillCashByPointValue_commision = netfillCash * pointValue - totalCommision + openProfit;

                subCalcMap.Clear();
                subCalcMap.Add("topic", data.topic.ToString());
                subCalcMap.Add("dateTime", dateTime.ToString());
                subCalcMap.Add("askFillCash", askFillCash.ToString());
                subCalcMap.Add("bidFillCash", bidFillCash.ToString());
                subCalcMap.Add("avgAskFillPrice", avgAskFillPrice.ToString());
                subCalcMap.Add("avgBidFillPrice", avgBidFillPrice.ToString());
                subCalcMap.Add("askQty", askQty.ToString());
                subCalcMap.Add("bidQty", bidQty.ToString());
                subCalcMap.Add("netQty", netQty.ToString());
                subCalcMap.Add("fillCash", fillCash.ToString());
                subCalcMap.Add("netfillCash", netfillCash.ToString());
                subCalcMap.Add("netfillCashByPointValue", netfillCashByPointValue.ToString());
                subCalcMap.Add("netfillCashByPointValue_commision", netfillCashByPointValue_commision.ToString());
                subCalcMap.Add("totalCommision", totalCommision.ToString());

                if (netQty > 0)
                {
                    data.POSITION = "+" + netQty.ToString();
                }
                else
                {
                    data.POSITION = netQty.ToString();
                }
                data.netPROFIT = netfillCashByPointValue_commision.ToString();
                data.netfillCashByPointValue_commision = netfillCashByPointValue_commision.ToString();
            }
            else
            {
                Dictionary<string, string> subCalcMap = new Dictionary<string, string>();
                double filledPrice = double.Parse(data.TRADING_PRICE);
                int filledCount = int.Parse(data.TRADING_VOLUMN);
                int side = int.Parse(data.ASK_BID_TYPE_CODE);
                string topic = data.topic;
                if ((SIDE)side == SIDE.ASK)
                {
                    askFillCash += filledPrice * filledCount;
                    askQty += filledCount;
                    fillCash += filledPrice * filledCount;
                    avgAskFillPrice = askFillCash / askQty;
                }
                else
                {
                    bidFillCash += filledPrice * filledCount;
                    bidQty += filledCount;
                    fillCash -= filledPrice * filledCount;
                    avgBidFillPrice = bidFillCash / bidQty;
                }
                netQty = bidQty - askQty;
                if (netQty > 0)
                {
                    netfillCash = netQty * avgBidFillPrice + fillCash;
                }
                else if (netQty < 0)
                {
                    netfillCash = netQty * avgAskFillPrice + fillCash;
                }
                else
                {
                    netfillCash = fillCash;
                }

                totalCommision += commission * filledCount;

                netfillCashByPointValue = netfillCash * pointValue + openProfit;
                netfillCashByPointValue_commision = netfillCash * pointValue - totalCommision + openProfit;

                subCalcMap.Add("topic", data.topic.ToString());
                subCalcMap.Add("dateTime", dateTime.ToString());
                subCalcMap.Add("askFillCash", askFillCash.ToString());
                subCalcMap.Add("bidFillCash", bidFillCash.ToString());
                subCalcMap.Add("avgAskFillPrice", avgAskFillPrice.ToString());
                subCalcMap.Add("avgBidFillPrice", avgBidFillPrice.ToString());
                subCalcMap.Add("askQty", askQty.ToString());
                subCalcMap.Add("bidQty", bidQty.ToString());
                subCalcMap.Add("netQty", netQty.ToString());
                subCalcMap.Add("fillCash", fillCash.ToString());
                subCalcMap.Add("netfillCash", netfillCash.ToString());
                subCalcMap.Add("netfillCashByPointValue", netfillCashByPointValue.ToString());
                subCalcMap.Add("netfillCashByPointValue_commision", netfillCashByPointValue_commision.ToString());
                subCalcMap.Add("totalCommision", totalCommision.ToString());

                calcMap.Add(data.topic, subCalcMap);
                if (netQty > 0)
                {
                    data.POSITION = "+" + netQty.ToString();
                }
                else
                {
                    data.POSITION = netQty.ToString();
                }
                data.netPROFIT = netfillCashByPointValue_commision.ToString();
                data.netfillCashByPointValue_commision = netfillCashByPointValue_commision.ToString();
            }
            insert로직(data);
            insert클라이언트(data);
            GETDATA();
        }

        public void PositionCalcFilled(STRUCT_0374_REPLY data)
        {
            double askFillCash = 0;
            double avgAskFillPrice = 0;
            int askQty = 0;

            double bidFillCash = 0;
            double avgBidFillPrice = 0;
            int bidQty = 0;
            int netQty = 0;

            double fillCash = 0;
            double netfillCash = 0;
            string dateTime = "";
            double netfillCashByPointValue = 0;
            double netfillCashByPointValue_commision = 0;
            double totalCommision = 0;
            double openProfit = 0;
            if (calcMap.ContainsKey(data.topic))
            {
                Dictionary<string, string> subCalcMap = calcMap[data.topic];
                double filledPrice = double.Parse(data.TRADING_PRICE);
                int filledCount = int.Parse(data.TRADING_VOLUMN);
                int side = int.Parse(data.ASK_BID_TYPE_CODE);
                dateTime = data.dateTime.ToString("HH:mm:ss.fff");
                string topic = data.topic;

                askFillCash = Double.Parse(subCalcMap["askFillCash"]);
                bidFillCash = Double.Parse(subCalcMap["bidFillCash"]);
                avgAskFillPrice = Double.Parse(subCalcMap["avgAskFillPrice"]);
                avgBidFillPrice = Double.Parse(subCalcMap["avgBidFillPrice"]);
                askQty = Int32.Parse(subCalcMap["askQty"]);
                bidQty = Int32.Parse(subCalcMap["bidQty"]);
                netQty = Int32.Parse(subCalcMap["netQty"]);
                fillCash = Double.Parse(subCalcMap["fillCash"]);
                netfillCash = Double.Parse(subCalcMap["netfillCash"]);
                totalCommision = Double.Parse(subCalcMap["totalCommision"]);
                netfillCashByPointValue = Double.Parse(subCalcMap["netfillCashByPointValue"]);
                netfillCashByPointValue_commision = Double.Parse(subCalcMap["netfillCashByPointValue_commision"]);

                if ((SIDE)side == SIDE.ASK)
                {
                    askFillCash += filledPrice * filledCount;
                    askQty += filledCount;
                    fillCash += filledPrice * filledCount;
                    avgAskFillPrice = askFillCash / askQty;
                }
                else
                {
                    bidFillCash += filledPrice * filledCount;
                    bidQty += filledCount;
                    fillCash -= filledPrice * filledCount;
                    avgBidFillPrice = bidFillCash / bidQty;
                }

                netQty = bidQty - askQty;
                if (netQty > 0)
                {
                    netfillCash = netQty * avgBidFillPrice + fillCash;
                    openProfit = netQty * (filledPrice - avgBidFillPrice);
                }
                else if (netQty < 0)
                {
                    netfillCash = netQty * avgAskFillPrice + fillCash;
                    openProfit = netQty * (avgAskFillPrice - filledPrice);

                }
                else
                {
                    netfillCash = fillCash;
                }
                totalCommision += commission * filledCount;
                netfillCashByPointValue = netfillCash * pointValue + openProfit;
                netfillCashByPointValue_commision = netfillCash * pointValue - totalCommision + openProfit;

                subCalcMap.Clear();
                subCalcMap.Add("topic", data.topic.ToString());
                subCalcMap.Add("dateTime", dateTime.ToString());
                subCalcMap.Add("askFillCash", askFillCash.ToString());
                subCalcMap.Add("bidFillCash", bidFillCash.ToString());
                subCalcMap.Add("avgAskFillPrice", avgAskFillPrice.ToString());
                subCalcMap.Add("avgBidFillPrice", avgBidFillPrice.ToString());
                subCalcMap.Add("askQty", askQty.ToString());
                subCalcMap.Add("bidQty", bidQty.ToString());
                subCalcMap.Add("netQty", netQty.ToString());
                subCalcMap.Add("fillCash", fillCash.ToString());
                subCalcMap.Add("netfillCash", netfillCash.ToString());
                subCalcMap.Add("netfillCashByPointValue", netfillCashByPointValue.ToString());
                subCalcMap.Add("netfillCashByPointValue_commision", netfillCashByPointValue_commision.ToString());
                subCalcMap.Add("totalCommision", totalCommision.ToString());

                if (netQty > 0)
                {
                    data.POSITION = "+" + netQty.ToString();
                }
                else
                {
                    data.POSITION = netQty.ToString();
                }
                data.netPROFIT = netfillCashByPointValue_commision.ToString();
                data.netfillCashByPointValue_commision = netfillCashByPointValue_commision.ToString();
            }
            else
            {
                Dictionary<string, string> subCalcMap = new Dictionary<string, string>();
                double filledPrice = double.Parse(data.TRADING_PRICE);
                int filledCount = int.Parse(data.TRADING_VOLUMN);
                int side = int.Parse(data.ASK_BID_TYPE_CODE);
                string topic = data.topic;
                if ((SIDE)side == SIDE.ASK)
                {
                    askFillCash += filledPrice * filledCount;
                    askQty += filledCount;
                    fillCash += filledPrice * filledCount;
                    avgAskFillPrice = askFillCash / askQty;
                }
                else
                {
                    bidFillCash += filledPrice * filledCount;
                    bidQty += filledCount;
                    fillCash -= filledPrice * filledCount;
                    avgBidFillPrice = bidFillCash / bidQty;
                }
                netQty = bidQty - askQty;
                if (netQty > 0)
                {
                    netfillCash = netQty * avgBidFillPrice + fillCash;
                }
                else if (netQty < 0)
                {
                    netfillCash = netQty * avgAskFillPrice + fillCash;
                }
                else
                {
                    netfillCash = fillCash;
                }

                totalCommision += commission * filledCount;

                netfillCashByPointValue = netfillCash * pointValue + openProfit;
                netfillCashByPointValue_commision = netfillCash * pointValue - totalCommision + openProfit;

                subCalcMap.Add("topic", data.topic.ToString());
                subCalcMap.Add("dateTime", dateTime.ToString());
                subCalcMap.Add("askFillCash", askFillCash.ToString());
                subCalcMap.Add("bidFillCash", bidFillCash.ToString());
                subCalcMap.Add("avgAskFillPrice", avgAskFillPrice.ToString());
                subCalcMap.Add("avgBidFillPrice", avgBidFillPrice.ToString());
                subCalcMap.Add("askQty", askQty.ToString());
                subCalcMap.Add("bidQty", bidQty.ToString());
                subCalcMap.Add("netQty", netQty.ToString());
                subCalcMap.Add("fillCash", fillCash.ToString());
                subCalcMap.Add("netfillCash", netfillCash.ToString());
                subCalcMap.Add("netfillCashByPointValue", netfillCashByPointValue.ToString());
                subCalcMap.Add("netfillCashByPointValue_commision", netfillCashByPointValue_commision.ToString());
                subCalcMap.Add("totalCommision", totalCommision.ToString());

                calcMap.Add(data.topic, subCalcMap);
                if (netQty > 0)
                {
                    data.POSITION = "+" + netQty.ToString();
                }
                else
                {
                    data.POSITION = netQty.ToString();
                }
                data.netPROFIT = netfillCashByPointValue_commision.ToString();
                data.netfillCashByPointValue_commision = netfillCashByPointValue_commision.ToString();
            }
            insert운용(data);
            GETDATA();
        }
    }

    public class OrderParam
    {
        public static string ASK = "1";
        public static string BID = "2";
        public static string NewOrder = "1";
        public static string ReplaceOrder = "2";
        public static string CancelOrder = "3";
    }

    public enum SIDE
    {
        ASK = 1,
        BID = 2
    }
}
