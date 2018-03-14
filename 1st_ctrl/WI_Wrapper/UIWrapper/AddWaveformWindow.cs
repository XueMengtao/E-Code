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

namespace WF1
{
    public partial class AddWaveformWindow : Form
    {

        TransmitterLib transLib;
        ServiceReference1.Service1Client client = null;
        ServiceReference1.WaveForm addWaveform = null;
        public AddWaveformWindow()
        {
            InitializeComponent();
            transLib = new TransmitterLib();
            client = new ServiceReference1.Service1Client();
            addWaveform = new ServiceReference1.WaveForm();
        }
        //在新建波形之前判断是否新建工程
        private bool Hasbug()
        {
            bool b = false;
            if (MainWindow.setupStr == null)
            {
                if (MainWindow.mProjectFullName == null)
                {
                    MessageBox.Show("请先创建一个工程之后再新建波形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return true;
                }
            }
            return b;
        }
        private void AddWaveformWindow_Load(object sender, EventArgs e)
        {
             
        }
        private void addWaveformType_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string WaveFormType = null;
            WaveFormType = (string)addwaveformtype_comboBox1.SelectedItem;
            switch (WaveFormType)
            {
                case "正弦波<Sinusoid>":
                    {
                        this.addsinusoid_panel1.Visible = true;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中正弦类型后，获取数据库中WaveForm表正弦类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addsinusoidname_comboBox3.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addsinusoidname_comboBox3.Items.Add(s);
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
                case "升余弦<Raised cosine>":
                    {
                        this.addraisedcosine_panel2.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中正弦类型后，获取数据库中WaveForm表正弦类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addraisedcosinename_comboBox4.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addraisedcosinename_comboBox4.Items.Add(s);
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
                case "根号升余弦<Root raised cosine>":
                    {
                        this.addrootraisedcosine_panel3.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中根号升余弦类型后，获取数据库中WaveForm表根号升余弦类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addrrootraisedcosinename_comboBox5.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addrrootraisedcosinename_comboBox5.Items.Add(s);
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
                case "高斯信号<Gaussian>":
                    {
                        this.addgaussian_panel4.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中高斯类型后，获取数据库中WaveForm表高斯类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addgaussianname_comboBox6.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addgaussianname_comboBox6.Items.Add(s);
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
                case "离散高斯信号<Gaussian derivative>":
                    {
                        this.addgaussianderivative_panel5.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中离散高斯类型后，获取数据库中WaveForm表离散高斯类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addgaussianderivativename_comboBox7.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addgaussianderivativename_comboBox7.Items.Add(s);
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
                case "海明波<Hamming>":
                    {
                        this.addhamming_panel6.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中海明类型后，获取数据库中WaveForm表海明类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addhammingname_comboBox8.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addhammingname_comboBox8.Items.Add(s);
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
                case "线性调频波<Chirp>":
                    {
                        this.addchrip_panel7.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addblackman_panel8.Visible = false;
                        //在选中线性调频类型后，获取数据库中WaveForm表线性调频类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addchripname_comboBox9.Items.Clear();
                            foreach (string s in WaveformNames)
                            {
                                addchripname_comboBox9.Items.Add(s);
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
                case "布莱曼波<Blackman>":
                    {
                        this.addblackman_panel8.Visible = true;
                        this.addsinusoid_panel1.Visible = false;
                        this.addraisedcosine_panel2.Visible = false;
                        this.addrootraisedcosine_panel3.Visible = false;
                        this.addgaussian_panel4.Visible = false;
                        this.addgaussianderivative_panel5.Visible = false;
                        this.addhamming_panel6.Visible = false;
                        this.addchrip_panel7.Visible = false;
                        //在选中布莱曼类型后，获取数据库中WaveForm表布莱曼类波形的名称，在波形名称组合框的下拉列表显示。
                        string[] WaveformNames = new string[] { };
                        try
                        {
                            WaveformNames = client.iGetAllWaveForm(Translate.KeyWordsDictionary(addwaveformtype_comboBox1));
                            addblackmanname_comboBox10.Items.Clear();
                            foreach (string s in WaveformNames)
                            {

                                addblackmanname_comboBox10.Items.Add(s);
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
        private void addSinusoidName_comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addsinusoidname_comboBox3.SelectedItem);
                addsinusoidcarrierfrequency_textBox2.Text = addWaveform.Frequency.ToString();
                addsinusoideffectivebandwith_textBox3.Text = addWaveform.BandWidth.ToString();
                addsinusoidphase_textBox4.Text = addWaveform.Phase.ToString();
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
        private void addSinusoidUpdate_button17_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addsinusoidcarrierfrequency_textBox2.Text);
                addWaveform.BandWidth = double.Parse(addsinusoideffectivebandwith_textBox3.Text);
                addWaveform.Phase = double.Parse(addsinusoidphase_textBox4.Text);
                if (addsinusoidname_comboBox3.SelectedItem == null || addsinusoidcarrierfrequency_textBox2.Text == "" || addsinusoideffectivebandwith_textBox3.Text == "" || addsinusoidphase_textBox4.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoidcarrierfrequency_textBox2.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addsinusoidcarrierfrequency_textBox2.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoideffectivebandwith_textBox3.Text))
                {
                    MessageBox.Show("带宽必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addsinusoideffectivebandwith_textBox3.Text) < 0.000)
                {
                    MessageBox.Show("带宽不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoidphase_textBox4.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addsinusoidphase_textBox4))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition(addsinusoideffectivebandwith_textBox3, addsinusoidcarrierfrequency_textBox2))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addSinusoidOk_button1_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if(addsinusoidname_comboBox3.SelectedItem ==null||addsinusoidcarrierfrequency_textBox2.Text==""||addsinusoideffectivebandwith_textBox3.Text ==""||addsinusoidphase_textBox4.Text =="")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                       return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoidcarrierfrequency_textBox2.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addsinusoidcarrierfrequency_textBox2.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoideffectivebandwith_textBox3.Text))
                {
                    MessageBox.Show("带宽必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addsinusoideffectivebandwith_textBox3.Text) < 0.000)
                {
                    MessageBox.Show("带宽不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addsinusoidphase_textBox4.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addsinusoidphase_textBox4))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition(addsinusoideffectivebandwith_textBox3, addsinusoidcarrierfrequency_textBox2))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addsinusoidname_comboBox3.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形在工程中已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = new string[] { (string)addsinusoidname_comboBox3.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addsinusoidcarrierfrequency_textBox2.Text, addsinusoideffectivebandwith_textBox3.Text, addsinusoidphase_textBox4.Text };

                MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, true);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addsinusoidname_comboBox3.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addsinusoidname_comboBox3.SelectedItem);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addsinusoidname_comboBox3.SelectedItem + "\r\n"
                                                 + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                                 + addsinusoidcarrierfrequency_textBox2.Text + "\r\n"
                                                 + addsinusoideffectivebandwith_textBox3.Text + "\r\n"
                                                 + addsinusoidphase_textBox4.Text + "\r\n"
                                                 + "END" + SetupContent.waveFormStr1 + " " + (string)addsinusoidname_comboBox3.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);

                MessageBox.Show("\"" + (string)addsinusoidname_comboBox3.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
        }
        private void addSinusoidCancel_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addRaisedCosineName_comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addraisedcosinename_comboBox4.SelectedItem);
                addraisedcosinecarrierfrequency_textBox6.Text = addWaveform.Frequency.ToString();
                addraisedcosineplusebandwith_textBox7.Text = addWaveform.BandWidth.ToString();
                addraisedcosinephase_textBox8.Text = addWaveform.Phase.ToString();
                addraisedcosinerolloff_textBox9.Text = addWaveform.RollOffFactor.ToString(); 
            }
            catch (FaultException<WcfException> ex)
            {
                MessageBox.Show(ex.Detail.message);
                client.Abort();
            }
            catch (CommunicationException ex)
            {
                MessageBox.Show(ex.Message);
                client.Abort();
            }
            catch (Exception exm)
            {
                LogFileManager.ObjLog.fatal(exm.Message, exm);
            }
        }
        private void addRaisedCosineUpdate_button18_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addraisedcosinecarrierfrequency_textBox6.Text);
                addWaveform.BandWidth = double.Parse(addraisedcosineplusebandwith_textBox7.Text);
                addWaveform.Phase = double.Parse(addraisedcosinephase_textBox8.Text);
                addWaveform.RollOffFactor = double.Parse(addraisedcosinerolloff_textBox9.Text);
                if (addraisedcosinename_comboBox4.SelectedItem == null || addraisedcosinecarrierfrequency_textBox6.Text == "" || addraisedcosineplusebandwith_textBox7.Text == "" || addraisedcosinephase_textBox8.Text == "" || addraisedcosinerolloff_textBox9.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinecarrierfrequency_textBox6.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addraisedcosinecarrierfrequency_textBox6.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosineplusebandwith_textBox7.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addraisedcosineplusebandwith_textBox7.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinephase_textBox8.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addraisedcosinephase_textBox8))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinerolloff_textBox9.Text))
                {
                    MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addraisedcosinerolloff_textBox9))
                {
                    MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addraisedcosineplusebandwith_textBox7, addraisedcosinecarrierfrequency_textBox6))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addRaisedCosineOk_button3_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if (addraisedcosinename_comboBox4.SelectedItem == null || addraisedcosinecarrierfrequency_textBox6.Text == "" || addraisedcosineplusebandwith_textBox7.Text == "" || addraisedcosinephase_textBox8.Text == "" || addraisedcosinerolloff_textBox9.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinecarrierfrequency_textBox6.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addraisedcosinecarrierfrequency_textBox6.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosineplusebandwith_textBox7.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addraisedcosineplusebandwith_textBox7.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinephase_textBox8.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addraisedcosinephase_textBox8))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addraisedcosinerolloff_textBox9.Text))
                {
                    MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addraisedcosinerolloff_textBox9))
                {
                    MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addraisedcosineplusebandwith_textBox7, addraisedcosinecarrierfrequency_textBox6))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addraisedcosinename_comboBox4.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形在工程中已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = { (string)addraisedcosinename_comboBox4.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addraisedcosinecarrierfrequency_textBox6.Text, addraisedcosineplusebandwith_textBox7.Text, addraisedcosinephase_textBox8.Text, addraisedcosinerolloff_textBox9.Text };

                MainWindow.setupStr = wf.InsertWaveform6(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addraisedcosinename_comboBox4.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addraisedcosinename_comboBox4.SelectedItem);

                string waveInfoArr = (SetupContent.waveFormStr1 + " " + (string)addraisedcosinename_comboBox4.SelectedItem + "\r\n"
                                                     + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                                     + addraisedcosinecarrierfrequency_textBox6.Text + "\r\n"
                                                     + addraisedcosineplusebandwith_textBox7.Text + "\r\n"
                                                     + addraisedcosinephase_textBox8.Text + "\r\n"
                                                     + addraisedcosinerolloff_textBox9.Text + "\r\n"
                                                     + "END" + SetupContent.waveFormStr1 + " " + (string)addraisedcosinename_comboBox4.SelectedItem + "\r\n");
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);

