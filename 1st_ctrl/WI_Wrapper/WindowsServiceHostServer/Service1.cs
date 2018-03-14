using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using WcfService;
using System.Threading;
namespace WindowsServiceHostServer
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host = null;
        Thread watcher = null;
        Thread Combine = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //承载WCF服务
            LogFileManager.ObjLog.info("host is start");
            try
            {
                if (host != null)
                {
                    host.Close();
                }
                host = new ServiceHost(typeof(WcfService.Service1));
                host.Open();
                //启动监控和合并线程
                watcher = new Thread(new ThreadStart(DBWatcher.ScaneTask));
                Combine = new Thread(new ThreadStart(CombinedResult.ResultCombine));
                watcher.Start();
                Combine.Start();
            }
            catch (Exception e)
            {
               //写日志
                LogFileManager.ObjLog.fatal(e.Message,e);
                if (host != null)
                {
                    host.Close();
                }
                if (watcher != null)
                {
                    watcher.Abort();
                }
                if (Combine != null)
                {
                    Combine.Abort();
                }
                //重启服务
                string  a ="Service1";
                ServiceController sc = new ServiceController(a);
                sc.Stop();
                sc.Start();
                LogFileManager.ObjLog.info("restart the service");
                sc.Close();
                
                
                
            }
            
         
        }

        protected override void OnStop()
        {
           
            if (host != null)
            {
                host.Close();
                host = null;
            }
            //线程关闭
            if (watcher != null)
            {
                watcher.Abort();
                watcher = null;
            }
            if (Combine != null)
            {
                Combine.Abort();
                Combine = null;
            }
            LogFileManager.ObjLog.info("host is stop");
        }
    }
}
