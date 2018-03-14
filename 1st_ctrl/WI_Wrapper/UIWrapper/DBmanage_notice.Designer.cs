namespace WF1
{
    partial class DBmanage_notice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DBmanage_notice_button1 = new System.Windows.Forms.Button();
            this.DBmanage_notice_button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DBmanage_notice_button1
            // 
            this.DBmanage_notice_button1.Location = new System.Drawing.Point(79, 102);
            this.DBmanage_notice_button1.Name = "DBmanage_notice_button1";
            this.DBmanage_notice_button1.Size = new System.Drawing.Size(75, 23);
            this.DBmanage_notice_button1.TabIndex = 0;
            this.DBmanage_notice_button1.Text = "确定";
            this.DBmanage_notice_button1.UseVisualStyleBackColor = true;
            this.DBmanage_notice_button1.Click += new System.EventHandler(this.DBmanage_notice_button1_Click);
            // 
            // DBmanage_notice_button2
            // 
            this.DBmanage_notice_button2.Location = new System.Drawing.Point(237, 102);
            this.DBmanage_notice_button2.Name = "DBmanage_notice_button2";
            this.DBmanage_notice_button2.Size = new System.Drawing.Size(75, 23);
            this.DBmanage_notice_button2.TabIndex = 1;
            this.DBmanage_notice_button2.Text = "取消";
            this.DBmanage_notice_button2.UseVisualStyleBackColor = true;
            this.DBmanage_notice_button2.Click += new System.EventHandler(this.DBmanage_notice_button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "数据已经更改，是否同步到数据库？";
            // 
            // DBmanage_notice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 152);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DBmanage_notice_button2);
            this.Controls.Add(this.DBmanage_notice_button1);
            this.Name = "DBmanage_notice";
            this.Text = "提示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DBmanage_notice_button1;
        private System.Windows.Forms.Button DBmanage_notice_button2;
        public System.Windows.Forms.Label label1;
    }
}