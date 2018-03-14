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
    public partial class AddTransmitterWindow : Form
    {
        TransmitterLib transLib;
        ServiceReference1.Service1Client client = null;
        ServiceReference1.Transmitter addTransimtter = null;
        ServiceReference1.Antenna matchAntenna = null;
        public AddTransmitterWindow()
        {
            InitializeComponent();
            transLib = new TransmitterLib();
            client = new ServiceReference1.Service1Client();
            addTransimtter = new ServiceReference1.Transmitter();
            if (File.Exists(MainWindow.nodeInfoFullPath))
            {
                //向窗口中添加经纬度信息
                addTransmitterLongitude_textBox2.Text = MainWindow.longitudeStr;
                addTransmitterLatitude_textBox3.Text = MainWindow.latitudeStr;


                string waveNamesOfCombox = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                addTransmitterWaveformName_comboBox1.Items.Clear();

                ArrayList waveNamesArr = new ArrayList();
                //向工程树中添加波形节点
                waveNamesArr = ProjectTreeMatchString.MatchStr(waveNamesOfCombox, SetupContent.waveIndeStr, SetupContent.waveIndeStr.Length);
                foreach (string s in waveNamesArr)
                {
                    addTransmitterWaveformName_comboBox1.Items.Add(s);
                }
            } 
        }
        private void AddTransmitterWindow_Load(object sender, EventArgs e)
        {
            //向窗口中辐射源名称中添加来自数据库的数据
            string[] TransmitterNames = new string[] { };
            try
            {
                TransmitterNames = client.iGetAllTransmitter();
                addTransmitterName_comboBox4.Items.Clear();
                foreach (string s in TransmitterNames)
                {
                    addTransmitterName_comboBox4.Items.Add(s);
                }
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
        //选中某个辐射源后将其信息加载到“添加辐射源”窗口
        private void addTransmitterName_comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addTransimtter = client.iGetTransmitter((string)addTransmitterName_comboBox4.SelectedItem);
                addTransmitterAntennaName_textBox1.Text = addTransimtter.AntennaName;
                addTransmitterAntennaRotateX_textBox7.Text = addTransimtter.RotateX.ToString();
                addTransmitterAntennaRotateY_textBox6.Text = addTransimtter.RotateY.ToString();
                addTransmitterAntennaRotateZ_textBox5.Text = addTransimtter.RotateZ.ToString();
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
        private void addTransmitterUpdate_button3_Click(object sender, EventArgs e)
        {
            try
            {
                addTransimtter.RotateX = double.Parse(addTransmitterAntennaRotateX_textBox7.Text);
                addTransimtter.RotateY = double.Parse(addTransmitterAntennaRotateY_textBox6.Text);
                addTransimtter.RotateZ = double.Parse(addTransmitterAntennaRotateZ_textBox5.Text);
                addTransimtter.Power = double.Parse(addTransmitterInputPower_textBox11.Text);
                if (addTransmitterName_comboBox4.SelectedItem == null || addTransmitterCoordinateSystem_comboBox1.SelectedItem == null || addTransmitterLongitude_textBox2.Text == "" || addTransmitterLatitude_textBox3.Text == "" || addTransimtterReferencePlane_comboBox2.SelectedItem == null || addTransmitterInputPower_textBox11.Text == "" || addTransmitterAntennaName_textBox1.Text == "" || addTransmitterWaveformName_comboBox1.SelectedItem == null || addTransmitterAntennaRotateX_textBox7.Text == "" || addTransmitterAntennaRotateY_textBox6.Text == "" || addTransmitterAntennaRotateZ_textBox5.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addTransmitterInputPower_textBox11.Text))
                {
                    MessageBox.Show("发射功率值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addTransmitterInputPower_textBox11.Text) < 0.000)
                {
                    MessageBox.Show("发射功率值需大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateX_textBox7.Text))
                {
                    MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateY_textBox6.Text))
                {
                    MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateZ_textBox5.Text))
                {
                    MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateX_textBox7))
                {
                    MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateY_textBox6))
                {
                    MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateZ_textBox5))
                {
                    MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateTransmitter(addTransimtter);
                MessageBox.Show("辐射源已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addTransmitterOk_button1_Click(object sender, EventArgs e)
        {
            client = new ServiceReference1.Service1Client();
            matchAntenna = new ServiceReference1.Antenna();
            string matchAntennaName = null;
            if (MainWindow.mProjectFullName == null)
            {
                this.Close();
                return;
            }
            //判断添加辐射源之前是否添加了地形
            if (!ConditionIsAll(MainWindow.nodeInfoFullPath))
            {
                MessageBox.Show("请先添加地形之后再添加辐射源!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            //判断工程波形中是否添加波形
            if (addTransmitterWaveformName_comboBox1.Items.Count >0)
            {
                if (addTransmitterWaveformName_comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("辐射源未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
               
            }
            else
            {
                MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (addTransmitterName_comboBox4.SelectedItem == null || addTransmitterCoordinateSystem_comboBox1.SelectedItem == null || addTransmitterLongitude_textBox2.Text == "" || addTransmitterLatitude_textBox3.Text == "" || addTransimtterReferencePlane_comboBox2.SelectedItem == null || addTransmitterInputPower_textBox11.Text == "" || addTransmitterAntennaName_textBox1.Text == "" || addTransmitterWaveformName_comboBox1.SelectedItem == null || addTransmitterAntennaRotateX_textBox7.Text == "" || addTransmitterAntennaRotateY_textBox6.Text == "" || addTransmitterAntennaRotateZ_textBox5.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(addTransmitterInputPower_textBox11.Text))
            {
                MessageBox.Show("发射功率值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(addTransmitterInputPower_textBox11.Text) < 0.000)
            {
                MessageBox.Show("发射功率值需大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateX_textBox7.Text))
            {
                MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateY_textBox6.Text))
            {
                MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(addTransmitterAntennaRotateZ_textBox5.Text))
            {
                MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateX_textBox7))
            {
                MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateY_textBox6))
            {
                MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.RotationLimition(addTransmitterAntennaRotateZ_textBox5))
            {
                MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            //判断是否存在.tx文件
            if (File.Exists(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx"))
            {
                WaveformWriting transm = new WaveformWriting(FileOperation.ReadFile(MainWindow.nodeInfoFullPath));
                //注意 新建的辐射源个数不能超过1000
                string[] transmitterNames = new string[1000];
                //判断是否存在重名的辐射源
                transmitterNames = transm.waveformNames(SetupContent.transmitterIndeStr);
                if (transm.judge((string)addTransmitterName_comboBox4.SelectedItem, transmitterNames))
                {
                    MessageBox.Show("此辐射源已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //将数据库中与辐射源匹配的天线写入setupStr中
            try
            {
                matchAntenna = client.iGetAntenna(addTransmitterAntennaName_textBox1.Text);
                //与辐射源匹配的天线名称
                matchAntennaName = matchAntenna.Name + "_DB" + (string)addTransmitterName_comboBox4.SelectedItem;

                //准备好插入的内容
                string[] AntennaStr = new string[8];
                AntennaStr[0] = SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n";
                AntennaStr[1] = matchAntenna.Type + "\r\n";
                AntennaStr[3] = "power_threshold " + matchAntenna.RecieveThrehold + "\r\n";
                AntennaStr[4] = "cable_loss " + matchAntenna.TransmissionLoss + "\r\n";
                AntennaStr[5] = "VSWR " + matchAntenna.VSWR + "\r\n";
                AntennaStr[6] = "temperature " + matchAntenna.Temperature + "\r\n";
                switch (matchAntenna.Type)
                {
                    case "type HalfWaveDipole":
                        {
                            
                                AntennaStr[2] = "polarization " + matchAntenna.Polarization + "\r\n";
                                AntennaStr[7] = SetupContent.antennaStr2 + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                                //将天线信息存储到.waveinfo文件中
                                string antennaInfoStr = SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n"
                                                                       +Translate.KeyWordsDictionary_DB( matchAntenna.Type) + "\r\n"
                                                                        //+ (string)addTransmitterWaveformName_comboBox1.SelectedItem + "\r\n"
                                                                        + matchAntenna.MaxGain + "\r\n"
                                                                        + matchAntenna.Polarization + "\r\n"                                    
                                                                        + matchAntenna.RecieveThrehold + "\r\n"
                                                                        + matchAntenna.TransmissionLoss + "\r\n"
                                                                        + matchAntenna.VSWR + "\r\n"
                                                                        + matchAntenna.Temperature + "\r\n"
                                                                        + "END" + SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n";
                                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
                        }
                        break;
                    case "type linear_monopole":
                        {
                            
                                AntennaStr[2] = "";
                                AntennaStr[7] = SetupContent.antennaStr2 + "\r\n" + "length " + matchAntenna.Length + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                                //将单极天线的参数按相应的顺序记录到waveinfo文件中
                                string antennaInfoStr = SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n"
                                                                  +Translate.KeyWordsDictionary_DB( matchAntenna.Type) + "\r\n"
                                                                   + (string)addTransmitterWaveformName_comboBox1.SelectedItem + "\r\n"
                                                                  + matchAntenna.Length+ "\r\n"
                                                                  + matchAntenna.MaxGain + "\r\n"
                                                                  + matchAntenna.RecieveThrehold + "\r\n"
                                                                  + matchAntenna.TransmissionLoss + "\r\n"
                                                                  + matchAntenna.VSWR + "\r\n"
                                                                  + matchAntenna.Temperature + "\r\n"
                                                                  + "END" + SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n";
                                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
                        }
                        break;
                    case "type Helical":
                        {
                            
                                AntennaStr[2] = "polarization " + matchAntenna.Polarization + "\r\n";
                                AntennaStr[7] = SetupContent.antennaStr2 + "\r\n"
                                                    + "radius " + matchAntenna.Radius + "\r\n"
                                                    + "length " + matchAntenna.Length + "\r\n"
                                                    + "pitch " + matchAntenna.Pitch + "\r\n"
                                                    + SetupContent.antennaStr3 + "\r\n";

                                string antennaInfoStr = SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n"
                                                         +Translate.KeyWordsDictionary_DB( matchAntenna.Type)+ "\r\n"
                                                           + (string)addTransmitterWaveformName_comboBox1.SelectedItem + "\r\n"
                                                         + matchAntenna.Polarization + "\r\n"
                                                         + matchAntenna.MaxGain+ "\r\n"
                                                         + matchAntenna.Radius + "\r\n"
                                                         + matchAntenna.Length + "\r\n"
                                                         + matchAntenna.Pitch + "\r\n"
                                                         + matchAntenna.RecieveThrehold + "\r\n"
                                                         + matchAntenna.TransmissionLoss + "\r\n"
                                                         + matchAntenna.VSWR + "\r\n"
                                                         + matchAntenna.Temperature + "\r\n"
                                                         + "END" + SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n";
                                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
                        }
                        break;
                    case "type ParabolicReflector":
                        {
                           
                                AntennaStr[2] = "polarization " + matchAntenna.Polarization + "\r\n";
                                AntennaStr[7] = SetupContent.antennaStr2 + "\r\n"
                                                        + "radius " + matchAntenna.Radius + "\r\n"
                                                        + "blockageradius " + matchAntenna.BlockageRadius + "\r\n"
                                                        + "EFieldDistribution " + matchAntenna.ApertureDistribution + "\r\n"
                                                        + "EdgeTaper " + matchAntenna.EdgeTeper + "\r\n"
                                                        + SetupContent.antennaStr3 + "\r\n";
                                //将抛物线天线相应的参数按控件顺序记录到waveinfo文件中
                                string antennaInfoStr = SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n"
                                                                +Translate.KeyWordsDictionary_DB( matchAntenna.Type) + "\r\n"
                                                                + (string)addTransmitterWaveformName_comboBox1.SelectedItem + "\r\n"
                                                                + matchAntenna.Polarization+ "\r\n"
                                                                + matchAntenna.Radius + "\r\n"
                                                                + matchAntenna.BlockageRadius + "\r\n"
                                                                + matchAntenna.ApertureDistribution + "\r\n"
                                                                + matchAntenna.EdgeTeper + "\r\n"
                                                                + matchAntenna.RecieveThrehold + "\r\n"
                                                                + matchAntenna.TransmissionLoss+ "\r\n"
                                                                + matchAntenna.VSWR + "\r\n"
                                                                +matchAntenna.Temperature + "\r\n"
                                                                + "END" + SetupContent.antennaStr1 + " " + matchAntennaName + "\r\n";
                                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
                        }
                        break;
                    case "对数周期天线":
                        {

                        }
                        break;
                    default:
                        break;
                }
                WaveformWriting Annt = new WaveformWriting(MainWindow.setupStr);
                MainWindow.setupStr = Annt.InsertAntenna8(AntennaStr, (string)addTransmitterWaveformName_comboBox1.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                //将与辐射源匹配的天线添加到工程树子节点
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(matchAntennaName);
                //将天线的信息存储到.setup.info文件中
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + matchAntennaName, MainWindow.nodeInfoFullPath, true);
                
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

            string transmitterCount = GetTransmitterNum(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx", SetupContent.transmitterStr2OfTr);
            string sourceStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);

            string trFileStr = SetupContent.transmitterStr1OfTr + " " + (string)addTransmitterName_comboBox4.SelectedItem + "\r\n"
                                    + SetupContent.transmitterStr2OfTr + transmitterCount + "\r\n"
                                    + SetupContent.transmitterStr3OfTr + "\r\n" + SetupContent.transmitterStr4OfTr + "\r\n"
                                    + SetupContent.transmitterStr5OfTr + "\r\n" + SetupContent.transmitterStr6OfTr + "\r\n"
                                    + SetupContent.transmitterStr7OfTr + "\r\n" + SetupContent.transmitterStr8OfTr + "\r\n"
                                    + SetupContent.transmitterStr9OfTr + "\r\n" + SetupContent.transmitterStr10OfTr + "\r\n"
                                    + SetupContent.transmitterStr11OfTr + "\r\n"
                                    + SetupContent.transmitterStr12OfTr + MainWindow.longitudeStr + "\r\n"
                                    + SetupContent.transmitterStr13OfTr + MainWindow.latitudeStr + "\r\n"
                                    + SetupContent.transmitterStr14OfTr + "\r\n"
                                    + Translate.KeyWordsDictionary(addTransimtterReferencePlane_comboBox2) + "\r\n"
                                    + SetupContent.transmitterStr15OfTr + "\r\n"
                                    + SetupContent.transmitterStr16OfTr + "\r\n"
                //+ SetupContent.transmitterStr17OfTr + "\r\n"
                                    + GetAntennaStr(MainWindow.transInfoFullPath, matchAntennaName)
                                    + SetupContent.transmitterStr18OfTr + addTransmitterAntennaRotateX_textBox7.Text + "\r\n"
                                    + SetupContent.transmitterStr19OfTr + addTransmitterAntennaRotateY_textBox6.Text + "\r\n"
                                    + SetupContent.transmitterStr20OfTr + addTransmitterAntennaRotateZ_textBox5.Text + "\r\n"
                                    + SetupContent.transmitterStr21OfTr + addTransmitterInputPower_textBox11.Text + "\r\n"
                                    + SetupContent.transmitterStr22OfTr + "\r\n"
                                    + SetupContent.transmitterStr23OfTr + "\r\n"
                                    + SetupContent.transmitterStr24OfTr + " "
            +WaveformNodeOfConMenu.GetWaveformNumLine((string)addTransmitterWaveformName_comboBox1.SelectedItem, sourceStr) + "MHZ" + "\r\n";
            FileOperation.WriteFile(trFileStr, MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx", true);
            //写到setup文件中
            string sourceStrOfTr = FileOperation.ReadFile(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx");
            //找出辐射源的个数
            int transmitCount = WaveformWriting.GetCountOfMatchStr("begin_<points>", sourceStrOfTr);
            //如果已经存在了transmitter块，则先删除
            if (MainWindow.setupStr.LastIndexOf("end_<transmitter>") != -1)
            {
                MainWindow.setupStr = MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<transmitter>"), MainWindow.setupStr.IndexOf("end_<transmitter>") - MainWindow.setupStr.IndexOf("begin_<transmitter>") + 19);
            }
            //将.tx路径信息插入到全局字符串setupStr中
            int insertSiteOfTr = MainWindow.setupStr.LastIndexOf("end_<feature>") + 15;//15是end_<feature>\r\n后的第一个字符
            string insertStr = SetupContent.transmitterStr1Ofsetup + "\r\n" + SetupContent.transmitterStr2Ofsetup
                                    + MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx" + "\r\n"
                                    + SetupContent.transmitterStr3Ofsetup + transmitCount.ToString() + "\r\n"
                                    + SetupContent.transmitterStr4Ofsetup + "\r\n";
            MainWindow.setupStr = MainWindow.setupStr.Insert(insertSiteOfTr, insertStr);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);





            //向工程树中添加结点信息
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3].Nodes.Add((string)addTransmitterName_comboBox4.SelectedItem);
            //将辐射源的信息存储到.setup.info文件中
            FileOperation.WriteLineFile(SetupContent.transmitterIndeStr + " " + (string)addTransmitterName_comboBox4.SelectedItem, MainWindow.nodeInfoFullPath, true);
            //将辐射源和天线的匹配关系记录到.match文件中
            FileOperation.WriteLineFile(SetupContent.transmitterIndeStr + " " + (string)addTransmitterName_comboBox4.SelectedItem + "*" + matchAntennaName, MainWindow.relationOfAntAndWavePath, true);

            //将辐射源的信息存储到.waveinfo文件中
            string transmitterInfoStr = SetupContent.transmitterStr1Ofsetup + " " + (string)addTransmitterName_comboBox4.SelectedItem + "\r\n"
                                                    + addTransimtterReferencePlane_comboBox2.Text + "\r\n"
                                                    + addTransmitterInputPower_textBox11.Text + "\r\n"
                                                    + matchAntennaName + "\r\n"
                                                    + (string)addTransmitterWaveformName_comboBox1.SelectedItem + "\r\n"
                                                    + addTransmitterAntennaRotateX_textBox7.Text + "\r\n"
                                                    + addTransmitterAntennaRotateY_textBox6.Text + "\r\n"
                                                    + addTransmitterAntennaRotateZ_textBox5.Text + "\r\n"
                                                    + "END" + SetupContent.transmitterStr1Ofsetup + " " + (string)addTransmitterName_comboBox4.SelectedItem + "\r\n";
            FileOperation.WriteFile(transmitterInfoStr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("辐射源" + (string)addTransmitterName_comboBox4.SelectedItem + "创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
     // 判断在添加辐射源之前是否添加了地形
        public static bool ConditionIsAll(string path)
         {
            string str = FileOperation.ReadFile(path);
            if ((str.IndexOf("<terrain>") != -1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       // 返回TxSet后面的编号
        public static string GetTransmitterNum(string path, string indexStr)
        {

            if (!File.Exists(path))
            {
                return "1";
            }
            else
            {
                string trConStr = FileOperation.ReadFile(path);
                string transmitterStr = null;
                int transmitterSite = trConStr.LastIndexOf("\n" + indexStr) + indexStr.Length + 1;//7是\nTxSet (还有一空格）的长度
                while (trConStr[transmitterSite] != '\r')
                    transmitterStr = transmitterStr + trConStr[transmitterSite++];
                int b = int.Parse(transmitterStr) + 1;
                transmitterStr = string.Format("{0:##}", b);
                return transmitterStr;
            }
        }
        //写tr文件中有关天线的那一段
        public static string GetAntennaStr(string path, string antennaname)
        {
            string antennaOfTrStr = FileOperation.ReadFile(path);
            string startStr = SetupContent.antennaStr1 + " " + antennaname + "\r\n";
            string endStr = SetupContent.antennaStr3 + " " + antennaname + "\r\n";
            int startSite = antennaOfTrStr.LastIndexOf(startStr);
            int endSite = antennaOfTrStr.LastIndexOf(endStr);
            antennaOfTrStr = antennaOfTrStr.Substring(startSite, endSite - startSite);
            return antennaOfTrStr;
            //string antennaOfTrStr = FileOperation.ReadFile(path);
            //string startStr = "begin_<antenna> " +antennaname;
            //int startSite = antennaOfTrStr.IndexOf(startStr) + startStr.Length + 2;
            //int endSite = antennaOfTrStr.IndexOf("ENDbegin_<antenna> " + antennaname);
            //antennaOfTrStr = antennaOfTrStr.Substring(startSite, endSite - startSite);
            //return antennaOfTrStr;
        }
        private void addTransmitterClose_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }         
    }
}
