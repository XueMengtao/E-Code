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
    class Compute
    {
        public static ServiceReference1.Service1Client client = null;
        public static void ComputeThread()
        {
            LogFileManager.ObjLog.info("计算线程开启");
            string path = ConfigurationManager.AppSettings["ProjectFold"];
            string exename = "calculate.exe";//或者 ConfigurationManager.AppSettings["ComputeModelPath"]
            string proname = null;
            string[] Filenames = null;
            int TaskID = 0;
            string path1 = null;
            string path2 = null;
            string[] paths = null;
            while (true)
            {
                if (client != null)
                {
                    client.Close();

                }
                client = new ServiceReference1.Service1Client();
                ServiceReference1.TaskInfo Ti = null;
                try
                {
                    Ti = client.iGetMinTaskID();//取状态为零的最小ID值,没有的时候为什么不返回null
                }
                catch (System.TimeoutException e)
                {
                    LogFileManager.ObjLog.error(e.Message, e);
                    //client.iSetTaskState(Ti.TaskID, 0);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (FaultException<ServiceReference1.WcfException> e)
                {
                    LogFileManager.ObjLog.error(e.Detail.message, e);
                    //client.iSetTaskState(Ti.TaskID, 0);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (CommunicationException e)
                {
                    LogFileManager.ObjLog.error(e.Message, e);
                    //client.iSetTaskState(Ti.TaskID, 0);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (Exception e)
                {
                    LogFileManager.ObjLog.error(e.Message, e);
                    //client.iSetTaskState(Ti.TaskID, 0);
                    Thread.Sleep(10000);
                    continue;
                }
                if (Ti == null)      //超时处理
                {
                    LogFileManager.ObjLog.debug("没有任务分配");
                    Thread.Sleep(10000);
                    continue;
                }

                try
                {
                    //client.iSetTaskState(TaskID, 1);

                    Filenames = Ti.filenames;
                    TaskID = Ti.TaskID;
                    proname = Ti.ProName;
                    path1 = path + "\\" + proname + "\\";
                    path2 = path1 + "result\\";
                    Directory.CreateDirectory(path1);
                    Directory.CreateDirectory(path2);
                   // paths = client.iGetFilePath(Filenames);
                    paths = Ti.filepaths;
                    LogFileManager.ObjLog.info("任务" + TaskID.ToString() + "开始");
                    LogFileManager.ObjLog.info("下载文件开始");
                    for (int i = 0; i < paths.Length; i++)   //下载工程文件
                    {
                        string data = client.GetData(paths[i] + "\\" + Filenames[i]);//异常检查
                        StreamWriter sw = new StreamWriter(path1 + Filenames[i]);
                        sw.Write(data);
                        sw.Close();
                    }
                    LogFileManager.ObjLog.info("下载文件完毕");
                }
                catch (System.TimeoutException e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (FaultException<ServiceReference1.WcfException> e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Detail.message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (CommunicationException e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (Exception e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                //修改状态之前检测是否出现异常，出现异常进行删除重新请求

                //计算出现错误异常检测

                DirectoryInfo di = new DirectoryInfo(path1);
                FileInfo[] txfile = di.GetFiles("*.tx");
                FileInfo[] setupdir = di.GetFiles("*.setup");
                FileInfo[] rxfile = di.GetFiles("*.rx");
                FileInfo[] terfile = di.GetFiles("*.ter");
                if (txfile != null && setupdir != null && rxfile != null && terfile != null)
                {
                    ComputeContral CC = new ComputeContral(path1, setupdir[0].Name, terfile[0].Name, txfile[0].Name, rxfile[0].Name, path2, exename);
                    LogFileManager.ObjLog.info("计算开始");
                    int a = CC.Compute();
                    if (a != 0)//检查计算模块,若出现问题退出本次循环，并将任务状态改为未处理  判断计算是否完成，计算超时处理还没处理
                    {
                        switch (a)
                        {
                            case -1:
                                LogFileManager.ObjLog.debug("计算内部失败");
                                break;
                            case 1:
                                LogFileManager.ObjLog.debug("计算文件读取失败");
                                break;
                            case 2:
                                LogFileManager.ObjLog.debug("计算文件内容错误");
                                break;
                            case 3:
                                LogFileManager.ObjLog.debug("计算API调用错误");
                                break;
                            case 21:
                                LogFileManager.ObjLog.debug("计算缺少licence");
                                break;
                            default: break;
                        }
                        LogFileManager.ObjLog.info("任务失败");
                        client.iSetTaskState(TaskID, 0);
                        Directory.Delete(path1, true);
                        Thread.Sleep(10000);
                        continue;
                    }
                    LogFileManager.ObjLog.info("计算结束");
                }
                else
                {
                    LogFileManager.ObjLog.debug("缺少仿真必要文件");
                    client.iSetTaskState(TaskID, 0);
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                try
                {

                    di = new DirectoryInfo(path2);
                    FileInfo[] resultfile = di.GetFiles();
                    string resultpath = client.iGetTaskResultDir(TaskID);
                    if (resultfile == null)
                    {
                        LogFileManager.ObjLog.debug("没有计算结果");
                        client.iSetTaskState(TaskID, 0);
                        LogFileManager.ObjLog.info("任务失败");
                        Directory.Delete(path1, true);
                        Thread.Sleep(10000);
                        continue;
                    }
                    LogFileManager.ObjLog.info("上传文件开始");
                    for (int i = 0; i < resultfile.Length; i++)
                    {
                        StreamReader sr = new StreamReader(resultfile[i].FullName);
                        string data = sr.ReadToEnd();
                        client.PutData(resultpath + "\\temp", resultfile[i].Name, data);//先保存在temp文件夹中待合并
                        sr.Close();
                    }
                    LogFileManager.ObjLog.info("上传文件结束");
                    client.iSetTaskState(TaskID, 2);
                    LogFileManager.ObjLog.info("本任务结束");
                }
                catch (System.TimeoutException e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (FaultException<ServiceReference1.WcfException> e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Detail.message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (CommunicationException e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                catch (Exception e)
                {
                    client.iSetTaskState(TaskID, 0);
                    LogFileManager.ObjLog.debug(e.Message, e);
                    LogFileManager.ObjLog.info("任务失败");
                    Directory.Delete(path1, true);
                    Thread.Sleep(10000);
                    continue;
                }
                //要不要删除工程文件

            }
        }
    }
}
