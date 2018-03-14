using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using System.Windows.Forms;

namespace NodeApplication
{
    public partial class Node : Form
    {
        Thread Computer = null;
        public Node()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogFileManager.ObjLog.info("client is started");
            MessageBox.Show("The client is started");
            button1.Enabled = false;
            try
            {
                const long TIMESPAN = 60000;       //时间间隔10min
                //运行子节点客户端

                //节点心跳
                //HeartBeatTimer timer = new HeartBeatTimer(TIMESPAN);
                //timer.Start();

                Computer = new Thread(new ThreadStart(Compute.ComputeThread));
                Computer.Start();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                if (Compute.client != null)
                    Compute.client.Close();
                if (Computer != null)
                    Computer.Abort();
                LogFileManager.ObjLog.info("client is stop");
                button1.Enabled = true;
            }
             
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Compute.client != null)
                Compute.client.Close();
            if (Computer != null)
                Computer.Abort();
            LogFileManager.ObjLog.info("client is stop");
            MessageBox.Show("client is stop");
            button1.Enabled = true;
        }

        private void Node_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Compute.client != null)
                Compute.client.Close();
            if (Computer != null)
                Computer.Abort();
            LogFileManager.ObjLog.info("client is stop");
        }

        private void Node_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            SettingForm setting = new SettingForm();
            setting.ShowDialog();
        }
    }
}
