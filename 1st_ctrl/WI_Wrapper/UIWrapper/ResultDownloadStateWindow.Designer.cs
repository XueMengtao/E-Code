namespace WF1
{
    partial class ResultDownloadStateWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.downcontent_comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.resultDownloadOk_button1 = new System.Windows.Forms.Button();
            this.resultDownloadClose_button2 = new System.Windows.Forms.Button();
            this.downloadProcessState_progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "所需下载内容";
            // 
            // downcontent_comboBox1
            // 
            this.downcontent_comboBox1.FormattingEnabled = true;
            this.downcontent_comboBox1.Items.AddRange(new object[] {
            "工程建模文件",
            "工程结果文件"});
            this.downcontent_comboBox1.Location = new System.Drawing.Point(179, 44);
            this.downcontent_comboBox1.Name = "downcontent_comboBox1";
            this.downcontent_comboBox1.Size = new System.Drawing.Size(146, 20);
            this.downcontent_comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "下载进度";
            // 
            // resultDownloadOk_button1
            // 
            this.resultDownloadOk_button1.Location = new System.Drawing.Point(72, 210);
            this.resultDownloadOk_button1.Name = "resultDownloadOk_button1";
            this.resultDownloadOk_button1.Size = new System.Drawing.Size(75, 23);
            this.resultDownloadOk_button1.TabIndex = 3;
            this.resultDownloadOk_button1.Text = "确定";
            this.resultDownloadOk_button1.UseVisualStyleBackColor = true;
            this.resultDownloadOk_button1.Click += new System.EventHandler(this.resultDownloadOk_button1_Click_1);
            // 
            // resultDownloadClose_button2
            // 
            this.resultDownloadClose_button2.Location = new System.Drawing.Point(222, 210);
            this.resultDownloadClose_button2.Name = "resultDownloadClose_button2";
            this.resultDownloadClose_button2.Size = new System.Drawing.Size(75, 23);
            this.resultDownloadClose_button2.TabIndex = 4;
            this.resultDownloadClose_button2.Text = "关闭";
            this.resultDownloadClose_button2.UseVisualStyleBackColor = true;
            this.resultDownloadClose_button2.Click += new System.EventHandler(this.resultDownloadClose_button2_Click);
            // 
            // downloadProcessState_progressBar1
            // 
            this.downloadProcessState_progressBar1.Location = new System.Drawing.Point(179, 123);
            this.downloadProcessState_progressBar1.Name = "downloadProcessState_progressBar1";
            this.downloadProcessState_progressBar1.Size = new System.Drawing.Size(146, 23);
            this.downloadProcessState_progressBar1.TabIndex = 5;
            // 
            // ResultDownloadStateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 273);
            this.Controls.Add(this.downloadProcessState_progressBar1);
            this.Controls.Add(this.resultDownloadClose_button2);
            this.Controls.Add(this.resultDownloadOk_button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.downcontent_comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "ResultDownloadStateWindow";
            this.Text = "仿真结果下载";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox downcontent_comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button resultDownloadOk_button1;
        private System.Windows.Forms.Button resultDownloadClose_button2;
        private System.Windows.Forms.ProgressBar downloadProcessState_progressBar1;

    }
}