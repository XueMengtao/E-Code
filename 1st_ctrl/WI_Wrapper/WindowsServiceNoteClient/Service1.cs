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
namespace WindowsServiceNoteClient
{
    public partial class NoteClient : ServiceBase
    {
        Thread Computer = null;
        public NoteClient()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)   //异常检查还没有完成，除了写log，重置任务，循环回来
        {
            LogFileManager.ObjLog.info("client is started");
            try
            {
                const long TIMESPAN = 60000;       //时间间隔10min
                //运行子节点客户端

                //节点心跳
                HeartBeatTimer timer = new HeartBeatTimer(TIMESPAN);
                timer.Start();

                Computer = new Thread(new ThreadStart(Compute.ComputeThread));
                Computer.Start();
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.error(e.Message, e);
                if(Compute.client!=null)
                    Compute.client.Close();
                if (Computer != null)
                    Computer.Abort();
                LogFileManager.ObjLog.info("client is stop");
        
            }
                
                     
  }
                 
             
    
            
            //计算完成上传结果文件到主控
            //重新请求
        

        protected override void OnStop()
        {
            
            //关闭客户端
            if(Compute.client!=null)
            Compute.client.Close();
            if (Computer != null)
                Computer.Abort();
            LogFileManager.ObjLog.info("client is stop");
        }
    }

}