                MessageBox.Show("\"" + (string)addraisedcosinename_comboBox4.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }
        private void addRaisedCosineCancel_button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addRootRaisedCosineName_comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addrrootraisedcosinename_comboBox5.SelectedItem);
                addrootraisedcosinecarrierfrequency_textBox11.Text = addWaveform.Frequency.ToString();
                addrootraisedcosineplusewidth_textBox12.Text = addWaveform.BandWidth.ToString();
                addrootraisedcosinephase_textBox13.Text = addWaveform.Phase.ToString();
                addrootraisedcosinerolloff_textBox14.Text = addWaveform.RollOffFactor.ToString();
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
        private void addRootRaisedCosineUpdate_button19_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addrootraisedcosinecarrierfrequency_textBox11.Text);
                addWaveform.BandWidth = double.Parse(addrootraisedcosineplusewidth_textBox12.Text);
                addWaveform.Phase = double.Parse(addrootraisedcosinephase_textBox13.Text);
                addWaveform.RollOffFactor = double.Parse(addrootraisedcosinerolloff_textBox14.Text);
                if (addrrootraisedcosinename_comboBox5.SelectedItem == null || addrootraisedcosinecarrierfrequency_textBox11.Text == "" || addrootraisedcosineplusewidth_textBox12.Text == "" || addrootraisedcosinephase_textBox13.Text == "" || addrootraisedcosinerolloff_textBox14.Text == "")
                {
                    MessageBox.Show("窗口中有未设置定的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinecarrierfrequency_textBox11.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addrootraisedcosinecarrierfrequency_textBox11.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosineplusewidth_textBox12.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addrootraisedcosineplusewidth_textBox12.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinephase_textBox13.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addrootraisedcosinephase_textBox13))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinerolloff_textBox14.Text))
                {
                    MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addrootraisedcosinerolloff_textBox14))
                {
                    MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addrootraisedcosineplusewidth_textBox12, addrootraisedcosinecarrierfrequency_textBox11))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addRootRaisedCosineOk_button5_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if (addrrootraisedcosinename_comboBox5.SelectedItem == null || addrootraisedcosinecarrierfrequency_textBox11.Text == "" || addrootraisedcosineplusewidth_textBox12.Text == "" || addrootraisedcosinephase_textBox13.Text == "" || addrootraisedcosinerolloff_textBox14.Text == "")
                {
                    MessageBox.Show("窗口中有未设置定的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinecarrierfrequency_textBox11.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addrootraisedcosinecarrierfrequency_textBox11.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosineplusewidth_textBox12.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addrootraisedcosineplusewidth_textBox12.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinephase_textBox13.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addrootraisedcosinephase_textBox13))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addrootraisedcosinerolloff_textBox14.Text))
                {
                    MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addrootraisedcosinerolloff_textBox14))
                {
                    MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addrootraisedcosineplusewidth_textBox12, addrootraisedcosinecarrierfrequency_textBox11))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                //matchPosition1 = wf.IndexFun(SetupContent.waveFormStr1);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addrrootraisedcosinename_comboBox5.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = { (string)addrrootraisedcosinename_comboBox5.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addrootraisedcosinecarrierfrequency_textBox11.Text, addrootraisedcosineplusewidth_textBox12.Text, addrootraisedcosinephase_textBox13.Text, addrootraisedcosinerolloff_textBox14.Text };

                MainWindow.setupStr = wf.InsertWaveform6(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addrrootraisedcosinename_comboBox5.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addrrootraisedcosinename_comboBox5.SelectedItem);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addrrootraisedcosinename_comboBox5.SelectedItem + "\r\n"
                                                + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                                + addrootraisedcosinecarrierfrequency_textBox11.Text + "\r\n"
                                                + addrootraisedcosineplusewidth_textBox12.Text + "\r\n"
                                                + addrootraisedcosinephase_textBox13.Text + "\r\n"
                                                + addrootraisedcosinerolloff_textBox14.Text + "\r\n"
                                                + "END" + SetupContent.waveFormStr1 + " " + (string)addrrootraisedcosinename_comboBox5.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);

                MessageBox.Show("\"" + (string)addrrootraisedcosinename_comboBox5.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void addRootRaisedCosineCancel_button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addGaussianName_comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addgaussianname_comboBox6.SelectedItem);
                addgaussiancarrierfrequency_textBox16.Text = addWaveform.Frequency.ToString();
                addgaussianplusewidth_textBox17.Text = addWaveform.BandWidth.ToString();
                addgaussianphase_textBox18.Text = addWaveform.Phase.ToString();
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
        private void addGaussianUpdate_button20_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addgaussiancarrierfrequency_textBox16.Text);
                addWaveform.BandWidth = double.Parse(addgaussianplusewidth_textBox17.Text);
                addWaveform.Phase = double.Parse(addgaussianphase_textBox18.Text);
                if (addgaussianname_comboBox6.SelectedItem == null || addgaussiancarrierfrequency_textBox16.Text == "" || addgaussianplusewidth_textBox17.Text == "" || addgaussianphase_textBox18.Text == "")
                {
                    MessageBox.Show("窗口中有未设置定的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussiancarrierfrequency_textBox16.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussiancarrierfrequency_textBox16.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianplusewidth_textBox17.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussianplusewidth_textBox17.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianphase_textBox18.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addgaussianphase_textBox18))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addgaussianplusewidth_textBox17, addgaussiancarrierfrequency_textBox16))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addGaussianOk_button7_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if (addgaussianname_comboBox6.SelectedItem == null || addgaussiancarrierfrequency_textBox16.Text == "" || addgaussianplusewidth_textBox17.Text == "" || addgaussianphase_textBox18.Text == "")
                {
                    MessageBox.Show("窗口中有未设置定的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussiancarrierfrequency_textBox16.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussiancarrierfrequency_textBox16.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianplusewidth_textBox17.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussianplusewidth_textBox17.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianphase_textBox18.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addgaussianphase_textBox18))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addgaussianplusewidth_textBox17, addgaussiancarrierfrequency_textBox16))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                //matchPosition1 = wf.IndexFun(SetupContent.waveFormStr1);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addgaussianname_comboBox6.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形在工程中已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string[] temp = { (string)addgaussianname_comboBox6.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addgaussiancarrierfrequency_textBox16.Text, addgaussianplusewidth_textBox17.Text, addgaussianphase_textBox18.Text };
                //int[] a = new int[20];

                //将波形块插入到全局字符串中
                MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, false);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addgaussianname_comboBox6.SelectedItem, MainWindow.nodeInfoFullPath, true);
                //添加高斯波形后加载到工程树相应的节点上，并将节点信息记录的.info文件中
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addgaussianname_comboBox6.SelectedItem);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addgaussianname_comboBox6.SelectedItem + "\r\n"
                            + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                            + addgaussiancarrierfrequency_textBox16.Text + "\r\n"
                            + addgaussianplusewidth_textBox17.Text + "\r\n"
                            + addgaussianphase_textBox18.Text + "\r\n"
                            + "END" + SetupContent.waveFormStr1 + " " + (string)addgaussianname_comboBox6.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                MessageBox.Show("\"" + (string)addgaussianname_comboBox6.SelectedItem + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
        private void addGaussianCancel_button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addGaussianDerivativeName_comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addgaussianderivativename_comboBox7.SelectedItem);
                addgaussianderivativeplusewidth_textBox21.Text = addWaveform.BandWidth.ToString();
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
        private void addGaussianDerivativeUpdate_button21_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addgaussianderivativecarrierfrequency_textBox20.Text);
                addWaveform.BandWidth = double.Parse(addgaussianderivativeplusewidth_textBox21.Text);
                if (addgaussianderivativename_comboBox7.SelectedItem == null || addgaussianderivativecarrierfrequency_textBox20.Text == "" || addgaussianderivativeplusewidth_textBox21.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianderivativeplusewidth_textBox21.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussianderivativeplusewidth_textBox21.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addGaussianDerivativeOk_button9_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if (addgaussianderivativename_comboBox7.SelectedItem == null || addgaussianderivativecarrierfrequency_textBox20.Text == "" || addgaussianderivativeplusewidth_textBox21.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addgaussianderivativeplusewidth_textBox21.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addgaussianderivativeplusewidth_textBox21.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                //matchPosition1 = wf.IndexFun(SetupContent.waveFormStr1);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addgaussianderivativename_comboBox7.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形在工程中已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = { (string)addgaussianderivativename_comboBox7.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), "0", addgaussianderivativeplusewidth_textBox21.Text + "\r\nDispersive", "0" };
                //int[] a = new int[20];

                //a = wf.IndexFun(SetupContent.waveFormStr2);
                MainWindow.setupStr = wf.InsertWaveform4(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addgaussianderivativename_comboBox7.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addgaussianderivativename_comboBox7.SelectedItem);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addgaussianderivativename_comboBox7.SelectedItem + "\r\n"
                                             + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                              + "9.155" + "\r\n"
                                             + addgaussianderivativeplusewidth_textBox21.Text + "\r\n"
                                             + "END" + SetupContent.waveFormStr1 + " " + (string)addgaussianderivativename_comboBox7.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                MessageBox.Show("\"" + (string)addgaussianderivativename_comboBox7.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private void addGaussianDerivativeCancel_button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addHammingName_comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addhammingname_comboBox8.SelectedItem);
                addhammingcarrierfrequency_textBox23.Text = addWaveform.Frequency.ToString();
                addhammingplusewidth_textBox24.Text = addWaveform.BandWidth.ToString();
                addhammingphase_textBox25.Text = addWaveform.Phase.ToString();
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
        private void addHammingUpdate_button22_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addhammingcarrierfrequency_textBox23.Text);
                addWaveform.BandWidth = double.Parse(addhammingplusewidth_textBox24.Text);
                addWaveform.Phase = double.Parse(addhammingphase_textBox25.Text);
                if (addblackmanname_comboBox10.SelectedItem == null || addblackmancarrierfrequency_textBox33.Text == "" || addblackmanplusewidth_textBox34.Text == "" || addblackmanphase_textBox35.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingcarrierfrequency_textBox23.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addhammingcarrierfrequency_textBox23.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingplusewidth_textBox24.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addhammingplusewidth_textBox24.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingphase_textBox25.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addhammingphase_textBox25))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addhammingplusewidth_textBox24, addhammingcarrierfrequency_textBox23))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Hamming(addhammingplusewidth_textBox24, addhammingcarrierfrequency_textBox23))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addHammingOk_button11_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {
                if (addblackmanname_comboBox10.SelectedItem == null || addblackmancarrierfrequency_textBox33.Text == "" || addblackmanplusewidth_textBox34.Text == "" || addblackmanphase_textBox35.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingcarrierfrequency_textBox23.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addhammingcarrierfrequency_textBox23.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingplusewidth_textBox24.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addhammingplusewidth_textBox24.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addhammingphase_textBox25.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addhammingphase_textBox25))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addhammingplusewidth_textBox24, addhammingcarrierfrequency_textBox23))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Hamming(addhammingplusewidth_textBox24, addhammingcarrierfrequency_textBox23))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }

                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                //matchPosition1 = wf.IndexFun(SetupContent.waveFormStr1);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addhammingname_comboBox8.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形在工程中已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = { (string)addhammingname_comboBox8.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addhammingcarrierfrequency_textBox23.Text, addhammingplusewidth_textBox24.Text, addhammingphase_textBox25.Text };
                //int[] a = new int[20];

                //a = wf.IndexFun(SetupContent.waveFormStr2);
                MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, false);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addhammingname_comboBox8.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addhammingname_comboBox8.SelectedItem);

                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addhammingname_comboBox8.SelectedItem + "\r\n"
                                                   + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                                   + addhammingcarrierfrequency_textBox23.Text + "\r\n"
                                                   + addhammingplusewidth_textBox24.Text + "\r\n"
                                                   + addhammingphase_textBox25.Text + "\r\n"
                                                   + "END" + SetupContent.waveFormStr1 + " " + (string)addhammingname_comboBox8.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                MessageBox.Show("\"" + (string)addhammingname_comboBox8.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private void addHammingCancel_button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addChripName_comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addchripname_comboBox9.SelectedItem);
                string FreChangeRate1 = null;
                string FreChangeRateValue1 = null;
                FreChangeRate1 = addWaveform.FreChangeRate;
                switch (FreChangeRate1)
                {
                    case "Linear": 
                        FreChangeRateValue1 = "线性变化<Linear>";
                        break;
                    case "Exponential": 
                        FreChangeRateValue1 = "指数变化<Exponential>"; 
                        break;
                  default:
                      break;                
                }
                addchripcarrierfrequency_textBox27.Text = addWaveform.Frequency.ToString();
                addchripplusewidth_textBox28.Text = addWaveform.BandWidth.ToString();
                addchriprolloff_textBox29.Text = addWaveform.RollOffFactor.ToString();
                addChripFrequencyChangeRate_textBox1.Text = FreChangeRateValue1; 
                addchripstartfrequency_textBox30.Text = addWaveform.EndFrequency.ToString();
                addchripstopfrequency_textBox31.Text = addWaveform.StartFrequency.ToString();
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
        private void addChripUpdate_button23_Click(object sender, EventArgs e)
        {
            try
            {
                string FreChangeRate2 = null;
                string FreChangeRateValue2 = null;
                FreChangeRate2 = addChripFrequencyChangeRate_textBox1.Text;
                switch (FreChangeRate2)
                {
                    case "线性变化<Linear>": FreChangeRateValue2 = "Linear"; break;
                    case "指数变化<Exponential>": FreChangeRateValue2 = "Exponential"; break;
                    default: break;
                }
                addWaveform = client.iGetWaveForm((string)addchripname_comboBox9.SelectedItem);
                addWaveform.Frequency = double.Parse(addchripcarrierfrequency_textBox27.Text);
                addWaveform.BandWidth = double.Parse(addchripplusewidth_textBox28.Text);
                addWaveform.RollOffFactor = double.Parse(addchriprolloff_textBox29.Text);
                addWaveform.FreChangeRate = FreChangeRateValue2;
                addWaveform.StartFrequency = double.Parse(addchripstartfrequency_textBox30.Text);
                addWaveform.EndFrequency = double.Parse(addchripstopfrequency_textBox31.Text);
                if(addchripname_comboBox9.SelectedItem ==null||addchripcarrierfrequency_textBox27.Text ==""||addchripplusewidth_textBox28.Text ==""||addchriprolloff_textBox29.Text ==""||addChripFrequencyChangeRate_textBox1.Text==""||addchripstartfrequency_textBox30.Text==""||addchripstopfrequency_textBox31.Text=="")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripcarrierfrequency_textBox27.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripcarrierfrequency_textBox27.Text) < 0.000)
                {
                    MessageBox.Show("载波频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripplusewidth_textBox28.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripplusewidth_textBox28.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchriprolloff_textBox29.Text))
                {
                    MessageBox.Show("滚降系数为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addchriprolloff_textBox29))
                {
                    MessageBox.Show("滚降系数值需在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripstartfrequency_textBox30.Text))
                {
                    MessageBox.Show("起始频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripstartfrequency_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("起始频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripstopfrequency_textBox31.Text))
                {
                    MessageBox.Show("截止频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripstopfrequency_textBox31.Text) < 0.000)
                {
                    MessageBox.Show("截止频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addChripOk_button13_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {

                if (addchripname_comboBox9.SelectedItem == null || addchripcarrierfrequency_textBox27.Text == "" || addchripplusewidth_textBox28.Text == "" || addchriprolloff_textBox29.Text == "" || addChripFrequencyChangeRate_textBox1.Text == "" || addchripstartfrequency_textBox30.Text == "" || addchripstopfrequency_textBox31.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addchripname_comboBox9.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripcarrierfrequency_textBox27.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripcarrierfrequency_textBox27.Text) < 0.000)
                {
                    MessageBox.Show("载波频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripplusewidth_textBox28.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripplusewidth_textBox28.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchriprolloff_textBox29.Text))
                {
                    MessageBox.Show("滚降系数为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(addchriprolloff_textBox29))
                {
                    MessageBox.Show("滚降系数值需在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripstartfrequency_textBox30.Text))
                {
                    MessageBox.Show("起始频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripstartfrequency_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("起始频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addchripstopfrequency_textBox31.Text))
                {
                    MessageBox.Show("截止频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addchripstopfrequency_textBox31.Text) < 0.000)
                {
                    MessageBox.Show("截止频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string FreChangeRate2 = null;
                string FreChangeRateValue2 = null;
                FreChangeRate2 = addChripFrequencyChangeRate_textBox1.Text;
                switch (FreChangeRate2)
                {
                    case "线性变化<Linear>": FreChangeRateValue2 = "Linear"; break;
                    case "指数变化<Exponential>": FreChangeRateValue2 = "Exponential"; break;
                    default: break;
                }
                string[] temp = { (string)addchripname_comboBox9.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addchripcarrierfrequency_textBox27.Text, addchripplusewidth_textBox28.Text, "0.000", addchriprolloff_textBox29.Text, FreChangeRateValue2, addchripstartfrequency_textBox30.Text, addchripstopfrequency_textBox31.Text };
                //int[] a = new int[20];

                //a = wf.IndexFun(SetupContent.waveFormStr2);
                MainWindow.setupStr = wf.InsertWaveform8(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addchripname_comboBox9.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addchripname_comboBox9.SelectedItem);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addchripname_comboBox9.SelectedItem + "\r\n"
                                    + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                    + addchripcarrierfrequency_textBox27.Text + "\r\n"
                                    + addchripplusewidth_textBox28.Text + "\r\n"
                                    + addchriprolloff_textBox29.Text + "\r\n"
                                    + FreChangeRateValue2 + "\r\n"
                                     + addchripstartfrequency_textBox30.Text + "\r\n"
                                      + addchripstopfrequency_textBox31.Text + "\r\n"
                                    + "END" + SetupContent.waveFormStr1 + " " + (string)addchripname_comboBox9.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                MessageBox.Show("\"" + (string)addchripname_comboBox9.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        private void addChripCancel_button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addBlackmanName_comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                addWaveform = client.iGetWaveForm((string)addblackmanname_comboBox10.SelectedItem);
                addblackmancarrierfrequency_textBox33.Text = addWaveform.Frequency.ToString();
                addblackmanplusewidth_textBox34.Text = addWaveform.BandWidth.ToString();
                addblackmanphase_textBox35.Text = addWaveform.Phase.ToString(); 
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
        private void addBlackmanCancel_button16_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void addBlackmanUpdate_button24_Click(object sender, EventArgs e)
        {
            try
            {
                addWaveform.Frequency = double.Parse(addblackmancarrierfrequency_textBox33.Text);
                addWaveform.BandWidth = double.Parse(addblackmanplusewidth_textBox34.Text);
                addWaveform.Phase = double.Parse(addblackmanphase_textBox35.Text);
                if (addblackmanname_comboBox10.SelectedItem == null || addblackmancarrierfrequency_textBox33.Text == "" || addblackmanplusewidth_textBox34.Text == "" || addblackmanphase_textBox35.Text == "" )
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmancarrierfrequency_textBox33.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addblackmancarrierfrequency_textBox33.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmanplusewidth_textBox34.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addblackmanplusewidth_textBox34.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmanphase_textBox35.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addblackmanphase_textBox35))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addblackmanplusewidth_textBox34, addblackmancarrierfrequency_textBox33))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Blackman(addblackmanplusewidth_textBox34, addblackmancarrierfrequency_textBox33))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iUpdateWaveForm(addWaveform);
                MessageBox.Show("波形已成功更新至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void addBlackmanOk_button15_Click(object sender, EventArgs e)
        {
            if (Hasbug())
                return;
            else
            {

                if (addblackmanname_comboBox10.SelectedItem == null || addblackmancarrierfrequency_textBox33.Text == "" || addblackmanplusewidth_textBox34.Text == "" || addblackmanphase_textBox35.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmancarrierfrequency_textBox33.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addblackmancarrierfrequency_textBox33.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmanplusewidth_textBox34.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(addblackmanplusewidth_textBox34.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(addblackmanphase_textBox35.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(addblackmanphase_textBox35))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(addblackmanplusewidth_textBox34, addblackmancarrierfrequency_textBox33))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Blackman(addblackmanplusewidth_textBox34, addblackmancarrierfrequency_textBox33))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                //ArrayList arrayWaveForm = new ArrayList();
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                //matchPosition1 = wf.IndexFun(SetupContent.waveFormStr1);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge((string)addblackmanname_comboBox10.SelectedItem, waveNames))
                {
                    MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = { (string)addblackmanname_comboBox10.SelectedItem, Translate.KeyWordsDictionary(addwaveformtype_comboBox1), addblackmancarrierfrequency_textBox33.Text, addblackmanplusewidth_textBox34.Text, addblackmanphase_textBox35.Text };
                //int[] a = new int[20];

                //a = wf.IndexFun(SetupContent.waveFormStr2);
                MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, false);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + (string)addblackmanname_comboBox10.SelectedItem, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add((string)addblackmanname_comboBox10.SelectedItem);

                string waveInfoArr = SetupContent.waveFormStr1 + " " + (string)addblackmanname_comboBox10.SelectedItem + "\r\n"
                                       + (string)addwaveformtype_comboBox1.SelectedItem + "\r\n"
                                       + addblackmancarrierfrequency_textBox33.Text + "\r\n"
                                       + addblackmanplusewidth_textBox34.Text + "\r\n"
                                       + addblackmanphase_textBox35.Text + "\r\n"
                                       + "END" + SetupContent.waveFormStr1 + " " + (string)addblackmanname_comboBox10.SelectedItem + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                MessageBox.Show("\"" + (string)addblackmanname_comboBox10.SelectedItem + "\"波形添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           }        
    }     
}

