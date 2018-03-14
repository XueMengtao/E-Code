using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace CalNode_server
{
    public partial class Service1 : ServiceBase
    {
        Thread Computer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogFileManager.ObjLog.info("client is started");
            try
            {
                const long TIMESPAN = 60000;       //时间间隔10min
                //运行子节点客户端

                //节点心跳
                //HeartBeatTimer timer = new HeartBeatTimer(TIMESPAN);
                //timer.Start();

                Computer = new Thread(new ThreadStart(Compute_server.ComputeThread));
                Computer.Start();

            }
            catch (Exception ex)
            {
                //进不来的...
                LogFileManager.ObjLog.error(ex.Message, ex);
                if (Compute_server.client != null)
                    Compute_server.client.Close();
                if (Computer != null)
                    Computer.Abort();
               LogFileManager.ObjLog.info("client is stop");
               // button1.Enabled = true;
               Service1 restart = new Service1();
               string[] restartstrings = null;
               restart.OnStart(restartstrings);
            }
             
        }

        protected override void OnStop()
        {
            if (Compute_server.client != null)
                Compute_server.client.Close();
            if (Computer != null)
                Computer.Abort();
           LogFileManager.ObjLog.info("client is stop");
        }
    }
}
