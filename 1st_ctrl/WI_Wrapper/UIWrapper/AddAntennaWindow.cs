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
using WcfService;
using System.ServiceModel;
using System.Data.SqlClient;
using LogFileManager;

namespace WF1
{
    public partial class AddAntennaWindow : Form
    {
        TransmitterLib transLib;
        ServiceReference1.Service1Client client = null;
        ServiceReference1.Antenna addAntenna = null;
        public AddAntennaWindow()
        {
            InitializeComponent();
            transLib = new TransmitterLib();
            client = new ServiceReference1.Service1Client();
            addAntenna = new ServiceReference1.Antenna();
            if (System.IO.File.Exists(MainWindow.nodeInfoFullPath))
            {
                string waveNamesOfCombox = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                addDipoleWaveformName_comboBox1.Items.Clear();
                addMonolopeWaveformName_comboBox1.Items.Clear();
                addParabolicWaveformName_comboBox1.Items.Clear();
                addHelicalWaveformName_comboBox1.Items.Clear();
                addLogperiodicWaveformName_comboBox1.Items.Clear();

                ArrayList waveNamesArr = new ArrayList();
                //向工程树中添加波形节点
                waveNamesArr = ProjectTreeMatchString.MatchStr(waveNamesOfCombox, SetupContent.waveIndeStr, SetupContent.waveIndeStr.Length);
                foreach (string s in waveNamesArr)
                {
                    addDipoleWaveformName_comboBox1.Items.Add(s);
                    addMonolopeWaveformName_comboBox1.Items.Add(s);
                    addParabolicWaveformName_comboBox1.Items.Add(s);
                    addHelicalWaveformName_comboBox1.Items.Add(s);
                    addLogperiodicWaveformName_comboBox1.Items.Add(s);
                }
            }
        }

