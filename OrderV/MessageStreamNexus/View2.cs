using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MessageStreamNexux
{
    public partial class View2 : Form
    {
        Mainform mainForm;
        public View2(Mainform mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        public void dataProc(STRUCT_0374_0022 data)
        {
            Mainform.logger.Info("STRUCT_0374_0022");
            DataTable dt = null;
            PropertyInfo[] Props = typeof(STRUCT_0374_0022).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string tableName = "STRUCT_0374_0022_CHART_"+data.topic;
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
                if (this.InvokeRequired)
                {
                    this.EndInvoke(
                        this.BeginInvoke((Action)delegate
                        {
                            if (data.topic.Contains("TXT1"))
                            {
                                chart1.DataSource = dt;
                                chart1.Series.Add("TXT1");
                                chart1.Series["TXT1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                chart1.Series["TXT1"].XValueMember = "dateTime";
                                chart1.Series["TXT1"].YValueMembers = "MSG_VALUE";
                            }
                            else
                            {
                                chart2.DataSource = dt;
                                chart2.Series.Add("TXT2");
                                chart2.Series["TXT2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                                chart2.Series["TXT2"].XValueMember = "dateTime";
                                chart2.Series["TXT2"].YValueMembers = "MSG_VALUE";
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
                        chart1.DataBind();
                        chart2.DataBind();
                    })
                 );
            }
        }
    }
}
