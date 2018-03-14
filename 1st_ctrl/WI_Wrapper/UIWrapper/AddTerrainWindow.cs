using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.IO;
using System.Data.SqlClient;
using WcfService;
namespace WF1
{   
    public partial class AddTerrainWindow : Form
    {
        public static string longtitudeStr = null;
        public static string latitudeStr = null;
        //存放从ter文件读取的字符串
        //string terrainTemp = null;
        //terinfo文件总共有八行数据
        static int n = 8;
        string[] terrainValueStr = new string[n];
        public AddTerrainWindow()
        {
            InitializeComponent();
        }

        private void browse_button1_Click(object sender, EventArgs e)
        {
            openTerrainDialog.FileName = null;
            openTerrainDialog.Filter = "TER文件(*.ter)|*.ter|所有文件|*.*";
            openTerrainDialog.Title = "添加地形文件";
            if (MainWindow.mProjectFullName != null)
            {
                //当单机确定按钮时
                if (openTerrainDialog.ShowDialog() == DialogResult.OK)
                {
                    //用t存放打开文件的完整路径名
                    locateroute_textBox2.Text = openTerrainDialog.FileName;
                    terrainname_textBox4.Text = openTerrainDialog.FileName.Substring(openTerrainDialog.FileName.LastIndexOf('\\') + 1, openTerrainDialog.FileName.Length - 1 - openTerrainDialog.FileName.LastIndexOf('\\'));
                    try
                    {
                        //如果读取的文件不存在，则会抛出异常，必须处理该异常
                        terrainValueStr = FileOperation.ReadLineFile(openTerrainDialog.FileName + ".terinfo", n);
                    }
                    catch (FileNotFoundException ex)
                    {
                        LogFileManager.ObjLog.fatal(ex.Message, ex);
                        MessageBox.Show("您导入的地形文件存在错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    longtitude_textBox1.Text = terrainValueStr[0];
                    latitude_textBox3.Text = terrainValueStr[1];

                    MainWindow.longitudeStr = terrainValueStr[0];
                    MainWindow.latitudeStr = terrainValueStr[1];
                    //将经纬度存储到.setup.info文件中
                    FileOperation.WriteLineFile(SetupContent.longitudeIndeStr + " " + terrainValueStr[0], MainWindow.nodeInfoFullPath, true);
                    FileOperation.WriteLineFile(SetupContent.latitudeIndeStr + " " + terrainValueStr[1], MainWindow.nodeInfoFullPath, true);
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("请先创建一个工程之后再从本地添加地形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newTerrainOk__Click(object sender, EventArgs e)
        {
            //确保先创建工程。否则插入数据将会出错
            if (MainWindow.mProjectFullName == null)
            {
                //MessageBox.Show("请先创建一个工程之后再从本地添加地形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                //必须先浏览地形，再单击确定
                if (openTerrainDialog.FileName != "")
                {
 
                    //查找begin_<project>后面的字符串temp
                    string temp = StringFound.FoundBackStr("begin_<project>", MainWindow.setupStr,true);
                    if (temp == null)
                    {
                        MessageBox.Show("文件被破坏，产生错误错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        //从信息文件中读取信息
                        string nodeNamesStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                        //若信息文件中已经添加了地形，再次添加时要从setup文件先删除原来的地形，从从工程树中删除原来的地形
                        if (nodeNamesStr.LastIndexOf("<terrain>")!=-1)
                        {
                            DialogResult result=MessageBox.Show("您确定要替换原来添加的地形吗？","提醒",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                            if (result == DialogResult.OK)
                            {
                                if (!TerrainOperation.DeleteTerrain(nodeNamesStr))
                                {
                                    MessageBox.Show("对不起，发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                    return;
                                }
                                MessageBox.Show("前一个地图已被替换", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                return;
                            }

                        }
                        //向全局字符串setupStr插入<global>段
                        int startPosiGlobal = MainWindow.setupStr.IndexOf("begin_<project>") + 15 + temp.Length + 3;
                        string globalStr = SetupContent.terrainGlobalFixedStr1 + longtitude_textBox1.Text + "\r\n"
                                                +SetupContent.terrainGlobalFixedStr2+ latitude_textBox3.Text + "\r\n"
                                                + SetupContent.terrainGlobalFixedStr3+"\r\n";
                        MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiGlobal, globalStr);
                        //向全局字符串插入<studyarea>段
                        int startPosiStudyArea = MainWindow.setupStr.IndexOf("FirstAvailableStudyAreaNumber") + 33;                       
                        string studyAreaStr = SetupContent.terrainStudyAreaStr1 + "\r\n"
                                                        + SetupContent.terrainStudyAreaStr2 + longtitude_textBox1.Text + "\r\n"
                                                        + SetupContent.terrainGlobalFixedStr2 + latitude_textBox3.Text + "\r\n"
                                                        + SetupContent.terrainStudyAreaStr3+terrainValueStr[2]+"\r\n"
                                                        + SetupContent.terrainStudyAreaStr4 + terrainValueStr[3]+"\r\n"
                                                        + SetupContent.terrainStudyAreaStr5 +"\r\n"+ terrainValueStr[4]+"\r\n"
                                                        +terrainValueStr[5]+"\r\n"+terrainValueStr[6]+"\r\n"+terrainValueStr[7]+"\r\n"
                                                        + SetupContent.terrainStudyAreaStr6 + "\r\n";
                        MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiStudyArea, studyAreaStr);
                        //向全局字符串插入<feature>段
                        int startPosiFeature = MainWindow.setupStr.IndexOf("end_<studyarea>") + 17; 
                        string featureStr= SetupContent.terrainFeatureFixedStr1 +"\r\n"
                        + SetupContent.terrainFeatureFixedStr2 + "0" + "\r\n"
                        + SetupContent.terrainFeatureFixedStr3 + "\r\n"
                        + SetupContent.terrainFeatureFixedStr4
                        + MainWindow.projectRealPath + "\\" + terrainname_textBox4.Text + "\r\n"
                        + SetupContent.terrainFeatureFixedStr5 + "\r\n";
                        MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiFeature, featureStr);

                        string t = openTerrainDialog.FileName;
                        string terFileName = t.Substring(t.LastIndexOf('\\') + 1, t.Length - 1 - t.LastIndexOf('\\'));
                        //将ter文件拷贝到工程目录下
                        FileCopyUI.FileCopy(openTerrainDialog.FileName, MainWindow.projectRealPath + "\\" + terFileName);
                        FileCopyUI.FileCopy(openTerrainDialog.FileName+".terinfo", MainWindow.projectRealPath + "\\" + terFileName+".terinfo");

                        //将工程树的节点信息写到.info文件中
                        FileOperation.WriteLineFile(SetupContent.terrainIndeStr +" " +terFileName , MainWindow.nodeInfoFullPath, true);
                        FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                        MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[0].Nodes.Add(terFileName);

                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请您从本地添加地形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void newTerrainCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newTerrainSave_button4_Click(object sender, EventArgs e)
        {
            //从.terinfo文件读取相应.ter文件的Zmin,Zmax及四个顶点的值


            //if (openTerrainDialog.ShowDialog() == DialogResult.OK)
            //{

                //将.terinfo中有关地图四个顶点信息的值取出
                string FileName = locateroute_textBox2.Text + ".terinfo";
                StreamReader sr = new StreamReader(FileName);
                string rows = sr.ReadToEnd();    //读取文件的信息       
                string[] b = new string[28];
                b = rows.Split(new Char[] { '\n', ' ' }); //将文件的内容以"空格"、"回车"分隔开取出某段字符

                ServiceReference1.TerInfo newterrain = new ServiceReference1.TerInfo();
                ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
                newterrain.name = terrainname_textBox4.Text;
                newterrain.originX = double.Parse(longtitude_textBox1.Text);
                newterrain.originY = double.Parse(latitude_textBox3.Text);
                newterrain.path = locateroute_textBox2.Text;
                newterrain.Zmin = double.Parse(b[2]);   //从数组b[2]开始存的是地图的Zmin、Zmax、及四个顶点坐标值
                newterrain.Zmax = double.Parse(b[3]);
                newterrain.Vertex1X = double.Parse(b[4]);
                newterrain.Vertex1Y = double.Parse(b[5]);
                newterrain.Vertex1Z = double.Parse(b[6]);
                newterrain.Vertex2X = double.Parse(b[7]);
                newterrain.Vertex2Y = double.Parse(b[8]);
                newterrain.Vertex2Z = double.Parse(b[9]);
                newterrain.Vertex3X = double.Parse(b[10]);
                newterrain.Vertex3Y = double.Parse(b[11]);
                newterrain.Vertex3Z = double.Parse(b[12]);
                newterrain.Vertex4X = double.Parse(b[13]);
                newterrain.Vertex4Y = double.Parse(b[14]);
                newterrain.Vertex4Z = double.Parse(b[15]);

                try
                {
                    client.iAddTerInfo(newterrain);
                    MessageBox.Show("地形已成功添加至地图数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.TimeoutException ex)
                {
                    MessageBox.Show(ex.Message);
                    LogFileManager.ObjLog.fatal(ex.Message, ex);
                    client.Abort();
                }
                catch (FaultException<WcfException> ex)
                {
                    MessageBox.Show(ex.Detail.message);
                    LogFileManager.ObjLog.fatal(ex.Message, ex);
                    client.Abort();
                }
                catch (CommunicationException ex)
                {
                    MessageBox.Show(ex.Message);
                    LogFileManager.ObjLog.fatal(ex.Message, ex);
                    client.Abort();
                }
                catch (Exception exm)
                {
                    MessageBox.Show(exm.Message);
                    LogFileManager.ObjLog.fatal(exm.Message, exm);
                    client.Abort();
                }
        }

        private void AddTerrainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
