namespace WF1
{
    partial class AntennaDBmanage_notice
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
            this.AntennaDBmanage_notice_button1 = new System.Windows.Forms.Button();
            this.AntennaDBmanage_notice_button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AntennaDBmanage_notice_button1
            // 
            this.AntennaDBmanage_notice_button1.Location = new System.Drawing.Point(88, 117);
            this.AntennaDBmanage_notice_button1.Name = "AntennaDBmanage_notice_button1";
            this.AntennaDBmanage_notice_button1.Size = new System.Drawing.Size(75, 23);
            this.AntennaDBmanage_notice_button1.TabIndex = 0;
            this.AntennaDBmanage_notice_button1.Text = "确定";
            this.AntennaDBmanage_notice_button1.UseVisualStyleBackColor = true;
            this.AntennaDBmanage_notice_button1.Click += new System.EventHandler(this.AntennaDBmanage_notice_button1_Click);
            // 
            // AntennaDBmanage_notice_button2
            // 
            this.AntennaDBmanage_notice_button2.Location = new System.Drawing.Point(238, 117);
            this.AntennaDBmanage_notice_button2.Name = "AntennaDBmanage_notice_button2";
            this.AntennaDBmanage_notice_button2.Size = new System.Drawing.Size(75, 23);
            this.AntennaDBmanage_notice_button2.TabIndex = 1;
            this.AntennaDBmanage_notice_button2.Text = "取消";
            this.AntennaDBmanage_notice_button2.UseVisualStyleBackColor = true;
            this.AntennaDBmanage_notice_button2.Click += new System.EventHandler(this.AntennaDBmanage_notice_button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // AntennaDBmanage_notice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 171);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AntennaDBmanage_notice_button2);
            this.Controls.Add(this.AntennaDBmanage_notice_button1);
            this.Name = "AntennaDBmanage_notice";
            this.Text = "提示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AntennaDBmanage_notice_button1;
        private System.Windows.Forms.Button AntennaDBmanage_notice_button2;
        public System.Windows.Forms.Label label1;
    }
}