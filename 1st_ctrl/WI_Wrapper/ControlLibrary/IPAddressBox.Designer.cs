namespace ControlLibrary
{
    partial class IPAddressBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ip_Panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ip_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ip_Panel
            // 
            this.ip_Panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ip_Panel.Controls.Add(this.label3);
            this.ip_Panel.Controls.Add(this.label2);
            this.ip_Panel.Controls.Add(this.label1);
            this.ip_Panel.Controls.Add(this.textBox4);
            this.ip_Panel.Controls.Add(this.textBox3);
            this.ip_Panel.Controls.Add(this.textBox2);
            this.ip_Panel.Controls.Add(this.textBox1);
            this.ip_Panel.Location = new System.Drawing.Point(3, 3);
            this.ip_Panel.Name = "ip_Panel";
            this.ip_Panel.Size = new System.Drawing.Size(232, 28);
            this.ip_Panel.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(46, 14);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
            this.textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(59, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(46, 14);
            this.textBox2.TabIndex = 1;
            //this.textBox2.KeyPress+=new System.Windows.Forms.KeyPressEventHandler(textBox2_KeyPress);
            this.textBox2.TextChanged+=new System.EventHandler(textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(119, 6);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(46, 14);
            this.textBox3.TabIndex = 2;
            //this.textBox3.KeyPress+=new System.Windows.Forms.KeyPressEventHandler(textBox3_KeyPress);
            this.textBox3.TextChanged+=new System.EventHandler(textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(179, 7);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(46, 14);
            this.textBox4.TabIndex = 3;
            //this.textBox4.KeyPress+=new System.Windows.Forms.KeyPressEventHandler(textBox4_KeyPress);
            this.textBox4.TextChanged+=new System.EventHandler(textBox4_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "-";
            // 
            // IPInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ip_Panel);
            this.Name = "IPInputControl";
            this.Size = new System.Drawing.Size(236, 34);
            this.ip_Panel.ResumeLayout(false);
            this.ip_Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ip_Panel;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
