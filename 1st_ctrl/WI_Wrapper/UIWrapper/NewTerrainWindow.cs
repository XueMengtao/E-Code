using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WcfService;
using System.ServiceModel;
using System.IO;
using System.Data.SqlClient;
namespace WF1
{
    public partial class NewTerrainWindow : Form
    {
        ServiceReference1.Service1Client client = null;
        ServiceReference1.TerInfo addTerrain = null;
        public NewTerrainWindow()
        {
            InitializeComponent();
            client = new ServiceReference1.Service1Client();
            addTerrain = new ServiceReference1.TerInfo();
        }
        private void NewTerrainWindow_Load(object sender, EventArgs e)
        {
            //在窗口加载时，获取数据库中TerInfo表全部地形的名称，在地形名称组合框的下拉列表显示。
            string[] TerNames = new string[] { };
            TerNames = client.iGetTerNames();
            newTerrainName_comboBox1.Items.Clear();
            foreach (string s in TerNames)
            {
                newTerrainName_comboBox1.Items.Add(s);
            }  
        }
        private void newTerrainName_comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //获得某个地图的源点信息加载到UI界面
            try
            {
                addTerrain = client.iGetTer((string)newTerrainName_comboBox1.SelectedItem);
                LogFileManager.ObjLog.info((string)newTerrainName_comboBox1.SelectedItem);
                newTerrainLongtitude_textBox2.Text = addTerrain.originX.ToString();
                newTerrainLatitude_textBox3.Text = addTerrain.originY.ToString();

                MainWindow.longitudeStr=addTerrain.originX.ToString();
                MainWindow.latitudeStr = addTerrain.originY.ToString();

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
        private void newTerrainWindowOk_button1_Click(object sender, EventArgs e)
        {
            addTerrain = client.iGetTer((string)newTerrainName_comboBox1.SelectedItem);
            LogFileManager.ObjLog.info((string)newTerrainName_comboBox1.SelectedItem);
            //确保先创建工程。否则插入数据将会出错
            if (MainWindow.mProjectFullName == null)
            {
                MessageBox.Show("请先创建一个工程之后再从本地添加地形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {

                //必须先选择地形，再单击确定
                if ((string)newTerrainName_comboBox1.SelectedItem != "")
                {
                    string t = addTerrain.path;
                    //将经纬度存储到.setup.info文件中
                    FileOperation.WriteLineFile(SetupContent.longitudeIndeStr + " " + addTerrain.originX, MainWindow.nodeInfoFullPath, true);
                    FileOperation.WriteLineFile(SetupContent.latitudeIndeStr + " " + addTerrain.originY, MainWindow.nodeInfoFullPath, true);
                    if (File.Exists(t) == false)
                    {
                        MessageBox.Show("数据库中已不存在此地图", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //查找begin_<project>后面的字符串temp
                    string temp = StringFound.FoundBackStr("begin_<project>", MainWindow.setupStr, true);
                    if (temp == null)
                    {
                        MessageBox.Show("程序内部发生错误!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        //查找begin_<project>后面的字符串temp
                        string nodeNamesStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                        //若信息文件中已经添加了地形，再次添加时要从setup文件先删除原来的地形，从从工程树中删除原来的地形
                        if (nodeNamesStr.LastIndexOf("<terrain>") != -1)
                        {
                            DialogResult result = MessageBox.Show("您确定要替换原来添加的地形吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result == DialogResult.OK)
                            {
                              
                                if (!TerrainOperation.DeleteTerrain(nodeNamesStr))
                                {
                                    MessageBox.Show("对不起，程序内部发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            string globalStr = SetupContent.terrainGlobalFixedStr1 + newTerrainLongtitude_textBox2.Text + "\r\n"
                                                    + SetupContent.terrainGlobalFixedStr2 + newTerrainLatitude_textBox3.Text + "\r\n"
                                                    + SetupContent.terrainGlobalFixedStr3 + "\r\n";
                            MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiGlobal, globalStr);
                            ////向全局字符串插入<studyarea>段  其中zmin zmax及地图顶点先设为默认值
                            int startPosiStudyArea = MainWindow.setupStr.IndexOf("FirstAvailableStudyAreaNumber") + 33;
                            string studyAreaStr = SetupContent.terrainStudyAreaStr1 + "\r\n"
                                                            + SetupContent.terrainStudyAreaStr2 + newTerrainLongtitude_textBox2.Text + "\r\n"
                                                            + SetupContent.terrainGlobalFixedStr2 + newTerrainLatitude_textBox3.Text + "\r\n"
                                                            + SetupContent.terrainStudyAreaStr3 + addTerrain.Zmin.ToString() + "\r\n"
                                                            + SetupContent.terrainStudyAreaStr4 + addTerrain.Zmax.ToString() + "\r\n"
                                                            + SetupContent.terrainStudyAreaStr5 + "\r\n"
                                                            + addTerrain.Vertex1X + " " + addTerrain.Vertex1Y + " " + addTerrain.Vertex1Z + "\r\n"
                                                            + addTerrain.Vertex2X + " " + addTerrain.Vertex2Y + " " + addTerrain.Vertex2Z + "\r\n"
                                                            + addTerrain.Vertex3X + " " + addTerrain.Vertex3Y + " " + addTerrain.Vertex3Z + "\r\n"
                                                            + addTerrain.Vertex4X + " " + addTerrain.Vertex4Y + " " + addTerrain.Vertex4Z + "\r\n"
                                                            + SetupContent.terrainStudyAreaStr6 + "\r\n";
                            MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiStudyArea, studyAreaStr);
                            //向全局字符串插入<feature>段
                            int startPosiFeature = MainWindow.setupStr.IndexOf("end_<studyarea>") + 17;
                            string featureStr = SetupContent.terrainFeatureFixedStr1 + "\r\n"
                            + SetupContent.terrainFeatureFixedStr2 + "0" + "\r\n"
                            + SetupContent.terrainFeatureFixedStr3 + "\r\n"
                            + SetupContent.terrainFeatureFixedStr4
                            + MainWindow.projectRealPath + "\\" + (string)newTerrainName_comboBox1.SelectedItem + "\r\n"
                            + SetupContent.terrainFeatureFixedStr5 + "\r\n";
                            MainWindow.setupStr = MainWindow.setupStr.Insert(startPosiFeature, featureStr);



                            string terFileName = t.Substring(t.LastIndexOf('\\') + 1, t.Length - 1 - t.LastIndexOf('\\'));
                            //将ter文件拷贝到工程目录下
                            FileCopyUI.FileCopy(addTerrain.path, MainWindow.projectRealPath + "\\" + terFileName);
                            FileCopyUI.FileCopy(addTerrain.path + ".terinfo", MainWindow.projectRealPath + "\\" + terFileName + ".terinfo");

                            string terainStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                            if (terainStr.IndexOf(SetupContent.terrainIndeStr) != -1)
                            {
                                terainStr = terainStr.Remove(terainStr.IndexOf(SetupContent.terrainIndeStr), SetupContent.terrainIndeStr.Length + 2);
                                FileOperation.WriteFile(terainStr, MainWindow.nodeInfoFullPath, false);
                            }
                            //将工程树的节点信息写到.info文件中
                            FileOperation.WriteLineFile(SetupContent.terrainIndeStr + " " + terFileName, MainWindow.nodeInfoFullPath, true);
                            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[0].Nodes.Add(terFileName);
                        }
                }
            
             }
            this.Close();
        }       
        private void newTerrainWindowClose_button2_Click(object sender, EventArgs e)
        {
            this.Close();
            client.Close();
        }
       
    }

}

