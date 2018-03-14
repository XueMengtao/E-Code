using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using WcfService;
using System.IO;
using System.Collections;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using System.Net;
namespace WF1
{
    public delegate void ChildClose();

    public partial class MainWindow : Form
    {        
        public static string exePath = null;

        public static bool creatSuccMesDisp = true;
        public static bool IsReturnMidwayInNewProcess = false;
        public static bool newFuncSign = true;
        public static string mProjectFullName = null;
        public static string nodeInfoFullPath = null;
        //.match文件的存放路径
        public static string relationOfAntAndWavePath = null;
        //新建波形,天线窗口中控件中的全部内容信息文件的路径
        public static string waveinfoFilePath = null;
        //项目的路径，不包括具体文件
        public static string projectRealPath = null;
        //项目的名字
        public static string onlyProjectName = null;
        //是transinfo文件的路径，此文件用于存放辐射源的天线信息
        public static string transInfoFullPath = null;
        //用于存放地形文件的经度和纬度
        public static string longitudeStr = null;
        public static string latitudeStr = null;
        //实现不同窗口之间数据的共享
       public static TreeView staticTreeView;
        //保存setup文件的全局字符串
       public static string setupStr=null;
       public static int wiProcessID;
       public static ServiceReference1.Service1Client client;

        //设置线程是否执行变量
       public static bool flag = true;

       public static ServiceReference1.ProjectInfo downloadProInfo = null;
       NewWaveformWindow f1 = null;
       AddWaveformWindow f2 =null;
       NewAntennaWindow f3=null;
       AddAntennaWindow f4 = null;
       NewTransmitterWindow f5=null; 
       AddTransmitterWindow f6=null;
       NewTerrainWindow f7=null;
       AddTerrainWindow f8 = null;
       DBmanageWindow f9 = null;
       ResultSetWindow f10 = null;
       NewProjectUI newProject = null;
       NewReceiverWindow f11 = null;
       ResultDownloadStateWindow f12 = null;

       TreeNode currentNode = null;
        //用于保存结果设定窗口中的状态
       public static CheckBox PathsCheck = null;
       public static CheckBox PathlossCheck = null;
       public static CheckBox EFieldCheck = null;
       public static CheckBox EFieldTotalCheck = null;
       public static CheckBox PowerCheck = null;

       ServiceReference1.ProjectInfo[] proInfoArray = null;        //服务器端工程状态信息
       Thread refreshThread = null;
        public MainWindow()
        {
            InitializeComponent();
            client = new ServiceReference1.Service1Client();
            //必须实例化
            PathsCheck = new CheckBox();
            PathlossCheck = new CheckBox();
            EFieldCheck = new CheckBox();
            EFieldTotalCheck = new CheckBox();
            PowerCheck = new CheckBox();

            staticTreeView = new TreeView();
            //将实例成员赋给类成员，以便不同窗口数据共享
            staticTreeView = project_TreeView1;

            //指定NewProjectTree_ContextMenuStrip菜单的某一项被单击时，所要执行的事件处理操作
            waveformOfProjectTreeView_ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(waveformOfProjectTreeView_ContextMenuStrip_ItemClicked);
            terrainOfProjectTreeView_ContextMenuStrip.ItemClicked+=new ToolStripItemClickedEventHandler(terrainOfProjectTreeView_ContextMenuStrip_ItemClicked);
            antennaOfProjectTreeView_ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(antennaOfProjectTreeView_ContextMenuStrip_ItemClicked);
            transmitterOfProjectTreeView_ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(transmitterOfProjectTreeView_ContextMenuStrip_ItemClicked);
            receiverOfProjectTreeView_ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(receiverOfProjectTreeView_ContextMenuStrip_ItemClicked);
            resultSetOfProjectTreeView_ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(resultSetOfProjectTreeView_ContextMenuStrip1_ItemClicked);

            //实现窗口最大化时控件同比例放大
            int count = this.Controls.Count * 2 + 2;
            float[] factor = new float[count];
            int i = 0;
            factor[i++] = Size.Width;
            factor[i++] = Size.Height;
            foreach (Control ctrl in this.Controls)
            {
                factor[i++] = ctrl.Location.X / (float)Size.Width;
                factor[i++] = ctrl.Location.Y / (float)Size.Height;
                ctrl.Tag = ctrl.Size;
            }
            Tag = factor;
        }

