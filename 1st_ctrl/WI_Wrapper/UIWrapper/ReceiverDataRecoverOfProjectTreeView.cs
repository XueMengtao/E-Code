using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WF1
{
    class ReceiverDataRecoverOfProjectTreeView
    {
        //根据接收机的名字和Waveinfo文件中全部的字符串提取出接收机的参数信息块
        private static string GetBlockOfReceiver(TreeNode currentNode, string allReceiverInfoStr)
        {
            string receiverBlockStr = null;
            try
            {
                string indStr = null;
                //点状分布和区域分布的索引字符串不一样
                switch (currentNode.Parent.Text)
                {
                    case "点状分布":
                        indStr = SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text+"\r\n";
                        break;
                    case "区域分布":
                        indStr = SetupContent.gridReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        break;
                }
               
                int start = allReceiverInfoStr.IndexOf(indStr);
                int longth = allReceiverInfoStr.IndexOf("END" + indStr) - start;
                receiverBlockStr = allReceiverInfoStr.Substring(start, longth);
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return receiverBlockStr;
        }
        //将接收机原来的参数信息加载到窗口中
        public static bool RecoverReceiverParOfWin(TreeNode currentNode, string allReceiverInfoStr, NewReceiverWindow newReceiverWin)
        {
            bool b = false;
            try
            {
                string transmitterBlockStr = GetBlockOfReceiver(currentNode, allReceiverInfoStr);
                char[] sep = { '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r', '\r' };
                string[] partStr = transmitterBlockStr.Split(sep);
                newReceiverWin.newReceiverName_textBox2.Text = currentNode.Text;
                newReceiverWin.newRceiverType_comboBox1.Text = partStr[1].Trim('\n');

                newReceiverWin.newReceiverReferencePlane_comboBox2.Text = partStr[2].Trim('\n');
                newReceiverWin.newReceiverAntennaName_comboBox3.Text = partStr[3].Trim('\n');
                //newAntennaWin.newdipolepolarization_comboBox3.SelectedItem = partStr[4].Trim('\n');
                newReceiverWin.newReceiverAntennaHeight_textBox1.Text = partStr[4].Trim('\n');
                newReceiverWin.newReceiverSpace_textBox1.Text = partStr[5].Trim('\n');
                newReceiverWin.newReceiverAntennaRotationX_textBox4.Text = partStr[6].Trim('\n');
                newReceiverWin.newReceiverAntennaRotationY_textBox6.Text = partStr[7].Trim('\n');
                newReceiverWin.newReceiverAntennaRotationXZ_textBox5.Text = partStr[8].Trim('\n');
                //if (currentNode.Parent.Text.Equals("区域分布"))
                //{
                //    newReceiverWin.newReceiverSpace_textBox1.Text = partStr[8].Trim('\n');
                //}

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
