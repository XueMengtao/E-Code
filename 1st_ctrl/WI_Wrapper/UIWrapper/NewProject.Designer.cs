namespace WF1
{
    partial class NewProjectUI
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
            this.label2 = new System.Windows.Forms.Label();
            this.projectName = new System.Windows.Forms.TextBox();
            this.projectPath = new System.Windows.Forms.TextBox();
            this.NewProject_browse = new System.Windows.Forms.Button();
            this.NewProject_ok = new System.Windows.Forms.Button();
            this.NewProject_cancel = new System.Windows.Forms.Button();
            this.objNewProject_browse = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(41, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "工   程";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(41, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "存放路径";
            // 
            // projectName
            // 
            this.projectName.Location = new System.Drawing.Point(171, 80);
            this.projectName.Name = "projectName";
            this.projectName.Size = new System.Drawing.Size(186, 21);
            this.projectName.TabIndex = 2;
            // 
            // projectPath
            // 
            this.projectPath.Location = new System.Drawing.Point(171, 169);
            this.projectPath.Name = "projectPath";
            this.projectPath.Size = new System.Drawing.Size(186, 21);
            this.projectPath.TabIndex = 3;
            // 
            // NewProject_browse
            // 
            this.NewProject_browse.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NewProject_browse.Location = new System.Drawing.Point(402, 169);
            this.NewProject_browse.Name = "NewProject_browse";
            this.NewProject_browse.Size = new System.Drawing.Size(75, 25);
            this.NewProject_browse.TabIndex = 4;
            this.NewProject_browse.Text = "浏览";
            this.NewProject_browse.UseVisualStyleBackColor = true;
            this.NewProject_browse.Click += new System.EventHandler(this.NewProject_browse_Click);
            // 
            // NewProject_ok
            // 
            this.NewProject_ok.Location = new System.Drawing.Point(282, 269);
            this.NewProject_ok.Name = "NewProject_ok";
            this.NewProject_ok.Size = new System.Drawing.Size(75, 23);
            this.NewProject_ok.TabIndex = 5;
            this.NewProject_ok.Text = "确定";
            this.NewProject_ok.UseVisualStyleBackColor = true;
            this.NewProject_ok.Click += new System.EventHandler(this.NewProject_ok_Click);
            // 
            // NewProject_cancel
            // 
            this.NewProject_cancel.Location = new System.Drawing.Point(402, 269);
            this.NewProject_cancel.Name = "NewProject_cancel";
            this.NewProject_cancel.Size = new System.Drawing.Size(75, 23);
            this.NewProject_cancel.TabIndex = 6;
            this.NewProject_cancel.Text = "关闭";
            this.NewProject_cancel.UseVisualStyleBackColor = true;
            this.NewProject_cancel.Click += new System.EventHandler(this.NewProject_cancel_Click);
            // 
            // NewProjectUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 304);
            this.Controls.Add(this.NewProject_cancel);
            this.Controls.Add(this.NewProject_ok);
            this.Controls.Add(this.NewProject_browse);
            this.Controls.Add(this.projectPath);
            this.Controls.Add(this.projectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "NewProjectUI";
            this.Text = "新建工程";
            this.Load += new System.EventHandler(this.NewProjectUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox projectName;
        private System.Windows.Forms.TextBox projectPath;
        private System.Windows.Forms.Button NewProject_browse;
        private System.Windows.Forms.Button NewProject_ok;
        private System.Windows.Forms.Button NewProject_cancel;
        private System.Windows.Forms.FolderBrowserDialog objNewProject_browse;
    }
}