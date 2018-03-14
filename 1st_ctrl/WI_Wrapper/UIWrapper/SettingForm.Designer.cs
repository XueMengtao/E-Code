namespace WF1
{
    partial class SettingForm
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
            this.setting_label = new System.Windows.Forms.Label();
            this.setting_textBox = new System.Windows.Forms.TextBox();
            this.setting_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.browseBut = new System.Windows.Forms.Button();
            this.OK_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setting_label
            // 
            this.setting_label.AutoSize = true;
            this.setting_label.Location = new System.Drawing.Point(12, 29);
            this.setting_label.Name = "setting_label";
            this.setting_label.Size = new System.Drawing.Size(155, 12);
            this.setting_label.TabIndex = 0;
            this.setting_label.Text = "Wireless InSite的安装路径";
            // 
            // setting_textBox
            // 
            this.setting_textBox.Location = new System.Drawing.Point(173, 26);
            this.setting_textBox.Name = "setting_textBox";
            this.setting_textBox.ReadOnly = true;
            this.setting_textBox.Size = new System.Drawing.Size(202, 21);
            this.setting_textBox.TabIndex = 1;
            // 
            // browseBut
            // 
            this.browseBut.Location = new System.Drawing.Point(159, 87);
            this.browseBut.Name = "browseBut";
            this.browseBut.Size = new System.Drawing.Size(97, 22);
            this.browseBut.TabIndex = 2;
            this.browseBut.Text = "浏览";
            this.browseBut.UseVisualStyleBackColor = true;
            this.browseBut.Click += new System.EventHandler(this.SettingBrowerButton_Click);
            // 
            // OK_Btn
            // 
            this.OK_Btn.Location = new System.Drawing.Point(300, 87);
            this.OK_Btn.Name = "OK_Btn";
            this.OK_Btn.Size = new System.Drawing.Size(75, 23);
            this.OK_Btn.TabIndex = 3;
            this.OK_Btn.Text = "确定";
            this.OK_Btn.UseVisualStyleBackColor = true;
            this.OK_Btn.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 151);
            this.Controls.Add(this.OK_Btn);
            this.Controls.Add(this.browseBut);
            this.Controls.Add(this.setting_textBox);
            this.Controls.Add(this.setting_label);
            this.Name = "SettingForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog setting_folderBrowserDialog;
        public System.Windows.Forms.Label setting_label;
        public System.Windows.Forms.Button OK_Btn;
        public System.Windows.Forms.Button browseBut;
        public System.Windows.Forms.TextBox setting_textBox;
    }
}