  //控制只能新建一个实例窗口
        private T OpenNewForm<T>(T t) where T : Form, new()
        {
            if (t == null)
            {
                t = new T();
                t.Show();
            }
            else
            {
                if (t.IsDisposed)
                {
                    t = new T();
                    t.Show();
                }
                else
                {
                    t.WindowState = FormWindowState.Normal;
                    t.Activate();
                }
            }
            return t;
        }
        private void NewWaveform_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f1 = OpenNewForm<NewWaveformWindow>(f1);
        }

        private void AddWaveform_ToolStripMenuItem_Click_Click(object sender, EventArgs e)
        {
            f2 = OpenNewForm<AddWaveformWindow>(f2);
        }
       
        private void NewAntenna_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = OpenNewForm<NewAntennaWindow>(f3);
        }

        private void AddAntenna_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f4 = OpenNewForm<AddAntennaWindow>(f4);
        }
        private void NewTransmitter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f5 = OpenNewForm<NewTransmitterWindow>(f5);
        }
        private void AddTransmitter_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f6 = OpenNewForm<AddTransmitterWindow>(f6);
        }
        private void AddTerrain_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f7 = OpenNewForm<NewTerrainWindow>(f7);
        }
        
        private void NewTerrain_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           f8= OpenNewForm<AddTerrainWindow>(f8);
        }
       
        private void NewProject_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newProject = OpenNewForm<NewProjectUI>(newProject);
        }

        private void ExitProject_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainWindow.mProjectFullName != null)
            {
                if (File.Exists(mProjectFullName))
                {
                    FileOperation.WriteFile(setupStr, MainWindow.mProjectFullName,false);
                    Process pro = null;
                    try
                    {
                        pro = Process.GetProcessById(wiProcessID);
                    }
                    catch (ArgumentException)
                    {
                        this.Close();
                        //如果WI进程已被关闭，则会产生异常。因此可以通过是否产生异常判断WI是否关闭
                        return;
                    }
                    {
                        //如果WI进程还存在，则先将其关闭再打开重新加载工
                        pro.Kill();
                    }
                }
            }
            this.Close();
        }
        //信息文件中结果设定索引字符串后面是1则返回true，否则返回false
        //即若复选框被选中则则返回true，否则返回false
        bool GetResultSetStr(string decisionStr)
        {
            bool b=true;
            switch (decisionStr)
            {
                case "1":
                    b= true;
                    break;
                case "0":
                    b= false;
                    break;
                default:
                    b = false;
                    break;
            }
            return b;
        }
        
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogFileManager.ObjLog.info("Begin");
            if (mProjectFullName!= null)
            {
                DialogResult result = MessageBox.Show("您是否要打开一个新的工程？若是则原工程将被关闭。", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (DialogResult.OK == result)
                {
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Remove();
                }
                else
                {
                    MainWindow.mProjectFullName = null;  //王佳修改
                    MainWindow.setupStr = null;
                    return;
                }
            }
            openProject.Filter = "setup文件(*.setup)|*.setup|所有文件|*.*";
            DialogResult dr = openProject.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //在新建一个工程时，又打开这个工程则什么也不做 否则执行下面的操作
                if (mProjectFullName != openProject.FileName)
                {
                    setupStr = FileOperation.ReadFile(openProject.FileName);
                    //.setup文件的包括路径的名字
                    mProjectFullName = openProject.FileName;
                    //.setup.info文件的包含路径的名字
                    nodeInfoFullPath = openProject.FileName + ".info";
                    transInfoFullPath = openProject.FileName + ".transinfo";

                    relationOfAntAndWavePath = openProject.FileName + ".match";
                    waveinfoFilePath = openProject.FileName + ".waveinfo";

                    //工程的路径 不包括文件
                    MainWindow.projectRealPath = openProject.FileName.Substring(0, openProject.FileName.LastIndexOf("\\"));
                    staticTreeView.Nodes.Clear();
                    //从信息文件中找出工程树中工程的名字将其加载到工程树中
                    string nodeNamesStr = FileOperation.ReadFile(nodeInfoFullPath);
                    string projectNameStr = StringFound.FoundBackStr(SetupContent.projectIndeStr, nodeNamesStr, true);
                    MainWindow.onlyProjectName = projectNameStr;
                    longitudeStr = StringFound.FoundBackStr(SetupContent.longitudeIndeStr, nodeNamesStr, true);
                    latitudeStr = StringFound.FoundBackStr(SetupContent.latitudeIndeStr, nodeNamesStr, true);


                    staticTreeView.Nodes.Add("工程名字");
                    staticTreeView.Nodes[0].Nodes.Add(projectNameStr);
                    staticTreeView.Nodes[0].Nodes[0].Nodes.Add("地形");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("波形");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("天线");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("辐射源");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("接收机");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes.Add("点状分布");
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes.Add("区域分布");

                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("仿真结果设定");


                    ArrayList nodeNamesArr = new ArrayList();
                    //向工程树中添加波形节点
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.waveIndeStr, SetupContent.waveIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(s);
                    //向工程树中添加地形节点
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.terrainIndeStr, SetupContent.terrainIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[0].Nodes.Add(s);
                    //向工程树种添加天线结点
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.antennaIndeStr, SetupContent.antennaIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(s);
                    //向工程树种添加辐射源结点
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.transmitterIndeStr, SetupContent.transmitterIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[3].Nodes.Add(s);
                    //向工程树种添加点状接收机
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.pointReceiverIndeStr, SetupContent.pointReceiverIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0].Nodes.Add(s);
                    //向工程树种添加线状接收机
                    nodeNamesArr = ProjectTreeMatchString.MatchStr(nodeNamesStr, SetupContent.gridReceiverIndeStr, SetupContent.gridReceiverIndeStr.Length);
                    foreach (string s in nodeNamesArr)
                        staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1].Nodes.Add(s);
                    //向工程树中加载结果设定的相应信息
                    string pathsResultStr = StringFound.FoundBackStr(SetupContent.resultSetIndeStr1, nodeNamesStr, false);
                    //若结果设定中的复选框被选中
                    if (GetResultSetStr(pathsResultStr))
                    {
                        //将复选框的状态保存到结果设定窗口中
                        PathsCheck.Checked = true;
                        //在工程树中加载结果设定结点中加载相应的信息
                        staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(SetupContent.resultSetOfTreeIndeStr1);
                    }
                    string pathlossResultStr = StringFound.FoundBackStr(SetupContent.resultSetIndeStr2, nodeNamesStr, false);
                    if (GetResultSetStr(pathlossResultStr))
                    {
                        PathlossCheck.Checked = true;
                        staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(SetupContent.resultSetOfTreeIndeStr2);
                    }
                    string eFieldResultStr = StringFound.FoundBackStr(SetupContent.resultSetIndeStr3, nodeNamesStr, false);
                    if (GetResultSetStr(eFieldResultStr))
                    {
                        EFieldCheck.Checked = true;
                        staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(SetupContent.resultSetOfTreeIndeStr3);
                    }
                    string eFieldTotalResultStr = StringFound.FoundBackStr(SetupContent.resultSetIndeStr4, nodeNamesStr, false);
                    if (GetResultSetStr(eFieldTotalResultStr))
                    {
                        EFieldTotalCheck.Checked = true;
                        staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(SetupContent.resultSetOfTreeIndeStr4);
                    }
                    string powerResultStr = StringFound.FoundBackStr(SetupContent.resultSetIndeStr5, nodeNamesStr, false);
                    if (GetResultSetStr(powerResultStr))
                    {
                        PowerCheck.Checked = true;
                        staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(SetupContent.resultSetOfTreeIndeStr5);
                    }

                    //判断用户是否关闭WI
                    Process pro = null;
                    try
                    {
                        pro = Process.GetProcessById(wiProcessID);
                    }
                    catch (ArgumentException)
                    {
                        //如果ID所对应的WI进程已被关闭，则再重新开启WI
                        Process wiProcess = Process.Start(exePath, openProject.FileName);
                        this.WindowState = FormWindowState.Normal;
                        this.Activate();
                        MainWindow.wiProcessID = wiProcess.Id;
                        return;
                    }
                    if (pro == null)
                    {
                        //如果WI进程还存在，则先将其关闭再打开重新加载工程
                        pro.Kill();
                    }
                    else
                    {
                        Process wiProcess = Process.Start(exePath, openProject.FileName);
                        //Process wiProcess = Process.Start(exePath, openProject.FileName);

                        this.WindowState = FormWindowState.Normal;
                        //this.TopMost = true;
                        this.Activate();
                        this.WindowState = FormWindowState.Maximized;

                        MainWindow.wiProcessID = wiProcess.Id;

                    }
                }
                else
                {
                    MessageBox.Show("该工程已经打开！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                mProjectFullName = null;
                
            }
        }

        private void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(MainWindow.mProjectFullName!=null)
            {
                if (Directory.Exists(MainWindow.projectRealPath))
                {
                    FileOperation.WriteFile(setupStr, MainWindow.mProjectFullName,false);
                    Process pro = null;
                    try
                    {
                        pro = Process.GetProcessById(wiProcessID);
                    }
                    catch(ArgumentException)
                    {
                        //如果ID所对应的WI进程已被关闭，则再重新开启WI
                            //Process wiProcess = Process.Start("notepad", mProjectFullName);
                            Process wiProcess = Process.Start(exePath, mProjectFullName);
                            //this.WindowState = FormWindowState.Normal;
                            this.Activate();
                            MainWindow.wiProcessID = wiProcess.Id;
                            return;
                    }
                    {
                        //如果WI进程还存在，则先将其关闭再打开重新加载工程
                        pro.Kill();
                        //Process wiProcess = Process.Start("notepad", mProjectFullName);
                        Process wiProcess = Process.Start(exePath, mProjectFullName);

                        //this.WindowState = FormWindowState.Normal;
                        //this.TopMost = true;
                        this.Activate();
                        //this.WindowState = FormWindowState.Maximized;

                        MainWindow.wiProcessID = wiProcess.Id;
                    }
                }
                else
                {
                    MessageBox.Show("您所创建的工程已被删除！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }      
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            float[] scale = (float[])Tag;
            int i = 2;
            //遍历每一个控件使其同比例放大
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Left = (int)(Size.Width * scale[i++]);
                ctrl.Top = (int)(Size.Height * scale[i++]);
                ctrl.Width = (int)(Size.Width / (float)scale[0] * ((Size)ctrl.Tag).Width);
                ctrl.Height = (int)(Size.Height / (float)scale[1] * ((Size)ctrl.Tag).Height);
                //工程树单独同比例放大
                if (WindowState == FormWindowState.Maximized)
                {
                    //最大化时调整字体的型号如下
                    ctrl.Font = new System.Drawing.Font("宋体", 12);
                    //最大化时调整状态栏的列宽
                    projectstate_dataGridView1.Columns[0].Width = 120;
                    projectstate_dataGridView1.Columns[1].Width = 50;
                    projectstate_dataGridView1.Columns[2].Width = 110;
                    projectstate_dataGridView1.Columns[3].Width = 180;
                    projectstate_dataGridView1.Columns[4].Width = 180;
                    //dataGridView1.Width = 1000;
                    projectstate_dataGridView1.RowHeadersWidth = 40;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    //默认大小的窗口时所需的操作
                    ctrl.Font = new System.Drawing.Font("宋体", 9);
                    projectstate_dataGridView1.Columns[0].Width = 50;
                    projectstate_dataGridView1.Columns[1].Width = 20;
                    projectstate_dataGridView1.Columns[2].Width = 60;
                    projectstate_dataGridView1.Columns[3].Width = 60;
                    projectstate_dataGridView1.Columns[4].Width = 60;
                    //dataGridView1.Width = 1000;
                    projectstate_dataGridView1.RowHeadersWidth = 30;
                } 
            }
        }

        private void DBview_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f9 = OpenNewForm<DBmanageWindow>(f9);
            //DBmanageWindow f9 = new DBmanageWindow();
            //f9.Show();
        }
        private void resultset_toolStripButton7_Click(object sender, EventArgs e)
        {
            f10 = OpenNewForm<ResultSetWindow>(f10);
            //ResultSetWindow f10 = new ResultSetWindow();
            //f10.Show();
        }
        private void receiver_toolStripButton5_Click(object sender, EventArgs e)
        {
            f11 = OpenNewForm<NewReceiverWindow>(f11);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MainWindow.mProjectFullName != null)
            {
                if (File.Exists(mProjectFullName))
                {
                    FileOperation.WriteFile(setupStr, MainWindow.mProjectFullName, false);
                    Process pro = null;
                    try
                    {
                        pro = Process.GetProcessById(wiProcessID);
                    }
                    catch (ArgumentException)
                    {
                        //如果WI进程已被关闭，则会产生异常。因此可以通过是否产生异常判断WI是否关闭
                        return;
                    }
                    {
                        //如果WI进程还存在，则先将其关闭再打开重新加载工
                        pro.Kill();
                    }
                }
            }

            flag = false;


        }



        private void project_TreeView1_MouseClick(object sender, MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            currentNode = MainWindow.staticTreeView.GetNodeAt(clickPoint);
            //判断是否在选定的节点上出现节点的右键操作
            if ((e.Button == MouseButtons.Right) && (currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1]))//判断你点的是不是右键
            {
                if (currentNode != null)//判断你点的是不是一个节点
                {
                    //为当前结点添加右键菜单，右键菜单通过工具栏中的控件添加
                    currentNode.ContextMenuStrip = waveformOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }
            if ((e.Button == MouseButtons.Right) && (currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[0]))//判断你点的是不是右键
            {

                if (currentNode != null)//判断你点的是不是一个节点
                {
                    currentNode.ContextMenuStrip = terrainOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }
            if ((e.Button == MouseButtons.Right) && (currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2]))//判断你点的是不是右键
            {
                if (currentNode != null)
                {
                    currentNode.ContextMenuStrip = antennaOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }

            if ((e.Button == MouseButtons.Right) && (currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3]))//判断你点的是不是右键
            {
                if (currentNode != null)
                {
                    currentNode.ContextMenuStrip = transmitterOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }
            if ((e.Button == MouseButtons.Right) && ((currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0]) || (currentNode.Parent == MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1])))//判断你点的是不是右键
            {
                if (currentNode != null)
                {
                    currentNode.ContextMenuStrip = receiverOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }
            if ((e.Button == MouseButtons.Right) && (currentNode== MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5]))//判断你点的是不是右键
            {
                if (currentNode != null)
                {
                    currentNode.ContextMenuStrip = resultSetOfProjectTreeView_ContextMenuStrip;
                    //NewProject_ContextMenuStrip.Show(this.Right  + e.X+100, this.Bottom  + e.Y+100);
                    MainWindow.staticTreeView.SelectedNode = currentNode;
                }
            }
 }


        private void waveformOfProjectTreeView_ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    //单击右键菜单的更改后，右键菜单便消失
                     waveformOfProjectTreeView_ContextMenuStrip.Hide();
                      //执行更改操作
                     if (!WaveformNodeOfConMenu.WaveformUpdateMenu(currentNode))
                     {
                         MessageBox.Show("该波形没有被更改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     MainWindow.creatSuccMesDisp = true;
                     MainWindow.newFuncSign = true;
                    break;
                case "删除":
                        waveformOfProjectTreeView_ContextMenuStrip.Hide();
                    //执行删除操作
                    if (!WaveformNodeOfConMenu.WaveformDelMenu(currentNode))
                    {
                        MessageBox.Show("该波形没有被删除", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    currentNode.Remove();
                    break;
                default:
                    break;
            }
        }
        
       
        private void terrainOfProjectTreeView_ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    //单击右键菜单的更改后，右键菜单便消失
                    terrainOfProjectTreeView_ContextMenuStrip.Hide();
                    //执行更改操作
                    AddTerrainWindow w = new AddTerrainWindow();
                    w.Show();
                    break;
                case "删除":
                    terrainOfProjectTreeView_ContextMenuStrip.Hide();
                    //执行删除操作
                    DialogResult result=MessageBox.Show("您确定要删除原来添加的地形吗？删除将不可恢复！","提醒",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        string nodeNamesStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                        if (!TerrainOperation.DeleteTerrain(nodeNamesStr))
                        {
                            MessageBox.Show("对不起，发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void antennaOfProjectTreeView_ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    antennaOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!AntennaNodeOfConMenu.AntennaUpdateMenu(currentNode))
                    {
                        MessageBox.Show("天线没有被更改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    MainWindow.creatSuccMesDisp = true;
                    MainWindow.newFuncSign = true;
                    break;
                case "删除":
                    antennaOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!AntennaNodeOfConMenu.AntennaDelMenu(currentNode))
                    {
                        MessageBox.Show("天线没有被删除!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    currentNode.Remove();
                    break;
                default:
                    break;
            }
        }
        private void transmitterOfProjectTreeView_ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    transmitterOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!TransmitterNodeOfConMenu.TransmitterUpdateMenu(currentNode))
                    {
                        MessageBox.Show("辐射源没有被更改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    }
                    MainWindow.creatSuccMesDisp = true;
                    MainWindow.newFuncSign = true;
                    break;
                case "删除":
                    transmitterOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!TransmitterNodeOfConMenu.TransmitterDelMenu(currentNode))
                    {
                        MessageBox.Show("辐射源没有被删除!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    currentNode.Remove();
                    break;
                default:
                    break;
            }
        }
        private void receiverOfProjectTreeView_ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    receiverOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!ReceiverNodeOfConMenu.ReceiverUpdateMenu(currentNode))
                    {
                        MessageBox.Show("接收机没有被更改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    MainWindow.creatSuccMesDisp = true;
                    MainWindow.newFuncSign = true;
                    break;
                case "删除":
                    receiverOfProjectTreeView_ContextMenuStrip.Hide();
                    if (!ReceiverNodeOfConMenu.ReceiverDelMenu(currentNode))
                    {
                        MessageBox.Show("文件没有被删除", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    currentNode.Remove();
                    break;
                default:
                    break;
            }
        }
        private void resultSetOfProjectTreeView_ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.ToString())
            {
                case "更改":
                    ResultSetWindow resultSetWin = new ResultSetWindow();
                    resultSetWin.Show();
                    break;
                default:
                    break;
            }
        }

        private void RefreshGridView()
         {
            toolStripStatusLabel1.Text="";
            try
            {
                proInfoArray = client.iGetProjectInfo();
                while (projectstate_dataGridView1.Rows.Count != 1)
                {
                    projectstate_dataGridView1.Rows.RemoveAt(0);
                }
                //if (projectstate_dataGridView1.Rows.Count == 1)
                //{
                //    projectstate_dataGridView1.Rows.RemoveAt(0);
                //}
                foreach (ServiceReference1.ProjectInfo proInfo in proInfoArray)
                {
                    projectstate_dataGridView1.Rows.Add(proInfo.Name, proInfo.ProState, proInfo.Percent, proInfo.CreateTime, proInfo.EndTime);//此处继续添加“仿真结果”列
                }
            }

            catch (System.TimeoutException ex)
            {
                //MessageBox.Show(ex.Message);
                toolStripStatusLabel1.Text="连接超时";
                LogFileManager.ObjLog.fatal(ex.Message, ex);
                client.Abort();
            }
            catch (FaultException<WcfException> ex)
            {
               // MessageBox.Show(ex.Detail.message);
                toolStripStatusLabel1.Text="通信异常";
                LogFileManager.ObjLog.fatal(ex.Message, ex);
                client.Abort();
            }
            catch (CommunicationException ex)
            {
               // MessageBox.Show(ex.Message);
                toolStripStatusLabel1.Text = "无法连接服务器";
                LogFileManager.ObjLog.fatal(ex.Message, ex);
                client.Abort();
            }
            catch (Exception exm)
            {
               // MessageBox.Show(exm.Message);
                toolStripStatusLabel1.Text = exm.Message;
                LogFileManager.ObjLog.fatal(exm.Message, exm);
                client.Abort();
            }

        }

        public void RefreshProjectInfo()
        {
            MethodInvoker mi = new MethodInvoker(RefreshGridView);

            while (flag)
            {

                try
                {
                    //等待窗口句柄生成
                    while (!this.IsHandleCreated)
                    {
                        ;
                    }
                    this.BeginInvoke(mi);
                    Thread.Sleep(5000);

                }
               
                catch (Exception ex)
                {

                    toolStripStatusLabel1.Text = ex.Message;
                    LogFileManager.ObjLog.error(ex.GetType() + ex.Message);
                    MessageBox.Show("更新状态失败");

                }
            }
        }

        private void simulationStart_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string projectpath = projectRealPath;
            string proname = onlyProjectName;
            string[] filepaths = null;
            StreamReader sr = null;
            string serverpath = ConfigurationManager.AppSettings["ServerProjectPath"];
            //string serverpath = "D:\\serverpath";
            if (projectpath == null || Directory.GetFiles(projectpath, "*.setup").Count() == 0)
            {
                MessageBox.Show("请添加工程");
            }
            else if (Directory.GetFiles(projectpath, "*.ter").Count() == 0)
            {
                MessageBox.Show("未添加地形信息");
            }
            else if (Directory.GetFiles(projectpath, "*.tx").Count() == 0)
            {
                MessageBox.Show("未添加发射机");
            }
            else if (Directory.GetFiles(projectpath, "*.rx").Count() == 0)
            {
                MessageBox.Show("未添加接收机");
            }
            else
            {
                try
                {

                    filepaths = Directory.GetFiles(projectpath);                                  //获取工程路径下的文件名称
                    foreach (string filepath in filepaths)
                    {
                        string filename = filepath.Substring(filepath.LastIndexOf("\\") + 1);     //获取文件名
                        sr = new StreamReader(filepath);
                        client.PutData(serverpath + "\\" + proname, filename, sr.ReadToEnd());    //上传文件
                    }

                    //分割文件、向数据库插入数据
                    client.CreatTables(serverpath + "\\" + proname);
                    MessageBox.Show("仿真成功开始");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("文件读取错误");
                    LogFileManager.ObjLog.error(ex.Message);
                }
                catch (FaultException<WcfException> ex)
                {
                    MessageBox.Show("上传文件失败");
                    LogFileManager.ObjLog.error(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    LogFileManager.ObjLog.error(ex.Message);
                }
                finally
                {
                    sr.Close();
                }
            }

        }

        private void simulationEnd_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onlyProjectName == null)
            {
                MessageBox.Show("未选择工程");
            }
            else
            {
                try
                {
                    client.iDelProject(onlyProjectName);
                    MessageBox.Show("仿真已停止");
                }
                catch (FaultException<WcfException> ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
  
            //开始工程进度线程
            refreshThread = new Thread(new ThreadStart(RefreshProjectInfo));
            refreshThread.Start();

        }
        private void projectstate_dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //单击“状态显示”栏的第五列即“仿真结果”列时，弹出下载结果文件进度对话框
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                downloadProInfo = proInfoArray[e.RowIndex];
                if (downloadProInfo != null)
                {
                    f12 = OpenNewForm<ResultDownloadStateWindow>(f12);
                }
            }
        }

        private void interferencetest_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterfenceWindow iw = new InterfenceWindow();
            iw.ShowDialog(this);
        }

        private void WIPath_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();
        }

        private void iPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlNodeDBSettingForm setting = new ControlNodeDBSettingForm();
            setting.closeFather += new ChildClose(this.Close);
            setting.ShowDialog();
        }

        void setting_closeFather()
        {
            throw new NotImplementedException();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["WIPath"].Equals(""))
            {
                MessageBox.Show("请配置Wireless Insite的安装路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            exePath = ConfigurationManager.AppSettings["WIPath"];
            LogFileManager.ObjLog.debug(exePath);
        }
     }
 }



        
   


