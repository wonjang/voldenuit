namespace MessageStreamNexux
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
            this.createListener = new System.Windows.Forms.Button();
            this.clearSocket = new System.Windows.Forms.Button();
            this.view = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createListener
            // 
            this.createListener.Location = new System.Drawing.Point(5, 12);
            this.createListener.Name = "createListener";
            this.createListener.Size = new System.Drawing.Size(271, 84);
            this.createListener.TabIndex = 0;
            this.createListener.Text = "createListener";
            this.createListener.UseVisualStyleBackColor = true;
            this.createListener.Click += new System.EventHandler(this.createListener_Click);
            // 
            // clearSocket
            // 
            this.clearSocket.Location = new System.Drawing.Point(4, 102);
            this.clearSocket.Name = "clearSocket";
            this.clearSocket.Size = new System.Drawing.Size(271, 84);
            this.clearSocket.TabIndex = 1;
            this.clearSocket.Text = "clearSocket";
            this.clearSocket.UseVisualStyleBackColor = true;
            this.clearSocket.Click += new System.EventHandler(this.clearSocket_Click);
            // 
            // view
            // 
            this.view.Location = new System.Drawing.Point(5, 192);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(271, 86);
            this.view.TabIndex = 2;
            this.view.Text = "view";
            this.view.UseVisualStyleBackColor = true;
            this.view.Click += new System.EventHandler(this.view_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 290);
            this.Controls.Add(this.view);
            this.Controls.Add(this.clearSocket);
            this.Controls.Add(this.createListener);
            this.Name = "Mainform";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createListener;
        private System.Windows.Forms.Button clearSocket;
        private System.Windows.Forms.Button view;
    }
}

