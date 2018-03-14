using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace WF1
{
    class AntennaDataRecoverOfProjectTree
    {
        //根据工程树中天线结点的名字在waveinfo文件中找出对应的天线块，第二个参数是：waveinfo文件中的字符串并且是删除更改块之前的字符串
        public static string GetBlockOfAntenna(TreeNode currentNode, string allAntennaInfoStr)
        {
            string waveBlockStr = null;
            try
            {
                //string allAntennaInfoStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
                string indStr = SetupContent.antennaStr1  + " " + currentNode.Text+"\r\n";
                waveBlockStr = allAntennaInfoStr.Substring(allAntennaInfoStr.IndexOf(indStr), allAntennaInfoStr.IndexOf("END" + indStr) - allAntennaInfoStr.IndexOf(indStr) - 2);
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return waveBlockStr;
        }
        //根据天线的名称找出所属的类型,第二个参数：是waveinfo文件中全部的字符串，并且是在删除以前的字符串
        public static string GetTypeOfAntenna(TreeNode currentNode, string allAntennaInfoStr)
        {
            //string allAntennaInfoStr = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
            string indStr = SetupContent.antennaStr1  + " " + currentNode.Text +"\r\n";
            StringBuilder sb = new StringBuilder();
            try
            {
                int startSite = allAntennaInfoStr.IndexOf(indStr) + indStr.Length ;

                //StringBuilder sb = new StringBuilder();
                while (allAntennaInfoStr[startSite] != '\r')
                {
                    sb = sb.Append(allAntennaInfoStr[startSite]);
                    startSite++;
                }
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                throw e;
            }
            return sb.ToString();
        }
        //根据天线名称找到天线类型，显示相应的面板，装载相应的参数
        public static bool AlterationAntennaPar(TreeNode currentNode, string antennaInfoStr,NewAntennaWindow newAntennaWin)
        {
            bool b = false;
            string waveTypeStr = null;
            try
            {
               waveTypeStr = GetTypeOfAntenna(currentNode, antennaInfoStr);
            }
            catch (Exception)
            {
                return false;
            }
            switch (waveTypeStr)
            {
                case "偶极子天线<Short dipole>":
                    if (!RecoverDipoleAntennaParOfWin(currentNode, antennaInfoStr,newAntennaWin))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //b = true;
                    break;
                case "单极天线<Short monopole>":
                    if (!RecoverMonopoleAntennaParOfWin(currentNode, antennaInfoStr, newAntennaWin))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //b = true;
                    break;
                case "抛物面反射天线<Parabolic reflector>":
                    if (!RecoverParabolicAntennaParOfWin(currentNode, antennaInfoStr, newAntennaWin))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //b = true;
                    break;
                case "螺旋天线<Helical>":
                    if (!RecoverHelicalAntennaParOfWin(currentNode, antennaInfoStr, newAntennaWin))
                    {
                        MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //b = true;
                    break;
                default:
                    MessageBox.Show("程序内部产生错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
            }
            return b;
        }
        //更改操作会弹出原来新建天线的窗口，将偶极子天线原来设定的参数重新装载到窗口中对应的控件中
        private static bool RecoverDipoleAntennaParOfWin(TreeNode currentNode, string allAntennaInfoStr ,NewAntennaWindow newAntennaWin)
        {
            bool b = false;
            try
            {
                string antennaBlockStr = GetBlockOfAntenna(currentNode,allAntennaInfoStr );
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = antennaBlockStr.Split(sep);
                newAntennaWin.newdipoleantennaname_textBox1.Text = currentNode.Text;
                newAntennaWin.comboBox1.SelectedItem = "偶极子天线<Short dipole>";
                newAntennaWin.newdipolesave_button11.Text = "更新";
                newAntennaWin.newdipolecancel_button2.Text = "取消";
                newAntennaWin.comboBox1.Enabled = false;
                newAntennaWin.newdipolesave_button11.Enabled = false;
                //newAntennaWin.newdipolewaveformname_comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                //newAntennaWin.newdipoleantenna_panel1.Visible = true;

                newAntennaWin.newdipolewaveformname_comboBox2.SelectedItem = partStr[2].Trim('\n');
                //newAntennaWin.newdipolewaveformname_comboBox2.Text = partStr[2].Trim('\n');
                //newAntennaWin.newdipolewaveformname_comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

                newAntennaWin.newdipolemaxgain_textBox2.Text = partStr[3].Trim('\n');
                //newAntennaWin.newdipolepolarization_comboBox3.SelectedItem = partStr[4].Trim('\n');
                newAntennaWin.newdipolepolarization_comboBox3.Text  = partStr[4].Trim('\n');
                newAntennaWin.newdipolereceiverthreshold_textBox3.Text = partStr[5].Trim('\n');
                newAntennaWin.newdipoletranmissionlineloss_textBox4.Text = partStr[6].Trim('\n');
                newAntennaWin.newdipolevswr_textBox5.Text = partStr[7].Trim('\n');
                newAntennaWin.newdipoletemperature_textBox6.Text = partStr[8].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        private static bool RecoverMonopoleAntennaParOfWin(TreeNode currentNode, string allAntennaInfoStr, NewAntennaWindow newAntennaWin)
        {
            bool b = false;
            try
            {
                string antennaBlockStr = GetBlockOfAntenna(currentNode,allAntennaInfoStr );
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = antennaBlockStr.Split(sep);
                newAntennaWin.newmonopoleantennaname_textBox18.Text = currentNode.Text;
                newAntennaWin.comboBox1.SelectedItem = "单极天线<Short monopole>";
                newAntennaWin.newmonopolesave_button12.Text = "更新";
                newAntennaWin.newmonopolecancel_button4.Text = "取消";
                newAntennaWin.comboBox1.Enabled = false;
                newAntennaWin.newmonopolesave_button12.Enabled = false;
                //newAntennaWin.newmonopolewaveformname_comboBox4.Text = partStr[2].Trim('\n');
                newAntennaWin.newmonopolewaveformname_comboBox4.SelectedItem = partStr[2].Trim('\n');
                newAntennaWin.newmonopolelength_textBox31.Text = partStr[3].Trim('\n');
                newAntennaWin.newmonopolemaxgain_textBox17.Text = partStr[4].Trim('\n');
                newAntennaWin.newmonopolereceiverthreshold_textBox16.Text = partStr[5].Trim('\n');
                newAntennaWin.newmonopoletransmissionlineloss_textBox15.Text = partStr[6].Trim('\n');
                newAntennaWin.newmonopolevswr_textBox14.Text = partStr[7].Trim('\n');
                newAntennaWin.newmonopoletempertature_textBox13.Text = partStr[8].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;

        }
        private static bool RecoverParabolicAntennaParOfWin(TreeNode currentNode, string allAntennaInfoStr, NewAntennaWindow newAntennaWin)
        {
            bool b = false;
            try
            {
                string antennaBlockStr = GetBlockOfAntenna(currentNode,allAntennaInfoStr );
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = antennaBlockStr.Split(sep);
                newAntennaWin.newparabolicantennaname_textBox12.Text = currentNode.Text;
                newAntennaWin.comboBox1.SelectedItem = "抛物面反射天线<Parabolic reflector>";
                newAntennaWin.newparabolic_savebutton13.Text = "更新";
                newAntennaWin.newparaboliccancel_button6.Text = "取消";
                newAntennaWin.comboBox1.Enabled = false;
                newAntennaWin.newparabolic_savebutton13.Enabled = false;
                newAntennaWin.newparabolicwaveformname_comboBox5.Text = partStr[2].Trim('\n');
                newAntennaWin.newparabolicpolarization_comboBox2.Text = partStr[3].Trim('\n');
                newAntennaWin.newparabolicradius_textBox9.Text = partStr[4].Trim('\n');
                newAntennaWin.newparabolicblockageradius_textBox10.Text = partStr[5].Trim('\n');
                newAntennaWin.newparabolicaperturedistribution_comboBox6.Text = partStr[6].Trim('\n');
                newAntennaWin.newparabolicedgetaper_textBox8.Text = partStr[7].Trim('\n');
                newAntennaWin.newparabolicreceiverthreshold_textBox7.Text = partStr[8].Trim('\n');
                newAntennaWin.newparabolictransmissionlineloss_textBox19.Text = partStr[9].Trim('\n');
                newAntennaWin.newparabolicvswr_textBox21.Text = partStr[10].Trim('\n');
                newAntennaWin.newparabolictemperature_textBox20.Text = partStr[11].Trim('\n');
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;

        }
        private static bool RecoverHelicalAntennaParOfWin(TreeNode currentNode, string allAntennaInfoStr, NewAntennaWindow newAntennaWin)
        {
            bool b = false;
            try
            {
                string antennaBlockStr = GetBlockOfAntenna(currentNode,allAntennaInfoStr );
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = antennaBlockStr.Split(sep);
                newAntennaWin.newhelicalantennaname_textBox27.Text = currentNode.Text;
                newAntennaWin.comboBox1.SelectedItem = "螺旋天线<Helical>";
                newAntennaWin.newhelicalsave_button14.Text = "更新";
                newAntennaWin.newhelicalcancel_button8.Text = "取消";
                newAntennaWin.comboBox1.Enabled = false;
                newAntennaWin.newhelicalsave_button14.Enabled = false;
                newAntennaWin.newhelicalwaveformname_comboBox7.Text = partStr[2].Trim('\n');
                newAntennaWin.newHelicalpolarization_comboBox3.Text = partStr[3].Trim('\n');
                newAntennaWin.newhelicalmaxgain_textBox26.Text = partStr[4].Trim('\n');
                newAntennaWin.newhelicalradius_textBox28.Text = partStr[5].Trim('\n');
                newAntennaWin.newhelicallength_textBox29.Text = partStr[6].Trim('\n');
                newAntennaWin.newhelicalpitch_textBox30.Text = partStr[7].Trim('\n');
                newAntennaWin.helicalreceiverthreshold_newtextBox25.Text = partStr[8].Trim('\n');
                newAntennaWin.newhelicaltransmissionlineloss_textBox24.Text = partStr[9].Trim('\n');
                newAntennaWin.newhelicalvswr_textBox23.Text = partStr[10].Trim('\n');
                newAntennaWin.newhelicaltemperature_textBox22.Text = partStr[11].Trim('\n');
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
