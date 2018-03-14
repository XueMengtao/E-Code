using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WF1
{
    class TransmitterDataRecoverOfProjectTreeView
    {
        //从waveinfo文件中提取出辐射源块，为后面RecoverTransmitterParOfWin方法提供原来的辐射源参数
         //第二个参数是waveinfo文件中的全部字符串      
        private static string GetBlockOfTransmitter(TreeNode currentNode, string allTransmitterInfoStr)
        {
            string transmitterBlockStr = null;
            try
            {
                string indStr = SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text + "\r\n";
                int start=allTransmitterInfoStr.IndexOf(indStr);
                int longth=allTransmitterInfoStr.IndexOf("END" + indStr) - start ;
                transmitterBlockStr = allTransmitterInfoStr.Substring(start,longth );
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return transmitterBlockStr;
        }
        //将原来辐射源设置的参数信息恢复到窗口中，第二个参数是waveinfo文件中全部的字符串，作为参数恢复的数据源
        public static bool RecoverTransmitterParOfWin(TreeNode currentNode, string allTransmitterInfoStr, NewTransmitterWindow newTransmitterWin)
        {
            bool b = false;
            try
            {
                string transmitterBlockStr = GetBlockOfTransmitter(currentNode, allTransmitterInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = transmitterBlockStr.Split(sep);
                newTransmitterWin.newTransmitterSave_button3.Text = "更新";
                newTransmitterWin.newTransmitterCancel_button2.Text = "取消";
                newTransmitterWin.newTransmitterSave_button3.Enabled = false;
                newTransmitterWin.newTransmitterName_textBox2.Text = currentNode.Text;
                newTransmitterWin.newTransimtterReferencePlane_comboBox2.Text = partStr[1].Trim('\n');
                newTransmitterWin.newTransmitterPower_textBox11.Text = partStr[2].Trim('\n');
                newTransmitterWin.newTransmitterAntennaName_comboBox3.Text = partStr[3].Trim('\n');
                //newAntennaWin.newdipolepolarization_comboBox3.SelectedItem = partStr[4].Trim('\n');
                newTransmitterWin.newTransmitterWaveformName_textBox1.Text = partStr[4].Trim('\n');
                newTransmitterWin.newTransmitterAntennaRotationX_textBox4.Text = partStr[5].Trim('\n');
                newTransmitterWin.newTransmitterAntennaRotationY_textBox6.Text = partStr[6].Trim('\n');
                newTransmitterWin.newTransmitterAntennaRotationZ_textBox5.Text = partStr[7].Trim('\n');

                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                return false;
            }
            return b;
        }
    }
}
