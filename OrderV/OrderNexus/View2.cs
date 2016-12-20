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
    public partial class View2 : Form
    {
        MainForm mainForm;
        BindingSource bs1;
        BindingSource bs2;
        public View2(MainForm form)
        {
            this.mainForm = form;
            InitializeComponent();
            // timer1.Start();
            bs1 = new BindingSource();
            bs2 = new BindingSource();
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
                        bs1.DataSource = mainForm.msgEventHandler.로직테이블;
                        bs2.DataSource = mainForm.msgEventHandler.운용테이블;
                        dataGridView1.DataSource = bs1;
                        dataGridView2.DataSource = bs2; 
                    })
                 );
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dtTicket;
            DataTable dtFilled;
            if (mainForm.dataTableMap.ContainsKey("로직테이블"))
            {
                dtTicket = mainForm.dataTableMap["로직테이블"];
                dataGridView1.DataSource = dtTicket;
                dataGridView1.Refresh();
            }
            if (mainForm.dataTableMap.ContainsKey("운용테이블"))
            {
                dtFilled = mainForm.dataTableMap["운용테이블"];
                dataGridView2.DataSource = dtFilled;
                dataGridView2.Refresh();
            }
        }
    }
}
