namespace WF1
{
    partial class AddTerrainWindow
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
            this.locateroute_textBox2 = new System.Windows.Forms.TextBox();
            this.browse_button1 = new System.Windows.Forms.Button();
            this.openTerrainDialog = new System.Windows.Forms.OpenFileDialog();
            this.latitude_panel2 = new System.Windows.Forms.Panel();
            this.north_radioButton3 = new System.Windows.Forms.RadioButton();
            this.south_radioButton4 = new System.Windows.Forms.RadioButton();
            this.longtitude_panel1 = new System.Windows.Forms.Panel();
            this.west_radioButton1 = new System.Windows.Forms.RadioButton();
            this.east_radioButton2 = new System.Windows.Forms.RadioButton();
            this.latitude_textBox3 = new System.Windows.Forms.TextBox();
            this.longtitude_textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.terrainname_textBox4 = new System.Windows.Forms.TextBox();
            this.newterraincancel_button2 = new System.Windows.Forms.Button();
            this.newterrainok_button3 = new System.Windows.Forms.Button();
            this.newterrainsave_button4 = new System.Windows.Forms.Button();
            this.latitude_panel2.SuspendLayout();
            this.longtitude_panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "从本地选择";
            // 
            // locateroute_textBox2
            // 
            this.locateroute_textBox2.Location = new System.Drawing.Point(106, 23);
            this.locateroute_textBox2.Name = "locateroute_textBox2";
            this.locateroute_textBox2.ReadOnly = true;
            this.locateroute_textBox2.Size = new System.Drawing.Size(167, 21);
            this.locateroute_textBox2.TabIndex = 69;
            // 
            // browse_button1
            // 
            this.browse_button1.Location = new System.Drawing.Point(198, 63);
            this.browse_button1.Name = "browse_button1";
            this.browse_button1.Size = new System.Drawing.Size(75, 23);
            this.browse_button1.TabIndex = 70;
            this.browse_button1.Text = "浏览";
            this.browse_button1.UseVisualStyleBackColor = true;
            this.browse_button1.Click += new System.EventHandler(this.browse_button1_Click);
            // 
            // latitude_panel2
            // 
            this.latitude_panel2.BackColor = System.Drawing.Color.Transparent;
            this.latitude_panel2.Controls.Add(this.north_radioButton3);
            this.latitude_panel2.Controls.Add(this.south_radioButton4);
            this.latitude_panel2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.latitude_panel2.Location = new System.Drawing.Point(232, 185);
            this.latitude_panel2.Name = "latitude_panel2";
            this.latitude_panel2.Size = new System.Drawing.Size(43, 43);
            this.latitude_panel2.TabIndex = 88;
            // 
            // north_radioButton3
            // 
            this.north_radioButton3.AutoSize = true;
            this.north_radioButton3.Checked = true;
            this.north_radioButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.north_radioButton3.Location = new System.Drawing.Point(8, 5);
            this.north_radioButton3.Name = "north_radioButton3";
            this.north_radioButton3.Size = new System.Drawing.Size(35, 16);
            this.north_radioButton3.TabIndex = 25;
            this.north_radioButton3.TabStop = true;
            this.north_radioButton3.Text = "北";
            this.north_radioButton3.UseVisualStyleBackColor = true;
            // 
            // south_radioButton4
            // 
            this.south_radioButton4.AutoSize = true;
            this.south_radioButton4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.south_radioButton4.Location = new System.Drawing.Point(8, 19);
            this.south_radioButton4.Name = "south_radioButton4";
            this.south_radioButton4.Size = new System.Drawing.Size(35, 16);
            this.south_radioButton4.TabIndex = 26;
            this.south_radioButton4.Text = "南";
            this.south_radioButton4.UseVisualStyleBackColor = true;
            // 
            // longtitude_panel1
            // 
            this.longtitude_panel1.Controls.Add(this.west_radioButton1);
            this.longtitude_panel1.Controls.Add(this.east_radioButton2);
            this.longtitude_panel1.Location = new System.Drawing.Point(232, 142);
            this.longtitude_panel1.Name = "longtitude_panel1";
            this.longtitude_panel1.Size = new System.Drawing.Size(43, 41);
            this.longtitude_panel1.TabIndex = 87;
            // 
            // west_radioButton1
            // 
            this.west_radioButton1.AutoSize = true;
            this.west_radioButton1.Checked = true;
            this.west_radioButton1.Location = new System.Drawing.Point(7, 3);
            this.west_radioButton1.Name = "west_radioButton1";
            this.west_radioButton1.Size = new System.Drawing.Size(35, 16);
            this.west_radioButton1.TabIndex = 23;
            this.west_radioButton1.TabStop = true;
            this.west_radioButton1.Text = "西";
            this.west_radioButton1.UseVisualStyleBackColor = true;
            // 
            // east_radioButton2
            // 
            this.east_radioButton2.AutoSize = true;
            this.east_radioButton2.Location = new System.Drawing.Point(7, 17);
            this.east_radioButton2.Name = "east_radioButton2";
            this.east_radioButton2.Size = new System.Drawing.Size(35, 16);
            this.east_radioButton2.TabIndex = 27;
            this.east_radioButton2.Text = "东";
            this.east_radioButton2.UseVisualStyleBackColor = true;
            // 
            // latitude_textBox3
            // 
            this.latitude_textBox3.Location = new System.Drawing.Point(107, 195);
            this.latitude_textBox3.Name = "latitude_textBox3";
            this.latitude_textBox3.ReadOnly = true;
            this.latitude_textBox3.Size = new System.Drawing.Size(119, 21);
            this.latitude_textBox3.TabIndex = 86;
            // 
            // longtitude_textBox1
            // 
            this.longtitude_textBox1.Location = new System.Drawing.Point(106, 154);
            this.longtitude_textBox1.Name = "longtitude_textBox1";
            this.longtitude_textBox1.ReadOnly = true;
            this.longtitude_textBox1.Size = new System.Drawing.Size(119, 21);
            this.longtitude_textBox1.TabIndex = 85;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 84;
            this.label5.Text = "维度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 83;
            this.label4.Text = "经度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 82;
            this.label6.Text = "源点";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 80;
            this.label2.Text = "地形名称";
            // 
            // terrainname_textBox4
            // 
            this.terrainname_textBox4.Location = new System.Drawing.Point(106, 100);
            this.terrainname_textBox4.Name = "terrainname_textBox4";
            this.terrainname_textBox4.ReadOnly = true;
            this.terrainname_textBox4.Size = new System.Drawing.Size(167, 21);
            this.terrainname_textBox4.TabIndex = 89;
            // 
            // newterraincancel_button2
            // 
            this.newterraincancel_button2.Location = new System.Drawing.Point(200, 245);
            this.newterraincancel_button2.Name = "newterraincancel_button2";
            this.newterraincancel_button2.Size = new System.Drawing.Size(75, 23);
            this.newterraincancel_button2.TabIndex = 91;
            this.newterraincancel_button2.Text = "关闭";
            this.newterraincancel_button2.UseVisualStyleBackColor = true;
            this.newterraincancel_button2.Click += new System.EventHandler(this.newTerrainCancel_Click);
            // 
            // newterrainok_button3
            // 
            this.newterrainok_button3.Location = new System.Drawing.Point(106, 245);
            this.newterrainok_button3.Name = "newterrainok_button3";
            this.newterrainok_button3.Size = new System.Drawing.Size(75, 23);
            this.newterrainok_button3.TabIndex = 90;
            this.newterrainok_button3.Text = "确定";
            this.newterrainok_button3.UseVisualStyleBackColor = true;
            this.newterrainok_button3.Click += new System.EventHandler(this.newTerrainOk__Click);
            // 
            // newterrainsave_button4
            // 
            this.newterrainsave_button4.Location = new System.Drawing.Point(13, 245);
            this.newterrainsave_button4.Name = "newterrainsave_button4";
            this.newterrainsave_button4.Size = new System.Drawing.Size(75, 23);
            this.newterrainsave_button4.TabIndex = 92;
            this.newterrainsave_button4.Text = "保存";
            this.newterrainsave_button4.UseVisualStyleBackColor = true;
            this.newterrainsave_button4.Click += new System.EventHandler(this.newTerrainSave_button4_Click);
            // 
            // AddTerrainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 280);
            this.Controls.Add(this.newterrainsave_button4);
            this.Controls.Add(this.newterraincancel_button2);
            this.Controls.Add(this.newterrainok_button3);
            this.Controls.Add(this.terrainname_textBox4);
            this.Controls.Add(this.latitude_panel2);
            this.Controls.Add(this.longtitude_panel1);
            this.Controls.Add(this.latitude_textBox3);
            this.Controls.Add(this.longtitude_textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.browse_button1);
            this.Controls.Add(this.locateroute_textBox2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "AddTerrainWindow";
            this.Text = "从本地添加地形";
            this.Load += new System.EventHandler(this.AddTerrainWindow_Load);
            this.latitude_panel2.ResumeLayout(false);
            this.latitude_panel2.PerformLayout();
            this.longtitude_panel1.ResumeLayout(false);
            this.longtitude_panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox locateroute_textBox2;
        private System.Windows.Forms.Button browse_button1;
        private System.Windows.Forms.OpenFileDialog openTerrainDialog;
        private System.Windows.Forms.Panel latitude_panel2;
        private System.Windows.Forms.RadioButton north_radioButton3;
        private System.Windows.Forms.RadioButton south_radioButton4;
        private System.Windows.Forms.Panel longtitude_panel1;
        private System.Windows.Forms.RadioButton west_radioButton1;
        private System.Windows.Forms.RadioButton east_radioButton2;
        private System.Windows.Forms.TextBox latitude_textBox3;
        private System.Windows.Forms.TextBox longtitude_textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox terrainname_textBox4;
        private System.Windows.Forms.Button newterraincancel_button2;
        private System.Windows.Forms.Button newterrainok_button3;
        private System.Windows.Forms.Button newterrainsave_button4;
    }
}