        private void AddAntennaWindow_Load(object sender, EventArgs e)
        {
            
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (MainWindow.nodeInfoFullPath != null)
            {
                string AntennaType = null;
                AntennaType = (string)addAntennaType_comboBox1.SelectedItem;
                switch (AntennaType)
                {
                    case "偶极子天线<Short dipole>":
                        {
                            this.addDipoleAntenna_panel1.Visible = true;
                            this.addMonopole_panel2.Visible = false;
                            this.addParabolic_panel3.Visible = false;
                            this.addHelical_panel4.Visible = false;
                            this.addLogperiodic_panel5.Visible = false;
                            //在选中偶极子类型后，获取数据库中Antenna表偶极子类天线的名称，在天线名称组合框的下拉列表显示。
                            string[] AntennaNames = new string[] { };
                            try
                            {
                                AntennaNames = client.iGetAllAntenna(Translate.KeyWordsDictionary(addAntennaType_comboBox1));
                                addDipoleAntennaName_comboBox9.Items.Clear();
                                foreach (string s in AntennaNames)
                                {
                                    addDipoleAntennaName_comboBox9.Items.Add(s);
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
                        break;
                    case "单极天线<Short monopole>":
                        {
                            this.addMonopole_panel2.Visible = true;
                            this.addDipoleAntenna_panel1.Visible = false;
                            this.addParabolic_panel3.Visible = false;
                            this.addHelical_panel4.Visible = false;
                            this.addLogperiodic_panel5.Visible = false;
                            //在选中单极类型后，获取数据库中Antenna表单极类天线的名称，在天线名称组合框的下拉列表显示。
                            string[] AntennaNames = new string[] { };
                            try
                            {
                                AntennaNames = client.iGetAllAntenna(Translate.KeyWordsDictionary(addAntennaType_comboBox1));
                                addMonopoleAntennaName_comboBox10.Items.Clear();
                                foreach (string s in AntennaNames)
                                {
                                    addMonopoleAntennaName_comboBox10.Items.Add(s);
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
                        break;
                    case "抛物面反射天线<Parabolic reflector>":
                        {
                            this.addParabolic_panel3.Visible = true;
                            this.addDipoleAntenna_panel1.Visible = false;
                            this.addMonopole_panel2.Visible = false;
                            this.addHelical_panel4.Visible = false;
                            this.addLogperiodic_panel5.Visible = false;
                            //在选中抛物面类型后，获取数据库中Antenna表抛物面类天线的名称，在天线名称组合框的下拉列表显示。
                            string[] AntennaNames = new string[] { };
                            try
                            {
                                AntennaNames = client.iGetAllAntenna(Translate.KeyWordsDictionary(addAntennaType_comboBox1));
                                addParabolicAntennaName_comboBox5.Items.Clear();
                                foreach (string s in AntennaNames)
                                {
                                    addParabolicAntennaName_comboBox5.Items.Add(s);
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
                        break;
                    case "螺旋天线<Helical>":
                        {
                            this.addHelical_panel4.Visible = true;
                            this.addDipoleAntenna_panel1.Visible = false;
                            this.addMonopole_panel2.Visible = false;
                            this.addParabolic_panel3.Visible = false;
                            this.addLogperiodic_panel5.Visible = false;
                            //在选中抛物面类型后，获取数据库中Antenna表抛物面类天线的名称，在天线名称组合框的下拉列表显示。
                            string[] AntennaNames = new string[] { };
                            try
                            {
                                AntennaNames = client.iGetAllAntenna(Translate.KeyWordsDictionary(addAntennaType_comboBox1));
                                addHelicalAntennaName_comboBox7.Items.Clear();
                                foreach (string s in AntennaNames)
                                {
                                    addHelicalAntennaName_comboBox7.Items.Add(s);
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
                        break;
                    case "对数周期天线<Log-periodical>":
                        {
                            this.addLogperiodic_panel5.Visible = true;
                            this.addDipoleAntenna_panel1.Visible = false;
                            this.addMonopole_panel2.Visible = false;
                            this.addParabolic_panel3.Visible = false;
                            this.addHelical_panel4.Visible = false;
                            //在选中对数周期类型后，获取数据库中Antenna表对数周期类天线的名称，在天线名称组合框的下拉列表显示。
                            string[] AntennaNames = new string[] { };
                            try
                            {
                                AntennaNames = null;
                                AntennaNames = client.iGetAllAntenna(Translate.KeyWordsDictionary(addAntennaType_comboBox1));
                                addLogperiodicAntennaName_comboBox8.Items.Clear();
                                foreach (string s in AntennaNames)
                                {
                                    addLogperiodicAntennaName_comboBox8.Items.Add(s);
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
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("请先新建个工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }        
        private void addDipoleAntennaName_comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                string AntennaPolarization1 = null;
                string AntennaPolarizationTranslate1 = null;
                AntennaPolarization1 = addAntenna.Polarization;
                if (AntennaPolarization1 == "vertical") AntennaPolarizationTranslate1 = "水平极化<Horizontal>";
                else AntennaPolarizationTranslate1 = "垂直极化<Vertical>";
                addAntenna = client.iGetAntenna((string)addDipoleAntennaName_comboBox9.SelectedItem);
                //addDipoleWaveformName_textBox18.Text = addAntenna.WaveFormName;
                //addDipoleMaxGain_textBox2.Text = addAntenna.MaxGain.ToString();
                addDipolePolarization_textBox1.Text = AntennaPolarizationTranslate1;
                addDipoleReceiverThreshold_textBox3.Text = addAntenna.RecieveThrehold.ToString();
                addDipoleTranmissionLineLoss_textBox4.Text = addAntenna.TransmissionLoss.ToString();
                addDipoleVSWR_textBox5.Text = addAntenna.VSWR.ToString();
                addDipoleTemperature_textBox6.Text = addAntenna.Temperature.ToString();
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
        private void addDipoleUpdate_button11_Click(object sender, EventArgs e)
        {
            try
            {
                //string AntennaPolarization2 = null;
                //string AntennaPolarizationTranslate2 = null;
                //AntennaPolarization2 = addDipolePolarization_textBox1.Text;
                //if (AntennaPolarization2 == "水平极化<Horizontal>") AntennaPolarizationTranslate2 = "vertical";
                //else AntennaPolarizationTranslate2 = "horizontal";
                addAntenna.Name = (string)addDipoleAntennaName_comboBox9.SelectedItem;
                //addAntenna.MaxGain = double.Parse(addDipoleMaxGain_textBox2.Text);
                //addAntenna.Polarization = AntennaPolarizationTranslate2;
                addAntenna.Polarization = Translate.KeyWordsDictionary_DB(addDipolePolarization_textBox1.Text);
                addAntenna.RecieveThrehold = double.Parse(addDipoleReceiverThreshold_textBox3.Text);
                addAntenna.TransmissionLoss = double.Parse(addDipoleTranmissionLineLoss_textBox4.Text);
                addAntenna.VSWR = double.Parse(addDipoleVSWR_textBox5.Text);
                addAntenna.Temperature = double.Parse(addDipoleTemperature_textBox6.Text);
                if (addDipoleAntennaName_comboBox9.SelectedItem == null || addDipoleWaveformName_comboBox1.SelectedItem == null || addDipolePolarization_textBox1.Text == "" || addDipoleReceiverThreshold_textBox3.Text == "" || addDipoleTranmissionLineLoss_textBox4.Text == "" || addDipoleVSWR_textBox5.Text == "" || addDipoleTemperature_textBox6.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleReceiverThreshold_textBox3.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleTranmissionLineLoss_textBox4.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleVSWR_textBox5.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addDipoleVSWR_textBox5))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleTemperature_textBox6.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateAntenna(addAntenna);
                MessageBox.Show("天线已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addDipoleOk_button1_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                if (addDipoleWaveformName_comboBox1.Items.Count > 0)
                {
                    if (addDipoleWaveformName_comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (addDipoleAntennaName_comboBox9.SelectedItem == null || addDipoleWaveformName_comboBox1.SelectedItem == null || addDipolePolarization_textBox1.Text == "" || addDipoleReceiverThreshold_textBox3.Text == "" || addDipoleTranmissionLineLoss_textBox4.Text == "" || addDipoleVSWR_textBox5.Text == "" || addDipoleTemperature_textBox6.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleReceiverThreshold_textBox3.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleTranmissionLineLoss_textBox4.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleVSWR_textBox5.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addDipoleVSWR_textBox5))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addDipoleTemperature_textBox6.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }

                //准备好插入的内容
                string[] dipoleStr = new string[8];
                //string AntennaPolarization2 = null;
                //string AntennaPolarizationTranslate2 = null;
                //AntennaPolarization2 = addDipolePolarization_textBox1.Text;
                //if (AntennaPolarization2 == "水平极化<Horizontal>") AntennaPolarizationTranslate2 = "vertical";
                //else AntennaPolarizationTranslate2 = "horizontal";
                dipoleStr[0] = SetupContent.antennaStr1 + " " + (string)addDipoleAntennaName_comboBox9.SelectedItem + "\r\n";
                dipoleStr[1] = Translate.KeyWordsDictionary(addAntennaType_comboBox1) + "\r\n";
                dipoleStr[2] = "polarization " + Translate.KeyWordsDictionary_DB(addDipolePolarization_textBox1.Text) + "\r\n";
                dipoleStr[3] = "power_threshold " + addDipoleReceiverThreshold_textBox3.Text + "\r\n";
                dipoleStr[4] = "cable_loss " + addDipoleTranmissionLineLoss_textBox4.Text + "\r\n";
                dipoleStr[5] = "VSWR " + addDipoleVSWR_textBox5.Text + "\r\n";
                dipoleStr[6] = "temperature " + addDipoleTemperature_textBox6.Text + "\r\n";
                dipoleStr[7] = SetupContent.antennaStr2 + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                //注意 新建的天线个数不能超过100
                string[] antennaNames = new string[100];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge((string)addDipoleAntennaName_comboBox9.SelectedItem, antennaNames))
                {
                    MessageBox.Show("此天线已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MainWindow.setupStr = an.InsertAntenna8(dipoleStr, (string)addDipoleWaveformName_comboBox1.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add((string)addDipoleAntennaName_comboBox9.SelectedItem);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + (string)addDipoleAntennaName_comboBox9.SelectedItem, MainWindow.nodeInfoFullPath, true);

                //将偶极子天线的参数按相应的顺序记录到waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + (string)addDipoleAntennaName_comboBox9.SelectedItem + "\r\n"
                                                    + (string)addAntennaType_comboBox1.SelectedItem + "\r\n"
                    //+ (string)newdipolewaveformname_comboBox2.SelectedItem +"\r\n"
                                                    + (string)addDipoleWaveformName_comboBox1.SelectedItem + "\r\n"
                                                    + addDipoleMaxGain_textBox2.Text + "\r\n"
                                                    + addDipolePolarization_textBox1.Text + "\r\n"
                    //+ newdipolepolarization_comboBox3.Text  + "\r\n"
                                                    + addDipoleReceiverThreshold_textBox3.Text + "\r\n"
                                                    + addDipoleTranmissionLineLoss_textBox4.Text + "\r\n"
                                                    + addDipoleVSWR_textBox5.Text + "\r\n"
                                                    + addDipoleTemperature_textBox6.Text + "\r\n"
                                                    + "END" + SetupContent.antennaStr1 + " " + (string)addDipoleAntennaName_comboBox9.SelectedItem + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + (string)addDipoleAntennaName_comboBox9.SelectedItem + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void addDipoleCancle_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }  
        private void addMonopoleAntennaName_comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addAntenna = client.iGetAntenna((string)addMonopoleAntennaName_comboBox10.SelectedItem);
                //addMonopoleWaveformName_textBox1.Text = addAntenna.WaveFormName;
                addMonolopeLengthtextBox1.Text = addAntenna.Length.ToString();
                //addMonopoleMaxGain_textBox17.Text = addAntenna.MaxGain.ToString();
                addMonopoleReceiverThreshold_textBox16.Text = addAntenna.RecieveThrehold.ToString();
                addMonopoleTransmissionLineLoss_textBox15.Text = addAntenna.TransmissionLoss.ToString();
                addMonopoleVSWR_textBox14.Text = addAntenna.VSWR.ToString();
                addMonopoleTempertature_textBox13.Text = addAntenna.Temperature.ToString();
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
        private void addMonopoleUpdate_button12_Click(object sender, EventArgs e)
        {
            try
            {
                addAntenna.Name = (string)addMonopoleAntennaName_comboBox10.SelectedItem;
                //addAntenna.WaveFormName = addMonopoleWaveformName_textBox1.Text;
                addAntenna.Length =double.Parse( addMonolopeLengthtextBox1.Text);
                //addAntenna.MaxGain = double.Parse(addMonopoleMaxGain_textBox17.Text);
                addAntenna.RecieveThrehold = double.Parse(addMonopoleReceiverThreshold_textBox16.Text);
                addAntenna.TransmissionLoss = double.Parse(addMonopoleTransmissionLineLoss_textBox15.Text);
                addAntenna.VSWR = double.Parse(addMonopoleVSWR_textBox14.Text);
                addAntenna.Temperature = double.Parse(addMonopoleTempertature_textBox13.Text);
                if (addMonopoleAntennaName_comboBox10.SelectedItem == null || addMonolopeWaveformName_comboBox1.SelectedItem == null || addMonolopeLengthtextBox1.Text == "" || addMonopoleReceiverThreshold_textBox16.Text == "" || addMonopoleTransmissionLineLoss_textBox15.Text == "" || addMonopoleVSWR_textBox14.Text == "" || addMonopoleTempertature_textBox13.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonolopeLengthtextBox1.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addMonolopeLengthtextBox1.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.0001", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleReceiverThreshold_textBox16.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleTransmissionLineLoss_textBox15.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleVSWR_textBox14.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addMonopoleVSWR_textBox14))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleTempertature_textBox13.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateAntenna(addAntenna);
                MessageBox.Show("天线已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addMonopoleOk_button3_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                if (addMonolopeWaveformName_comboBox1.Items.Count > 0)
                {
                    if (addMonolopeWaveformName_comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (addMonopoleAntennaName_comboBox10.SelectedItem == null || addMonolopeWaveformName_comboBox1.SelectedItem == null || addMonolopeLengthtextBox1.Text == "" || addMonopoleReceiverThreshold_textBox16.Text == "" || addMonopoleTransmissionLineLoss_textBox15.Text == "" || addMonopoleVSWR_textBox14.Text == "" || addMonopoleTempertature_textBox13.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonolopeLengthtextBox1.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addMonolopeLengthtextBox1.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.0001", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleReceiverThreshold_textBox16.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleTransmissionLineLoss_textBox15.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleVSWR_textBox14.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addMonopoleVSWR_textBox14))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addMonopoleTempertature_textBox13.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] monopoleStr = new string[8];
                monopoleStr[0] = SetupContent.antennaStr1 + " " + (string)addMonopoleAntennaName_comboBox10.SelectedItem + "\r\n";
                monopoleStr[1] = Translate.KeyWordsDictionary(addAntennaType_comboBox1) + "\r\n";
                monopoleStr[2] = "";
                monopoleStr[3] = "power_threshold " + addMonopoleReceiverThreshold_textBox16.Text + "\r\n";
                monopoleStr[4] = "cable_loss " + addMonopoleTransmissionLineLoss_textBox15.Text + "\r\n";
                monopoleStr[5] = "VSWR " + addMonopoleVSWR_textBox14.Text + "\r\n";
                monopoleStr[6] = "temperature " + addMonopoleTempertature_textBox13.Text + "\r\n";
                monopoleStr[7] = SetupContent.antennaStr2 + "\r\n" + "length " + addMonolopeLengthtextBox1.Text + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge((string)addMonopoleAntennaName_comboBox10.SelectedItem, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MainWindow.setupStr = an.InsertAntenna8(monopoleStr, (string)addMonolopeWaveformName_comboBox1.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add((string)addMonopoleAntennaName_comboBox10.SelectedItem);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + (string)addMonopoleAntennaName_comboBox10.SelectedItem, MainWindow.nodeInfoFullPath, true);
               
                //将单极天线的参数按相应的顺序记录到waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + (string)addMonopoleAntennaName_comboBox10.SelectedItem + "\r\n"
                                                  + (string)addAntennaType_comboBox1.SelectedItem + "\r\n"             
                                                  + (string)addMonolopeWaveformName_comboBox1.SelectedItem + "\r\n"
                                                  + addMonolopeLengthtextBox1.Text + "\r\n"
                                                  + addMonopoleMaxGain_textBox17.Text + "\r\n"
                                                  + addMonopoleReceiverThreshold_textBox16.Text + "\r\n"
                                                  + addMonopoleTransmissionLineLoss_textBox15.Text + "\r\n"
                                                  + addMonopoleVSWR_textBox14.Text + "\r\n"
                                                  + addMonopoleTempertature_textBox13.Text + "\r\n"
                                                  + "END" + SetupContent.antennaStr1 + " " + (string)addMonopoleAntennaName_comboBox10.SelectedItem + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + (string)addMonopoleAntennaName_comboBox10.SelectedItem + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void addMonopoleCancel_button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addParabolicAntennaName_comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ParabolicPolarization = null;
                string ParabolicPolarizationTranslate = null;
                addAntenna = client.iGetAntenna((string)addParabolicAntennaName_comboBox5.SelectedItem);
                ParabolicPolarization = addAntenna.Polarization;
                if (ParabolicPolarization == "horizontal")
                    ParabolicPolarizationTranslate = "水平极化<Horizontal>";
                else            
                    ParabolicPolarizationTranslate = "垂直极化<Vertical>";                
                //addParabolicWaveformName_textBox12.Text = addAntenna.WaveFormName;
                addParabolicPolarization_textBox1.Text = ParabolicPolarizationTranslate;
                
                addParabolicRadius_textBox9.Text = addAntenna.Radius.ToString();
                addParabolicBlockageRadius_textBox10.Text = addAntenna.BlockageRadius.ToString();
                addParabolicApertureDistribution_textBox1.Text = addAntenna.ApertureDistribution;
                addParabolicEdgeTaper_textBox8.Text = addAntenna.EdgeTeper.ToString();
                addParabolicReceiverThreshold_textBox7.Text = addAntenna.RecieveThrehold.ToString();
                addParabolicTransmissionLineLoss_textBox19.Text = addAntenna.TransmissionLoss.ToString();
                addParabolicVSWR_textBox21.Text = addAntenna.VSWR.ToString();
                addParabolicTemperature_textBox20.Text = addAntenna.Temperature.ToString();
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
        private void addParabolicUpdate_button13_Click(object sender, EventArgs e)
        {
            try
            {
                //string ParabolicPolarization1 = null;
                //string ParabolicPolarizationTranslate1 = null;
                //ParabolicPolarization1 = addParabolicPolarization_textBox1.Text;
                //if (ParabolicPolarization1 == "水平极化<Horizontal>") ParabolicPolarizationTranslate1 = "horizontal";
                //else ParabolicPolarizationTranslate1 = "vertical";
                addAntenna.Name = (string)addParabolicAntennaName_comboBox5.SelectedItem;
                //addAntenna.Polarization = ParabolicPolarizationTranslate1;
                addAntenna.Polarization = Translate.KeyWordsDictionary_DB(addParabolicPolarization_textBox1.Text);
               
                addAntenna.Radius = double.Parse(addParabolicRadius_textBox9.Text);
                addAntenna.BlockageRadius = double.Parse(addParabolicBlockageRadius_textBox10.Text);
                addAntenna.ApertureDistribution = addParabolicApertureDistribution_textBox1.Text;
                addAntenna.EdgeTeper = double.Parse(addParabolicEdgeTaper_textBox8.Text);
                addAntenna.RecieveThrehold = double.Parse(addParabolicReceiverThreshold_textBox7.Text);
                addAntenna.TransmissionLoss = double.Parse(addParabolicTransmissionLineLoss_textBox19.Text);
                addAntenna.VSWR = double.Parse(addParabolicVSWR_textBox21.Text);
                addAntenna.Temperature = double.Parse(addParabolicTemperature_textBox20.Text);
                if (addParabolicAntennaName_comboBox5.SelectedItem == null || addParabolicWaveformName_comboBox1.SelectedItem == null || addParabolicPolarization_textBox1.Text == "" || addParabolicRadius_textBox9.Text == "" || addParabolicBlockageRadius_textBox10.Text == "" || addParabolicApertureDistribution_textBox1.Text == "" || addParabolicEdgeTaper_textBox8.Text == "" || addParabolicReceiverThreshold_textBox7.Text == "" || addParabolicTransmissionLineLoss_textBox19.Text == "" || addParabolicVSWR_textBox21.Text == "" || addParabolicTemperature_textBox20.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicRadius_textBox9.Text))
                {
                    MessageBox.Show("抛物面半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicRadius_textBox9.Text) < 0.0001)
                {
                    MessageBox.Show("抛物面半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicBlockageRadius_textBox10.Text))
                {
                    MessageBox.Show("馈源半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicBlockageRadius_textBox10.Text) < 0.0001)
                {
                    MessageBox.Show("馈源半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicEdgeTaper_textBox8.Text))
                {
                    MessageBox.Show("EdgeTaper值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicEdgeTaper_textBox8.Text) > 0.0001)
                {
                    MessageBox.Show("EdgeTaper值应小于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicReceiverThreshold_textBox7.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicTransmissionLineLoss_textBox19.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicVSWR_textBox21.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addParabolicVSWR_textBox21))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicTemperature_textBox20.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RadiusLimition(addParabolicRadius_textBox9, addParabolicBlockageRadius_textBox10))
                    {
                        MessageBox.Show("馈源半径不能大于抛物面半径的20%", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                client.iUpdateAntenna(addAntenna);
                MessageBox.Show("天线已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addParabolicOk_button5_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                if (addParabolicWaveformName_comboBox1.Items.Count > 0)
                {
                    if (addParabolicWaveformName_comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (addParabolicAntennaName_comboBox5.SelectedItem == null || addParabolicWaveformName_comboBox1.SelectedItem == null || addParabolicPolarization_textBox1.Text == "" || addParabolicRadius_textBox9.Text == "" || addParabolicBlockageRadius_textBox10.Text == "" || addParabolicApertureDistribution_textBox1.Text == "" || addParabolicEdgeTaper_textBox8.Text == "" || addParabolicReceiverThreshold_textBox7.Text == "" || addParabolicTransmissionLineLoss_textBox19.Text == "" || addParabolicVSWR_textBox21.Text == "" || addParabolicTemperature_textBox20.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicRadius_textBox9.Text))
                {
                    MessageBox.Show("抛物面半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicRadius_textBox9.Text) < 0.0001)
                {
                    MessageBox.Show("抛物面半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicBlockageRadius_textBox10.Text))
                {
                    MessageBox.Show("馈源半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicBlockageRadius_textBox10.Text) < 0.0001)
                {
                    MessageBox.Show("馈源半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicEdgeTaper_textBox8.Text))
                {
                    MessageBox.Show("EdgeTaper值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addParabolicEdgeTaper_textBox8.Text) > 0.0001)
                {
                    MessageBox.Show("EdgeTaper值应小于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicReceiverThreshold_textBox7.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicTransmissionLineLoss_textBox19.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicVSWR_textBox21.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addParabolicVSWR_textBox21))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addParabolicTemperature_textBox20.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RadiusLimition(addParabolicRadius_textBox9, addParabolicBlockageRadius_textBox10))
                {
                    MessageBox.Show("馈源半径不能大于抛物面半径的20%", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] parabolicStr = new string[8];
                parabolicStr[0] = SetupContent.antennaStr1 + " " + (string)addParabolicAntennaName_comboBox5.SelectedItem + "\r\n";
                parabolicStr[1] = Translate.KeyWordsDictionary(addAntennaType_comboBox1) + "\r\n";
                parabolicStr[2] = "polarization " + Translate.KeyWordsDictionary_DB(addParabolicPolarization_textBox1.Text) + "\r\n";
                parabolicStr[3] = "power_threshold " + addParabolicReceiverThreshold_textBox7.Text + "\r\n";
                parabolicStr[4] = "cable_loss " + addParabolicTransmissionLineLoss_textBox19.Text + "\r\n";
                parabolicStr[5] = "VSWR " + addParabolicVSWR_textBox21.Text + "\r\n";
                parabolicStr[6] = "temperature " + addParabolicTemperature_textBox20.Text + "\r\n";
                parabolicStr[7] = SetupContent.antennaStr2 + "\r\n"
                                        + "radius " + addParabolicRadius_textBox9.Text + "\r\n"
                                        + "blockageradius " + addParabolicBlockageRadius_textBox10.Text + "\r\n"
                                        + "EFieldDistribution " + addParabolicApertureDistribution_textBox1.Text + "\r\n"
                                        + "EdgeTaper " + addParabolicEdgeTaper_textBox8.Text + "\r\n"
                                        + SetupContent.antennaStr3 + "\r\n";
                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge((string)addParabolicAntennaName_comboBox5.SelectedItem, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MainWindow.setupStr = an.InsertAntenna8(parabolicStr, (string)addParabolicWaveformName_comboBox1.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add((string)addParabolicAntennaName_comboBox5.SelectedItem);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + (string)addParabolicAntennaName_comboBox5.SelectedItem, MainWindow.nodeInfoFullPath, true);

                //将抛物线天线相应的参数按控件顺序记录到waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + (string)addParabolicAntennaName_comboBox5.SelectedItem + "\r\n"
                                                + (string)addAntennaType_comboBox1.SelectedItem + "\r\n"
                    //+ newparabolicwaveformname_comboBox5.SelectedItem + "\r\n"
                                                + (string)addParabolicWaveformName_comboBox1.SelectedItem + "\r\n"
                                                + addParabolicPolarization_textBox1.Text + "\r\n"
                                                + addParabolicRadius_textBox9.Text + "\r\n"
                                                + addParabolicBlockageRadius_textBox10.Text + "\r\n"
                                                + addParabolicApertureDistribution_textBox1.Text + "\r\n"
                                                + addParabolicEdgeTaper_textBox8.Text + "\r\n"
                                                + addParabolicReceiverThreshold_textBox7.Text + "\r\n"
                                                + addParabolicTransmissionLineLoss_textBox19.Text + "\r\n"
                                                + addParabolicVSWR_textBox21.Text + "\r\n"
                                                + addParabolicTemperature_textBox20.Text + "\r\n"
                                                + "END" + SetupContent.antennaStr1 + " " + (string)addParabolicAntennaName_comboBox5.SelectedItem + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + (string)addParabolicAntennaName_comboBox5.SelectedItem + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void addParabolicCancel_button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addHelicalAntennaName_comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string HelicalPolarization = null;
                string HelicalPolarizationTranslate = null;
                addAntenna = client.iGetAntenna((string)addHelicalAntennaName_comboBox7.SelectedItem);
                HelicalPolarization = addAntenna.Polarization;
                if (HelicalPolarization == "LeftCircular") 
                    HelicalPolarizationTranslate = "左旋圆极化Left-hand circular"; 
                else
                    HelicalPolarizationTranslate = "右旋圆极化Right-hand circular";
                //addHelicalWaveformName_textBox27.Text = addAntenna.WaveFormName;
                addHelicalPolarization_textBox1.Text = HelicalPolarizationTranslate;
                //addHelicalMaxGain_textBox26.Text = addAntenna.MaxGain.ToString();
                addHelicalRadius_textBox28.Text = addAntenna.Radius.ToString();
                addHelicalLength_textBox29.Text = addAntenna.Length.ToString();
                addHelicalPitch_textBox30.Text = addAntenna.Pitch.ToString();
                addHelicalReceiverThreshold_textBox25.Text = addAntenna.RecieveThrehold.ToString();
                addHelicalTransmissionLineLoss_textBox24.Text = addAntenna.TransmissionLoss.ToString();
                addHelicalVSWR_textBox23.Text = addAntenna.VSWR.ToString();
                addHelicalTemperature_textBox22.Text = addAntenna.Temperature.ToString();
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
        private void addHelicalUpdate_button14_Click(object sender, EventArgs e)
        {
            try
            {
                //string HelicalPolarization = null;
                //string HelicalPolarizationTranslate = null;
                //HelicalPolarization = addHelicalPolarization_textBox1.Text;
                //if (HelicalPolarization == "左旋圆极化Left-hand circular")
                //    HelicalPolarizationTranslate = "LeftCircular";
                //else
                //    HelicalPolarizationTranslate = "RightCircular";
                addAntenna.Name = (string)addHelicalAntennaName_comboBox7.SelectedItem;
                //addAntenna.Polarization = HelicalPolarizationTranslate;
                addAntenna.Polarization = Translate.KeyWordsDictionary_DB(addHelicalPolarization_textBox1.Text);
                //addAntenna.MaxGain = double.Parse(addHelicalMaxGain_textBox26.Text);
                addAntenna.Radius = double.Parse(addHelicalRadius_textBox28.Text);
                addAntenna.Length = double.Parse(addHelicalLength_textBox29.Text);
                addAntenna.Pitch = double.Parse(addHelicalPitch_textBox30.Text);
                addAntenna.RecieveThrehold = double.Parse(addHelicalReceiverThreshold_textBox25.Text);
                addAntenna.TransmissionLoss = double.Parse(addHelicalTransmissionLineLoss_textBox24.Text);
                addAntenna.VSWR = double.Parse(addHelicalVSWR_textBox23.Text);
                addAntenna.Temperature = double.Parse(addHelicalTemperature_textBox22.Text);
                if (addHelicalAntennaName_comboBox7.SelectedItem == null || addHelicalWaveformName_comboBox1.SelectedItem == null || addHelicalPolarization_textBox1.Text == "" ||  addHelicalRadius_textBox28.Text == "" || addHelicalLength_textBox29.Text == "" || addHelicalPitch_textBox30.Text == "" || addHelicalReceiverThreshold_textBox25.Text == "" || addHelicalTransmissionLineLoss_textBox24.Text == "" || addHelicalVSWR_textBox23.Text == "" || addHelicalVSWR_textBox23.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalRadius_textBox28.Text))
                {
                    MessageBox.Show("螺纹半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalRadius_textBox28.Text) < 0.0001)
                {
                    MessageBox.Show("螺纹半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalLength_textBox29.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalLength_textBox29.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalPitch_textBox30.Text))
                {
                    MessageBox.Show("螺距值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalPitch_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("螺距值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalReceiverThreshold_textBox25.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalTransmissionLineLoss_textBox24.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalVSWR_textBox23.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addHelicalVSWR_textBox23))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalTemperature_textBox22.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateAntenna(addAntenna);
                MessageBox.Show("天线已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addHelicalOk_button7_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                if (addHelicalWaveformName_comboBox1.Items.Count > 0)
                {
                    if (addHelicalWaveformName_comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (addHelicalAntennaName_comboBox7.SelectedItem == null || addHelicalWaveformName_comboBox1.SelectedItem == null || addHelicalPolarization_textBox1.Text == "" ||  addHelicalRadius_textBox28.Text == "" || addHelicalLength_textBox29.Text == "" || addHelicalPitch_textBox30.Text == "" || addHelicalReceiverThreshold_textBox25.Text == "" || addHelicalTransmissionLineLoss_textBox24.Text == "" || addHelicalVSWR_textBox23.Text == "" || addHelicalVSWR_textBox23.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalRadius_textBox28.Text))
                {
                    MessageBox.Show("螺纹半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalRadius_textBox28.Text) < 0.0001)
                {
                    MessageBox.Show("螺纹半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalLength_textBox29.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalLength_textBox29.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalPitch_textBox30.Text))
                {
                    MessageBox.Show("螺距值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addHelicalPitch_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("螺距值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalReceiverThreshold_textBox25.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalTransmissionLineLoss_textBox24.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalVSWR_textBox23.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(addHelicalVSWR_textBox23))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addHelicalTemperature_textBox22.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] halicalStr = new string[8];
                halicalStr[0] = SetupContent.antennaStr1 + " " + (string)addHelicalAntennaName_comboBox7.SelectedItem + "\r\n";
                halicalStr[1] = Translate.KeyWordsDictionary(addAntennaType_comboBox1) + "\r\n";
                halicalStr[2] = "polarization " + addHelicalPolarization_textBox1.Text + "\r\n";
                halicalStr[3] = "power_threshold " + addHelicalReceiverThreshold_textBox25.Text + "\r\n";
                halicalStr[4] = "cable_loss " + addHelicalTransmissionLineLoss_textBox24.Text + "\r\n";
                halicalStr[5] = "VSWR " + addHelicalVSWR_textBox23.Text + "\r\n";
                halicalStr[6] = "temperature " + addHelicalTemperature_textBox22.Text + "\r\n";
                halicalStr[7] = SetupContent.antennaStr2 + "\r\n"
                                    + "radius " + addHelicalRadius_textBox28.Text + "\r\n"
                                    + "length " + addHelicalLength_textBox29.Text + "\r\n"
                                    + "pitch " + addHelicalPitch_textBox30.Text + "\r\n"
                                    + SetupContent.antennaStr3 + "\r\n";
                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge((string)addHelicalAntennaName_comboBox7.SelectedItem, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MainWindow.setupStr = an.InsertAntenna8(halicalStr, (string)addHelicalWaveformName_comboBox1.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add((string)addHelicalAntennaName_comboBox7.SelectedItem);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + (string)addHelicalAntennaName_comboBox7.SelectedItem, MainWindow.nodeInfoFullPath, true);
                
                //将螺旋天线参数信息写入到.waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + (string)addHelicalAntennaName_comboBox7.SelectedItem + "\r\n"
                                                      + (string)addAntennaType_comboBox1.SelectedItem + "\r\n"
                    //+ newhelicalwaveformname_comboBox7.SelectedItem + "\r\n"
                                                        + (string)addHelicalWaveformName_comboBox1.SelectedItem + "\r\n"
                                                      + addHelicalPolarization_textBox1.Text + "\r\n"
                                                      + addHelicalMaxGain_textBox26.Text + "\r\n"
                                                      + addHelicalRadius_textBox28.Text + "\r\n"
                                                      + addHelicalLength_textBox29.Text + "\r\n"
                                                      + addHelicalPitch_textBox30.Text + "\r\n"
                                                      + addHelicalReceiverThreshold_textBox25.Text + "\r\n"
                                                      + addHelicalTransmissionLineLoss_textBox24.Text + "\r\n"
                                                      + addHelicalVSWR_textBox23.Text + "\r\n"
                                                      + addHelicalTemperature_textBox22.Text + "\r\n"
                                                      + "END" + SetupContent.antennaStr1 + " " + (string)addHelicalAntennaName_comboBox7.SelectedItem + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + (string)addHelicalAntennaName_comboBox7.SelectedItem + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void addHelicalCancel_button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }                
        private void addLogperiodicAntennaName_comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addAntenna = client.iGetAntenna((string)addLogperiodicAntennaName_comboBox8.SelectedItem);
                //addLogperiodicWaveformName_textBox39.Text = addAntenna.WaveFormName;
                addLogperiodicMaxGain_textBox38.Text = addAntenna.MaxGain.ToString();
                addLogperiodicReceiverThreshold_textBox37.Text = addAntenna.RecieveThrehold.ToString();
                addLogperiodicTransmissionLineLoss_textBox36.Text = addAntenna.TransmissionLoss.ToString();
                addLogperiodicVSWR_textBox35.Text = addAntenna.VSWR.ToString();
                addLogperiodicTemperature_textBox34.Text = addAntenna.Temperature.ToString();
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
        private void addLogperiodicUpdate_button15_Click(object sender, EventArgs e)
        {
            try
            {
                addAntenna.Name = (string)addLogperiodicAntennaName_comboBox8.SelectedItem;                
                addAntenna.MaxGain = double.Parse(addLogperiodicMaxGain_textBox38.Text);
                addAntenna.RecieveThrehold = double.Parse(addLogperiodicReceiverThreshold_textBox37.Text);
                addAntenna.TransmissionLoss = double.Parse(addLogperiodicTransmissionLineLoss_textBox36.Text);
                addAntenna.VSWR = double.Parse(addLogperiodicVSWR_textBox35.Text);
                addAntenna.Temperature = double.Parse(addLogperiodicTemperature_textBox34.Text);
                client.iUpdateAntenna(addAntenna);
                MessageBox.Show("天线已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addLogperiodicOk_button9_Click(object sender, EventArgs e)
        {

        }
        private void addLogperiodicCancel_button10_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
    }
}

