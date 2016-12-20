namespace MsgSend
{
    partial class MsgSender
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
            this.IP = new System.Windows.Forms.Label();
            this.tbxip = new System.Windows.Forms.TextBox();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxVolumn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnconnect = new System.Windows.Forms.Button();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnipSave = new System.Windows.Forms.Button();
            this.tbxCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.AutoSize = true;
            this.IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IP.Location = new System.Drawing.Point(24, 22);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(21, 18);
            this.IP.TabIndex = 0;
            this.IP.Text = "IP";
            // 
            // tbxip
            // 
            this.tbxip.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbxip.Location = new System.Drawing.Point(49, 22);
            this.tbxip.Name = "tbxip";
            this.tbxip.Size = new System.Drawing.Size(100, 22);
            this.tbxip.TabIndex = 1;
            this.tbxip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbxPort
            // 
            this.tbxPort.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbxPort.Location = new System.Drawing.Point(232, 24);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(100, 22);
            this.tbxPort.TabIndex = 3;
            this.tbxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(178, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "PORT";
            // 
            // tbxPrice
            // 
            this.tbxPrice.Location = new System.Drawing.Point(234, 121);
            this.tbxPrice.Name = "tbxPrice";
            this.tbxPrice.Size = new System.Drawing.Size(100, 21);
            this.tbxPrice.TabIndex = 7;
            this.tbxPrice.Text = "1111.10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(180, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "가격";
            // 
            // tbxType
            // 
            this.tbxType.Location = new System.Drawing.Point(51, 119);
            this.tbxType.Name = "tbxType";
            this.tbxType.Size = new System.Drawing.Size(100, 21);
            this.tbxType.TabIndex = 5;
            this.tbxType.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "유형";
            // 
            // tbxVolumn
            // 
            this.tbxVolumn.Location = new System.Drawing.Point(426, 120);
            this.tbxVolumn.Name = "tbxVolumn";
            this.tbxVolumn.Size = new System.Drawing.Size(100, 21);
            this.tbxVolumn.TabIndex = 9;
            this.tbxVolumn.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(372, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "수량";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(55, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "(매도:1 매수:2)";
            // 
            // btnconnect
            // 
            this.btnconnect.Location = new System.Drawing.Point(357, 16);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(75, 32);
            this.btnconnect.TabIndex = 11;
            this.btnconnect.Text = "서버연결";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.btnconnect_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(554, 108);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(75, 32);
            this.btnOrder.TabIndex = 12;
            this.btnOrder.Text = "주문";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnclose
            // 
            this.btnclose.Location = new System.Drawing.Point(451, 16);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(75, 32);
            this.btnclose.TabIndex = 13;
            this.btnclose.Text = "서버끊기";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(555, 175);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 32);
            this.button4.TabIndex = 16;
            this.button4.Text = "대량주문";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnipSave
            // 
            this.btnipSave.Location = new System.Drawing.Point(554, 16);
            this.btnipSave.Name = "btnipSave";
            this.btnipSave.Size = new System.Drawing.Size(75, 32);
            this.btnipSave.TabIndex = 17;
            this.btnipSave.Text = "서버IP저장";
            this.btnipSave.UseVisualStyleBackColor = true;
            this.btnipSave.Click += new System.EventHandler(this.btnipSave_Click);
            // 
            // tbxCount
            // 
            this.tbxCount.Location = new System.Drawing.Point(426, 175);
            this.tbxCount.Name = "tbxCount";
            this.tbxCount.Size = new System.Drawing.Size(100, 21);
            this.tbxCount.TabIndex = 19;
            this.tbxCount.Text = "10";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(315, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 18);
            this.label6.TabIndex = 18;
            this.label6.Text = "대량주문반복횟수";
            // 
            // MsgSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 261);
            this.Controls.Add(this.tbxCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnipSave);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.btnconnect);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxVolumn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxip);
            this.Controls.Add(this.IP);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MsgSender";
            this.Text = "MsgSender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IP;
        private System.Windows.Forms.TextBox tbxip;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxVolumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnipSave;
        private System.Windows.Forms.TextBox tbxCount;
        private System.Windows.Forms.Label label6;
    }
}

