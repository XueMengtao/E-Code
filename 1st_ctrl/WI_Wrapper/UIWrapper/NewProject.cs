using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace WF1
{
    public partial class NewProjectUI : Form
    {

        public static string projectNameStr = null;
        internal static Process wiProcess = null;
        public NewProjectUI()
        {
            InitializeComponent();
        }

        private void NewProject_browse_Click(object sender, EventArgs e)
        {
            objNewProject_browse.ShowNewFolderButton = true;
            objNewProject_browse.RootFolder = Environment.SpecialFolder.Desktop;
            objNewProject_browse.Description = "新建文件夹";
            if (objNewProject_browse.ShowDialog() == DialogResult.OK)
            {
                projectPath.Text = objNewProject_browse.SelectedPath;
            }
        }

        private void NewProject_cancel_Click(object sender, EventArgs e)
        {
            MainWindow.mProjectFullName = null;
            MainWindow.setupStr = null;
            this.Close();
        }

        private void NewProject_ok_Click(object sender, EventArgs e)
        {

            string tipTitle = "错误";   
            //对路径后面是否有\做出两种不同的处理
            
            string slashExist = null;
            if (projectPath.Text[projectPath.Text.Length-1] == '\\')
            {
                slashExist = null;
            }
            else
            {
                slashExist = "\\";
            }
            MainWindow.projectRealPath = projectPath.Text + slashExist  + projectName.Text;
            MainWindow.onlyProjectName = projectName.Text;
            //判断路径的格式是否合法

            //string patten = @"^[a-zA-Z]:(\\)+$|^[a-zA-Z]:(\\(\s*_*[0-9a-zA-Z]+_*\s*[-0-9a-zA-Z]+_*))+$";
            string pattenPath = @"^[a-zA-Z]:(\\+\w*)*$";
            string pattenName = @"^\w*$";
            bool pathMatchResult = Regex.IsMatch(projectPath.Text, pattenPath);
            bool nameMatchResult = Regex.IsMatch(projectName.Text, pattenName);

            string driver = Directory.GetDirectoryRoot(projectPath.Text);
            bool signbool = false;
            DriveInfo[] allDrivers = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrivers)
            {
                if ((d.Name[0] == driver[0] || driver[0] == d.Name[0] + 32) && (d.DriveType == DriveType.Fixed||d.DriveType==DriveType.Removable))
                {
                    signbool = true;
                }
            }
            if (pathMatchResult && nameMatchResult && signbool)
            {
                //路径格式合法，判断项目名称是否为空
                if (projectName.Text == "")
                {
                    
                    MessageBox.Show("工程名不能为空，请输入工程名", tipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ServiceReference1.Service1Client client = client = new ServiceReference1.Service1Client();;
                    ServiceReference1.ProjectInfo[] DBProjectNames = client.iGetProjectInfo();
                    for(int i=0;i<DBProjectNames.Length;i++)
                    {
                        if (projectName.Text ==DBProjectNames[i].Name)
                        {
                            string tipMessage = "该工程名称在数据库中已存在，请重新命名";
                            MessageBox.Show(tipMessage, tipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }


                    //路径合法，项目名称不为空，接着判断所建项目是否已经存在
                    if (Directory.Exists(MainWindow.projectRealPath))
                    {
                        string tipMessage = "该工程已存在，请重新命名";
                        MessageBox.Show(tipMessage, tipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                            Directory.CreateDirectory( MainWindow.projectRealPath);
                            string nodeName = projectName.Text+".setup" + ".info";
                        
                            MainWindow.mProjectFullName = MainWindow.projectRealPath+ "\\" + projectName.Text + ".setup";
                            MainWindow.nodeInfoFullPath = MainWindow.projectRealPath + "\\" + nodeName;
                            //MainWindow.transInfoFullPath = MainWindow.projectRealPath + "\\" + projectName.Text + ".transinfo";
                            MainWindow.transInfoFullPath = MainWindow.mProjectFullName  + ".transinfo";

                            MainWindow.relationOfAntAndWavePath = MainWindow.mProjectFullName + ".match";
                            MainWindow.waveinfoFilePath  = MainWindow.mProjectFullName + ".waveinfo";

                            //将工程名字存到全局字符串
                            MainWindow.setupStr = SetupContent.setupcontent1 + projectName.Text + "\r\n" + SetupContent.setupcontent2;
                            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName,false);
                            //FileOperation.WriteFile(SetupContent.vwFileStr, MainWindow.projectRealPath + "\\" + projectName.Text + ".vw");
                            //给全局变量projectNameStr赋值
                            projectNameStr = projectName.Text;
                            //FileCopyUI.FileCopy(nodeInfoFullPath, MainWindow.mProjectFullName);
                             //启动WI
                            //Process wiProcess=Process.Start("notepad", MainWindow.mProjectFullName);
                            wiProcess = Process.Start(MainWindow.exePath, MainWindow.mProjectFullName);
                            MainWindow.wiProcessID = wiProcess.Id;

                            FileOperation.WriteLineFile(SetupContent.projectIndeStr +" "+projectName.Text, MainWindow.nodeInfoFullPath, true);
                            //创建子节点
                            MainWindow.staticTreeView.Nodes[0].Nodes.Add(projectName.Text);
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("地形");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("波形");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("天线");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("辐射源");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("接收机");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("仿真结果设定");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes.Add("点状分布");
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes.Add("区域分布");
                            this.Close();
                     }
                 }
             }
             else
             {
                 MessageBox.Show("您所输入的路径不合法，请重新输入", tipTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }

        private void NewProjectUI_Load(object sender, EventArgs e)
        {
            projectPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"\\RWPSProjects";
            if (!Directory.Exists(projectPath.Text))
                Directory.CreateDirectory(projectPath.Text);
            if (File.Exists(MainWindow.mProjectFullName))
            {
                DialogResult result = MessageBox.Show("您是否要重新建一个工程？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (!wiProcess.HasExited)
                {
                    wiProcess.Kill();
                }

                if (DialogResult.OK == result)
                {

                    DialogResult res = MessageBox.Show("原来的工程您是否要保存？", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult.No == res)
                    {
                        Directory.Delete(MainWindow.projectRealPath, true);
                        MainWindow.staticTreeView.Nodes[0].Nodes[0].Remove();
                    }
                    else
                    {
                        FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                        Directory.Delete(MainWindow.projectRealPath, true);  //王佳修改
                        MainWindow.staticTreeView.Nodes[0].Nodes[0].Remove();
                        
                    }
                    MainWindow.mProjectFullName = null;  
                    MainWindow.setupStr = null;
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
