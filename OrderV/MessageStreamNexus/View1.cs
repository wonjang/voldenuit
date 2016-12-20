using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
namespace MessageStreamNexux
{
    public partial class View1 : Form
    {
        Mainform mainForm;
        public View1(Mainform mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
          //  Task dequeueProc = new Task(() => this.GetDataFromQueue());
          //  dequeueProc.Start();
        }

        public void GetDataFromQueue()
        {
            STRUCT_0374_0022 querrydata = null;
            STRUCT_0022 data0022 = null;
            while (true)
            {
                while (mainForm.messageQueue.Count > 0)
                {
                    StremEntity entity = mainForm.messageQueue.Dequeue();
                    if (entity != null)
                    {
                        if (entity.header.Equals("0022"))
                        {
                            STRUCT_0022 data = new STRUCT_0022(entity.topic, entity.content);
                            data0022 = data;
                            dataProc(data);
                            /*
                            if (querrydata != null)
                            {
                                mainForm.view2.dataProc(querrydata);
                            }
                            */
                        }
                        else if (entity.header.Equals("0374"))
                        {
                            STRUCT_0374 data = new STRUCT_0374(entity.topic, entity.content);
                            if (data0022 != null)
                            {
                                querrydata = dataProc_Query(data, data0022);
                                dataProc(querrydata);
                            }
                        }
                    }
                    // Thread.Sleep(1);
                }
            }
        }
       STRUCT_0374_0022 querrydata = null;
       STRUCT_0022 data0022 = null;

        public void sendmessage(StremEntity entity)
        {
            if (entity != null)
            {
                if (entity.header.Equals("0022"))
                {
                    STRUCT_0022 data = new STRUCT_0022(entity.topic, entity.content);
                    data0022 = data;
                    dataProc(data);
                    /*
                    if (querrydata != null)
                    {
                        mainForm.view2.dataProc(querrydata);
                    }
                    */
                }
                else if (entity.header.Equals("0374"))
                {
                    STRUCT_0374 data = new STRUCT_0374(entity.topic, entity.content);
                    if (data0022 != null)
                    {
                        querrydata = dataProc_Query(data, data0022);
                        dataProc(querrydata);
                    }
                }
            }
        }

        int seq_topic1 = 0;
        int seq_topic2 = 0;

        public STRUCT_0374_0022 dataProc_Query(STRUCT_0374 data)
        {
            PropertyInfo[] Props0022 = typeof(STRUCT_0022).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            DataTable dt0022 = null;
            string tableName = "STRUCT_0022_" + data.topic;

            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt0022 = mainForm.dataTableMap[tableName];
            }
            else
            {
                dt0022 = new DataTable();
                dt0022.TableName = tableName;
                foreach (PropertyInfo prop in Props0022)
                {
                    dt0022.Columns.Add(prop.Name);
                }
                mainForm.dataTableMap.Add(tableName, dt0022);
            }
            var query = from p in dt0022.AsEnumerable()
                        where Convert.ToDateTime(p.Field<string>("dateTime")).Hour <= data.dateTime.Hour &&
                                Convert.ToDateTime(p.Field<string>("dateTime")).Minute <= data.dateTime.Minute &&
                              Convert.ToDateTime(p.Field<string>("dateTime")).Second <= data.dateTime.Second &&
                              Convert.ToDateTime(p.Field<string>("dateTime")).Millisecond <= data.dateTime.Millisecond &&
                              p.Field<string>("topic") == data.topic
                        orderby Convert.ToDateTime(p.Field<string>("dateTime")) descending
                        select p;



