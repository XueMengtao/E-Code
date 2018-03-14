using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using TranmitterLib;

namespace ServiceHost_using_WCF
{
    class DBWatcher
    {
        //改变数据库中Project的状态为1，已经加循环,加异常处理
        public static void ScaneTask()
        {
            LogFileManager.ObjLog.info("监控线程开启");
            string str = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel taskModel=new TaskModel(str);
            while (true)
            {
                try
                {
                    taskModel.SetProjectFinished();
                }
                catch (Exception e)
                {
                    LogFileManager.ObjLog.debug(e.Message);
                    Thread.Sleep(10000);
                    continue;
                }
            }
        }

    }
}
