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
    public partial class NewWaveformWindow : Form
    {
        
        TransmitterLib transLib;
        public NewWaveformWindow()
        {
            InitializeComponent();
            transLib = new TransmitterLib();
        }
        //在新建波形之前判断是否新建工程
        private bool Hasbug()
        {
            bool b = false;
            if (MainWindow.setupStr == null)
            {
                if (MainWindow.mProjectFullName == null)
                {
                    //MessageBox.Show("请先创建一个工程之后再新建波形!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return true;
                }
            }
            return b;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string WaveFormType = null;
            WaveFormType = (string)comboBox1.SelectedItem;
            switch (WaveFormType)
            {
                case "正弦波<Sinusoid>":
                    {
                        this.newsinusoid_panel1.Visible = true;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "升余弦<Raised cosine>":
                    {
                        this.newraisedcosine_panel2.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "根号升余弦<Root raised cosine>":
                    {
                        this.newrootraisedcosine_panel3.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "高斯信号<Gaussian>":
                    {
                        this.newgaussian_panel4.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "离散高斯信号<Gaussian derivative>":
                    {
                        this.newgaussianderivative_panel5.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "布莱曼波<Blackman>":
                    {
                        this.newblackman_panel6.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newchrip_panel7.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "线性调频波<Chirp>":
                    {
                        this.newchrip_panel7.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newhamming_panel8.Visible = false;
                    }
                    break;
                case "海明波<Hamming>":
                    {
                        this.newhamming_panel8.Visible = true;
                        this.newsinusoid_panel1.Visible = false;
                        this.newraisedcosine_panel2.Visible = false;
                        this.newrootraisedcosine_panel3.Visible = false;
                        this.newgaussian_panel4.Visible = false;
                        this.newgaussianderivative_panel5.Visible = false;
                        this.newblackman_panel6.Visible = false;
                        this.newchrip_panel7.Visible = false;

                    }
                    break;
                default:
                    break;
            }
        }
        private void newSinusoidOk_button1_Click(object sender, EventArgs e)
        {

            {
                if (Hasbug())
                {

                    this.Close();
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (newsinusoidname_textBox1.Text == "" || newsinusoidcarrierfrequency_textBox2.Text == "" || newsinusoideffectivebandwith_textBox3.Text == "" || newsinusoidphase_textBox4.Text=="")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return ;
                }
                if (!BoudingLimition.IsScienceFigure(newsinusoidcarrierfrequency_textBox2.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newsinusoidcarrierfrequency_textBox2.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newsinusoideffectivebandwith_textBox3.Text))
                {
                    MessageBox.Show("带宽必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newsinusoideffectivebandwith_textBox3.Text) < 0.000)
                {
                    MessageBox.Show("带宽不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newsinusoidphase_textBox4.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(newsinusoidphase_textBox4))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition(newsinusoideffectivebandwith_textBox3, newsinusoidcarrierfrequency_textBox2))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] waveNames = new string[30];
                WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
                waveNames = wf.waveformNames(SetupContent.waveFormStr1);
                if (wf.judge(newsinusoidname_textBox1.Text, waveNames))
                {
                    MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK , MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                string[] temp = new string[] { newsinusoidname_textBox1.Text, Translate.KeyWordsDictionary(comboBox1), newsinusoidcarrierfrequency_textBox2.Text, newsinusoideffectivebandwith_textBox3.Text, newsinusoidphase_textBox4.Text };

                MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, true);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newsinusoidname_textBox1.Text, MainWindow.nodeInfoFullPath, true);
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newsinusoidname_textBox1.Text);
                string waveInfoArr = SetupContent.waveFormStr1 + " " + newsinusoidname_textBox1.Text + "\r\n"
                                                 + (string)comboBox1.SelectedItem + "\r\n"
                                                 + newsinusoidcarrierfrequency_textBox2.Text + "\r\n"
                                                 + newsinusoideffectivebandwith_textBox3.Text + "\r\n"
                                                 + newsinusoidphase_textBox4.Text + "\r\n"
                                                 + "END" + SetupContent.waveFormStr1 + " " + newsinusoidname_textBox1.Text + "\r\n";
                FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
                if (MainWindow.creatSuccMesDisp)
                {
                    MessageBox.Show("\"" + newsinusoidname_textBox1.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void newSinusoidSave_button17_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newSinusoid = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newSinusoid.Name = newsinusoidname_textBox1.Text;
                newSinusoid.Type = Translate.KeyWordsDictionary(comboBox1);
                newSinusoid.Frequency = Double.Parse(newsinusoidcarrierfrequency_textBox2.Text);
                newSinusoid.BandWidth = Double.Parse(newsinusoideffectivebandwith_textBox3.Text);
                newSinusoid.Phase = Double.Parse(newsinusoidphase_textBox4.Text);
                try
                {
                    if (newsinusoidname_textBox1.Text == "" || newsinusoidcarrierfrequency_textBox2.Text == "" || newsinusoideffectivebandwith_textBox3.Text == "" || newsinusoidphase_textBox4.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newsinusoidcarrierfrequency_textBox2.Text))
                    {
                        MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newsinusoidcarrierfrequency_textBox2.Text) < 0.000)
                    {
                        MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newsinusoideffectivebandwith_textBox3.Text))
                    {
                        MessageBox.Show("带宽必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newsinusoideffectivebandwith_textBox3.Text) < 0.000)
                    {
                        MessageBox.Show("带宽不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newsinusoidphase_textBox4.Text))
                    {
                        MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.PhaseLimition(newsinusoidphase_textBox4))
                    {
                        MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.EffectiveBandwidthLimition(newsinusoideffectivebandwith_textBox3, newsinusoidcarrierfrequency_textBox2))
                    {
                        MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddWaveForm(newSinusoid);
                    MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newSinusoidOk_button1_Click(sender,e);
                try
                {
                    newSinusoid = client.iGetWaveForm(newsinusoidname_textBox1.Text);

                    newSinusoid.Name = newsinusoidname_textBox1.Text;
                    newSinusoid.Frequency = double.Parse(newsinusoidcarrierfrequency_textBox2.Text);
                    newSinusoid.BandWidth = double.Parse(newsinusoideffectivebandwith_textBox3.Text);
                    newSinusoid.Phase = double.Parse(newsinusoidphase_textBox4.Text);
                    client.iUpdateWaveForm(newSinusoid);
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
            

        } 
        private void newSinusoidCancel_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newRaisedCosineSave_button18_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newRaisedCosine = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newRaisedCosine.Name = newraisedcosinename_textBox5.Text;
                newRaisedCosine.Type = Translate.KeyWordsDictionary(comboBox1);
                newRaisedCosine.Frequency = Double.Parse(newraisedcosinecarrierfrequency_textBox6.Text);
                newRaisedCosine.BandWidth = Double.Parse(newraisedcosineplusebandwith_textBox7.Text);
                newRaisedCosine.Phase = Double.Parse(newraisedcosinephase_textBox8.Text);
                newRaisedCosine.RollOffFactor = Double.Parse(newraisedcosinerolloff_textBox9.Text);

                try
                {

                    if (newraisedcosinename_textBox5.Text == "" || newraisedcosinecarrierfrequency_textBox6.Text == "" || newraisedcosineplusebandwith_textBox7.Text == "" || newraisedcosinephase_textBox8.Text == "" || newraisedcosinephase_textBox8.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newraisedcosinecarrierfrequency_textBox6.Text))
                    {
                        MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newraisedcosinecarrierfrequency_textBox6.Text) < 0.000)
                    {
                        MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newraisedcosineplusebandwith_textBox7.Text))
                    {
                        MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newraisedcosineplusebandwith_textBox7.Text) < 0.000)
                    {
                        MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newraisedcosinephase_textBox8.Text))
                    {
                        MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.PhaseLimition(newraisedcosinephase_textBox8))
                    {
                        MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newraisedcosinerolloff_textBox9.Text))
                    {
                        MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.Roll0ffFactorLimition(newraisedcosinerolloff_textBox9))
                    {
                        MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.PluseWidthLimition(newraisedcosineplusebandwith_textBox7, newraisedcosinecarrierfrequency_textBox6))
                    {
                        MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    client.iAddWaveForm(newRaisedCosine);
                    MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newRaisedCosineOk_button3_Click(sender, e);
                try
                {
                    newRaisedCosine = client.iGetWaveForm(newraisedcosinename_textBox5.Text);
                    newRaisedCosine.Name = newraisedcosinename_textBox5.Text;
                    newRaisedCosine.Frequency = double.Parse(newraisedcosinecarrierfrequency_textBox6.Text);
                    newRaisedCosine.BandWidth = double.Parse(newraisedcosineplusebandwith_textBox7.Text);
                    newRaisedCosine.Phase = double.Parse(newraisedcosinephase_textBox8.Text);
                    newRaisedCosine.RollOffFactor = double.Parse(newraisedcosinerolloff_textBox9.Text);
                    client.iUpdateWaveForm(newRaisedCosine);
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
        }        
        private void newRaisedCosineOk_button3_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (newraisedcosinename_textBox5.Text == "" || newraisedcosinecarrierfrequency_textBox6.Text == "" || newraisedcosineplusebandwith_textBox7.Text == "" || newraisedcosinephase_textBox8.Text == "" || newraisedcosinephase_textBox8.Text=="")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newraisedcosinecarrierfrequency_textBox6.Text))
            {
                MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newraisedcosinecarrierfrequency_textBox6.Text) < 0.000)
            {
                MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newraisedcosineplusebandwith_textBox7.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newraisedcosineplusebandwith_textBox7.Text) < 0.000)
            {
                MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newraisedcosinephase_textBox8.Text))
            {
                MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PhaseLimition(newraisedcosinephase_textBox8))
            {
                MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newraisedcosinerolloff_textBox9.Text))
            {
                MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.Roll0ffFactorLimition(newraisedcosinerolloff_textBox9))
            {
                MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PluseWidthLimition(newraisedcosineplusebandwith_textBox7, newraisedcosinecarrierfrequency_textBox6))
            {
                MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] waveNames = new string[30];
            WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
            waveNames = wf.waveformNames(SetupContent.waveFormStr1);
            if (wf.judge(newraisedcosinename_textBox5.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newraisedcosinename_textBox5.Text, Translate.KeyWordsDictionary(comboBox1), newraisedcosinecarrierfrequency_textBox6.Text, newraisedcosineplusebandwith_textBox7.Text, newraisedcosinephase_textBox8.Text, newraisedcosinerolloff_textBox9.Text };

            MainWindow.setupStr = wf.InsertWaveform6(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newraisedcosinename_textBox5.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newraisedcosinename_textBox5.Text);

            string waveInfoArr = (SetupContent.waveFormStr1 + " " + newraisedcosinename_textBox5.Text + "\r\n"
                                                 + (string)comboBox1.SelectedItem + "\r\n"
                                                 + newraisedcosinecarrierfrequency_textBox6.Text + "\r\n"
                                                 + newraisedcosineplusebandwith_textBox7.Text + "\r\n"
                                                 + newraisedcosinephase_textBox8.Text + "\r\n"
                                                 + newraisedcosinerolloff_textBox9.Text + "\r\n"
                                                 + "END" + SetupContent.waveFormStr1 + " " + newraisedcosinename_textBox5.Text + "\r\n");
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newraisedcosinename_textBox5.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newRaisedCosineCancel_button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newRootRaisedCosineSave_button19_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newRootRaisedCosine = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
                newRootRaisedCosine.Name = newrootraisedcosinename_textBox10.Text;
                newRootRaisedCosine.Type = Translate.KeyWordsDictionary(comboBox1);
                newRootRaisedCosine.Frequency = Double.Parse(newrootraisedcosinecarrierfrequency_textBox11.Text);
                newRootRaisedCosine.BandWidth = Double.Parse(newrootraisedcosineplusewidth_textBox12.Text);
                newRootRaisedCosine.Phase = Double.Parse(newrootraisedcosinephase_textBox13.Text);
                newRootRaisedCosine.RollOffFactor = Double.Parse(newrootraisedcosinerolloff_textBox14.Text);

                try
                {

                    if (newrootraisedcosinename_textBox10.Text == "" || newrootraisedcosinecarrierfrequency_textBox11.Text == "" || newrootraisedcosineplusewidth_textBox12.Text == "" || newrootraisedcosinephase_textBox13.Text == "" || newrootraisedcosinerolloff_textBox14.Text == "")
                    {
                        MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (!BoudingLimition.IsScienceFigure(newrootraisedcosinecarrierfrequency_textBox11.Text))
                    {
                        MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newrootraisedcosinecarrierfrequency_textBox11.Text) < 0.000)
                        {
                            MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MainWindow.IsReturnMidwayInNewProcess = true;
                            return;
                        }
                    if (!BoudingLimition.IsScienceFigure(newrootraisedcosineplusewidth_textBox12.Text))
                    {
                        MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (double.Parse(newrootraisedcosineplusewidth_textBox12.Text) < 0.000)
                        {
                            MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MainWindow.IsReturnMidwayInNewProcess = true;
                            return;
                        }
                    if (!BoudingLimition.IsScienceFigure(newrootraisedcosinephase_textBox13.Text))
                    {
                        MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.PhaseLimition(newrootraisedcosinephase_textBox13))
                        {
                            MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MainWindow.IsReturnMidwayInNewProcess = true;
                            return;
                        }
                    if (!BoudingLimition.IsScienceFigure(newrootraisedcosinerolloff_textBox14.Text))
                    {
                        MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MainWindow.IsReturnMidwayInNewProcess = true;
                        return;
                    }
                    if (BoudingLimition.Roll0ffFactorLimition(newrootraisedcosinerolloff_textBox14))
                        {
                            MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MainWindow.IsReturnMidwayInNewProcess = true;
                            return;
                        }
                     if (BoudingLimition.PluseWidthLimition(newrootraisedcosineplusewidth_textBox12, newrootraisedcosinecarrierfrequency_textBox11))
                        {
                            MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MainWindow.IsReturnMidwayInNewProcess = true;
                            return;
                        }
                    client.iAddWaveForm(newRootRaisedCosine);
                    MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newRootRaisedCosineOk_button5_Click(sender,  e);
                try
                {
                    newRootRaisedCosine = client.iGetWaveForm(newrootraisedcosinename_textBox10.Text);
                    newRootRaisedCosine.Name = newrootraisedcosinename_textBox10.Text;
                    newRootRaisedCosine.Frequency = double.Parse(newrootraisedcosinecarrierfrequency_textBox11.Text);
                    newRootRaisedCosine.BandWidth = double.Parse(newrootraisedcosineplusewidth_textBox12.Text);
                    newRootRaisedCosine.Phase = double.Parse(newrootraisedcosinephase_textBox13.Text);
                    newRootRaisedCosine.RollOffFactor = double.Parse(newrootraisedcosinerolloff_textBox14.Text);
                    client.iUpdateWaveForm(newRootRaisedCosine);
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
        }
        private void newRootRaisedCosineOk_button5_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }

            if (newrootraisedcosinename_textBox10.Text == "" || newrootraisedcosinecarrierfrequency_textBox11.Text == "" || newrootraisedcosineplusewidth_textBox12.Text == "" || newrootraisedcosinephase_textBox13.Text == "" || newrootraisedcosinerolloff_textBox14.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newrootraisedcosinecarrierfrequency_textBox11.Text))
            {
                MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newrootraisedcosinecarrierfrequency_textBox11.Text) < 0.000)
            {
                MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newrootraisedcosineplusewidth_textBox12.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newrootraisedcosineplusewidth_textBox12.Text) < 0.000)
            {
                MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newrootraisedcosinephase_textBox13.Text))
            {
                MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PhaseLimition(newrootraisedcosinephase_textBox13))
            {
                MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newrootraisedcosinerolloff_textBox14.Text))
            {
                MessageBox.Show("滚降系数必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.Roll0ffFactorLimition(newrootraisedcosinerolloff_textBox14))
            {
                MessageBox.Show("滚降系数应在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PluseWidthLimition(newrootraisedcosineplusewidth_textBox12, newrootraisedcosinecarrierfrequency_textBox11))
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
            if (wf.judge(newrootraisedcosinename_textBox10.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newrootraisedcosinename_textBox10.Text, Translate.KeyWordsDictionary(comboBox1), newrootraisedcosinecarrierfrequency_textBox11.Text, newrootraisedcosineplusewidth_textBox12.Text, newrootraisedcosinephase_textBox13.Text, newrootraisedcosinerolloff_textBox14.Text };

            MainWindow.setupStr = wf.InsertWaveform6(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newrootraisedcosinename_textBox10.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newrootraisedcosinename_textBox10.Text);
            string waveInfoArr = SetupContent.waveFormStr1 + " " + newrootraisedcosinename_textBox10.Text + "\r\n"
                                            + (string)comboBox1.SelectedItem + "\r\n"
                                            + newrootraisedcosinecarrierfrequency_textBox11.Text + "\r\n"
                                            + newrootraisedcosineplusewidth_textBox12.Text + "\r\n"
                                            + newrootraisedcosinephase_textBox13.Text + "\r\n"
                                            + newrootraisedcosinerolloff_textBox14.Text + "\r\n"
                                            + "END" + SetupContent.waveFormStr1 + " " + newrootraisedcosinename_textBox10.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newrootraisedcosinename_textBox10.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newRootRaisedCosineCancel_button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newGaussianOk_button7_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }

            if (newgaussianname_textBox15.Text == "" || newgaussiancarrierfrequency_textBox16.Text == "" || newgaussianplusewidth_textBox17.Text == "" || newgaussianphase_textBox18.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newgaussiancarrierfrequency_textBox16.Text))
            {
                MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newgaussiancarrierfrequency_textBox16.Text) < 0.000)
            {
                MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newgaussianplusewidth_textBox17.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newgaussianplusewidth_textBox17.Text) < 0.000)
            {
                MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newgaussianphase_textBox18.Text))
            {
                MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PhaseLimition(newgaussianphase_textBox18))
            {
                MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PluseWidthLimition(newgaussianplusewidth_textBox17, newgaussiancarrierfrequency_textBox16))
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
            if (wf.judge(newgaussianname_textBox15.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newgaussianname_textBox15.Text, Translate.KeyWordsDictionary(comboBox1), newgaussiancarrierfrequency_textBox16.Text, newgaussianplusewidth_textBox17.Text, newgaussianphase_textBox18.Text };
            //int[] a = new int[20];

            //将波形块插入到全局字符串中
            MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, false);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newgaussianname_textBox15.Text, MainWindow.nodeInfoFullPath, true);
            //添加高斯波形后加载到工程树相应的节点上，并将节点信息记录的.info文件中
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newgaussianname_textBox15.Text);
            string waveInfoArr = SetupContent.waveFormStr1 + " " + newgaussianname_textBox15.Text + "\r\n"
                        + (string)comboBox1.SelectedItem + "\r\n"
                        + newgaussiancarrierfrequency_textBox16.Text + "\r\n"
                        + newgaussianplusewidth_textBox17.Text + "\r\n"
                        + newgaussianphase_textBox18.Text + "\r\n"
                        + "END" + SetupContent.waveFormStr1 + " " + newgaussianname_textBox15.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newgaussianname_textBox15.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newGaussianSave_button20_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newGaussian = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
            newGaussian.Name = newgaussianname_textBox15.Text;
            newGaussian.Type = Translate.KeyWordsDictionary(comboBox1);
            newGaussian.Frequency = Double.Parse(newgaussiancarrierfrequency_textBox16.Text);
            newGaussian.BandWidth = Double.Parse(newgaussianplusewidth_textBox17.Text);
            newGaussian.Phase = Double.Parse(newgaussianphase_textBox18.Text);
            
            try
            {

                if (newgaussianname_textBox15.Text == "" || newgaussiancarrierfrequency_textBox16.Text == "" || newgaussianplusewidth_textBox17.Text == "" || newgaussianphase_textBox18.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newgaussiancarrierfrequency_textBox16.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newgaussiancarrierfrequency_textBox16.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newgaussianplusewidth_textBox17.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newgaussianplusewidth_textBox17.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newgaussianphase_textBox18.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(newgaussianphase_textBox18))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(newgaussianplusewidth_textBox17, newgaussiancarrierfrequency_textBox16))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddWaveForm(newGaussian);
                MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newGaussianOk_button7_Click( sender, e);
                try
                {
                    newGaussian = client.iGetWaveForm(newgaussianname_textBox15.Text);
                    newGaussian.Name = newgaussianname_textBox15.Text;
                    newGaussian.Frequency = double.Parse(newgaussiancarrierfrequency_textBox16.Text);
                    newGaussian.BandWidth = double.Parse(newgaussianplusewidth_textBox17.Text);
                    newGaussian.Phase = double.Parse(newgaussianphase_textBox18.Text);
                    client.iUpdateWaveForm(newGaussian);
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
        }
        private void newGaussianCancel_button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newGaussianDerivativeOk_button9_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (newgaussianderivativename_textBox19.Text == "" || newGaussianDerivativeBandWidth_textBox21.Text == "" || newGaussianDerivativeCarrierFrenquency_textBox20.Text == "" )
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newGaussianDerivativeBandWidth_textBox21.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newGaussianDerivativeBandWidth_textBox21.Text) < 0.000)
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
            if (wf.judge(newgaussianderivativename_textBox19.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newgaussianderivativename_textBox19.Text, Translate.KeyWordsDictionary(comboBox1), "0", newGaussianDerivativeBandWidth_textBox21.Text + "\r\nDispersive", "0" };
            //int[] a = new int[20];

            //a = wf.IndexFun(SetupContent.waveFormStr2);
            MainWindow.setupStr = wf.InsertWaveform4(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newgaussianderivativename_textBox19.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newgaussianderivativename_textBox19.Text);
            string waveInfoArr = SetupContent.waveFormStr1 + " " + newgaussianderivativename_textBox19.Text + "\r\n"
                                         + (string)comboBox1.SelectedItem + "\r\n"
                                          + "9.155" + "\r\n"
                                         + newGaussianDerivativeBandWidth_textBox21.Text + "\r\n"
                                         + "END" + SetupContent.waveFormStr1 + " " + newgaussianderivativename_textBox19.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newgaussianderivativename_textBox19.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newGaussianDerivativeSave_button21_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newGaussianDerivative = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
            newGaussianDerivative.Name = newgaussianderivativename_textBox19.Text;
            newGaussianDerivative.Type = Translate.KeyWordsDictionary(comboBox1);
            newGaussianDerivative.Frequency = 9.155;
            newGaussianDerivative.BandWidth = Double.Parse(newGaussianDerivativeBandWidth_textBox21.Text);
            
            try
            {
                if (newgaussianderivativename_textBox19.Text == "" || newGaussianDerivativeBandWidth_textBox21.Text == "" || newGaussianDerivativeCarrierFrenquency_textBox20.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newGaussianDerivativeBandWidth_textBox21.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newGaussianDerivativeBandWidth_textBox21.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddWaveForm(newGaussianDerivative);
                MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newGaussianDerivativeOk_button9_Click( sender,  e);
                try
                {
                    newGaussianDerivative = client.iGetWaveForm(newgaussianderivativename_textBox19.Text);
                    newGaussianDerivative.Name = newgaussianderivativename_textBox19.Text;
                    newGaussianDerivative.Frequency = double.Parse(newGaussianDerivativeCarrierFrenquency_textBox20.Text);
                    newGaussianDerivative.BandWidth = double.Parse(newGaussianDerivativeBandWidth_textBox21.Text);
                    client.iUpdateWaveForm(newGaussianDerivative);
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
        }
        private void newGaussianDerivativeCancel_button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newBlackmanOk_button11_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (newblackmanname_textBox22.Text == "" || newblackmancarrierfrequency_textBox23.Text == "" || newblackmanplusewidth_textBox24.Text == "" || newblackmanphase_textBox25.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newblackmancarrierfrequency_textBox23.Text))
            {
                MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newblackmancarrierfrequency_textBox23.Text) < 0.000)
            {
                MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newblackmanplusewidth_textBox24.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newblackmanplusewidth_textBox24.Text) < 0.000)
            {
                MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newblackmanphase_textBox25.Text))
            {
                MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PhaseLimition(newblackmanphase_textBox25))
            {
                MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PluseWidthLimition(newblackmanplusewidth_textBox24, newblackmancarrierfrequency_textBox23))
            {
                MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.EffectiveBandwidthLimition_Blackman(newblackmanplusewidth_textBox24, newblackmancarrierfrequency_textBox23))
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
            if (wf.judge(newblackmanname_textBox22.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] temp = { newblackmanname_textBox22.Text, Translate.KeyWordsDictionary(comboBox1), newblackmancarrierfrequency_textBox23.Text, newblackmanplusewidth_textBox24.Text, newblackmanphase_textBox25.Text };
            //int[] a = new int[20];

            //a = wf.IndexFun(SetupContent.waveFormStr2);
            MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3, false);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newblackmanname_textBox22.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newblackmanname_textBox22.Text);

            string waveInfoArr = SetupContent.waveFormStr1 + " " + newblackmanname_textBox22.Text + "\r\n"
                                   + (string)comboBox1.SelectedItem + "\r\n"
                                   + newblackmancarrierfrequency_textBox23.Text + "\r\n"
                                   + newblackmanplusewidth_textBox24.Text + "\r\n"
                                   + newblackmanphase_textBox25.Text + "\r\n"
                                   + "END" + SetupContent.waveFormStr1 + " " + newblackmanname_textBox22.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newblackmanname_textBox22.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newBlackmanSave_button22_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newBlackman = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
            newBlackman.Name = newblackmanname_textBox22.Text;
            newBlackman.Type = Translate.KeyWordsDictionary(comboBox1);
            newBlackman.Frequency = Double.Parse(newblackmancarrierfrequency_textBox23.Text);
            newBlackman.BandWidth = Double.Parse(newblackmanplusewidth_textBox24.Text);
            newBlackman.Phase = Double.Parse(newblackmanphase_textBox25.Text);
            
            try
            {
                if (newblackmanname_textBox22.Text == "" || newblackmancarrierfrequency_textBox23.Text == "" || newblackmanplusewidth_textBox24.Text == "" || newblackmanphase_textBox25.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newblackmancarrierfrequency_textBox23.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newblackmancarrierfrequency_textBox23.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newblackmanplusewidth_textBox24.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newblackmanplusewidth_textBox24.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newblackmanphase_textBox25.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(newblackmanphase_textBox25))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(newblackmanplusewidth_textBox24, newblackmancarrierfrequency_textBox23))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Blackman(newblackmanplusewidth_textBox24, newblackmancarrierfrequency_textBox23))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddWaveForm(newBlackman);
                MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newBlackmanOk_button11_Click(sender,  e);
                try
                {
                    newBlackman = client.iGetWaveForm(newblackmanname_textBox22.Text);
                    newBlackman.Name = newblackmanname_textBox22.Text;
                    newBlackman.Frequency = double.Parse(newblackmancarrierfrequency_textBox23.Text);
                    newBlackman.BandWidth = double.Parse(newblackmanplusewidth_textBox24.Text);
                    newBlackman.Phase = double.Parse(newblackmanphase_textBox25.Text);
                    client.iUpdateWaveForm(newBlackman);
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
        }
        private void newBlackmanCancel_button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newChripSave_button23_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newChrip = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
            newChrip.Name = newchripname_textBox26.Text;
            newChrip.Type = Translate.KeyWordsDictionary(comboBox1);
            newChrip.Frequency = Double.Parse(newchripcarrierfrequency_textBox27.Text);
            newChrip.BandWidth = Double.Parse(newchripplusewidth_textBox28.Text);
            newChrip.Phase = 0.00;
            newChrip.RollOffFactor = Double.Parse(newchriprolloff_textBox29.Text);
            newChrip.StartFrequency = Double.Parse(newchripstartfrequency_textBox30.Text);
            newChrip.EndFrequency = Double.Parse(newchripstopfrequency_textBox31.Text);
            newChrip.FreChangeRate = Translate.KeyWordsDictionary(newchripfrequencyvariation_comboBox2);//FreChangeRate应为string类型
            
            try
            {
                if (newchripname_textBox26.Text == "" || newchripcarrierfrequency_textBox27.Text == "" || newchripplusewidth_textBox28.Text == "" || newchriprolloff_textBox29.Text == "" || newchripstartfrequency_textBox30.Text == "" || newchripstopfrequency_textBox31.Text == "" || newchripfrequencyvariation_comboBox2.SelectedItem == null)
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newchripcarrierfrequency_textBox27.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newchripcarrierfrequency_textBox27.Text) < 0.000)
                {
                    MessageBox.Show("载波频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newchripplusewidth_textBox28.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newchripplusewidth_textBox28.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newchriprolloff_textBox29.Text))
                {
                    MessageBox.Show("滚降系数为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.Roll0ffFactorLimition(newchriprolloff_textBox29))
                {
                    MessageBox.Show("滚降系数值需在0至1之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newchripstartfrequency_textBox30.Text))
                {
                    MessageBox.Show("起始频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newchripstartfrequency_textBox30.Text) < 0.000)
                {
                    MessageBox.Show("起始频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newchripstopfrequency_textBox31.Text))
                {
                    MessageBox.Show("截止频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newchripstopfrequency_textBox31.Text) < 0.000)
                {
                    MessageBox.Show("截止频率大于0.00", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddWaveForm(newChrip);
                MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                newChripOk_button13_Click(sender, e);
                try
                {
                    newChrip = client.iGetWaveForm(newchripname_textBox26.Text);
                    newChrip.Name = newchripname_textBox26.Text;
                    newChrip.Frequency = double.Parse(newchripcarrierfrequency_textBox27.Text);
                    newChrip.BandWidth = double.Parse(newchripplusewidth_textBox28.Text);
                    newChrip.RollOffFactor = double.Parse(newchriprolloff_textBox29.Text);
                    newChrip.FreChangeRate = Translate.KeyWordsDictionary(newchripfrequencyvariation_comboBox2);
                    newChrip.StartFrequency = double.Parse(newchripstartfrequency_textBox30.Text);
                    newChrip.EndFrequency = double.Parse(newchripstopfrequency_textBox31.Text);
                    client.iUpdateWaveForm(newChrip);
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
        }
        private void newChripOk_button13_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (newchripname_textBox26.Text == "" || newchripcarrierfrequency_textBox27.Text == "" || newchripplusewidth_textBox28.Text == "" || newchriprolloff_textBox29.Text == "" || newchripstartfrequency_textBox30.Text == "" || newchripstopfrequency_textBox31.Text == "" || newchripfrequencyvariation_comboBox2.SelectedItem == null)
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            } 
            string[] waveNames = new string[30];
            //ArrayList arrayWaveForm = new ArrayList();
            WaveformWriting wf = new WaveformWriting(MainWindow.setupStr);
            waveNames = wf.waveformNames(SetupContent.waveFormStr1);
            if (wf.judge(newchripname_textBox26.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newchripname_textBox26.Text, Translate.KeyWordsDictionary(comboBox1), newchripcarrierfrequency_textBox27.Text, newchripplusewidth_textBox28.Text, "0.000", newchriprolloff_textBox29.Text, Translate.KeyWordsDictionary(newchripfrequencyvariation_comboBox2), newchripstartfrequency_textBox30.Text, newchripstopfrequency_textBox31.Text };
            //int[] a = new int[20];

            //a = wf.IndexFun(SetupContent.waveFormStr2);
            MainWindow.setupStr = wf.InsertWaveform8(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newchripname_textBox26.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newchripname_textBox26.Text);
            string waveInfoArr = SetupContent.waveFormStr1 + " " + newchripname_textBox26.Text + "\r\n"
                                + (string)comboBox1.SelectedItem + "\r\n"
                                + newchripcarrierfrequency_textBox27.Text + "\r\n"
                                + newchripplusewidth_textBox28.Text + "\r\n"
                                + newchriprolloff_textBox29.Text + "\r\n"
                                + (string)newchripfrequencyvariation_comboBox2.SelectedItem + "\r\n"
                                 + newchripstartfrequency_textBox30.Text + "\r\n"
                                  + newchripstopfrequency_textBox31.Text + "\r\n"
                                + "END" + SetupContent.waveFormStr1 + " " + newchripname_textBox26.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newchripname_textBox26.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void newChripCancel_button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void newHammingSave_button24_Click(object sender, EventArgs e)
        {
            ServiceReference1.WaveForm newHamming = new ServiceReference1.WaveForm();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (MainWindow.newFuncSign)
            {
            newHamming.Name = newhammingname_textBox32.Text;
            newHamming.Type = Translate.KeyWordsDictionary(comboBox1);
            newHamming.Frequency = Double.Parse(newhammingcarrierfrequency_textBox33.Text);
            newHamming.BandWidth = Double.Parse(newhammingplusewidth_textBox34.Text);
            newHamming.Phase = Double.Parse(newhammingphase_textBox35.Text);
            
            try
            {
                if (newhammingname_textBox32.Text == "" || newhammingcarrierfrequency_textBox33.Text == "" || newhammingplusewidth_textBox34.Text == "" || newhammingphase_textBox35.Text == "")
                {
                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhammingcarrierfrequency_textBox33.Text))
                {
                    MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhammingcarrierfrequency_textBox33.Text) < 0.000)
                {
                    MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhammingplusewidth_textBox34.Text))
                {
                    MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (double.Parse(newhammingplusewidth_textBox34.Text) < 0.000)
                {
                    MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (!BoudingLimition.IsScienceFigure(newhammingphase_textBox35.Text))
                {
                    MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PhaseLimition(newhammingphase_textBox35))
                {
                    MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.PluseWidthLimition(newhammingplusewidth_textBox34, newhammingcarrierfrequency_textBox33))
                {
                    MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                if (BoudingLimition.EffectiveBandwidthLimition_Hamming(newhammingplusewidth_textBox34, newhammingcarrierfrequency_textBox33))
                {
                    MessageBox.Show("带宽大于二倍载波频率", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
                client.iAddWaveForm(newHamming);
                MessageBox.Show("波形已成功添加至数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
               newHammingOk_button15_Click(sender, e);
               try
               {
                   newHamming = client.iGetWaveForm(newhammingname_textBox32.Text);
                   newHamming.Name = newhammingname_textBox32.Text;
                   newHamming.Frequency = double.Parse(newhammingcarrierfrequency_textBox33.Text);
                   newHamming.BandWidth = double.Parse(newhammingplusewidth_textBox34.Text);
                   newHamming.Phase = double.Parse(newhammingphase_textBox35.Text);
                   client.iUpdateWaveForm(newHamming);
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
        }
        private void newHammingCancel_button16_Click(object sender, EventArgs e)
        {
            this.Close();
        }       
        private void newHammingOk_button15_Click(object sender, EventArgs e)
        {
            if (Hasbug())
            {
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }

            if (newhammingname_textBox32.Text == "" || newhammingcarrierfrequency_textBox33.Text == "" || newhammingplusewidth_textBox34.Text == "" || newhammingphase_textBox35.Text == "")
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newhammingcarrierfrequency_textBox33.Text))
            {
                MessageBox.Show("载波频率必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newhammingcarrierfrequency_textBox33.Text) < 0.000)
            {
                MessageBox.Show("载波频率不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newhammingplusewidth_textBox34.Text))
            {
                MessageBox.Show("脉冲宽度必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (double.Parse(newhammingplusewidth_textBox34.Text) < 0.000)
            {
                MessageBox.Show("脉冲宽度不能为负值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (!BoudingLimition.IsScienceFigure(newhammingphase_textBox35.Text))
            {
                MessageBox.Show("相位必须为实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PhaseLimition(newhammingphase_textBox35))
            {
                MessageBox.Show("相位范围应在-180度至180度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.PluseWidthLimition(newhammingplusewidth_textBox34, newhammingcarrierfrequency_textBox33))
            {
                MessageBox.Show("脉冲宽度小于信号时间范围", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            if (BoudingLimition.EffectiveBandwidthLimition_Hamming(newhammingplusewidth_textBox34, newhammingcarrierfrequency_textBox33))
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
            if (wf.judge(newhammingname_textBox32.Text, waveNames))
            {
                MessageBox.Show("该波形名称已经存在，请您换个名称", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            string[] temp = { newhammingname_textBox32.Text, Translate.KeyWordsDictionary(comboBox1), newhammingcarrierfrequency_textBox33.Text, newhammingplusewidth_textBox34.Text,newhammingphase_textBox35.Text };
            //int[] a = new int[20];

            //a = wf.IndexFun(SetupContent.waveFormStr2);
            MainWindow.setupStr = wf.InsertWaveform5(temp, SetupContent.waveFormStr2, SetupContent.waveFormStr3,false);
            FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

            FileOperation.WriteLineFile(SetupContent.waveIndeStr + " " + newhammingname_textBox32.Text, MainWindow.nodeInfoFullPath, true);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].Nodes.Add(newhammingname_textBox32.Text);

            string waveInfoArr = SetupContent.waveFormStr1 + " " + newhammingname_textBox32.Text + "\r\n"
                                               + (string)comboBox1.SelectedItem + "\r\n"
                                               + newhammingcarrierfrequency_textBox33.Text+ "\r\n"
                                               + newhammingplusewidth_textBox34.Text + "\r\n"
                                               + newhammingphase_textBox35.Text + "\r\n"
                                               + "END" + SetupContent.waveFormStr1 + " " + newhammingname_textBox32.Text + "\r\n";
            FileOperation.WriteFile(waveInfoArr, MainWindow.waveinfoFilePath, true);
            if (MainWindow.creatSuccMesDisp)
            {
                MessageBox.Show("\"" + newhammingname_textBox32.Text + "\"波形创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void NewWaveformWindow_Load(object sender, EventArgs e)
        {

        }
    }
}