            var last = query.AsEnumerable()
                            .FirstOrDefault();
            if (data.topic.Contains("TXT1"))
            {
                seq_topic1++;
                return new STRUCT_0374_0022(seq_topic1, data.topic, data, new STRUCT_0022(last));
            }
            else
            {
                seq_topic2++;
                return new STRUCT_0374_0022(seq_topic2, data.topic, data, new STRUCT_0022(last));

            }
        }
        public STRUCT_0374_0022 dataProc_Query(STRUCT_0374 data, STRUCT_0022 data2)
        {
            PropertyInfo[] Props0022 = typeof(STRUCT_0022).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            DataTable dt0022 = null;
            string tableName = "STRUCT_0022_" + data.topic;

            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt0022 = mainForm.dataTableMap[tableName];
            }
            else
            {
                dt0022 = new DataTable();
                dt0022.TableName = tableName;
                foreach (PropertyInfo prop in Props0022)
                {
                    dt0022.Columns.Add(prop.Name);
                }
                mainForm.dataTableMap.Add(tableName, dt0022);
            }
            if (data.topic.Contains("TXT1"))
            {
                seq_topic1++;
                return new STRUCT_0374_0022(seq_topic1, data.topic, data, data2);
            }
            else
            {
                seq_topic2++;
                return new STRUCT_0374_0022(seq_topic2, data.topic, data, data2);

            }
        }

        int chart1min = 0;
        int chart2min = 0;
        public void dataProc(STRUCT_0022 data)
        {
            Mainform.logger.Info("STRUCT_0022");
            DataTable dt = null;
            DataTable dt_min = null;

            PropertyInfo[] Props = typeof(STRUCT_0022).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string tableName = "STRUCT_0022_" + data.topic;

            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt = mainForm.dataTableMap[tableName];
                dt_min = mainForm.dataTableMap[tableName + "_MIN"];
            }
            else
            {
                dt = new DataTable();
                dt_min = new DataTable();

                dt.TableName = tableName;
                dt_min.TableName = tableName + "_MIN";

                foreach (PropertyInfo prop in Props)
                {
                    dt.Columns.Add(prop.Name);
                    dt_min.Columns.Add(prop.Name);
                    if (prop.Name.Equals("formatDateTime") || prop.Name.Equals("VALUE"))
                    {

                    }
                    else
                    {
                        dt_min.Columns[prop.Name].ColumnMapping = MappingType.Hidden;
                    }
                }
                mainForm.dataTableMap.Add(tableName, dt);
                mainForm.dataTableMap.Add(tableName + "_MIN", dt_min);

                if (this.InvokeRequired)
                {
                    this.EndInvoke(
                        this.BeginInvoke((Action)delegate
                        {
                            if (data.topic.Contains("TXT1"))
                            {
                                mainForm.view2.chart1.DataSource = dt_min;
                                mainForm.view2.chart1.Series.Add("MSG_VALUE");
                                mainForm.view2.chart1.Series["MSG_VALUE"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                mainForm.view2.chart1.Series["MSG_VALUE"].XValueMember = "chartTime";
                                mainForm.view2.chart1.Series["MSG_VALUE"].YValueMembers = "MSG_VALUE";
                                mainForm.view2.chart1.DataBind();
                            }
                            else
                            {
                                mainForm.view2.chart2.DataSource = dt_min;
                                mainForm.view2.chart2.Series.Add("MSG_VALUE");
                                mainForm.view2.chart2.Series["MSG_VALUE"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                mainForm.view2.chart2.Series["MSG_VALUE"].XValueMember = "chartTime";
                                mainForm.view2.chart2.Series["MSG_VALUE"].YValueMembers = "MSG_VALUE";
                                mainForm.view2.chart2.DataBind();

                            }
                        })
                     );
                }
            }

            DataRow row = dt.NewRow();
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                row[i] = Props[i].GetValue(data, null);
            }
            dt.Rows.Add(row);

            if (this.InvokeRequired)
            {
                this.EndInvoke(
                    this.BeginInvoke((Action)delegate
                    {
                        DataRow row_min = dt_min.NewRow();
                        var values_min = new object[Props.Length];
                        for (int i = 0; i < Props.Length; i++)
                        {
                            row_min[i] = Props[i].GetValue(data, null);
                        }
                        if (data.topic.Contains("TXT1"))
                        {
                            if (chart1min < data.dateTime.Minute)
                            {
                                chart1min = data.dateTime.Minute;
                                dt_min.Rows.Add(row_min);
                                mainForm.view2.dataGridView1.DataSource = dt_min;
                                mainForm.view2.chart1.DataBind();

                            }
                        }
                        else
                        {
                            if (chart2min < data.dateTime.Minute)
                            {
                                chart2min = data.dateTime.Minute;
                                dt_min.Rows.Add(row_min);
                                mainForm.view2.dataGridView3.DataSource = dt_min;
                                mainForm.view2.chart2.DataBind();
                            }
                        }
                    })
                 );
            }

        }
        public void dataProc(STRUCT_0374 data)
        {
            Mainform.logger.Info("STRUCT_0374");
            DataTable dt = null;
            PropertyInfo[] Props = typeof(STRUCT_0374).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string tableName = "STRUCT_0374" + data.topic;

            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt = mainForm.dataTableMap[tableName];
            }
            else
            {
                dt = new DataTable();
                dt.TableName = tableName;
                foreach (PropertyInfo prop in Props)
                {
                    dt.Columns.Add(prop.Name);

                }

                mainForm.dataTableMap.Add(tableName, dt);
            }
            DataRow row = dt.NewRow();
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                row[i] = Props[i].GetValue(data, null);
            }
            dt.Rows.Add(row);


        }
        int chart3min = 0;
        int chart4min = 0;
        public void dataProc(STRUCT_0374_0022 data)
        {
            Mainform.logger.Info("STRUCT_0374_0022");
            calcPositionProfit(data);
            DataTable dt = null;
            DataTable dt_min = null;

            PropertyInfo[] Props = typeof(STRUCT_0374_0022).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string tableName = "STRUCT_0374_0022_" + data.topic;

            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt = mainForm.dataTableMap[tableName];
                dt_min = mainForm.dataTableMap[tableName + "_MIN"];

            }
            else
            {
                dt = new DataTable();
                dt_min = new DataTable();

                dt.TableName = tableName;
                dt_min.TableName = tableName + "_MIN";

                foreach (PropertyInfo prop in Props)
                {
                    dt.Columns.Add(prop.Name);
                    dt_min.Columns.Add(prop.Name);

                    if (prop.Name.Equals("seq") || prop.Name.Equals("formatDateTime") || prop.Name.Equals("PRICE") || prop.Name.Equals("QTY") || prop.Name.Equals("POSITION") || prop.Name.Equals("netPROFIT") || prop.Name.Equals("VALUE"))
                    {

                    }
                    else
                    {
                        dt.Columns[prop.Name].ColumnMapping = MappingType.Hidden;
                        dt_min.Columns[prop.Name].ColumnMapping = MappingType.Hidden;
                    }

                }
                mainForm.dataTableMap.Add(tableName, dt);
                mainForm.dataTableMap.Add(tableName + "_MIN", dt_min);

                if (this.InvokeRequired)
                {
                    this.EndInvoke(
                        this.BeginInvoke((Action)delegate
                        {
                            if (data.topic.Contains("TXT1"))
                            {
                                dataGridView1.DataSource = dt;
                                /*
                                dataGridView1.Columns[0].Width = 30;
                                dataGridView1.Columns[3].Width = 30;
                                dataGridView1.Columns[5].Width = 30;
                                dataGridView1.Columns[6].Width = 50;
                                dataGridView1.Columns[7].Width = 50;
                                */
                                mainForm.view2.chart3.DataSource = dt_min;
                                mainForm.view2.chart3.Series.Add("netPROFIT");
                                mainForm.view2.chart3.Series["netPROFIT"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                mainForm.view2.chart3.Series["netPROFIT"].XValueMember = "chartTime";
                                mainForm.view2.chart3.Series["netPROFIT"].YValueMembers = "netPROFIT";
                            }
                            else
                            {
                                dataGridView2.DataSource = dt;
                                /*
                                dataGridView2.Columns[0].Width = 30;
                                dataGridView2.Columns[3].Width = 30;
                                dataGridView2.Columns[5].Width = 30;
                                dataGridView2.Columns[6].Width = 50;
                                dataGridView2.Columns[7].Width = 50;
                                */
                                mainForm.view2.chart4.DataSource = dt_min;
                                mainForm.view2.chart4.Series.Add("netPROFIT");
                                mainForm.view2.chart4.Series["netPROFIT"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                mainForm.view2.chart4.Series["netPROFIT"].XValueMember = "chartTime";
                                mainForm.view2.chart4.Series["netPROFIT"].YValueMembers = "netPROFIT";

                            }
                        })
                     );
                }
            }

            DataRow row = dt.NewRow();
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                row[i] = Props[i].GetValue(data, null);
            }
            dt.Rows.Add(row);
            if (this.InvokeRequired)
            {
                this.EndInvoke(
                    this.BeginInvoke((Action)delegate
                    {
                        if (data.topic.Contains("TXT1"))
                        {
                            dataGridView1.DataSource = dt;

                        }
                        else
                        {
                            dataGridView2.DataSource = dt;
                        }
                    })
                 );
            }

            if (this.InvokeRequired)
            {
                this.EndInvoke(
                    this.BeginInvoke((Action)delegate
                    {
                        DataRow row_min = dt_min.NewRow();
                        var values_min = new object[Props.Length];
                        for (int i = 0; i < Props.Length; i++)
                        {
                            row_min[i] = Props[i].GetValue(data, null);
                        }
                        if (data.topic.Contains("TXT1"))
                        {
                            if (chart3min < data.dateTime.Minute)
                            {
                                chart3min = data.dateTime.Minute;
                                dt_min.Rows.Add(row_min);
                                mainForm.view2.chart3.DataBind();

                            }
                        }
                        else
                        {
                            if (chart4min < data.dateTime.Minute)
                            {
                                chart4min = data.dateTime.Minute;
                                dt_min.Rows.Add(row_min);
                                mainForm.view2.chart4.DataBind();
                            }
                        }
                    })
                 );
            }
        }

        double commission = 0.01;
        double pointValue = 1;
        Dictionary<string, Dictionary<string, string>> calcMap = new Dictionary<string, Dictionary<string, string>>();
        public void calcPositionProfit(STRUCT_0374_0022 data)
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
                subCalcMap.Add("chartTime", data.chartTime.ToString());
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
                    data.POSITION =  netQty.ToString();
                }
                data.netPROFIT = netfillCashByPointValue_commision.ToString();
                drawCalcMap(subCalcMap);

            }
            else
            {
                Dictionary<string, string> subCalcMap = new Dictionary<string, string>();
                double filledPrice = double.Parse(data.TRADING_PRICE);
                int filledCount = int.Parse(data.TRADING_VOLUMN);
                int side = int.Parse(data.ASK_BID_TYPE_CODE);
                dateTime = data.dateTime2.ToString("HH:mm:ss.fff");
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
                subCalcMap.Add("chartTime", data.chartTime.ToString());
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
                drawCalcMap(subCalcMap);
            }
        }
        public void drawCalcMap(Dictionary<string, string> data)
        {
            Dictionary<string, string> subMap = data;
            string json = JsonConvert.SerializeObject(subMap);
            string tableName = "POSITION_" + data["topic"];
            DataTable dt;
            if (mainForm.dataTableMap.ContainsKey(tableName))
            {
                dt = mainForm.dataTableMap[tableName];
                DataRow row = dt.NewRow();
                foreach (string fieldName in subMap.Keys)
                {
                    row[fieldName] = subMap[fieldName];
                }
                dt.Rows.Add(row);
                if (this.InvokeRequired)
                {
                    this.EndInvoke(
                        this.BeginInvoke((Action)delegate
                        {
                            if (data["topic"].Contains("TXT1"))
                            {
                                mainForm.view2.dataGridView2.DataSource = dt;
                            }
                            else
                            {
                                mainForm.view2.dataGridView4.DataSource = dt;
                            }
                        })
                    );
                }
            }
            else
            {
                dt = new DataTable();
                dt.TableName = tableName;
                foreach (string fieldName in subMap.Keys)
                {
                    dt.Columns.Add(fieldName);
                    if (fieldName.Equals("dateTime") || fieldName.Equals("netQty") || fieldName.Equals("netfillCashByPointValue_commision"))
                    {
                    }
                    else
                    {
                        dt.Columns[fieldName].ColumnMapping = MappingType.Hidden;
                    }

                }
                DataRow row = dt.NewRow();
                foreach (string fieldName in subMap.Keys)
                {
                    row[fieldName] = subMap[fieldName];
                }
                dt.Rows.Add(row);
                mainForm.dataTableMap.Add(tableName, dt);
                if (this.InvokeRequired)
                {
                    this.EndInvoke(
                        this.BeginInvoke((Action)delegate
                        {
                            if (data["topic"].Contains("TXT1"))
                            {
                                mainForm.view2.dataGridView2.DataSource = dt;
                                mainForm.view2.dataGridView2.Refresh();
                            }
                            else
                            {
                                mainForm.view2.dataGridView4.DataSource = dt;
                                mainForm.view2.dataGridView2.Refresh();

                            }
                        })
                    );
                }
            }
            if (this.InvokeRequired)
            {
                this.EndInvoke(
                    this.BeginInvoke((Action)delegate
                    {
                        //textBox1.Text = textBox1.Text +"\r\n"+ data["topic"]+" : "+json;
                        string msg = string.Format("topic   :{0,-12}     profit:{1,-12}     commision:{2,-12}   profit-comm:{3,-12}", data["topic"], data["netfillCashByPointValue"], (double.Parse(data["netfillCashByPointValue"]) - double.Parse(data["netfillCashByPointValue_commision"])).ToString(), data["netfillCashByPointValue_commision"]);
                        mainForm.view2.chart3.DataBind();
                        mainForm.view2.chart4.DataBind();
                        if (data["topic"].Contains("TXT1"))
                        {
                            mainForm.view2.dataGridView2.DataSource = dt;
                            textBox1.Clear();
                            textBox1.Text = msg;
                        }
                        else
                        {
                            mainForm.view2.dataGridView2.DataSource = dt;
                            textBox2.Clear();
                            textBox2.Text = msg;

                        }
                    })
                );
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
    public enum SIDE
    {
        ASK = 1,
        BID = 2
    }
}
