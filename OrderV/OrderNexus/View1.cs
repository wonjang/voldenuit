using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderV
{
    public partial class View1 : Form
    {
        MainForm mainForm;
        BindingSource bs1;
        BindingSource bs2;
        BindingSource bs3;
        public View1(MainForm form)
        {
            this.mainForm = form;
            InitializeComponent();
            //timer1.Start();
            bs1 = new BindingSource();
            bs2 = new BindingSource();
            bs3 = new BindingSource();
            mainForm.msgEventHandler.GETDATA += new MsgEventHandler.SocketEventHadler(on_data);
        }

        public void on_data()
        {
            if (this.InvokeRequired)
            {
                this.EndInvoke(
                    this.BeginInvoke((Action)delegate
                    {
                        bs1 = new BindingSource();
                        bs2 = new BindingSource();
                        bs3 = new BindingSource();

                        bs1.DataSource  = mainForm.msgEventHandler.클라이언트테이블;
                        bs2.DataSource = mainForm.msgEventHandler.신규주문테이블;
                        bs3.DataSource = mainForm.msgEventHandler.취소주문테이블;

                        dataGridView1.DataSource = bs1;
                        dataGridView2.DataSource = bs2;
                        dataGridView3.DataSource = bs3;

                        dataGridView1.Columns[0].Width = 20;
                        dataGridView1.Columns[1].Width = 50;
                        dataGridView1.Columns[2].Width = 75;
                        dataGridView1.Columns[3].Width = 75;
                        dataGridView1.Columns[4].Width = 75;
                        dataGridView1.Columns[5].Width = 20;
                        dataGridView1.Columns[6].Width = 75;
                        dataGridView1.Columns[7].Width = 75;
                        dataGridView1.Columns[8].Width = 75;

                        dataGridView2.Columns[0].Width = 20;
                        dataGridView2.Columns[1].Width = 50;
                        dataGridView2.Columns[2].Width = 20;
                        dataGridView2.Columns[3].Width = 65;
                        dataGridView2.Columns[4].Width = 65;
                        dataGridView2.Columns[5].Width = 65;
                        dataGridView2.Columns[6].Width = 65;
                        dataGridView2.Columns[7].Width = 20;
                        dataGridView2.Columns[8].Width = 65;
                        dataGridView2.Columns[9].Width = 65;
                        dataGridView2.Columns[10].Width = 25;
                        dataGridView2.Columns[11].Width = 25;
                        dataGridView2.Columns[12].Width = 25;

                        dataGridView3.Columns[0].Width = 20;
                        dataGridView3.Columns[1].Width = 50;
                        dataGridView3.Columns[2].Width = 20;
                        dataGridView3.Columns[3].Width = 65;
                        dataGridView3.Columns[4].Width = 65;
                        dataGridView3.Columns[5].Width = 65;
                        dataGridView3.Columns[6].Width = 65;
                        dataGridView3.Columns[7].Width = 20;
                        dataGridView3.Columns[8].Width = 65;
                        dataGridView3.Columns[9].Width = 25;
                        dataGridView3.Columns[10].Width = 25;
                        dataGridView3.Columns[11].Width = 25;

                    })
                 );
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dtTicket;
            DataTable dtNewOrder;
            DataTable dtCancel;
            if (mainForm.dataTableMap.ContainsKey("클라이언트"))
            {
                dtTicket = mainForm.dataTableMap["클라이언트"];
                dataGridView1.DataSource = dtTicket;
                dataGridView1.Refresh();
            }
            if (mainForm.dataTableMap.ContainsKey("신규주문"))
            {
                dtNewOrder = mainForm.dataTableMap["신규주문"];
                dataGridView2.DataSource = dtNewOrder;
                dataGridView2.Refresh();
            }
            if (mainForm.dataTableMap.ContainsKey("취소주문"))
            {
                dtCancel = mainForm.dataTableMap["취소주문"];
                dataGridView3.DataSource = dtCancel;
                dataGridView3.Refresh();
            }
        }
    }
}
