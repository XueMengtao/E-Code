using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Data.SqlClient;
using WcfService;
using LogFileManager;
using System.Collections;

namespace WF1
{
    public partial class NewAntennaWindow : Form
    {
        ServiceReference1.Service1Client client = null;
        string[] AllWaveformNames = new string[] { };
        public NewAntennaWindow()
        {
            InitializeComponent();
            client = new ServiceReference1.Service1Client();
            if (System.IO.File.Exists(MainWindow.nodeInfoFullPath))
            {
                string waveNamesOfCombox = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);

                ArrayList waveNamesArr = new ArrayList();
                //向工程树中添加波形节点
                waveNamesArr = ProjectTreeMatchString.MatchStr(waveNamesOfCombox, SetupContent.waveIndeStr, SetupContent.waveIndeStr.Length);
                foreach (string s in waveNamesArr)
                {
                    newdipolewaveformname_comboBox2.Items.Add(s);
                    newmonopolewaveformname_comboBox4.Items.Add(s);
                    newparabolicwaveformname_comboBox5.Items.Add(s);
                    newhelicalwaveformname_comboBox7.Items.Add(s);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainWindow.nodeInfoFullPath != null)
            {
                string AntennaType = null;
                AntennaType = (string)comboBox1.SelectedItem;
                switch (AntennaType)
                {
                    case "偶极子天线<Short dipole>":
                        {
                            this.newdipoleantenna_panel1.Visible = true;
                            this.newmonopole_panel2.Visible = false;
                            this.newparabolic_panel3.Visible = false;
                            this.newhelical_panel4.Visible = false;
                            this.newlogperiodic_panel5.Visible = false;
                        }
                        break;
                    case "单极天线<Short monopole>":
                        {
                            this.newmonopole_panel2.Visible = true;
                            this.newdipoleantenna_panel1.Visible = false;
                            this.newparabolic_panel3.Visible = false;
                            this.newhelical_panel4.Visible = false;
                            this.newlogperiodic_panel5.Visible = false;
                        }
                        break;
                    case "抛物面反射天线<Parabolic reflector>":
                        {
                            this.newparabolic_panel3.Visible = true;
                            this.newdipoleantenna_panel1.Visible = false;
                            this.newmonopole_panel2.Visible = false;
                            this.newhelical_panel4.Visible = false;
                            this.newlogperiodic_panel5.Visible = false;
                        }
                        break;
                    case "螺旋天线<Helical>":
                        {
                            this.newhelical_panel4.Visible = true;
                            this.newdipoleantenna_panel1.Visible = false;
                            this.newmonopole_panel2.Visible = false;
                            this.newparabolic_panel3.Visible = false;
                            this.newlogperiodic_panel5.Visible = false;
                        }
                        break;
                    case "对数周期天线<Log-periodical>":
                        {
                            this.newlogperiodic_panel5.Visible = true;
                            this.newdipoleantenna_panel1.Visible = false;
                            this.newmonopole_panel2.Visible = false;
                            this.newparabolic_panel3.Visible = false;
                            this.newhelical_panel4.Visible = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            //else
            //{
            //    MessageBox.Show("请先新建工程","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //   return;
            //}
        }
        private void newDipoleOk_button1_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
                //if (newdipolewaveformname_comboBox2.Items.Count > 0)
                //{
                //    if (newdipolewaveformname_comboBox2.SelectedItem == null)
                //    {
                //        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        //return;
                //    }

                //}
                if (newdipolewaveformname_comboBox2.Items.Count < 0)
                {
                    MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (newdipoleantennaname_textBox1.Text == "" || newdipolewaveformname_comboBox2.SelectedItem == null || newdipolepolarization_comboBox3.SelectedItem == null || newdipolereceiverthreshold_textBox3.Text == "" || newdipoletranmissionlineloss_textBox4.Text == "" || newdipolevswr_textBox5.Text == "" || newdipoletemperature_textBox6.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newdipolereceiverthreshold_textBox3.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newdipoletranmissionlineloss_textBox4.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newdipolevswr_textBox5.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(newdipolevswr_textBox5))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newdipoletemperature_textBox6.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                //准备好插入的内容
                string[] dipoleStr = new string[8];
                dipoleStr[0] = SetupContent.antennaStr1 + " " + newdipoleantennaname_textBox1.Text + "\r\n";
                dipoleStr[1] = Translate.KeyWordsDictionary(comboBox1) + "\r\n";
                dipoleStr[2] = "polarization " + Translate.KeyWordsDictionary(newdipolepolarization_comboBox3) + "\r\n";
                dipoleStr[3] = "power_threshold " + newdipolereceiverthreshold_textBox3.Text + "\r\n";
                dipoleStr[4] = "cable_loss " + newdipoletranmissionlineloss_textBox4.Text + "\r\n";
                dipoleStr[5] = "VSWR " + newdipolevswr_textBox5.Text + "\r\n";
                dipoleStr[6] = "temperature " + newdipoletemperature_textBox6.Text + "\r\n";
                dipoleStr[7] = SetupContent.antennaStr2 + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                //注意 新建的天线个数不能超过100
                string[] antennaNames = new string[100];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge(newdipoleantennaname_textBox1.Text, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                MainWindow.setupStr = an.InsertAntenna8(dipoleStr, newdipolewaveformname_comboBox2.Text, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(newdipoleantennaname_textBox1.Text);

                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);


                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + newdipoleantennaname_textBox1.Text, MainWindow.nodeInfoFullPath, true);
                //将单极子天线的参数按相应的顺序记录到waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + newdipoleantennaname_textBox1.Text + "\r\n"
                                                    + comboBox1.SelectedItem + "\r\n"
                    //+ (string)newdipolewaveformname_comboBox2.SelectedItem +"\r\n"
                                                    + newdipolewaveformname_comboBox2.SelectedItem + "\r\n"
                                                    + newdipolemaxgain_textBox2.Text + "\r\n"
                                                    + newdipolepolarization_comboBox3.SelectedItem + "\r\n"
                    //+ newdipolepolarization_comboBox3.Text  + "\r\n"
                                                    + newdipolereceiverthreshold_textBox3.Text + "\r\n"
                                                    + newdipoletranmissionlineloss_textBox4.Text + "\r\n"
                                                    + newdipolevswr_textBox5.Text + "\r\n"
                                                    + newdipoletemperature_textBox6.Text + "\r\n"
                                                    + "END" + SetupContent.antennaStr1 + " " + newdipoleantennaname_textBox1.Text + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
                //}
                if (MainWindow.creatSuccMesDisp)
                {
                    MessageBox.Show("\"" + newdipoleantennaname_textBox1.Text + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }
        private void newDipoleSave_button11_Click(object sender, EventArgs e)
        {
            ServiceReference1.Antenna newDipole = new ServiceReference1.Antenna();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newDipole.Name = newdipoleantennaname_textBox1.Text;
                newDipole.Type = Translate.KeyWordsDictionary(comboBox1);
                //newDipole.WaveFormName = (string)newdipolewaveformname_comboBox2.SelectedItem;
                //newDipole.MaxGain = double.Parse(newdipolemaxgain_textBox2.Text);
                newDipole.Polarization = Translate.KeyWordsDictionary(newdipolepolarization_comboBox3);
                newDipole.RecieveThrehold = double.Parse(newdipolereceiverthreshold_textBox3.Text);
                newDipole.TransmissionLoss = double.Parse(newdipoletranmissionlineloss_textBox4.Text);
                newDipole.VSWR = double.Parse(newdipolevswr_textBox5.Text);
                newDipole.Temperature = double.Parse(newdipoletemperature_textBox6.Text);

                try
                {
                    if (newdipolewaveformname_comboBox2.Items.Count < 0)
                    {
                        MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (newdipoleantennaname_textBox1.Text == "" || newdipolewaveformname_comboBox2.SelectedItem == null || newdipolepolarization_comboBox3.SelectedItem == null || newdipolereceiverthreshold_textBox3.Text == "" || newdipoletranmissionlineloss_textBox4.Text == "" || newdipolevswr_textBox5.Text == "" || newdipoletemperature_textBox6.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newdipolereceiverthreshold_textBox3.Text))
                    {
                        MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newdipoletranmissionlineloss_textBox4.Text))
                    {
                        MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newdipolevswr_textBox5.Text))
                    {
                        MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.VSWRLimition(newdipolevswr_textBox5))
                    {
                        MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newdipoletemperature_textBox6.Text))
                    {
                        MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddAntenna(newDipole);
                    MessageBox.Show("天线已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newDipoleOk_button1_Click(sender, e);
                try
                {
                    newDipole = client.iGetAntenna(newdipoleantennaname_textBox1.Text);
                    newDipole.Name = newdipoleantennaname_textBox1.Text;
                    //newDipole.MaxGain = double.Parse(newdipolemaxgain_textBox2.Text);
                    //addAntenna.Polarization = AntennaPolarizationTranslate2;
                    newDipole.Polarization = Translate.KeyWordsDictionary(newdipolepolarization_comboBox3);
                    newDipole.RecieveThrehold = double.Parse(newdipolereceiverthreshold_textBox3.Text);
                    newDipole.TransmissionLoss = double.Parse(newdipoletranmissionlineloss_textBox4.Text);
                    newDipole.VSWR = double.Parse(newdipolevswr_textBox5.Text);
                    newDipole.Temperature = double.Parse(newdipoletemperature_textBox6.Text);
                    client.iUpdateAntenna(newDipole);
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
        }
        private void newDipoleCancelbutton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newMonopoleOk_button3_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                //if (newmonopolewaveformname_comboBox4.Items.Count > 0)
                //{
                //    if (newmonopolewaveformname_comboBox4.SelectedItem == null)
                //    {
                //        MessageBox.Show("该天线未设置波形", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }

                //}
                if (newmonopolewaveformname_comboBox4.Items.Count < 0)
                {
                    MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if(newmonopoleantennaname_textBox18.Text==""||newmonopolewaveformname_comboBox4.SelectedItem == null||newmonopolelength_textBox31.Text ==""||newmonopolereceiverthreshold_textBox16.Text ==""||newmonopoletransmissionlineloss_textBox15.Text==""||newmonopolevswr_textBox14.Text ==""||newmonopoletempertature_textBox13.Text=="")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newmonopoletransmissionlineloss_textBox15.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newmonopolelength_textBox31.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.0001", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newmonopolereceiverthreshold_textBox16.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newmonopoletransmissionlineloss_textBox15.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newmonopolevswr_textBox14.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(newmonopolevswr_textBox14))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newmonopoletempertature_textBox13.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] monopoleStr = new string[8];
                monopoleStr[0] = SetupContent.antennaStr1 + " " + newmonopoleantennaname_textBox18.Text + "\r\n";
                monopoleStr[1] = Translate.KeyWordsDictionary(comboBox1) + "\r\n";
                monopoleStr[2] = "";
                monopoleStr[3] = "power_threshold " + newmonopolereceiverthreshold_textBox16.Text + "\r\n";
                monopoleStr[4] = "cable_loss " + newmonopoletransmissionlineloss_textBox15.Text + "\r\n";
                monopoleStr[5] = "VSWR " + newmonopolevswr_textBox14.Text + "\r\n";
                monopoleStr[6] = "temperature " + newmonopoletempertature_textBox13.Text + "\r\n";
                monopoleStr[7] = SetupContent.antennaStr2 + "\r\n" + "length " + newmonopolelength_textBox31.Text + "\r\n" + SetupContent.antennaStr3 + "\r\n";

                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge(newmonopoleantennaname_textBox18.Text, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }

                MainWindow.setupStr = an.InsertAntenna8(monopoleStr, (string)newmonopolewaveformname_comboBox4.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(newmonopoleantennaname_textBox18.Text);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + newmonopoleantennaname_textBox18.Text, MainWindow.nodeInfoFullPath, true);

                string antennaInfoStr = SetupContent.antennaStr1 + " " + newmonopoleantennaname_textBox18.Text + "\r\n"
                                                  + comboBox1.SelectedItem + "\r\n"
                    //+newmonopolewaveformname_comboBox4.SelectedItem+"\r\n"
                                                  + newmonopolewaveformname_comboBox4.Text + "\r\n"
                                                  + newmonopolelength_textBox31.Text + "\r\n"
                                                  + newmonopolemaxgain_textBox17.Text + "\r\n"
                                                  + newmonopolereceiverthreshold_textBox16.Text + "\r\n"
                                                  + newmonopoletransmissionlineloss_textBox15.Text + "\r\n"
                                                  + newmonopolevswr_textBox14.Text + "\r\n"
                                                  + newmonopoletempertature_textBox13.Text + "\r\n"
                                                  + "END" + SetupContent.antennaStr1 + " " + newmonopoleantennaname_textBox18.Text + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);
            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newmonopoleantennaname_textBox18.Text + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newMonopoleSave_button12_Click(object sender, EventArgs e)
        {
            ServiceReference1.Antenna newMonopole = new ServiceReference1.Antenna();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newMonopole.Name = newmonopoleantennaname_textBox18.Text;
                newMonopole.Type = Translate.KeyWordsDictionary(comboBox1);
                //newMonopole.WaveFormName = (string)newmonopolewaveformname_comboBox4.SelectedItem;
                newMonopole.Length = double.Parse(newmonopolelength_textBox31.Text);
                //newMonopole.MaxGain = double.Parse(newmonopolemaxgain_textBox17.Text);
                newMonopole.RecieveThrehold = double.Parse(newmonopolereceiverthreshold_textBox16.Text);
                newMonopole.TransmissionLoss = double.Parse(newmonopoletransmissionlineloss_textBox15.Text);
                newMonopole.VSWR = double.Parse(newmonopolevswr_textBox14.Text);
                newMonopole.Temperature = double.Parse(newmonopoletempertature_textBox13.Text);

                try
                {
                    if (newmonopolewaveformname_comboBox4.Items.Count < 0)
                    {
                        MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (newmonopoleantennaname_textBox18.Text == "" || newmonopolewaveformname_comboBox4.SelectedItem == null || newmonopolelength_textBox31.Text == "" || newmonopolereceiverthreshold_textBox16.Text == "" || newmonopoletransmissionlineloss_textBox15.Text == "" || newmonopolevswr_textBox14.Text == "" || newmonopoletempertature_textBox13.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newmonopoletransmissionlineloss_textBox15.Text))
                    {
                        MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newmonopolelength_textBox31.Text) < 0.0001)
                    {
                        MessageBox.Show("天线长度值应大于0.0001", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newmonopolereceiverthreshold_textBox16.Text))
                    {
                        MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newmonopoletransmissionlineloss_textBox15.Text))
                    {
                        MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.IsScienceFigure(newmonopolevswr_textBox14.Text))
                    {
                        MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.VSWRLimition(newmonopolevswr_textBox14))
                    {
                        MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newmonopoletempertature_textBox13.Text))
                    {
                        MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddAntenna(newMonopole);
                    MessageBox.Show("天线已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newMonopoleOk_button3_Click(sender, e);
                try
                {
                    newMonopole = client.iGetAntenna(newmonopoleantennaname_textBox18.Text);
                    newMonopole.Name = newmonopoleantennaname_textBox18.Text;
                    //addAntenna.WaveFormName = addMonopoleWaveformName_textBox1.Text;
                    newMonopole.Length = double.Parse(newmonopolelength_textBox31.Text);
                    //newMonopole.MaxGain = double.Parse(newmonopolemaxgain_textBox17.Text);
                    newMonopole.RecieveThrehold = double.Parse(newmonopolereceiverthreshold_textBox16.Text);
                    newMonopole.TransmissionLoss = double.Parse(newmonopoletransmissionlineloss_textBox15.Text);
                    newMonopole.VSWR = double.Parse(newmonopolevswr_textBox14.Text);
                    newMonopole.Temperature = double.Parse(newmonopoletempertature_textBox13.Text);
                    client.iUpdateAntenna(newMonopole);
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
        }
        private void newMonopoleCancelbutton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newParabolicOk_button5_Click(object sender, EventArgs e)
        {
           if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
           {
                if(newparabolicwaveformname_comboBox5.Items.Count < 0)
                {
                    MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }

                if (newparabolicantennaname_textBox12.Text == "" || newparabolicwaveformname_comboBox5.SelectedItem == null || newparabolicpolarization_comboBox2.SelectedItem == null || newparabolicradius_textBox9.Text == "" || newparabolicblockageradius_textBox10.Text == "" || newparabolicaperturedistribution_comboBox6.SelectedItem == null || newparabolicedgetaper_textBox8.Text == "" || newparabolicreceiverthreshold_textBox7.Text == "" || newparabolictransmissionlineloss_textBox19.Text == "" || newparabolicvswr_textBox21.Text == "" || newparabolictemperature_textBox20.Text == "")
                {
                  MessageBox.Show("窗口中有未设置的信息，请您设置完整","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                  MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolicradius_textBox9.Text))
                {
                    MessageBox.Show("抛物面半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newparabolicradius_textBox9.Text) < 0.0001)
                {
                    MessageBox.Show("抛物面半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolicblockageradius_textBox10.Text))
                {
                    MessageBox.Show("馈源半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newparabolicblockageradius_textBox10.Text) < 0.0001)
                {
                    MessageBox.Show("馈源半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolicedgetaper_textBox8.Text))
                {
                    MessageBox.Show("EdgeTaper值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newparabolicedgetaper_textBox8.Text) > 0.0001)
                {
                    MessageBox.Show("EdgeTaper值应小于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolicreceiverthreshold_textBox7.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolictransmissionlineloss_textBox19.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolicvswr_textBox21.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(newparabolicvswr_textBox21))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newparabolictemperature_textBox20.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.RadiusLimition(newparabolicradius_textBox9, newparabolicblockageradius_textBox10))
                {
                    MessageBox.Show("馈源半径不能大于抛物面半径的20%", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] parabolicStr = new string[8];
                parabolicStr[0] = SetupContent.antennaStr1 + " " + newparabolicantennaname_textBox12.Text + "\r\n";
                parabolicStr[1] = Translate.KeyWordsDictionary(comboBox1) + "\r\n";
                parabolicStr[2] = "polarization " + Translate.KeyWordsDictionary(newparabolicpolarization_comboBox2) + "\r\n";
                parabolicStr[3] = "power_threshold " + newparabolicreceiverthreshold_textBox7.Text + "\r\n";
                parabolicStr[4] = "cable_loss " + newparabolictransmissionlineloss_textBox19.Text + "\r\n";
                parabolicStr[5] = "VSWR " + newparabolicvswr_textBox21.Text + "\r\n";
                parabolicStr[6] = "temperature " + newparabolictemperature_textBox20.Text + "\r\n";
                parabolicStr[7] = SetupContent.antennaStr2 + "\r\n"
                                        + "radius " + newparabolicradius_textBox9.Text + "\r\n"
                                        + "blockageradius " + newparabolicblockageradius_textBox10.Text + "\r\n"
                                        + "EFieldDistribution " + (string)newparabolicaperturedistribution_comboBox6.SelectedItem + "\r\n"
                                        + "EdgeTaper " + newparabolicedgetaper_textBox8.Text + "\r\n"
                                        + SetupContent.antennaStr3 + "\r\n";
                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge(newparabolicantennaname_textBox12.Text, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }


                MainWindow.setupStr = an.InsertAntenna8(parabolicStr, (string)newparabolicwaveformname_comboBox5.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(newparabolicantennaname_textBox12.Text);

                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + newparabolicantennaname_textBox12.Text, MainWindow.nodeInfoFullPath, true);
                //将抛物线天线相应的参数按控件顺序记录到waveinfo文件中
                string antennaInfoStr = SetupContent.antennaStr1 + " " + newparabolicantennaname_textBox12.Text + "\r\n"
                                                + comboBox1.SelectedItem + "\r\n"
                    //+ newparabolicwaveformname_comboBox5.SelectedItem + "\r\n"
                                                + newparabolicwaveformname_comboBox5.Text + "\r\n"
                                                + newparabolicpolarization_comboBox2.SelectedItem + "\r\n"
                                                + newparabolicradius_textBox9.Text + "\r\n"
                                                + newparabolicblockageradius_textBox10.Text + "\r\n"
                                                + newparabolicaperturedistribution_comboBox6.SelectedItem + "\r\n"
                                                + newparabolicedgetaper_textBox8.Text + "\r\n"
                                                + newparabolicreceiverthreshold_textBox7.Text + "\r\n"
                                                + newparabolictransmissionlineloss_textBox19.Text + "\r\n"
                                                + newparabolicvswr_textBox21.Text + "\r\n"
                                                + newparabolictemperature_textBox20.Text + "\r\n"
                                                + "END" + SetupContent.antennaStr1 + " " + newparabolicantennaname_textBox12.Text + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);

            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newparabolicantennaname_textBox12.Text + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newParabolicSavebutton13_Click(object sender, EventArgs e)
        {
            ServiceReference1.Antenna newParabolic = new ServiceReference1.Antenna();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newParabolic.Name = newparabolicantennaname_textBox12.Text;
                newParabolic.Type = Translate.KeyWordsDictionary(comboBox1);
                //newParabolic.WaveFormName = (string)newparabolicwaveformname_comboBox5.SelectedItem;
                newParabolic.Polarization = Translate.KeyWordsDictionary(newparabolicpolarization_comboBox2);
                newParabolic.Radius = double.Parse(newparabolicradius_textBox9.Text);
                newParabolic.BlockageRadius = double.Parse(newparabolicblockageradius_textBox10.Text);
                newParabolic.ApertureDistribution = (string)newparabolicaperturedistribution_comboBox6.SelectedItem;
                newParabolic.EdgeTeper = double.Parse(newparabolicedgetaper_textBox8.Text);
                newParabolic.RecieveThrehold = double.Parse(newparabolicreceiverthreshold_textBox7.Text);
                newParabolic.TransmissionLoss = double.Parse(newparabolictransmissionlineloss_textBox19.Text);
                newParabolic.VSWR = double.Parse(newparabolicvswr_textBox21.Text);
                newParabolic.Temperature = double.Parse(newparabolictemperature_textBox20.Text);

                try
                {

                    if (newparabolicwaveformname_comboBox5.Items.Count < 0)
                    {
                        MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;

                    }

                    if (newparabolicantennaname_textBox12.Text == "" || newparabolicwaveformname_comboBox5.SelectedItem == null || newparabolicpolarization_comboBox2.SelectedItem == null || newparabolicradius_textBox9.Text == "" || newparabolicblockageradius_textBox10.Text == "" || newparabolicaperturedistribution_comboBox6.SelectedItem == null || newparabolicedgetaper_textBox8.Text == "" || newparabolicreceiverthreshold_textBox7.Text == "" || newparabolictransmissionlineloss_textBox19.Text == "" || newparabolicvswr_textBox21.Text == "" || newparabolictemperature_textBox20.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolicradius_textBox9.Text))
                    {
                        MessageBox.Show("抛物面半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newparabolicradius_textBox9.Text) < 0.0001)
                    {
                        MessageBox.Show("抛物面半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolicblockageradius_textBox10.Text))
                    {
                        MessageBox.Show("馈源半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newparabolicblockageradius_textBox10.Text) < 0.0001)
                    {
                        MessageBox.Show("馈源半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolicedgetaper_textBox8.Text))
                    {
                        MessageBox.Show("EdgeTaper值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newparabolicedgetaper_textBox8.Text) > 0.0001)
                    {
                        MessageBox.Show("EdgeTaper值应小于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolicreceiverthreshold_textBox7.Text))
                    {
                        MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolictransmissionlineloss_textBox19.Text))
                    {
                        MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolicvswr_textBox21.Text))
                    {
                        MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.VSWRLimition(newparabolicvswr_textBox21))
                    {
                        MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newparabolictemperature_textBox20.Text))
                    {
                        MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.RadiusLimition(newparabolicradius_textBox9, newparabolicblockageradius_textBox10))
                    {
                        MessageBox.Show("馈源半径不能大于抛物面半径的20%", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddAntenna(newParabolic);
                    MessageBox.Show("天线已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
              newParabolicOk_button5_Click(sender, e);
                try
                {
                    newParabolic = client.iGetAntenna(newparabolicantennaname_textBox12.Text);
                    newParabolic.Name = newparabolicantennaname_textBox12.Text;
                    newParabolic.Polarization = Translate.KeyWordsDictionary(newparabolicpolarization_comboBox2);
                    newParabolic.Radius = double.Parse(newparabolicradius_textBox9.Text);
                    newParabolic.BlockageRadius = double.Parse(newparabolicblockageradius_textBox10.Text);
                    newParabolic.ApertureDistribution = (string)newparabolicaperturedistribution_comboBox6.SelectedItem;
                    newParabolic.EdgeTeper = double.Parse(newparabolicedgetaper_textBox8.Text);
                    newParabolic.RecieveThrehold = double.Parse(newparabolicreceiverthreshold_textBox7.Text);
                    newParabolic.TransmissionLoss = double.Parse(newparabolictransmissionlineloss_textBox19.Text);
                    newParabolic.VSWR = double.Parse(newparabolicvswr_textBox21.Text);
                    newParabolic.Temperature = double.Parse(newparabolictemperature_textBox20.Text);
                    client.iUpdateAntenna(newParabolic);
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
        }
        private void newParabolicCancelbutton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newHelicalOk_button7_Click(object sender, EventArgs e)
        {
            if (MainWindow.setupStr == null)
            {
                MessageBox.Show("请先建立工程", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                if (newhelicalwaveformname_comboBox7.Items.Count < 0)
                {
                    MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (newhelicalantennaname_textBox27.Text == "" || newhelicalwaveformname_comboBox7.SelectedItem == null || newHelicalpolarization_comboBox3.SelectedItem == null ||  newhelicalradius_textBox28.Text == "" || newhelicallength_textBox29.Text == "" || newhelicalpitch_textBox30.Text == "" || helicalreceiverthreshold_newtextBox25.Text == "" || newhelicaltransmissionlineloss_textBox24.Text == "" || newhelicalvswr_textBox23.Text == "" || newhelicaltemperature_textBox22.Text =="")
                {
                   MessageBox.Show("窗口中有未设置的信息，请您设置完整","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                   MainWindow.IsReturnMidwayInNewProcess = true;
                    return;

                }
                if (!BoudingLimition.IsScienceFigure(newhelicalradius_textBox28.Text))
                {
                    MessageBox.Show("螺纹半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicalradius_textBox28.Text) < 0.0001)
                {
                    MessageBox.Show("螺纹半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicallength_textBox29.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicallength_textBox29.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicalpitch_textBox30.Text))
                {
                    MessageBox.Show("螺距值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicalpitch_textBox30.Text) <0.000)
                {
                    MessageBox.Show("螺距值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(helicalreceiverthreshold_newtextBox25.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicaltransmissionlineloss_textBox24.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicalvswr_textBox23.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(newhelicalvswr_textBox23))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicaltemperature_textBox22.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                
                string[] halicalStr = new string[8];
                halicalStr[0] = SetupContent.antennaStr1 + " " + newhelicalantennaname_textBox27.Text + "\r\n";
                halicalStr[1] = Translate.KeyWordsDictionary(comboBox1) + "\r\n";
                halicalStr[2] = "polarization " + Translate.KeyWordsDictionary(newHelicalpolarization_comboBox3) + "\r\n";
                halicalStr[3] = "power_threshold " + helicalreceiverthreshold_newtextBox25.Text + "\r\n";
                halicalStr[4] = "cable_loss " + newhelicaltransmissionlineloss_textBox24.Text + "\r\n";
                halicalStr[5] = "VSWR " + newhelicalvswr_textBox23.Text + "\r\n";
                halicalStr[6] = "temperature " + newparabolictemperature_textBox20.Text + "\r\n";
                halicalStr[7] = SetupContent.antennaStr2 + "\r\n"
                                    + "radius " + newhelicalradius_textBox28.Text + "\r\n"
                                    + "length " + newhelicallength_textBox29.Text + "\r\n"
                                    + "pitch " + newhelicalpitch_textBox30.Text + "\r\n"
                                    + SetupContent.antennaStr3 + "\r\n";
                WaveformWriting an = new WaveformWriting(MainWindow.setupStr);
                string[] antennaNames = new string[30];
                antennaNames = an.waveformNames(SetupContent.antennaStr1);
                if (an.judge(newhelicalantennaname_textBox27.Text, antennaNames))
                {
                    MessageBox.Show("此天线名称已存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }

                MainWindow.setupStr = an.InsertAntenna8(halicalStr, (string)newhelicalwaveformname_comboBox7.SelectedItem, SetupContent.antennaStr3, SetupContent.waveFormStr3);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].Nodes.Add(newhelicalantennaname_textBox27.Text);
                FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + newhelicalantennaname_textBox27.Text, MainWindow.nodeInfoFullPath, true);

                string antennaInfoStr = SetupContent.antennaStr1 + " " + newhelicalantennaname_textBox27.Text + "\r\n"
                                                  + comboBox1.SelectedItem + "\r\n"
                    //+ newhelicalwaveformname_comboBox7.SelectedItem + "\r\n"
                                                    + newhelicalwaveformname_comboBox7.Text + "\r\n"
                                                  + newHelicalpolarization_comboBox3.SelectedItem + "\r\n"
                                                  + newhelicalmaxgain_textBox26.Text + "\r\n"
                                                  + newhelicalradius_textBox28.Text + "\r\n"
                                                  + newhelicallength_textBox29.Text + "\r\n"
                                                  + newhelicalpitch_textBox30.Text + "\r\n"
                                                  + helicalreceiverthreshold_newtextBox25.Text + "\r\n"
                                                  + newhelicaltransmissionlineloss_textBox24.Text + "\r\n"
                                                  + newhelicalvswr_textBox23.Text + "\r\n"
                                                  + newhelicaltemperature_textBox22.Text + "\r\n"
                                                  + "END" + SetupContent.antennaStr1 + " " + newhelicalantennaname_textBox27.Text + "\r\n";
                FileOperation.WriteFile(antennaInfoStr, MainWindow.waveinfoFilePath, true);


            }
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newhelicalantennaname_textBox27.Text + "\"天线创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newHelicalSave_button14_Click(object sender, EventArgs e)
        {
            ServiceReference1.Antenna newHelical = new ServiceReference1.Antenna();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if(MainWindow.newFuncSign)
            {
            newHelical.Name = newhelicalantennaname_textBox27.Text;
            newHelical.Type = Translate.KeyWordsDictionary(comboBox1);
            //newHelical.WaveFormName = (string)newhelicalwaveformname_comboBox7.SelectedItem;
            newHelical.Polarization = Translate.KeyWordsDictionary(newHelicalpolarization_comboBox3);
            newHelical.Radius = double.Parse(newhelicalradius_textBox28.Text);
            newHelical.Length = double.Parse(newhelicallength_textBox29.Text);
            newHelical.Pitch = double.Parse(newhelicalpitch_textBox30.Text);
            //newHelical.MaxGain = double.Parse(newhelicalmaxgain_textBox26.Text);
            newHelical.RecieveThrehold = double.Parse(helicalreceiverthreshold_newtextBox25.Text);
            newHelical.TransmissionLoss = double.Parse(newhelicaltransmissionlineloss_textBox24.Text);
            newHelical.VSWR = double.Parse(newhelicalvswr_textBox23.Text);
            newHelical.Temperature = double.Parse(newhelicaltemperature_textBox22.Text);
            
            try
            {
                if (newhelicalwaveformname_comboBox7.Items.Count < 0)
                {
                    MessageBox.Show("请先在工程中添加波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (newhelicalantennaname_textBox27.Text == "" || newhelicalwaveformname_comboBox7.SelectedItem == null || newHelicalpolarization_comboBox3.SelectedItem == null || newhelicalradius_textBox28.Text == "" || newhelicallength_textBox29.Text == "" || newhelicalpitch_textBox30.Text == "" || helicalreceiverthreshold_newtextBox25.Text == "" || newhelicaltransmissionlineloss_textBox24.Text == "" || newhelicalvswr_textBox23.Text == "" || newhelicaltemperature_textBox22.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;

                }
                if (!BoudingLimition.IsScienceFigure(newhelicalradius_textBox28.Text))
                {
                    MessageBox.Show("螺纹半径值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicalradius_textBox28.Text) < 0.0001)
                {
                    MessageBox.Show("螺纹半径值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicallength_textBox29.Text))
                {
                    MessageBox.Show("天线长度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicallength_textBox29.Text) < 0.0001)
                {
                    MessageBox.Show("天线长度值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicalpitch_textBox30.Text))
                {
                    MessageBox.Show("螺距值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhelicalpitch_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("螺距值应大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(helicalreceiverthreshold_newtextBox25.Text))
                {
                    MessageBox.Show("接受门限值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicaltransmissionlineloss_textBox24.Text))
                {
                    MessageBox.Show("传输线损耗值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicalvswr_textBox23.Text))
                {
                    MessageBox.Show("VSWR值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.VSWRLimition(newhelicalvswr_textBox23))
                {
                    MessageBox.Show("VSWR值应大于等于1", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhelicaltemperature_textBox22.Text))
                {
                    MessageBox.Show("噪声温度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddAntenna(newHelical);
                MessageBox.Show("天线已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
              newHelicalOk_button7_Click(sender, e);
              try
              {
                  newHelical = client.iGetAntenna(newhelicalantennaname_textBox27.Text);
                  newHelical.Name = newhelicalantennaname_textBox27.Text;
                  //addAntenna.Polarization = HelicalPolarizationTranslate;
                  newHelical.Polarization = Translate.KeyWordsDictionary(newHelicalpolarization_comboBox3);
                  //newHelical.MaxGain = double.Parse(newhelicalmaxgain_textBox26.Text);
                  newHelical.Radius = double.Parse(newhelicalradius_textBox28.Text);
                  newHelical.Length = double.Parse(newhelicallength_textBox29.Text);
                  newHelical.Pitch = double.Parse(newhelicalpitch_textBox30.Text);
                  newHelical.RecieveThrehold = double.Parse(helicalreceiverthreshold_newtextBox25.Text);
                  newHelical.TransmissionLoss = double.Parse(newhelicaltransmissionlineloss_textBox24.Text);
                  newHelical.VSWR = double.Parse(newhelicalvswr_textBox23.Text);
                  newHelical.Temperature = double.Parse(newhelicaltemperature_textBox22.Text);
                  client.iUpdateAntenna(newHelical);
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

        }
        private void newHelicalCancelbutton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newLogPeriodicCancelbutton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newLogPeriodicOk_button9_Click(object sender, EventArgs e)
        {

        }
        private void newlogperiodicsave_button15_Click(object sender, EventArgs e)
        {
            ServiceReference1.Antenna newParabolic = new ServiceReference1.Antenna();
            newParabolic.Name = newparabolicantennaname_textBox12.Text;
            newParabolic.Type = Translate.KeyWordsDictionary(comboBox1);
            //newParabolic.WaveFormName = (string)newparabolicwaveformname_comboBox5.SelectedItem;
            newParabolic.Polarization = Translate.KeyWordsDictionary(newparabolicpolarization_comboBox2);
            newParabolic.Radius = double.Parse(newparabolicradius_textBox9.Text);
            newParabolic.Pitch = double.Parse(newparabolicblockageradius_textBox10.Text);
            newParabolic.ApertureDistribution = (string)newparabolicaperturedistribution_comboBox6.SelectedItem;
            newParabolic.EdgeTeper = double.Parse(newparabolicedgetaper_textBox8.Text);
            newParabolic.RecieveThrehold = double.Parse(newparabolicreceiverthreshold_textBox7.Text);
            newParabolic.TransmissionLoss = double.Parse(newparabolictransmissionlineloss_textBox19.Text);
            newParabolic.VSWR = double.Parse(newparabolicvswr_textBox21.Text);
            newParabolic.Temperature = double.Parse(newparabolictemperature_textBox20.Text);
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            try
            {
                client.iAddAntenna(newParabolic);
                MessageBox.Show("天线已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            this.Close();
        }
    }
}
