namespace MessageStreamManager
{
    partial class Mainform
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.send = new System.Windows.Forms.Button();
            this.connect = new System.Windows.Forms.Button();
            this.inputStreamGrid = new System.Windows.Forms.DataGridView();
            this.outStreamGrid = new System.Windows.Forms.DataGridView();
            this.socketGrid = new System.Windows.Forms.DataGridView();
            this.read = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.inputStreamGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outStreamGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.socketGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(104, 356);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(94, 23);
            this.send.TabIndex = 0;
            this.send.Text = "sendStream";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(12, 315);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(186, 35);
            this.connect.TabIndex = 1;
            this.connect.Text = "connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // inputStreamGrid
            // 
            this.inputStreamGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inputStreamGrid.Location = new System.Drawing.Point(1, 3);
            this.inputStreamGrid.Name = "inputStreamGrid";
            this.inputStreamGrid.RowTemplate.Height = 23;
            this.inputStreamGrid.Size = new System.Drawing.Size(818, 150);
            this.inputStreamGrid.TabIndex = 2;
            this.inputStreamGrid.Visible = false;
            // 
            // outStreamGrid
            // 
            this.outStreamGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outStreamGrid.Location = new System.Drawing.Point(1, 159);
            this.outStreamGrid.Name = "outStreamGrid";
            this.outStreamGrid.RowTemplate.Height = 23;
            this.outStreamGrid.Size = new System.Drawing.Size(818, 150);
            this.outStreamGrid.TabIndex = 3;
            this.outStreamGrid.Visible = false;
            // 
            // socketGrid
            // 
            this.socketGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.socketGrid.Location = new System.Drawing.Point(204, 315);
            this.socketGrid.Name = "socketGrid";
            this.socketGrid.RowTemplate.Height = 23;
            this.socketGrid.Size = new System.Drawing.Size(615, 64);
            this.socketGrid.TabIndex = 4;
            this.socketGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.socketGrid_CellContentClick);
            this.socketGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.socketGrid_rowContentClick);
            // 
            // read
            // 
            this.read.Location = new System.Drawing.Point(12, 356);
            this.read.Name = "read";
            this.read.Size = new System.Drawing.Size(86, 23);
            this.read.TabIndex = 5;
            this.read.Text = "readStream";
            this.read.UseVisualStyleBackColor = true;
            this.read.Click += new System.EventHandler(this.read_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 391);
            this.Controls.Add(this.read);
            this.Controls.Add(this.socketGrid);
            this.Controls.Add(this.outStreamGrid);
            this.Controls.Add(this.inputStreamGrid);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.send);
            this.Name = "Mainform";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.inputStreamGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outStreamGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.socketGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.DataGridView inputStreamGrid;
        private System.Windows.Forms.DataGridView outStreamGrid;
        private System.Windows.Forms.DataGridView socketGrid;
        private System.Windows.Forms.Button read;
    }
}

