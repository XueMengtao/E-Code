namespace WF1
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
            this.ControlNode_label = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ControlNode_ipAddressBox = new ControlLibrary.IPAddressBox();
            this.SuspendLayout();
            // 
            // ControlNode_label
            // 
            this.ControlNode_label.AutoSize = true;
            this.ControlNode_label.Location = new System.Drawing.Point(16, 21);
            this.ControlNode_label.Name = "ControlNode_label";
            this.ControlNode_label.Size = new System.Drawing.Size(77, 12);
            this.ControlNode_label.TabIndex = 1;
            this.ControlNode_label.Text = "主控的IP地址";
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(236, 67);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 4;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ControlNode_ipAddressBox
            // 
            this.ControlNode_ipAddressBox.Location = new System.Drawing.Point(146, 12);
            this.ControlNode_ipAddressBox.Name = "ControlNode_ipAddressBox";
            this.ControlNode_ipAddressBox.Size = new System.Drawing.Size(236, 34);
            this.ControlNode_ipAddressBox.TabIndex = 0;
            // 
            // ControlNodeDBSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 116);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.ControlNode_label);
            this.Controls.Add(this.ControlNode_ipAddressBox);
            this.MaximizeBox = false;
            this.Name = "ControlNodeDBSettingForm";
            this.Text = "主控和数据库服务器的设置";
            this.Load += new System.EventHandler(this.ControlNodeDBSettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlLibrary.IPAddressBox ControlNode_ipAddressBox;
        private System.Windows.Forms.Label ControlNode_label;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Button button1;
    }
}