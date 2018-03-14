namespace ControlNodeSetting
{
    partial class ControlNodeDBSettingForm
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
            this.OK_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DBAccount_textBox = new System.Windows.Forms.TextBox();
            this.DBPassWord_textBox = new System.Windows.Forms.TextBox();
            this.DB_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.DB_ipAddressBox = new ControlLibrary.IPAddressBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "数据库服务器的IP地址";
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(326, 152);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 4;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "数据库服务器登陆的用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "数据库服务器登陆的密码";
            // 
            // DBAccount_textBox
            // 
            this.DBAccount_textBox.Location = new System.Drawing.Point(182, 89);
            this.DBAccount_textBox.Name = "DBAccount_textBox";
            this.DBAccount_textBox.Size = new System.Drawing.Size(114, 21);
            this.DBAccount_textBox.TabIndex = 7;
            // 
            // DBPassWord_textBox
            // 
            this.DBPassWord_textBox.Location = new System.Drawing.Point(182, 122);
            this.DBPassWord_textBox.Name = "DBPassWord_textBox";
            this.DBPassWord_textBox.Size = new System.Drawing.Size(114, 21);
            this.DBPassWord_textBox.TabIndex = 8;
            this.DBPassWord_textBox.UseSystemPasswordChar = true;
            // 
            // DB_textBox
            // 
            this.DB_textBox.Location = new System.Drawing.Point(181, 59);
            this.DB_textBox.Name = "DB_textBox";
            this.DB_textBox.Size = new System.Drawing.Size(114, 21);
            this.DB_textBox.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "数据库的名称";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DB_ipAddressBox
            // 
            this.DB_ipAddressBox.Location = new System.Drawing.Point(146, 14);
            this.DB_ipAddressBox.Name = "DB_ipAddressBox";
            this.DB_ipAddressBox.Size = new System.Drawing.Size(236, 34);
            this.DB_ipAddressBox.TabIndex = 3;
            // 
            // ControlNodeDBSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 189);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DB_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DBPassWord_textBox);
            this.Controls.Add(this.DBAccount_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.DB_ipAddressBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ControlNodeDBSettingForm";
            this.Text = "主控和数据库服务器的设置";
            this.Load += new System.EventHandler(this.ControlNodeDBSettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ControlLibrary.IPAddressBox DB_ipAddressBox;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DBAccount_textBox;
        private System.Windows.Forms.TextBox DBPassWord_textBox;
        private System.Windows.Forms.TextBox DB_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}