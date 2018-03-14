using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using WcfService;
using System.Collections;
using System.Configuration;

namespace WF1
{
    public partial class ResultDownloadStateWindow : Form
    {
        public ResultDownloadStateWindow()
        {
            InitializeComponent();
        }

        private void resultDownloadOk_button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resultDownloadOk_button1_Click_1(object sender, EventArgs e)
        {
            int selectedindex = downcontent_comboBox1.SelectedIndex;
            if (selectedindex == -1)
            {
                MessageBox.Show("请选择下载项");
            }
            else
            {
                string downloadpProName = MainWindow.downloadProInfo.Name;
                string proPath = MainWindow.downloadProInfo.Directory;
                string[] projectfiles = null;                                                    //所有工程文件路径
                string[] resultFolders = null;                                                   //结果文件夹路径
                //string projectPath = "D:\\Projects";
                string projectPath = MainWindow.projectRealPath;

                //string projectPath = ConfigurationManager.AppSettings["LocalProjectPath"];

                string localresultdir = projectPath + "\\" + downloadpProName + "\\studyarea";    //本地结果文件路径
                StreamWriter sw = null;
                string resultPath = MainWindow.downloadProInfo.ResultDirectory;
                ArrayList resfilepaths = new ArrayList();                                        //所有结果文件路径
                

                //设置进度条的值
                downloadProcessState_progressBar1.Minimum = 1;
                downloadProcessState_progressBar1.Value = 1;
                downloadProcessState_progressBar1.Step = 1;
                try
                {
                    projectfiles = MainWindow.client.iGetResultFileNames(proPath);         //获取工程文件夹中文件路径   
                    //resfilepaths = new ArrayList(MainWindow.client.iGetResultFileNames(resultPath));        //获取结果文件路径
                    resultFolders = MainWindow.client.iGetResultDirNames(resultPath);
                    foreach(string resultFolder in resultFolders)
                    {
                       
                      
                        string[] result = MainWindow.client.iGetResultFileNames(resultFolder);
                        foreach (string resultfile in result)
                        {
                            resfilepaths.Add(resultfile);
                        }
                        
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    MessageBox.Show("与服务器通信故障");
                    return;
                }
                if (selectedindex == 1)                                                          //仅下载结果文件
                {
                    downloadProcessState_progressBar1.Maximum = resfilepaths.Count;
                    if (!Directory.Exists(projectPath + "\\" + downloadpProName))
                    {
                        MessageBox.Show("未找到工程文件夹");
                        return;
                    }
                    
                }
                if (selectedindex == 0)                                                           //下载工程文件
                {
                    if (Directory.Exists(projectPath + "\\" + downloadpProName))
                    {
                        MessageBox.Show("工程已存在");
                        return;
                    }
                    
                    Directory.CreateDirectory(projectPath + "\\" + downloadpProName);

                    //设置进度条的最大值
                    downloadProcessState_progressBar1.Maximum = projectfiles.Length + resfilepaths.Count;
                    
                    try
                    {
                       
                        foreach (string projectfile in projectfiles)
                        {
                            string filecontent = MainWindow.client.GetData(projectfile);
                            string filename = projectfile.Substring(projectfile.LastIndexOf("\\") + 1);
                            sw = new StreamWriter(projectPath + "\\" + downloadpProName + "\\" + filename);
                            sw.Write(filecontent);
                            sw.Close();
                            downloadProcessState_progressBar1.PerformStep();
                        }

                    }
                    catch (Exception ex)
                    {
                        LogFileManager.ObjLog.error(ex.Message);
                        MessageBox.Show("与服务器通信故障");

                    }
                    finally
                    {
                        sw.Close();
                    }
                }
                
                try
                {
                    if (Directory.Exists(localresultdir))                         //创建结果文件 
                        Directory.Delete(localresultdir, true);
                    Directory.CreateDirectory(localresultdir);
                    string[] totalresultfilepaths = MainWindow.client.iGetResultFileNames(resultPath);
                    foreach (string totalresultfilepath in totalresultfilepaths)
                    {
                        string filecontent = MainWindow.client.GetData(totalresultfilepath);
                        string filename = totalresultfilepath.Substring(totalresultfilepath.LastIndexOf("\\") + 1);
                        sw = new StreamWriter(localresultdir + "\\"+ filename);
                        sw.Write(filecontent);
                        sw.Close();
                        downloadProcessState_progressBar1.PerformStep();
                    }
                    foreach (string resFolder in resultFolders)
                    {
                        int index = resFolder.LastIndexOf("\\");
                        string substring = resFolder.Substring(index + 1);
                        Directory.CreateDirectory(localresultdir + "\\" + substring);
                        string[] filepaths = MainWindow.client.iGetResultFileNames(resFolder);
                        foreach (string resfilepath in filepaths)                          //下载结果文件
                        {
                            string filecontent = MainWindow.client.GetData(resfilepath);
                            string filename = resfilepath.Substring(resfilepath.LastIndexOf("\\") + 1);
                            int indexf = resFolder.LastIndexOf("\\");
                            string substringf = resFolder.Substring(index + 1);
                            sw = new StreamWriter(localresultdir + "\\"+substringf+"\\" + filename);
                            sw.Write(filecontent);
                            sw.Close();
                            downloadProcessState_progressBar1.PerformStep();

                        }
                    }
                   
                    MessageBox.Show("下载完毕");
                }
                catch (FaultException<WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    MessageBox.Show("服务器端产生异常");

                }
                catch (IOException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    MessageBox.Show("文件写入错误");
                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    MessageBox.Show("下载错误");
                }
                finally
                {
                    sw.Close();
                }
                
            }
        }

        private void resultDownloadClose_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
