namespace NodeApplication
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
            this.ControlNode_IpAddressBox = new ControlLibrary.IPAddressBox();
            this.label1 = new System.Windows.Forms.Label();
            this.setting_OkBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ControlNode_IpAddressBox
            // 
            this.ControlNode_IpAddressBox.Location = new System.Drawing.Point(100, 21);
            this.ControlNode_IpAddressBox.Name = "ControlNode_IpAddressBox";
            this.ControlNode_IpAddressBox.Size = new System.Drawing.Size(236, 34);
            this.ControlNode_IpAddressBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "主控的IP地址";
            // 
            // setting_OkBtn
            // 
            this.setting_OkBtn.Location = new System.Drawing.Point(253, 74);
            this.setting_OkBtn.Name = "setting_OkBtn";
            this.setting_OkBtn.Size = new System.Drawing.Size(75, 23);
            this.setting_OkBtn.TabIndex = 2;
            this.setting_OkBtn.Text = "确定";
            this.setting_OkBtn.UseVisualStyleBackColor = true;
            this.setting_OkBtn.Click += new System.EventHandler(this.setting_OkBtn_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 111);
            this.Controls.Add(this.setting_OkBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ControlNode_IpAddressBox);
            this.MaximizeBox = false;
            this.Name = "SettingForm";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlLibrary.IPAddressBox ControlNode_IpAddressBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button setting_OkBtn;
    }
}