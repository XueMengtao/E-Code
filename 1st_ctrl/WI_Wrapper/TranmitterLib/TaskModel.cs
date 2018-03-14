using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;




namespace TranmitterLib
{
   [Serializable]  //测试能否序列化
   public class TaskInfo
    {
        public int TaskID;
        public string ProName;
        public string[] filenames;
        public string[] filepaths;
        public TaskInfo(int taskid, string ProName,string[] filenames,string[] filepaths)   //经测试，方法不能传递给客户端
        {
            TaskID = taskid;
            this.ProName = ProName;
            this.filenames = filenames;
            this.filepaths = filepaths;
        }
       
    }

   [Serializable]
   public class ProjectInfo
   {
      
       public string Name;
       public short ProState;
       public string Directory;
       public string ResultDirectory;
       public string Percent;
       public DateTime? CreateTime;
       public DateTime? EndTime;
       
       public ProjectInfo(string Name, string Directory, string ResultDirectory, short ProState, string Percent, DateTime? CreateTime, DateTime? EndTime)
       {
           this.Name = Name;
           this.ProState = ProState;
           this.Directory = Directory;
           this.ResultDirectory = ResultDirectory;
           this.CreateTime = CreateTime;
           this.EndTime = EndTime;
           if (Percent == null)
               this.Percent = "0%";
           else
           this.Percent = Percent;
       }
   }

    //主控自己使用，不用于传输
   public class ProjectResult
   {
       public string name;
       public string ResultDir;
       public ProjectResult(string name, string ResultDir)
       {
           this.name=name;
           this.ResultDir = ResultDir;
       }
   }
   public class TaskModel
    {
       private string cnStr;
        public TaskModel(string cnStr)
        {
           this.cnStr=cnStr;
        }


