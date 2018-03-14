using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WcfService;
using System.Threading;


namespace ServiceHost_using_WCF
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Service1));
            host.Open();
            Thread watcher = null;
            Thread Combine = null;
            Console.WriteLine("Service using WCF is up and running...");
            Console.WriteLine("Press Enter to exit the service");
            //启动监控和合并线程
            watcher = new Thread(new ThreadStart(DBWatcher.ScaneTask));
            Combine = new Thread(new ThreadStart(CombinedResult.ResultCombine));
            watcher.Start();
            Combine.Start();
            Console.ReadLine();
            host.Close();

        }
    }
}
