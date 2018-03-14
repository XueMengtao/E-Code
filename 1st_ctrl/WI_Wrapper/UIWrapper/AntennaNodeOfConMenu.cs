using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;
namespace WF1
{
    class AntennaNodeOfConMenu
    {
        //将相应的天线信息从setup文件和info文件信息中删除
        private static bool AntennaDelOfSetupAndInfo(TreeNode currentNode)
        {
            bool b = false;
            int start = MainWindow.setupStr.IndexOf(SetupContent.antennaStr1 + " " + currentNode.Text+"\r\n" );
            //只能用LastIndexOf(nextNodeStr) 因为当删除的天线结点是树种同一级的最后一个时，只能查找end_<antenna>
            int end;
            //删除的结点后面有没有结点，索引的天线结束字符串不一样
            //当删除结点后面没有结点时，索引end_<antenna>
            if (currentNode.NextNode != null)
            {
                end = MainWindow.setupStr.LastIndexOf(SetupContent.antennaStr1 + " " + currentNode.NextNode.Text + "\r\n");
            }
             //当删除结点后面还有结点时，索引begin_<antenna> currentNode.Text字符串
            else
            {
                end = MainWindow.setupStr.LastIndexOf(SetupContent.antennaStr3) + SetupContent.antennaStr3.Length + 2;
            }
            try
            {
                MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);
                //删除后保存到setup文件中
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                //还要从.info文件中将天线的信息删除
                string infoStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                infoStr = infoStr.Remove(infoStr.IndexOf(SetupContent.antennaStr1 + " " + currentNode.Text+"\r\n" ), (SetupContent.antennaStr1 + " " + currentNode.Text+"\r\n" ).Length );
              
                FileOperation.WriteFile(infoStr, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        //从waveinfo文件中删除相应的天线信息
        private static bool AntennaDelOfWaveInfo(TreeNode currentNode)
        {
            bool b = false;
            try
            {
                string antennaAllInfo = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
                //只能用IndexOf(waveName)函数，因为"END" + antennaName也含有antennaName
                int delStart = antennaAllInfo.IndexOf(SetupContent.antennaStr1 + " " + currentNode.Text+"\r\n" );
                int delEnd = antennaAllInfo.LastIndexOf("END" + SetupContent.antennaStr1 + " " + currentNode.Text + "\r\n") + ("END" + SetupContent.antennaStr1 + " " + currentNode.Text + "\r\n").Length ;
                antennaAllInfo = antennaAllInfo.Remove(delStart, delEnd - delStart);
                FileOperation.WriteFile(antennaAllInfo, MainWindow.waveinfoFilePath, false);
                //从match文件中删除天线的相关信息
                //StringFound.DelBackIndStr(SetupContent.antennaStr1 + " " + currentNode.Text);
                b = true;
            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.fatal(ex.Message, ex);
            }
            return b;
        }
    

        //当单击删除时执行如下方法
        public static bool AntennaDelMenu(TreeNode currentNode)
        {
            bool b = false;
            if (!StringFound.WaveOrAntennaInUse(currentNode.Text ,"antenna"))
            {
                DialogResult result = MessageBox.Show("您确定要删除\"" + currentNode.Text  + "\"天线吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                        //AntennaDelOfSetupAndInfo(currentNode);
                        //AntennaDelOfWaveInfo(currentNode);
                        if (AntennaDelOfSetupAndInfo(currentNode) && AntennaDelOfWaveInfo(currentNode))
                        {
                            StringFound.DelBackIndStr(SetupContent.antennaStr1 + " " + currentNode.Text+"#");
                            b = true;
                        }
                }
            }
            else
            {
                MessageBox.Show("您所删除的天线正在被使用，请修改辐射源或接收机后再删除此天线！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return b;
        }
       
        //单击更新菜单时，执行下面的操作
        public static bool AntennaUpdateMenu(TreeNode currentNode)
        {
            bool b = false;
            //保存setup文件中的字符串
            string setupStrTemp = MainWindow.setupStr;

            int initialAntennaBlockSite;
            string initAntennaNum = null;
            string antennaNodeInfoTemp = null;
            int initialAntennaNodeSite;
            string antennaParStrTemp = null; 
            try
            {
                //保存天线在setup文件中的位置
                initialAntennaBlockSite = MainWindow.setupStr.IndexOf(SetupContent.antennaStr1 + " " + currentNode.Text + "\r\n");
                //保存info文件中的所有字符串，原来天线的编号也要保存下来
                initAntennaNum = GetAntennaNumLine(currentNode.Text, MainWindow.setupStr);
                antennaNodeInfoTemp = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                //保存天线在waveinfo文件中的位置信息
                initialAntennaNodeSite = antennaNodeInfoTemp.IndexOf(SetupContent.antennaStr1 + " " + currentNode.Text + "\r\n");
                antennaParStrTemp = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
            }
            catch (Exception e)
            {
                return false;
            }

            NewAntennaWindow newAntennaWin = new NewAntennaWindow();
            //newAntennaWin.newdipolewaveformname_comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
           
            AntennaDataRecoverOfProjectTree.AlterationAntennaPar(currentNode, antennaParStrTemp, newAntennaWin);
            //newAntennaWin.newdipolewaveformname_comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!(AntennaDelOfSetupAndInfo(currentNode) && AntennaDelOfWaveInfo(currentNode)))
            {
                MessageBox.Show("文件被破坏，导致错误，原工程不可再用！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            MainWindow.creatSuccMesDisp = false;
            MainWindow.newFuncSign = false;
            newAntennaWin.Text="更改天线";
            newAntennaWin.ShowDialog();
           
            switch (newAntennaWin.DialogResult)
            {
                case DialogResult.OK:
                    if (MainWindow.IsReturnMidwayInNewProcess)
                    {
                        FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                        MainWindow.setupStr = setupStrTemp;
                        FileOperation.WriteFile(antennaParStrTemp, MainWindow.waveinfoFilePath, false);
                        FileOperation.WriteFile(antennaNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        MainWindow.IsReturnMidwayInNewProcess = false;
                        return false;
                    }
                    StringFound.DelBackIndStr(SetupContent.antennaStr1 + " " + currentNode.Text + "#");

                    if (!setupStrRecoverInitAntennaNum(currentNode,initAntennaNum, initialAntennaBlockSite, initialAntennaNodeSite))
                    {
                        MessageBox.Show("程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop );
                       
                        FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                        MainWindow.setupStr = setupStrTemp;
                        FileOperation.WriteFile(antennaParStrTemp, MainWindow.waveinfoFilePath, false);
                        FileOperation.WriteFile(antennaNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        return false ;
                    }


                    MainWindow.staticTreeView.SelectedNode.Text = MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].LastNode.Text;
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[2].LastNode.Remove();
                    b = true;
                    break;
                default:
                    FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                    MainWindow.setupStr = setupStrTemp;
                    FileOperation.WriteFile(antennaParStrTemp, MainWindow.waveinfoFilePath, false);
                    FileOperation.WriteFile(antennaNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                    b = false;
                    break;
            }
            //b = true;
            return b;
        }
        //根据天线名antennaName,在源字符串sourceStr中找到天线的编号
        private static string GetAntennaNumLine(string antennaName,String sourceStr)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int antennaNameSite = sourceStr.IndexOf(SetupContent.antennaStr1 + " " + antennaName + "\r\n") + (SetupContent.antennaStr1 + " " + antennaName + "\r\n").Length ;
                while (sourceStr[antennaNameSite] != '\r')
                {
                    sb = sb.Append(sourceStr[antennaNameSite]);
                    antennaNameSite++;
                }
             }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                throw e;
            }
            return sb.ToString();
        }
        private static  bool setupStrRecoverInitAntennaNum(TreeNode currentNode,string initAntennaNum, int initialAntennaBlockSite, int initialAntennaNodeSite)
        {
            bool b = false;
            //将单击新建波形窗口中确定按钮后插入的新波形块提取出来 放在newInsertAntennaBlock
            try
            {
                int start = MainWindow.setupStr.LastIndexOf(SetupContent.antennaStr1);
                int end = MainWindow.setupStr.LastIndexOf(SetupContent.antennaStr3 + "\r\n") + (SetupContent.antennaStr3+"\r\n").Length ;
                //提取出来单击新建天线确定按钮后的心添加的相应的天线块
                string newInsertAntennaBlock = MainWindow.setupStr.Substring(start, end - start);
                //将新增加的天线块删除
                MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);

                string antennaName = StringFound.FoundBackStr(SetupContent.antennaStr1, newInsertAntennaBlock, true);
                //获得新增加天线的编号
                string newAntennaNum = GetAntennaNumLine(antennaName, newInsertAntennaBlock);
                //用原来天线的编号去代替新加天线的编号
                newInsertAntennaBlock = newInsertAntennaBlock.Replace(newAntennaNum, initAntennaNum);
                //将修改了天线编号后的天线块插入到setupStr中
                MainWindow.setupStr = MainWindow.setupStr.Insert(initialAntennaBlockSite, newInsertAntennaBlock);
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);

                string antennaAndTranMatch = FileOperation.ReadFile(MainWindow.relationOfAntAndWavePath);            
                antennaAndTranMatch = antennaAndTranMatch.Replace("*"+currentNode.Text +"\r\n","*" + antennaName+"\r\n");         
                FileOperation.WriteFile(antennaAndTranMatch, MainWindow.relationOfAntAndWavePath, false);

                string antennaNodeInfo = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                //在info文件中先删除增加的天线结点信息
                antennaNodeInfo = antennaNodeInfo.Remove(antennaNodeInfo.LastIndexOf(SetupContent.antennaStr1));
                //将更改后的天线信息插入到原来的位置
                antennaNodeInfo = antennaNodeInfo.Insert(initialAntennaNodeSite, SetupContent.antennaStr1 + " " + antennaName+"\r\n");
                FileOperation.WriteFile(antennaNodeInfo, MainWindow.nodeInfoFullPath, false);
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
