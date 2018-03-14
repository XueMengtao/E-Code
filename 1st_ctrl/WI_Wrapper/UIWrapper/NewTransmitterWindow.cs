using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.ServiceModel;
using System.Data.SqlClient;
using WcfService;
using LogFileManager;
namespace WF1
{
    public partial class NewTransmitterWindow : Form
    {
        string antennaNamesOfCombox = null;
        public NewTransmitterWindow()
        {
            InitializeComponent();
            if (MainWindow.mProjectFullName == null)
            {
                return;
            }
            if (File.Exists(MainWindow.nodeInfoFullPath))
            {
                antennaNamesOfCombox = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
            }
            if (MainWindow.nodeInfoFullPath != null)
            {
                newTransmitterLongitude_textBox1.Text = MainWindow.longitudeStr;
                newTransmitterLatitude_textBox3.Text = MainWindow.latitudeStr;
                ArrayList antennaNamesArr = new ArrayList();
                //向工程树中添加波形节点
                antennaNamesArr = ProjectTreeMatchString.MatchStr(antennaNamesOfCombox, SetupContent.antennaIndeStr, SetupContent.antennaIndeStr.Length);
                foreach (string s in antennaNamesArr)
                {
                    newTransmitterAntennaName_comboBox3.Items.Add(s);
                }
            }
        }

        private void newTransmitterClose_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    

        private void NewTransmitter_Load(object sender, EventArgs e)
        {
            //if (MainWindow.nodeInfoFullPath != null)
            //{
            //    newTransmitterLongitude_textBox1.Text = MainWindow.longitudeStr;
            //    newTransmitterLatitude_textBox3.Text = MainWindow.latitudeStr;
            //    ArrayList antennaNamesArr = new ArrayList();
            //    //向工程树中添加波形节点
            //    antennaNamesArr = ProjectTreeMatchString.MatchStr(antennaNamesOfCombox, SetupContent.antennaIndeStr, SetupContent.antennaIndeStr.Length);
            //    foreach (string s in antennaNamesArr)
            //    {
            //        newTransmitterAntennaName_comboBox3.Items.Add(s);
            //    }
            //}
        }

