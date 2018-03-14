using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace WF1
{
    class TransmitterNodeOfConMenu
    {
        //将相应的辐射源信息从Tx文件和info文件中删除
        private static bool TransmitterDelOfTrAndInfoFile(TreeNode currentNode)
        {
            bool b = false;
            string trFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx";
            string trFileStr = FileOperation.ReadFile(trFilePath );
            int start = trFileStr.IndexOf(SetupContent.transmitterStr1OfTr + " " + currentNode.Text + "\r\n");
            //只能用LastIndexOf(nextNodeStr) 因为当删除的辐射源结点是树种同一级的最后一个时，只能查找end_<points>
            int end;
            //工程树种删除的结点后面有没有结点，索引的辐射源结束字符串不一样
            //当删除结点后面还有结点时，索引begin_<points> currentNode.Text字符串
            if (currentNode.NextNode != null)
            {
                end = trFileStr.LastIndexOf(SetupContent.transmitterStr1OfTr + " " + currentNode.NextNode.Text+"\r\n");
            }
            //当删除结点后面没有辐射源结点时，索引end_<points>
            else
            {
                //end = trFileStr.LastIndexOf(SetupContent.transmitterStr24OfTr + "\r\n") + (SetupContent.transmitterStr24OfTr+"\r\n").Length ;
                end = trFileStr.LastIndexOf("MHZ\r\n") + ("MHZ\r\n").Length;

            }
            try
            {
                //从tr文件中删除辐射源的信息
                trFileStr = trFileStr.Remove(start, end - start);
                //删除后保存到tr文件中
                FileOperation.WriteFile(trFileStr, trFilePath , false);

                string infoStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                infoStr = infoStr.Remove(infoStr.IndexOf(SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text + "\r\n"), (SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text + "\r\n").Length);
                //还要从.info文件中将波形的信息删除
                FileOperation.WriteFile(infoStr, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                //b = false;
                //throw e;
             }
            return b;
        }
        //删除辐射源后应及时更新setup文件中关于辐射源的信息
        private static bool TransmitterUpdateOfSetup()
        {
            bool b = false;
            string trFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx";
            string trFileStr = FileOperation.ReadFile(trFilePath);
            try
            {
                //当tr文件中的辐射源全部被删除时
                if (trFileStr.Length <= 0)
                {
                    //辐射源全部被删除时,要删除tr文件
                    File.Delete(trFilePath);
                    //辐射源全部被删除时,setup文件中关于辐射源的信息也要被删除
                    int start = MainWindow.setupStr.IndexOf(SetupContent.transmitterStr1Ofsetup + "\r\n");
                    int end = MainWindow.setupStr.IndexOf(SetupContent.transmitterStr4Ofsetup + "\r\n") + SetupContent.transmitterStr4Ofsetup.Length ;
                    MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);
                    //将删除辐射源后的setupStr保存到setup文件中
                    FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                }
                //当tr文件中的辐射源没有全部被删除时
                else
                {
                    //使setup文件中辐射源的数量减1
                    string transmitterCountStr = StringFound.FoundBackStr("\nFirstAvailableTxNumber", MainWindow.setupStr, true);
                    string transmitterCountLineOld = "\nFirstAvailableTxNumber " + transmitterCountStr + "\r\n";
                    int transmitterCount = int.Parse(transmitterCountStr);
                    transmitterCount = transmitterCount - 1;
                    string transmitterCountLineNew = "\nFirstAvailableTxNumber " + transmitterCount.ToString() + "\r\n";
                    MainWindow.setupStr = MainWindow.setupStr.Replace(transmitterCountLineOld, transmitterCountLineNew);
                    FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                }
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        //从waveinfo文件中删除相应的辐射源信息
        private static bool TransmitterDelOfWaveInfo(TreeNode currentNode)
        {
            bool b = false;
            try
            {
                string transmitterAllInfo = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
                //只能用IndexOf(transmitterName)函数，因为"END" + antennaName也含有antennaName
                string indStr = SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text + "\r\n";
                int delStart = transmitterAllInfo.IndexOf(indStr );
                int delEnd = transmitterAllInfo.LastIndexOf("END" + indStr ) + ("END" + indStr).Length ;
                transmitterAllInfo = transmitterAllInfo.Remove(delStart, delEnd - delStart);
                FileOperation.WriteFile(transmitterAllInfo, MainWindow.waveinfoFilePath, false);

                StringFound.DelBackIndStr(SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text);
                b = true;
            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.fatal(ex.Message, ex);
            }
            return b;
        }
        //当单击删除时执行此函数
        public static bool TransmitterDelMenu(TreeNode currentNode)
        {
            bool b=false;
            DialogResult result = MessageBox.Show("您确定要删除\"" + currentNode.Text + "\"辐射源吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                if (TransmitterDelOfTrAndInfoFile(currentNode) && TransmitterDelOfWaveInfo(currentNode) && TransmitterUpdateOfSetup())
                {
                     b = true;
                 }
            }
            return b;
        }
        //单击更改时执行此函数
        public static bool TransmitterUpdateMenu(TreeNode currentNode)
        {
            //bool b = false;
            string trFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx";
            //将Tr文件中的字符串保护起来
            string trFileStrTemp = FileOperation.ReadFile(trFilePath);
         
            //将辐射源在tr文件中的位置保护起来
            int initialTransmitterBlockSite = trFileStrTemp.IndexOf(SetupContent.transmitterStr1OfTr + " " + currentNode.Text+"\r\n");
            //tx文件中原来辐射源的编号也要保存下来
            string initTransmitterNum = GetTransmitterNumLine(currentNode.Text, trFileStrTemp );
            //保存info文件中的所有字符串
            string transmitterNodeInfoTemp = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
            //保存辐射源在info文件中的位置信息
            int initialTransmitterNodeSite = transmitterNodeInfoTemp.IndexOf(SetupContent.transmitterStr1Ofsetup + " " + currentNode.Text+"\r\n");
            //保存waveinfo文件中有关辐射源的信息
            string transmitterParStrTemp = FileOperation.ReadFile(MainWindow.waveinfoFilePath);

            NewTransmitterWindow newTransmitterWin = new NewTransmitterWindow();
            //将原来辐射源设置的参数信息恢复到窗口中
            if (!TransmitterDataRecoverOfProjectTreeView.RecoverTransmitterParOfWin(currentNode, transmitterParStrTemp, newTransmitterWin))
            {
                MessageBox.Show("程序运行发生错误， 更改失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                FileOperation.RecoverOldRevision(trFileStrTemp, trFilePath, transmitterParStrTemp, transmitterNodeInfoTemp);
                return false;
            }

            //TransmitterDataRecoverOfProjectTreeView.RecoverTransmitterParOfWin(currentNode, transmitterParStrTemp, newTransmitterWin);
          //在单击确定之前应先删除需更改的辐射源的全部信息
            if(! (TransmitterDelOfTrAndInfoFile(currentNode) && TransmitterDelOfWaveInfo(currentNode) && TransmitterUpdateOfSetup()))
            {
                MessageBox.Show("程序运行发生错误, 工程文件可能被破坏，更改失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                FileOperation.RecoverOldRevision(trFileStrTemp, trFilePath, transmitterParStrTemp, transmitterNodeInfoTemp);
                return false;
            }

            MainWindow.creatSuccMesDisp = false;
            MainWindow.newFuncSign = false;
            newTransmitterWin.Text = "更改辐射源";
            newTransmitterWin.ShowDialog();
            switch (newTransmitterWin.DialogResult)
            {
                case DialogResult.OK:
                    //如果在更改过程中出现了中途返回的情况，则应把原来的信息写到相应的文件中
                    if (MainWindow.IsReturnMidwayInNewProcess)
                    {
                        //FileOperation.WriteFile(trFileStrTemp , trFilePath , false);
                        //FileOperation.WriteFile(transmitterParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(transmitterNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        FileOperation.RecoverOldRevision(trFileStrTemp, trFilePath, transmitterParStrTemp, transmitterNodeInfoTemp);

                        //将”全局变量“置为初始值
                        MainWindow.IsReturnMidwayInNewProcess = false;
                        return false;
                    }
                    if (!trStrRecoverInitTransmitterNum(initTransmitterNum, initialTransmitterBlockSite, initialTransmitterNodeSite))
                    {
                        MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3].LastNode.Remove();

                        MessageBox.Show("程序运行发生错误,更改操作失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                       
                        //FileOperation.WriteFile(trFileStrTemp, trFilePath, false);
                        //FileOperation.WriteFile(transmitterParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(transmitterNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        FileOperation.RecoverOldRevision(trFileStrTemp, trFilePath, transmitterParStrTemp, transmitterNodeInfoTemp);
                        return false;
                    }

                    MainWindow.staticTreeView.SelectedNode.Text = MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3].LastNode.Text;
                    MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[3].LastNode.Remove();
                    return  true;
                    //break;
                default:
                        //FileOperation.WriteFile(trFileStrTemp , trFilePath , false);
                        //FileOperation.WriteFile(transmitterParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(transmitterNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                    FileOperation.RecoverOldRevision(trFileStrTemp, trFilePath, transmitterParStrTemp, transmitterNodeInfoTemp);
                    return false ;
                    //break;
            }
            //return b;
        }
        //获取带有辐射源编号的那一行字符串，第一个参数是辐射源的名字；第二个参数是更改辐射源时新插入的辐射源块
        private static string GetTransmitterNumLine(string transmitterName, string sourceStr)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int transmitterNameSite = sourceStr.IndexOf(SetupContent.transmitterStr1OfTr + " " + transmitterName + "\r\n") + (SetupContent.transmitterStr1OfTr + " " + transmitterName + "\r\n").Length;
                while (sourceStr[transmitterNameSite] != '\r')
                {
                    sb = sb.Append(sourceStr[transmitterNameSite]);
                    transmitterNameSite++;
                }
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                throw e;
            }
            return sb.ToString();
        }
        //将更改后辐射源的编号调整为原来的编号
        private static bool trStrRecoverInitTransmitterNum(string initTransmitterNum, int initialTransmitterBlockSite, int initialTransmitterNodeSite)
        {
            bool b = false;
            //将单击新建波形窗口中确定按钮后tr文件中新插入的辐射源块提取出来 放在newInsertTransmitterBlock
            try
            {
                string trFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".tx";
                string trFileStr = FileOperation.ReadFile(trFilePath);
                int start = trFileStr.LastIndexOf(SetupContent.transmitterStr1OfTr);
                //int end = trFileStr.LastIndexOf(SetupContent.transmitterStr24OfTr + "\r\n") +( SetupContent.transmitterStr24OfTr + "\r\n").Length ;
                int end = trFileStr.LastIndexOf("MHZ\r\n") + ("MHZ\r\n").Length;
               
                //提取出来单击新建辐射源确定按钮后的新添加的相应的天线块
                string newInsertTransmitterBlock = trFileStr.Substring(start, end - start);
                //将新增加的辐射源块删除
                trFileStr  =trFileStr.Remove(start, end - start);
                //在更改时可能会改变辐射源的名字，因此需要在提取出辐射源的名字
                string transmitterName = StringFound.FoundBackStr(SetupContent.transmitterStr1OfTr, newInsertTransmitterBlock, true);
                //获得新增加辐射源的编号
                string newTransmitterNum = GetTransmitterNumLine(transmitterName, newInsertTransmitterBlock);
                //用原来天线的编号去代替新加辐射源的编号
                newInsertTransmitterBlock = newInsertTransmitterBlock.Replace(newTransmitterNum, initTransmitterNum);
                //将修改了辐射源编号后的天线块写入到tr文件中
                trFileStr  = trFileStr.Insert(initialTransmitterBlockSite, newInsertTransmitterBlock);
                FileOperation.WriteFile(trFileStr , trFilePath, false);

                string transmitterNodeInfo = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                //将更改后的辐射源信息插入到原来的位置
                transmitterNodeInfo = transmitterNodeInfo.Insert(initialTransmitterNodeSite, SetupContent.transmitterStr1Ofsetup + " " + transmitterName + "\r\n");
                //在info文件中先删除增加的辐射源节点信息
                transmitterNodeInfo = transmitterNodeInfo.Remove(transmitterNodeInfo.LastIndexOf(SetupContent.transmitterStr1Ofsetup));
                FileOperation.WriteFile(transmitterNodeInfo, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
        //private static void RecoverOldRevision(string trFileStrTemp, string trFilePath,string transmitterParStrTemp, string transmitterNodeInfoTemp)
        //{
        //    FileOperation.WriteFile(trFileStrTemp, trFilePath, false);
        //    FileOperation.WriteFile(transmitterParStrTemp, MainWindow.waveinfoFilePath, false);
        //    FileOperation.WriteFile(transmitterNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
        //}
    }
}
