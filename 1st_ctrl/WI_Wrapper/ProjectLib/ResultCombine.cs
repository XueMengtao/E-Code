using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using TranmitterLib;

namespace ProjectLib
{
    public class CombinedResult
    {
        public static void ResultCombine()
        {
            Thread.Sleep(60000);
            LogFileManager.ObjLog.info("合并线程开启");
            string str = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
            TaskModel taskModel = new TaskModel(str);
            ProjectResult[] waitForCombineProject = null;
            while (true)
            {
                try
                {
                    waitForCombineProject = taskModel.GetProjectResult(1);
                }
                catch (Exception e)
                {
                    LogFileManager.ObjLog.debug(e.Message);
                    continue;
                }
                if (waitForCombineProject == null)
                {
                    continue;
                }
                foreach (ProjectResult singleProject in waitForCombineProject)
                {
                    Project project = new Project();
                    try
                    {
                        project = new Project(singleProject.ResultDir);
                        project.CombineResult();
                        taskModel.SetProjectState(project.ProjectName, 2);
                        LogFileManager.ObjLog.info("合并本工程完毕");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        LogFileManager.ObjLog.debug(ex.Message);
                        taskModel.SetProjectState(project.ProjectName, 2);
                        continue;
                    }
                    catch (FileNotFoundException ex)
                    {
                        LogFileManager.ObjLog.debug(ex.Message);
                        taskModel.SetProjectState(project.ProjectName, 2);
                        continue;
                    }
                    catch (LackOfSomeTypeFileException ex)
                    {
                        LogFileManager.ObjLog.debug(ex.Message);
                        taskModel.SetProjectState(project.ProjectName, 2);
                        continue;
                    }
                    catch (System.Exception ex)
                    {
                        LogFileManager.ObjLog.debug(ex.Message);
                        LogFileManager.ObjLog.debug("工程合成异常，删除工程：" + singleProject.name);
                        taskModel.SetProjectState(project.ProjectName, 2);
                        continue;
                    }
                }
            }
        }
    }
    
}