        private void newTransmitterOK_Click(object sender, EventArgs e)
        {
            if (MainWindow.mProjectFullName==null)
            {
                this.Close();
                return;               
            }
            if(!ConditionIsAll(MainWindow.nodeInfoFullPath ))
            {
                MessageBox.Show("请先创建天线和添加波形之后再新建辐射源!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (newTransmitterName_textBox2.Text == "" || newTransmitterLongitude_textBox1.Text == "" || newTransmitterLatitude_textBox3.Text == "" || newTransimtterReferencePlane_comboBox2.SelectedItem == null || newTransmitterPower_textBox11.Text == "" || newTransmitterAntennaName_comboBox3.SelectedItem == null || newTransmitterWaveformName_textBox1.Text == "" || newTransmitterAntennaRotationX_textBox4.Text == "" || newTransmitterAntennaRotationY_textBox6.Text == "" || newTransmitterAntennaRotationZ_textBox5.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newTransmitterPower_textBox11.Text))
            {
                MessageBox.Show("发射功率值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newTransmitterPower_textBox11.Text) < 0.000)
            {
                MessageBox.Show("发射功率值需大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationX_textBox4.Text))
            {
                MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationY_textBox6.Text))
            {
                MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationZ_textBox5.Text))
            {
                MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationX_textBox4))
            {
                MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationY_textBox6))
            {
                MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationZ_textBox5))
            {
                MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
                //先判断是否存在.tx文件
            if (File.Exists(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx"))
            {
                WaveformWriting transm = new WaveformWriting(FileOperation.ReadFile(MainWindow.nodeInfoFullPath ));
                //注意 新建的辐射源个数不能超过1000
                string[] transmitterNames = new string[1000];
                //判断是否存在重名的辐射源
                transmitterNames = transm.waveformNames(SetupContent.transmitterIndeStr);
                if (transm.judge(newTransmitterName_textBox2.Text, transmitterNames))
                {
                    MessageBox.Show("此辐射源已存在,请您换一个辐射源名称！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
            }
            string transmitterCount = GetTransmitterNum(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx", SetupContent.transmitterStr2OfTr);
            string sourceStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);

            string trFileStr = SetupContent.transmitterStr1OfTr +" "+ newTransmitterName_textBox2.Text + "\r\n"
                                    + SetupContent.transmitterStr2OfTr + transmitterCount +"\r\n"
                                    + SetupContent.transmitterStr3OfTr + "\r\n" + SetupContent.transmitterStr4OfTr + "\r\n"
                                    + SetupContent.transmitterStr5OfTr + "\r\n" + SetupContent.transmitterStr6OfTr + "\r\n"
                                    + SetupContent.transmitterStr7OfTr + "\r\n" + SetupContent.transmitterStr8OfTr + "\r\n"
                                    + SetupContent.transmitterStr9OfTr + "\r\n" + SetupContent.transmitterStr10OfTr + "\r\n"
                                    + SetupContent.transmitterStr11OfTr + "\r\n"
                                    + SetupContent.transmitterStr12OfTr + MainWindow.longitudeStr  + "\r\n"
                                    + SetupContent.transmitterStr13OfTr + MainWindow.latitudeStr  + "\r\n"
                                    + SetupContent.transmitterStr14OfTr + "\r\n"
                                    + Translate.KeyWordsDictionary(newTransimtterReferencePlane_comboBox2) + "\r\n"
                                    + SetupContent.transmitterStr15OfTr + "\r\n"
                                    + SetupContent.transmitterStr16OfTr + "\r\n"
                                    //+ SetupContent.transmitterStr17OfTr + "\r\n"
                                    + GetAntennaStr(MainWindow.transInfoFullPath, newTransmitterAntennaName_comboBox3)
                                    + SetupContent.transmitterStr18OfTr + newTransmitterAntennaRotationX_textBox4.Text + "\r\n"
                                    + SetupContent.transmitterStr19OfTr + newTransmitterAntennaRotationY_textBox6.Text + "\r\n"
                                    + SetupContent.transmitterStr20OfTr + newTransmitterAntennaRotationZ_textBox5.Text + "\r\n"
                                    + SetupContent.transmitterStr21OfTr + newTransmitterPower_textBox11.Text + "\r\n"
                                    + SetupContent.transmitterStr22OfTr + "\r\n"
                                    + SetupContent.transmitterStr23OfTr + "\r\n"
                                    + SetupContent.transmitterStr24OfTr + " "
                                    + WaveformNodeOfConMenu.GetWaveformNumLine(newTransmitterWaveformName_textBox1.Text , sourceStr)+"MHZ" + "\r\n";
            FileOperation.WriteFile(trFileStr, MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx", true);
            //写到setup文件中
            string sourceStrOfTr = FileOperation.ReadFile(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx");
            //找出辐射源的个数
            int transmitCount = WaveformWriting.GetCountOfMatchStr("begin_<points>", sourceStrOfTr);
            //如果已经存在了transmitter块，则先删除
            if (MainWindow.setupStr.LastIndexOf("end_<transmitter>") != -1)
            {
               MainWindow.setupStr= MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<transmitter>"), MainWindow.setupStr.IndexOf("end_<transmitter>") - MainWindow.setupStr.IndexOf("begin_<transmitter>") + 19);
            }
            int insertSiteOfTr = MainWindow.setupStr.LastIndexOf("end_<feature>") + 15;//15是end_<feature>\r\n后的第一个字符
            string insertStr = SetupContent.transmitterStr1Ofsetup + "\r\n" + SetupContent.transmitterStr2Ofsetup
                                    + MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx" + "\r\n"
                                    + SetupContent.transmitterStr3Ofsetup + transmitCount.ToString() + "\r\n"
                                    + SetupContent.transmitterStr4Ofsetup + "\r\n";
            //将字符串插入到全局字符串setupStr中
            MainWindow.setupStr = MainWindow.setupStr.Insert(insertSiteOfTr, insertStr);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
            //向工程树中添加结点信息
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3].Nodes.Add(newTransmitterName_textBox2.Text);
            //将辐射源的信息存储到.setup.info文件中
            FileOperation.WriteLineFile(SetupContent.transmitterIndeStr + " " + newTransmitterName_textBox2.Text, MainWindow.nodeInfoFullPath, true);
            //将辐射源和天线的匹配关系记录到.match文件中
            FileOperation.WriteLineFile(SetupContent.transmitterIndeStr + " " + newTransmitterName_textBox2.Text + "*"  + newTransmitterAntennaName_comboBox3.SelectedItem, MainWindow.relationOfAntAndWavePath, true);
            //将辐射源的信息存储到.waveinfo文件中
            string transmitterInfoStr = SetupContent.transmitterStr1Ofsetup + " " + newTransmitterName_textBox2.Text + "\r\n"
                                                    + newTransimtterReferencePlane_comboBox2.Text + "\r\n"
                                                    + newTransmitterPower_textBox11.Text + "\r\n"
                                                    + newTransmitterAntennaName_comboBox3.Text + "\r\n"
                                                    + newTransmitterWaveformName_textBox1.Text + "\r\n"
                                                    + newTransmitterAntennaRotationX_textBox4.Text + "\r\n"
                                                    + newTransmitterAntennaRotationY_textBox6.Text + "\r\n"
                                                    + newTransmitterAntennaRotationZ_textBox5.Text + "\r\n"
                                                    + "END" + SetupContent.transmitterStr1Ofsetup + " " + newTransmitterName_textBox2.Text + "\r\n";
            FileOperation.WriteFile(transmitterInfoStr, MainWindow.waveinfoFilePath, true);
                
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newTransmitterName_textBox2.Text + "\"辐射源创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //判断在新建辐射源之前是否添加了地形和新建了天线
     public static bool ConditionIsAll(string path)
        {
            string str=FileOperation.ReadFile(path);
            if ((str.LastIndexOf("<antenna>") != -1) && (str.IndexOf("<terrain>") != -1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //返回TxSet后面的编号
        public static  string GetTransmitterNum(string path,string indexStr)
        {

            if (!File.Exists(path ))
            {
                return "1";
            }
            else
            {
                string trConStr = FileOperation.ReadFile(path);
                string transmitterStr = null;
                int transmitterSite = trConStr.LastIndexOf("\n"+indexStr) + indexStr.Length+1;//7是\nTxSet (还有一空格）的长度
                while (trConStr[transmitterSite] != '\r')
                    transmitterStr = transmitterStr + trConStr[transmitterSite++];
                int b = int.Parse(transmitterStr) + 1;
                transmitterStr = string.Format("{0:##}", b);
                return transmitterStr;
            }
        }
        //写tr文件中有关天线的那一段
       public  static string GetAntennaStr(string path,ComboBox c)
        {
            string antennaOfTrStr = FileOperation.ReadFile(path);

            string startStr = SetupContent.antennaStr1 +" " + c.Text +"\r\n";
            string endStr = SetupContent.antennaStr3 + " " + c.Text + "\r\n";
            int startSite = antennaOfTrStr.LastIndexOf(startStr) ;
            int endSite = antennaOfTrStr.LastIndexOf(endStr);

            antennaOfTrStr = antennaOfTrStr.Substring(startSite, endSite - startSite);
            return antennaOfTrStr;
        }

        private void newTransmitterSave_button3_Click(object sender, EventArgs e)
        {   
            //向数据库中添加新建的辐射源（暂时波形的名字未从UI传到数据库）
            ServiceReference1.Transmitter newTransmitter = new ServiceReference1.Transmitter();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newTransmitter.Name = newTransmitterName_textBox2.Text;
                newTransmitter.RotateX = double.Parse(newTransmitterAntennaRotationX_textBox4.Text);
                newTransmitter.RotateY = double.Parse(newTransmitterAntennaRotationY_textBox6.Text);
                newTransmitter.RotateZ = double.Parse(newTransmitterAntennaRotationZ_textBox5.Text);
                newTransmitter.Power = double.Parse(newTransmitterPower_textBox11.Text);
                //newTransmitter.WaveFormName = newTransmitterWaveformName_textBox1.Text;
                newTransmitter.AntennaName = (string)newTransmitterAntennaName_comboBox3.SelectedItem;

                try
                {
                    if (newTransmitterName_textBox2.Text == "" || newTransmitterLongitude_textBox1.Text == "" || newTransmitterLatitude_textBox3.Text == "" || newTransimtterReferencePlane_comboBox2.SelectedItem == null || newTransmitterPower_textBox11.Text == "" || newTransmitterAntennaName_comboBox3.SelectedItem == null || newTransmitterWaveformName_textBox1.Text == "" || newTransmitterAntennaRotationX_textBox4.Text == "" || newTransmitterAntennaRotationY_textBox6.Text == "" || newTransmitterAntennaRotationZ_textBox5.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newTransmitterPower_textBox11.Text))
                    {
                        MessageBox.Show("发射功率值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newTransmitterPower_textBox11.Text) < 0.000)
                    {
                        MessageBox.Show("发射功率值需大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationX_textBox4.Text))
                    {
                        MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationY_textBox6.Text))
                    {
                        MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newTransmitterAntennaRotationZ_textBox5.Text))
                    {
                        MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationX_textBox4))
                    {
                        MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationY_textBox6))
                    {
                        MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.RotationLimition(newTransmitterAntennaRotationZ_textBox5))
                    {
                        MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddTransmitter(newTransmitter);
                    MessageBox.Show("辐射源已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else
            {
                newTransmitterOK_Click(sender, e);
                try
                {
                    newTransmitter = client.iGetTransmitter(newTransmitterName_textBox2.Text);
                    newTransmitter.RotateX = double.Parse(newTransmitterAntennaRotationX_textBox4.Text);
                    newTransmitter.RotateY = double.Parse(newTransmitterAntennaRotationY_textBox6.Text);
                    newTransmitter.RotateZ = double.Parse(newTransmitterAntennaRotationZ_textBox5.Text);
                    newTransmitter.Power = double.Parse(newTransmitterPower_textBox11.Text);
                    client.iUpdateTransmitter(newTransmitter);
                    MessageBox.Show("辐射源已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (FaultException<WcfException> ex)
                {
                    MessageBox.Show(ex.Detail.message);
                }
                catch (CommunicationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception exm)
                {
                    LogFileManager.ObjLog.fatal(exm.Message, exm);
                }
            }
        }
        //选中天线后，波形自动加载
        private void newTransmitterAntennaName_comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (File.Exists(MainWindow.relationOfAntAndWavePath))
            {
                string antennaAndWaveStr = FileOperation.ReadFile(MainWindow.relationOfAntAndWavePath);
                string waveOfAntenna = StringFound.FoundBackStr(SetupContent.antennaIndeStr+" "+(string)newTransmitterAntennaName_comboBox3.SelectedItem+"#", antennaAndWaveStr);
                //char[] sep = { '#' };
                //string[] waveStr = waveOfAntenna.Split(sep);
                //newTransmitterWaveformName_textBox1.Text = waveStr[1];
                newTransmitterWaveformName_textBox1.Text = waveOfAntenna;

            }
        }
    }
}