        //set the state of the task
       //有用的
        public void SetTaskState(int TaskID,short state)
        {
            
            if ( state == 2 || state==0)
            {
                

                SqlConnection cn = new SqlConnection(cnStr);
               

                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("setTaskState", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@TaskInfoID";
                    param1.Value = TaskID;
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@TaskState";
                    param2.Value =state;
                    param2.SqlDbType = SqlDbType.SmallInt;
                    param2.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param2);

                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message); 
                    throw ex;
                }
                finally
                {
                    cn.Close();
                }

                
            }
           
        }

       


        




       


        //get the min taskInfo when the state is 0
       //有用的
       //task表不对
        public TaskInfo GetMinTaskInfo()
        {
           

            SqlConnection cn = new SqlConnection(cnStr);

            ArrayList filenames = new ArrayList();
            ArrayList filepaths = new ArrayList();
            int TaskID = -1;
            string proName=null;
            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getMinTaskInfo", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    TaskID = dataReader.GetInt32(0);
                    proName = dataReader.GetString(1);
                    filenames.Add(dataReader.GetString(2));
                    filepaths.Add(dataReader.GetString(3)); 

                }
                if (TaskID == -1)
                    return null;
                else
                {

                    TaskInfo taskInfo = new TaskInfo(TaskID, proName, (string[])filenames.ToArray(typeof(string)), (string[])filepaths.ToArray(typeof(string)));
                    return taskInfo;
                }
                 

               
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


        //get the filepath based on the filenames TASK节点             
        //????该存储过程好像没有使用的
        public string[] GetFilePaths(string[] filenames)
        {
            if(filenames==null)
                return null;


            else
            {
                

                SqlConnection cn = new SqlConnection(cnStr);
                ArrayList filePaths = new ArrayList();

                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("getFilePath", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();              //param1为输入参数filaname
                    SqlParameter param2 = new SqlParameter("@FilePath",SqlDbType.NVarChar,64);              //param2为输入参数filePath

                    param1.ParameterName = "@FileName";
                    param1.SqlDbType = SqlDbType.NVarChar;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                   
                    param2.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(param2);

                    foreach (string filename in filenames)
                    {
                        param1.Value = filename;
                        
                        sqlCommand.ExecuteNonQuery();
                        filePaths.Add((string)param2.Value);
                    }

                   
                    return (string[])filePaths.ToArray(typeof(string));
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
        }


        //set the project state
       //有用的
        public void SetProjectState(string Name, short ProState)
        {
            if (ProState == 1 || ProState == 2 || ProState == 0)
            {
                

                SqlConnection cn = new SqlConnection(cnStr);


                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("setProjectState", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@Name";
                    param1.Value = Name;
                    param1.SqlDbType = SqlDbType.NVarChar;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@ProState";
                    param2.Value = ProState;
                    param2.SqlDbType = SqlDbType.SmallInt;
                    param2.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param2);

                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    throw ex;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        //set the state of the project to 1 when all the tasks related to the project are finished
       //有用的
        public void SetProjectFinished()
        {
            

            SqlConnection cn = new SqlConnection(cnStr);


            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("setProjectFinished", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
               

                sqlCommand.ExecuteNonQuery();
                
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
            finally
            {
                cn.Close();
            }

        }
       
        //get all the projectID when the state is proState  主控线程中使用
       //有用的
        public ProjectResult[] GetProjectResult(short proState)
        {
            if (proState != 0 && proState != 1 && proState != 2)
                return null;
            else
            {
                
                

                SqlConnection cn = new SqlConnection(cnStr);

                ArrayList projecArray = new ArrayList();

                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("getProjects", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@ProjectState";
                    param1.Value = proState;
                    param1.SqlDbType = SqlDbType.SmallInt;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        ProjectResult proResult = new ProjectResult(sqlDataReader.GetString(0), sqlDataReader.GetString(1));
                        projecArray.Add(proResult);
                    }
                    return (ProjectResult[])projecArray.ToArray(typeof(ProjectResult));


                    
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
        }

      

        //get all the TaskID when the state is taskState
       //没有使用
        public int[] GetTaskID(short taskState)
        {
            if (taskState != 0 || taskState != 1 || taskState != 2)
                return null;
            else
            {

                

                SqlConnection cn = new SqlConnection(cnStr);

                ArrayList projecArray = new ArrayList();

                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("getTasks", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@TaskState";
                    param1.Value = taskState;
                    param1.SqlDbType = SqlDbType.SmallInt;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        projecArray.Add((int)sqlDataReader["TaskInfoID"]);
                    }
                    return (int[])projecArray.ToArray(typeof(int));



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
        }
       
       
        //get the task result directory of the task
       //有用的
       //task表不对
        public string GetTaskResultDir(int TaskID)
        {
            
            

            SqlConnection cn = new SqlConnection(cnStr);


            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getResultDirectory", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@TaskInfoID";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = TaskID;
                param1.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@ResultDirectory",SqlDbType.NVarChar,64);
               
                
                param2.Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add(param2);

                sqlCommand.ExecuteNonQuery();
                return (string)param2.Value;
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

       

        //insert data into Project Table
        public void CreatePro(string name,string der,string resultder)
        {
           
                

            SqlConnection cn = new SqlConnection(cnStr);


            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("createProject", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@Name";
                param1.Value = name;
                param1.SqlDbType = SqlDbType.NVarChar;
                param1.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param1);

                SqlParameter param2=new SqlParameter();
                param2.ParameterName="@Directory";
                param2.Value = der;
                param2.SqlDbType=SqlDbType.NVarChar;
                param2.Direction=ParameterDirection.Input;
                sqlCommand.Parameters.Add(param2);

                SqlParameter param3=new SqlParameter();
                param3.ParameterName="@ResultDirectory";
                param3.Value = resultder;
                param3.SqlDbType=SqlDbType.NVarChar;
                param3.Direction=ParameterDirection.Input;
                sqlCommand.Parameters.Add(param3);

                sqlCommand.ExecuteNonQuery();
               
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
            finally
            {
                cn.Close();
            }
                 
                
        }
                
        

        //insert data into FileInfo Table  
        public void CreateFileInfo(string[] filenames,string[] filepaths,string proname)
        {
            if (filenames.Length == filepaths.Length)
            {
               

                SqlConnection cn = new SqlConnection(cnStr);


                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("createFileInfo", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@FileName";
                    param1.SqlDbType = SqlDbType.NVarChar;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@FilePath";
                    param2.SqlDbType = SqlDbType.NVarChar;
                    param2.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@ProjectName";
                    param3.SqlDbType = SqlDbType.NVarChar;
                    param3.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param3);

                    for (int i = 0; i < filenames.Length; i++)
                    {
                        param1.Value = filenames[i];
                        
                        param2.Value = filepaths[i];
                        param3.Value = proname;
                       
                        sqlCommand.ExecuteNonQuery();
                    }

                    
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    throw ex;
                }
                finally
                {
                    cn.Close();
                }
            }
                
        }


        //insert data into TaskInfo and TaskFileRelation string[] filenames is the file names in task
        public void CreateTask(string proName,  string[] filenames)
        {
           
               

                SqlConnection cn = new SqlConnection(cnStr);
                
                string sqlFormat= GetSqlFormat(filenames);

                try
                {
                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("createTask", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //add params
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@ProName";
                    param1.Value = proName;
                    param1.SqlDbType = SqlDbType.NVarChar;
                    param1.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@FileNames";
                    param2.Value = sqlFormat;
                    param2.SqlDbType = SqlDbType.NVarChar;
                    param2.Direction = ParameterDirection.Input;
                    sqlCommand.Parameters.Add(param2);

                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    throw ex;
                }
                finally
                {
                    cn.Close();
                }
           

            }

        //get the formate of the string need to pass to the storageprocedure
       //非存储过程
        private string GetSqlFormat(string[] stringArray)
        {
            if (stringArray.Length == 0)
                return null;
            else
            {
                string sqlFormat = null;
                foreach (string member in stringArray)
                {
                    sqlFormat += member + ",";
                }
                return sqlFormat;
            }

        }

        //delete the information about the project including taskinfo and fileinfo
       //有用的
        public bool DeletePro(string proName)
        {
            //SqlConnection cn = new SqlConnection(cnStr);
            //try
            //{
            //    cn.Open();
            //    SqlCommand sqlCommand = new SqlCommand("deleteProjectF", cn);
            //    sqlCommand.CommandType = CommandType.StoredProcedure;
            //    //add params
            //    SqlParameter param1 = new SqlParameter();
            //    param1.ParameterName = "@Name";
            //    param1.Value = proName;
            //    param1.SqlDbType = SqlDbType.NVarChar;
            //    param1.Direction = ParameterDirection.Input;
            //    sqlCommand.Parameters.Add(param1);
            //    sqlCommand.ExecuteNonQuery();

            //}
            //catch (SqlException ex)
            //{
            //    LogFileManager.ObjLog.error(ex.Message);
            //    throw ex;
            //}
            //finally
            //{
            //    cn.Close();
            //}

            //Modify Date 2013-10-16: By Chenlimin
            //添加删除工程文件夹
            SqlConnection cn = new SqlConnection(cnStr);
            try
            {
                cn.Open();
                SqlCommand sqlCommand_t = new SqlCommand("getProjectByTaskinfo", cn);
                sqlCommand_t.CommandType = CommandType.StoredProcedure;
                SqlParameter param_t = new SqlParameter();
                param_t.ParameterName = "@TaskState";
                param_t.SqlDbType = SqlDbType.SmallInt;
                param_t.Direction = ParameterDirection.Input;
                param_t.Value = 1;
        
                sqlCommand_t.Parameters.Add(param_t);
                SqlDataReader sqlDataReader=sqlCommand_t.ExecuteReader();
                List<string> projectNames=new List<string>();
                while (sqlDataReader.Read())
                {
                    string projectName = sqlDataReader.GetString(0);
                    projectNames.Add(projectName);
                    //projectNames.Add(sqlDataReader.GetString(1));
                }
                //foreach(string name in projectNames)
                //{
                //    if (name == proName)
                //    {
                //        return false;
                //    }
                //}
                cn.Close();
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteProjectF", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@Name";
                param1.Value = proName;
                param1.SqlDbType = SqlDbType.NVarChar;
                param1.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param1);
                sqlCommand.ExecuteNonQuery();
                String projectPath=ConfigurationManager.AppSettings.Get("path");
                projectPath = "d:\\projects\\";
                DeleteDir(projectPath + proName);//默认在D:\\projects下
                return true;
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }

        //get ProjectInfo 
       //有用的
        public ProjectInfo[] GetProjectInfo()
        {
            ProjectInfo[] proInfo = null;
            ArrayList projectList = new ArrayList();
            SqlConnection cn = new SqlConnection(cnStr);

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getProjectInfo", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataReader sqlDataReader=sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string name = sqlDataReader.GetString(0);
                    string Directory = sqlDataReader.GetString(1);
                    string ResultDirectory = sqlDataReader.GetString(2);
                    short state = sqlDataReader.GetInt16(3);
                    string percent = null;
                    if (!sqlDataReader.IsDBNull(4))
                    {
                        percent = sqlDataReader.GetString(4);
                    }
                    DateTime? starttime = null;
                    if(!sqlDataReader.IsDBNull(5))
                    {
                        starttime = sqlDataReader.GetDateTime(5);
                    }
                    DateTime? endtime = null;
                    if (!sqlDataReader.IsDBNull(6))
                    {
                        endtime = sqlDataReader.GetDateTime(6);
                    }
                    projectList.Add(new ProjectInfo(name,Directory,ResultDirectory,state,percent,starttime,endtime));
                }
                proInfo=(ProjectInfo[])(projectList.ToArray(typeof(ProjectInfo)));
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
            finally
            {
                cn.Close();
            }
           
            return proInfo;
        }
        
        //update the nodeinfo form
       //有用的
        public void HeartBeat(Guid nodeNumber, string ip)
        {
            SqlConnection cn = new SqlConnection(cnStr);

           
            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("heartBeat", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@NodeNumber";
                param1.Value = nodeNumber;
                param1.SqlDbType = SqlDbType.UniqueIdentifier;
                param1.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@NodeIP";
                param2.Value = ip;
                param2.SqlDbType = SqlDbType.NChar;
                param2.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param2);

                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }


       //修复超时 timespan是超时时间，以ms为单位
       //没有使用过
        public void FixTaskIimeout(int timespan)
        {
            SqlConnection cn = new SqlConnection(cnStr);
            try
            {
                cn.Open();
                SqlCommand command = new SqlCommand("fixTaskTimeout",cn);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@TimeSpane", timespan);
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }
      //插入节点错误信息
       //没有使用过
        public void InsertErrorInfo(int type)
        {
            SqlConnection cn = new SqlConnection(cnStr);
            try
            {
                cn.Open();
                SqlCommand command = new SqlCommand("insertErrorInfo", cn);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@ErrorType",type);
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }


        //New Add :按照工程全路径删除工程文件
        //Add Date 2013-10-16: By Chenlimin
        private void DeleteDir(string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    aimPath += System.IO.Path.DirectorySeparatorChar;
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
                string[] fileList = System.IO.Directory.GetFiles(aimPath);
                //string[] fileList = Directory.GetFileSystemEntries(aimPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        DeleteDir(aimPath + System.IO.Path.GetFileName(file));
                    }
                    // 否则直接Delete文件
                    else
                    {
                        System.IO.File.Delete(aimPath + System.IO.Path.GetFileName(file));
                    }
                }
                //删除文件夹
                System.IO.Directory.Delete(aimPath, true);
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.error(e.Message);
                throw e;
            }
        }
   
   }

       
       
        
        
    }
