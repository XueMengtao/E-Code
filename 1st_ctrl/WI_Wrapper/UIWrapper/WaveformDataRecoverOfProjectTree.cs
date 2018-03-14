using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WF1
{
    class WaveformDataRecoverOfProjectTree
    {
        //根据工程树中波形结点的名字在waveinfo文件中找出对应的波形快
        public static string GetBlockOfWave(TreeNode currentNode, string allWaveformInfoStr)
        {
            string waveBlockStr = null;
            try
            {
                //string allWaveInfoStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
                string indStr = SetupContent.waveFormStr1 + " " + currentNode.Text+"\r\n";
                waveBlockStr = allWaveformInfoStr.Substring(allWaveformInfoStr.IndexOf(indStr), allWaveformInfoStr.IndexOf("END" + indStr) - allWaveformInfoStr.IndexOf(indStr) - 2);
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return waveBlockStr;
        }
        //根据波形的名称找出所属的类型
        public static string GetTypeOfWave(TreeNode currentNode, string allWaveformInfoStr)
        {
            //string allWaveInfoStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
            string indStr = SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n";

            int startSite = allWaveformInfoStr.IndexOf(indStr) + indStr.Length;

            StringBuilder sb = new StringBuilder();
            while (allWaveformInfoStr[startSite] != '\r')
            {
                sb = sb.Append(allWaveformInfoStr[startSite]);
                startSite++;
            }
            return sb.ToString();
        }
        //根据波形类型的不同，调出不同的窗口
        public static void AlterationWavePar(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            string waveTypeStr = GetTypeOfWave(currentNode,allWaveformInfoStr );     
            switch (waveTypeStr)
            {
                case "正弦波<Sinusoid>":
                    if (!GetLastParOfSinusoid(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "升余弦<Raised cosine>":
                    if (!GetLastParOfRaisedCosine(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "根号升余弦<Root raised cosine>":
                    if (!GetLastParOfRootRaisedCosine(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "高斯信号<Gaussian>":
                    if (!GetLastParOfGaussian(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "离散高斯信号<Gaussian derivative>":
                    if (!GetLastParOfGaussianDerivative( currentNode,  allWaveformInfoStr ,  wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "布莱曼波<Blackman>":
                    if (!GetLastParOfBlackman(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "线性调频波<Chirp>":
                    if (!GetLastParOfChirp(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "海明波<Hamming>":
                    if (!GetLastParOfHamming(currentNode, allWaveformInfoStr, wfw))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }

        }
        //将waveinfo中相应块的内容写到控件中
        private static bool GetLastParOfSinusoid(TreeNode currentNode, string allWaveformInfoStr , NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newsinusoidname_textBox1.Text = currentNode.Text ;
                wfw.newsinusoidsave_button17.Text = "更新";
                wfw.newsinusoidcancel_button2.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newsinusoidsave_button17.Enabled = false;
                //wfw.newsinusoidname_textBox1.ReadOnly = true;
                //combox控件中的选中项是正弦波<Sinusoid>，将调出它所对应的panel
                wfw.comboBox1.SelectedItem = "正弦波<Sinusoid>";
                wfw.newsinusoidcarrierfrequency_textBox2.Text = partStr[2].Trim('\n');
                wfw.newsinusoideffectivebandwith_textBox3.Text = partStr[3].Trim('\n');
                wfw.newsinusoidphase_textBox4.Text = partStr[4].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfRaisedCosine(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newraisedcosinename_textBox5.Text = currentNode.Text;
                wfw.newraisedcosinesave_button18.Text = "更新";
                wfw.newraisedcosinecancel_button4.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newraisedcosinesave_button18.Enabled = false;
                //wfw.newraisedcosinename_textBox5.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "升余弦<Raised cosine>";
                wfw.newraisedcosinecarrierfrequency_textBox6.Text = partStr[2].Trim('\n');
                wfw.newraisedcosineplusebandwith_textBox7.Text = partStr[3].Trim('\n');
                wfw.newraisedcosinephase_textBox8.Text = partStr[4].Trim('\n');
                wfw.newraisedcosinerolloff_textBox9.Text = partStr[5].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfRootRaisedCosine(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r','\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newrootraisedcosinename_textBox10.Text = currentNode.Text;
                wfw.newrootraisedcosinesave_button19.Text = "更新";
                wfw.newrootraisedcosinecancel_button6.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newrootraisedcosinesave_button19.Enabled = false;
                //wfw.newrootraisedcosinename_textBox10.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "根号升余弦<Root raised cosine>";
                wfw.newrootraisedcosinecarrierfrequency_textBox11.Text = partStr[2].Trim('\n');
                wfw.newrootraisedcosineplusewidth_textBox12.Text = partStr[3].Trim('\n');
                wfw.newrootraisedcosinephase_textBox13.Text = partStr[4].Trim('\n');
                wfw.newrootraisedcosinerolloff_textBox14.Text = partStr[5].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfGaussian(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newgaussianname_textBox15.Text = currentNode.Text;
                wfw.newgaussiansave_button20.Text = "更新";
                wfw.newgaussiancancel_button8.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newgaussiansave_button20.Enabled = false;
                //wfw.newgaussianname_textBox15.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "高斯信号<Gaussian>";
                wfw.newgaussiancarrierfrequency_textBox16.Text = partStr[2].Trim('\n');
                wfw.newgaussianplusewidth_textBox17.Text = partStr[3].Trim('\n');
                wfw.newgaussianphase_textBox18.Text = partStr[4].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfGaussianDerivative(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newgaussianderivativename_textBox19.Text = currentNode.Text;
                wfw.newgaussianderivativesave_button21.Text = "更新";
                wfw.newgaussianderivativecancel_button10.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newgaussianderivativesave_button21.Enabled = false;
                //wfw.newgaussianderivativename_textBox19.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "离散高斯信号<Gaussian derivative>";
                wfw.newGaussianDerivativeCarrierFrenquency_textBox20.Text = partStr[2].Trim('\n');
                wfw.newGaussianDerivativeBandWidth_textBox21.Text = partStr[3].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfBlackman(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newblackmanname_textBox22.Text = currentNode.Text;
                wfw.newblackmansave_button22.Text = "更新";
                wfw.newblackmancancel_button12.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newblackmansave_button22.Enabled = false;
                //wfw.newblackmanname_textBox22.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "布莱曼波<Blackman>";
                wfw.newblackmancarrierfrequency_textBox23.Text = partStr[2].Trim('\n');
                wfw.newblackmanplusewidth_textBox24.Text = partStr[3].Trim('\n');
                wfw.newblackmanphase_textBox25.Text = partStr[4].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfChirp(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                //NewWaveformWindow wfw = new NewWaveformWindow();
                //wfw.Show();
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r' ,'\r'};
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newchripname_textBox26.Text = currentNode.Text;
                wfw.newchripsave_button23.Text = "更新";
                wfw.newchripcancel_button14.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newchripsave_button23.Enabled = false;
                //wfw.newchripname_textBox26.ReadOnly = true;
                //combox控件中的选中项是正弦波<Sinusoid>，将调出它所对应的panel
                wfw.comboBox1.SelectedItem = "线性调频波<Chirp>";
                wfw.newchripcarrierfrequency_textBox27.Text = partStr[2].Trim('\n');
                wfw.newchripplusewidth_textBox28.Text = partStr[3].Trim('\n');
                wfw.newchriprolloff_textBox29.Text = partStr[4].Trim('\n');
                wfw.newchripfrequencyvariation_comboBox2.SelectedItem = partStr[5].Trim('\n');
                wfw.newchripstartfrequency_textBox30.Text = partStr[6].Trim('\n');
                wfw.newchripstopfrequency_textBox31.Text = partStr[7].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool GetLastParOfHamming(TreeNode currentNode, string allWaveformInfoStr, NewWaveformWindow wfw)
        {
            bool b = false;
            try
            {
                string waveBlockStr = GetBlockOfWave(currentNode, allWaveformInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r' };
                string[] partStr = waveBlockStr.Split(sep);
                wfw.newhammingname_textBox32.Text = currentNode.Text;
                wfw.newhammingsave_button24.Text = "更新";
                wfw.newhammingcancel_button16.Text = "取消";
                wfw.comboBox1.Enabled = false;
                wfw.newhammingsave_button24.Enabled = false;
                //wfw.newhammingname_textBox32.ReadOnly = true;
                wfw.comboBox1.SelectedItem = "海明波<Hamming>";
                wfw.newhammingcarrierfrequency_textBox33.Text = partStr[2].Trim('\n');
                wfw.newhammingplusewidth_textBox34.Text = partStr[3].Trim('\n');
                wfw.newhammingphase_textBox35.Text = partStr[4].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
    }
}
