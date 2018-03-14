using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using TranmitterLib;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;



//异常的传递使用WCF的错误传递机制，自定义的异常无法传到客户端，待处理！！！！！
//还没有进行异常处理
namespace WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class Service1 : IService1
    {
        public string txt = null;
       
       //UI客户端数据库操作
        //运用FaultException泛型传递异常
        //波形操作
        public string[] iGetAllWaveFormNames() //返回所有的波形名称，在天线或者辐射源的按钮中使用
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetWaveFormNames());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public string[] iGetAllWaveForm(string type) //返回选中的波形类型所有的波形名称，在波形窗口中使用
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetWaveFormNames(type));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public WaveForm iGetWaveForm(string name)//返回选中名称的波形的所有信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetWaveForm(name));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iAddWaveForm(WaveForm waveform) //向数据库中增加波形信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.AddWaveForm(waveform);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iDelWaveForm(string name) //删除数据库中的波形
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.DeleteWaveForm(name);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }

        public void iUpdateWaveForm(WaveForm waveform) //更新波形信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.UpdateWaveForm(waveform);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        //天线操作
        public string[] iGetAllAntennaNames() //返回所有的天线名称，在辐射源的按钮中使用
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetAntennaNames());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public string[] iGetAllAntenna(string type) //返回选中的天线类型所有的天线名称，在天线窗口中使用
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetAntennaNames(type));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }

        }
        public Antenna iGetAntenna(string name)//返回选中名称的天线的所有信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetAntenna(name));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;               
                throw new FaultException<WcfException>(ex,ex.message);

            }
        }
        public void iAddAntenna(Antenna antenna) //向数据库中增加天线信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                bool flag = false;
                string[] antennaNames = tm.GetAntennaNames();
                if (antennaNames != null)
                {
                    foreach (string name in antennaNames)
                    {
                        if (name == antenna.Name)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if(!flag)
                    tm.AddAntenna(antenna);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex,ex.message);

            }
        }
        public void iDelAntenna(string name) //删除数据库中的天线
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.DeleteAntenna(name);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iUpdateAntenna(Antenna antenna) //更新天线信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.UpdateAntenna(antenna);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        //辐射源/接收机操作
        public string[] iGetAllTransmitter() //返回所有的辐射源名称
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetTransmitterNames());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public string[] iGetAllReceiver() //返回所有的辐射源名称
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetReceiverNames());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public Transmitter iGetTransmitter(string name)//返回选中名称的辐射源的所有信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetTransmitter(name));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public Receiver iGetReceiver(string name)//返回选中名称的辐射源的所有信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                return (tm.GetReceiver(name));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iAddTransmitter(Transmitter transmitter) //向数据库中增加辐射源信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.AddTransmitter(transmitter);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iAddReceiver(Receiver receiver) //向数据库中增加辐射源信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.AddReceiver(receiver);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iDelTransmitter(string name) //删除数据库中的辐射源
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.DeleteTransmitter(name);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iDelReceiver(string name) //删除数据库中的辐射源
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.DeleteReceiver(name);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iUpdateTransmitter(Transmitter transmitter) //更新辐射源信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.UpdateTransmitter(transmitter);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iUpdateReceiver(Receiver receiver) //更新辐射源信息
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TransmitterLib tm = new TransmitterLib(cnStr);
            try
            {
                tm.UpdateReceiver(receiver);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        //地形操作
        public string[] iGetTerNames()
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TerLib te = new TerLib(cnStr);
            try
            {
                return (te.GetTerNames());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public TerInfo iGetTer(string name)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TerLib te = new TerLib(cnStr);
            try
            {
                return (te.GetTerInfo(name));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        public void iAddTerInfo(TerInfo terinfo)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TerLib te = new TerLib(cnStr);
            try
            {
                te.AddTerInfo(terinfo);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);
             
            }   
        }
        public void iDelTerInfo(string name)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TerLib te = new TerLib(cnStr);
            try
            {
                te.DeleteTerInfo(name);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        //UI提取工程状态进度的函数  还不完整随时增加

        


        //UI和主控之间的文件传输，仍用字符串传输，UI端循环遍历文件夹中的文件

       
        //分割文件、创建工程，任务，文件等数据表插入数据库
        public void CreatTables(string path)//path是指工程文件夹的目录
        {
            if (!Directory.Exists(path))
            {
                WcfException ex = new WcfException();
                ex.message = "工程文件夹不存在";
                LogFileManager.ObjLog.error(ex.message);
                throw new FaultException<WcfException>(ex, ex.message);   //测试异常的传递能否成功
            }
            else
            {

                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] txfile = di.GetFiles("*.tx");
                FileInfo[] setupdir = di.GetFiles( "*.setup");
                FileInfo[] rxfile = di.GetFiles("*.rx");
                FileInfo[] terfile = di.GetFiles("*.ter");
                //如果文件全部存在，则执行分割写表操作，否则抛出异常
                if (txfile.Length!=0 && setupdir.Length!=0 && rxfile.Length!=0 && terfile.Length!=0)
                {
                    string resultdir = path + "\\" + "studyarea";
                    Directory.CreateDirectory(resultdir);
                    string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
                    TaskModel tm = new TaskModel(cnStr);
                    string splitTxFileDir=null;
                    //分割tx文件，放在该目录下的新文件夹中
                    try
                    {
                        splitTxFileDir = Split.FileSplit(txfile[0].FullName,setupdir[0].FullName);
                    }
                    catch (nullException ex)
                    {
                        WcfException e = new WcfException();
                        e.message = ex.Message;
                        LogFileManager.ObjLog.error(ex.Message);
                        throw new FaultException<WcfException>(e, e.message);
                    }

                    //创建工程表


                    string[] seperate = new string[] { "." };
                    string[] setupfilename = setupdir[0].Name.Split(seperate, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        tm.CreatePro(setupfilename[0], path, resultdir);//异常还未处理
                    }
                    catch (SqlException e)
                    {
                        WcfException ex = new WcfException();
                        ex.message = e.Message;
                        throw new FaultException<WcfException>(ex, ex.message);
                    }
                    //创建FileInfo表
                    //创建新的文件名数组，不包括被分割的tx总文件
                    FileInfo[] Allfilenames = new FileInfo[4];
                    Allfilenames[0] = txfile[0];
                    Allfilenames[1] = rxfile[0];
                    Allfilenames[2] = setupdir[0];
                    Allfilenames[3] = terfile[0];
                    string[] Allfilenames1 = new string[Allfilenames.Length - 1];//没有tx总文件的工程文件夹中的文件名
                    DirectoryInfo ditx = new DirectoryInfo(splitTxFileDir);
                    FileInfo[] splitTxNames =ditx.GetFiles();//分割后的文件名
                    string[] filenames = new string[Allfilenames1.Length + splitTxNames.Length];//新文件名数组
                    int j = 0;
                    for (int i = 0; i < Allfilenames.Length; i++)
                    {
                        if (Allfilenames[i].Name == txfile[0].Name)
                            continue;
                        else
                        {
                            Allfilenames1[j] = Allfilenames[i].Name;
                            j++;
                        }

                    }
                    for (int i = 0; i < Allfilenames1.Length; i++)
                    {
                        filenames[i] = Allfilenames1[i];
                    }
                    for (int i = Allfilenames1.Length; i < Allfilenames1.Length + splitTxNames.Length; i++)
                    {
                        filenames[i] = splitTxNames[i - Allfilenames1.Length].Name;
                    }
                    //创建新的文件路径,异常还没有处理
                    string[] filepaths = new string[Allfilenames1.Length + splitTxNames.Length];

                    for (int a = 0; a < Allfilenames1.Length; a++)
                    {
                        filepaths[a] = path +"\\"+ filenames[a];
                    }
                    for (int a = Allfilenames1.Length; a < Allfilenames1.Length + splitTxNames.Length; a++)
                    {
                        filepaths[a] = splitTxFileDir+"\\" + filenames[a];
                    }
                    try
                    {
                        tm.CreateFileInfo(filenames, filepaths,setupfilename[0]);
                    }
                    catch (SqlException e)
                    {
                        WcfException ex = new WcfException();
                        ex.message = e.Message;
                        throw new FaultException<WcfException>(ex, ex.message);
                    }
                    //创建TaskInfo表
                    int n = splitTxNames.Length;
                    string[] Tfilenames = new string[Allfilenames1.Length + 1];
                    for (int i = 0; i < Allfilenames1.Length; i++)
                    {
                        Tfilenames[i] = Allfilenames1[i];
                    }
                    for (int i = 0; i < n; i++)
                    {
                        Tfilenames[Allfilenames1.Length] = splitTxNames[i].Name;
                        try
                        {
                            tm.CreateTask(setupfilename[0], Tfilenames);
                        }
                        catch (SqlException e)
                        {
                            WcfException ex = new WcfException();
                            ex.message = e.Message;
                            throw new FaultException<WcfException>(ex, ex.message);

                        }
                    }


                }
                else
                {
                    WcfException ex = new WcfException();
                    ex.message ="工程缺少必要文件" ;
                    LogFileManager.ObjLog.error(ex.message);
                    throw new FaultException<WcfException>(ex, ex.message);
                }
            }
        }

      
        

        //从主控下载文件，需要提供结果文件的路径
        public string GetData(string path)
        { 
            StreamReader sr=null;
            try
            {
                sr = new StreamReader(path, System.Text.Encoding.GetEncoding("GBK"));
                txt = sr.ReadToEnd();
                return txt;
            }
            catch (IOException exc)
            {
                
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
            catch (Exception exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
           
            
        }
        //向主控上传文件，提供路径，文件名称，及文件的内容
        public void PutData(string path,string name,string putStr)
        {
            //File.AppendAllText(path, putStr);
            
            StreamWriter sw=null;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory( path);
                    LogFileManager.ObjLog.debug( path);
                }
                sw = File.CreateText( path + "\\" + name);

                sw.Write(putStr);
            }
            catch (IOException exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                throw new FaultException<WcfException>(ex, ex.message);
            }
            catch (OutOfMemoryException exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                throw new FaultException<WcfException>(ex, ex.message);
            }
            catch (Exception exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                throw new FaultException<WcfException>(ex, ex.message);
            }
            finally
            {
                sw.Close();
            }
        }

        //UI从主控获得进度和计算结果存放位置
        /*public string iGetProResultDir(int proID)
        {
            TaskModel tm = new TaskModel();
            return (tm.GetProjectResultDir(proID));
        }
         public int[] GetProID(short proName)
         {
             TaskModel tm = new TaskModel();
             return (tm.GetProjectID(proName));
         }
         public int[] iGetTaskID(short state)
         {
             TaskModel tm = new TaskModel();
             return (tm.GetTaskID(state));
         }
         */
        //暂时没有确定是否需要



        //UI获取工程信息，工程信息中包含工程含有的信息
        public ProjectInfo[] iGetProjectInfo()
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                return (tm.GetProjectInfo());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }

        public string[] iGetResultDirNames(string path)
        {
            try
            {
                string[] dirnames = Directory.GetDirectories(path);
                return dirnames;
            }
            catch (IOException exc)
            {

                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
            catch (Exception exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
        }

        //UI从主控上获取结果文件夹中的文件的名称
        public string[] iGetResultFileNames(string path)
        {
            try
            {
                string[] filenames = Directory.GetFiles(path);
                return filenames;
            }
            catch (IOException exc)
            {

                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
            catch (Exception exc)
            {
                WcfException ex = new WcfException();
                ex.message = exc.Message;
                LogFileManager.ObjLog.error(ex.message);
                return null;
                throw new FaultException<WcfException>(ex, ex.message);
            }
            
        }
        
        
        //节点的函数
        public void iSetTaskState(int TaskID, short state)//当上传结果文件后设置为2，其他不用设置
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                tm.SetTaskState(TaskID, state);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        

         public  TaskInfo iGetMinTaskID()
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                return (tm.GetMinTaskInfo());
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
         //get the last taskInfo when the state is 0
         //有用的
         //task表不对
         public int[] GetLastTaskId(string projectName)
         {

             string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
             SqlConnection cn = new SqlConnection(cnStr);

             int[] taskId = new int[10];

             int TaskID = -1;
             int i = 0;
             try
             {
                 cn.Open();
                 SqlCommand sqlCommand = new SqlCommand("getLastTaskId", cn);
                 sqlCommand.CommandType = CommandType.StoredProcedure;
                 //add params

                 SqlDataReader dataReader = sqlCommand.ExecuteReader();
                 while (dataReader.Read())
                 {
                     TaskID = dataReader.GetInt32(0);
                     taskId[i] = TaskID;
                     i++;

                 }
                 return taskId;
             }
             catch (SqlException ex)
             {
                 LogFileManager.ObjLog.error(ex.Message);
                 return null;
                 throw ex;
             }
             finally
             {
                 cn.Close();
             }

         }
        public  string[] iGetFilePath(string[] filenames)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                return (tm.GetFilePaths(filenames));
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
        
       
      
        public string iGetTaskResultDir(int TaskID)
         {
             string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
             TaskModel tm = new TaskModel(cnStr);
             try
             {
                 return (tm.GetTaskResultDir(TaskID));
             }
             catch (SqlException e)
             {
                 WcfException ex = new WcfException();
                 ex.message = e.Message;
                 throw new FaultException<WcfException>(ex, ex.message);

             }

         }

        public void iDelProject(string proname)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                if (!tm.DeletePro(proname))
                {
                    return;
                }
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }

        public void HeartBeat(Guid guid, string ip)
        {
            string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel tm = new TaskModel(cnStr);
            try
            {
                tm.HeartBeat(guid, ip);
            }
            catch (SqlException e)
            {
                WcfException ex = new WcfException();
                ex.message = e.Message;
                throw new FaultException<WcfException>(ex, ex.message);

            }
        }
    


    }
}
