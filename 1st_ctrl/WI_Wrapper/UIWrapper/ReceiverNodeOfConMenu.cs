using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace WF1
{
    class ReceiverNodeOfConMenu
    {
        //删除rx文件和info文件中接收机的相关信息
        private static bool ReceiverDelOfRxAndInfoFile(TreeNode currentNode)
        {
            bool b = false;
            string rxFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx";
            string rxFileStr = FileOperation.ReadFile(rxFilePath);
            
            string beginStr = null;
            string endStr = null;
            if (currentNode.NextNode != null)
            {
                switch (currentNode.Parent.Text)
                {
                    case "点状分布":
                        beginStr = SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        endStr = SetupContent.pointReceiverOfRxStr0 + " " + currentNode.NextNode.Text + "\r\n";
                        break;
                    case "区域分布":
                        beginStr = SetupContent.gridReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        endStr = SetupContent.gridReceiverOfRxStr0 + " " + currentNode.NextNode.Text + "\r\n";
                        break;
                }
                int start = rxFileStr.IndexOf(beginStr);
                //只能用LastIndexOf(nextNodeStr) 因为当删除的接收机结点是树中同一级的最后一个时，只能查找end串
                int end = rxFileStr.LastIndexOf(endStr);
                rxFileStr = rxFileStr.Remove(start, end - start);             
            }
            else
            {
                switch (currentNode.Parent.Text)
                {
                    case "点状分布":
                        beginStr = SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        endStr = SetupContent.transmitterStr24OfTr+"\r\n";
                        break;
                    case "区域分布":
                        beginStr = SetupContent.gridReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        endStr = SetupContent.gridReceiverOfRxStr11+"\r\n";
                        break;
                }
                int start = rxFileStr.IndexOf(beginStr);
                //只能用LastIndexOf(nextNodeStr) 因为当删除的接收机结点是树中同一级的最后一个时，只能查找end串
                int end = rxFileStr.LastIndexOf(endStr)+endStr.Length;
                rxFileStr = rxFileStr.Remove(start, end - start);               
            }
            //删除后保存到rx文件中
            FileOperation.WriteFile(rxFileStr, rxFilePath, false);
            
            //还要从.info文件中将波形的信息删除
            try
            {
                string infoStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                infoStr = infoStr.Remove(infoStr.IndexOf(beginStr ), beginStr.Length );
                FileOperation.WriteFile(infoStr, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                //LogFileManager.ObjLog.fatal(e.Message, e);
                b = false;
                throw e;
            }
            return b;
        }
        //删除接收机时要更新setup文件中接收机的数目
        private static bool ReceiverUpdateOfSetup()
        {
            bool b = false;
            string rxFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx";
            string rxFileStr = FileOperation.ReadFile(rxFilePath);
            try
            {
                //如果接收机全部被删除
                if (rxFileStr.Length <=0)//删除后
                {
                    File.Delete(rxFilePath);

                    int start = MainWindow.setupStr.IndexOf(SetupContent.receiverOfSetupStr0 + "\r\n");
                    int end = MainWindow.setupStr.IndexOf(SetupContent.receiverOfSetupStr3 + "\r\n") + (SetupContent.receiverOfSetupStr3 + "\r\n").Length;
                    MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);
                    FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                   
                }
                //接收机没有全部被删除
                else
                {
                    string receiverCountStr = StringFound.FoundBackStr("\nFirstAvailableRxNumber", MainWindow.setupStr, true);
                    string receiverCountLineOld = "\nFirstAvailableRxNumber " + receiverCountStr + "\r\n";
                    int receiverCount = int.Parse(receiverCountStr);
                    receiverCount = receiverCount - 1;
                    string receiverCountLineNew = "\nFirstAvailableRxNumber " + receiverCount.ToString() + "\r\n";
                    MainWindow.setupStr = MainWindow.setupStr.Replace(receiverCountLineOld, receiverCountLineNew);
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
        //从waveinfo文件中将接收机的信息删除
        private static bool ReceiverDelOfWaveInfo(TreeNode currentNode)
        {
            bool b = false;
            try
            {
                string receiverAllInfo = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
                string beginStr = null;
                switch (currentNode.Parent.Text)
                {
                    case "点状分布":
                        beginStr = SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        break;
                    case "区域分布":
                        beginStr = SetupContent.gridReceiverOfRxStr0 + " " + currentNode.Text + "\r\n";
                        break;
                }
                //只能用IndexOf(receiverName)函数，因为"END" + receiverName也含有receiverName
                int delStart = receiverAllInfo.IndexOf(beginStr );
                int delEnd = receiverAllInfo.LastIndexOf("END" + beginStr ) + ("END" + beginStr ).Length ;
                receiverAllInfo = receiverAllInfo.Remove(delStart, delEnd - delStart);
                FileOperation.WriteFile(receiverAllInfo, MainWindow.waveinfoFilePath, false);
                //删除match文件中有关辐射源的信息
                //StringFound.DelBackIndStr(SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text+"*");
                b = true;
            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.fatal(ex.Message, ex);
                //b = false;
                //throw ex;
            }
            return b;
        }
        //当单击删除时执行此函数
        public static bool ReceiverDelMenu(TreeNode currentNode)
        {
            bool b = false;
            DialogResult result = MessageBox.Show("您确定要删除\"" + currentNode.Text + "\"接收机吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                try
                {
                    //分别调用上面的三个函数
                    if (ReceiverDelOfRxAndInfoFile(currentNode) && ReceiverDelOfWaveInfo(currentNode) && ReceiverUpdateOfSetup())
                    {
                        switch (currentNode.Parent.Text)
                        {
                            case "点状分布":
                                //删除match文件中有关辐射源的信息
                                StringFound.DelBackIndStr(SetupContent.pointReceiverOfRxStr0 + " " + currentNode.Text + "*");
                                break;
                            case "区域分布":
                                StringFound.DelBackIndStr(SetupContent.gridReceiverOfRxStr0 + " " + currentNode.Text + "*");
                                break;
                        }
                        b = true;
                    }
                }
                catch (Exception e)
                {
                    LogFileManager.ObjLog.fatal(e.Message, e);
                    //b = false;
                    //MainWindow.IsReturnMidwayInNewProcess = true;
                }
            }
            return b;
        }
        //单击更改时执行此方法
        public static bool ReceiverUpdateMenu(TreeNode currentNode)
        {
            //bool b = false;
            string rxFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx";
            //保存Tr文件中的字符串
            string rxFileStrTemp = FileOperation.ReadFile(rxFilePath);
            string indStr = null;
            switch (currentNode.Parent.Text)
            {
                case "点状分布":
                    indStr = SetupContent.pointReceiverOfRxStr0 ;
                    break;
                case "区域分布":
                    indStr = SetupContent.gridReceiverOfRxStr0 ;
                    break;
            }
           
            //将接收机在rx文件中的位置保护起来
            int initialReceiverBlockSite = rxFileStrTemp.IndexOf(indStr + " " + currentNode.Text + "\r\n");
            //rx文件中原来接收机的编号也要保存下来
            string initReceiverNum = GetReceiverNumLine(indStr,currentNode.Text, rxFileStrTemp);
            //保存info文件中的所有字符串
            string receiverNodeInfoTemp = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
            //保存接收机在info文件中的位置信息
            int initialReceiverNodeSite = receiverNodeInfoTemp.IndexOf(indStr + " " + currentNode.Text + "\r\n");
            //保存waveinfo文件中有关接收机的信息
            string receiverParStrTemp = FileOperation.ReadFile(MainWindow.waveinfoFilePath);

            NewReceiverWindow newReceiverWin = new NewReceiverWindow();
            //将接收机原来的参数信息装载到窗口中
            if (!ReceiverDataRecoverOfProjectTreeView.RecoverReceiverParOfWin(currentNode, receiverParStrTemp, newReceiverWin))
            {
                MessageBox.Show("程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                FileOperation.RecoverOldRevision(rxFileStrTemp, rxFilePath, receiverParStrTemp, receiverNodeInfoTemp);
                return false;
            }
            //ReceiverDataRecoverOfProjectTreeView.RecoverReceiverParOfWin(currentNode, receiverParStrTemp, newReceiverWin);
            //在单击确定按钮之前应将需更改的接收机的信息从各个文件中删除
            if (!(ReceiverDelOfRxAndInfoFile(currentNode) && ReceiverDelOfWaveInfo(currentNode) && ReceiverUpdateOfSetup()))
            {
                MessageBox.Show("程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                FileOperation.RecoverOldRevision(rxFileStrTemp, rxFilePath, receiverParStrTemp, receiverNodeInfoTemp);
                return false;
            }

            MainWindow.creatSuccMesDisp = false;
            MainWindow.newFuncSign = false;
            newReceiverWin.Text = "更改接收机";
            newReceiverWin.ShowDialog();
            switch (newReceiverWin.DialogResult)
            {
                case DialogResult.OK:
                    if (MainWindow.IsReturnMidwayInNewProcess)
                    {
                        //FileOperation.WriteFile(rxFileStrTemp, rxFilePath, false);
                        //FileOperation.WriteFile(receiverParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(receiverNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        FileOperation.RecoverOldRevision(rxFileStrTemp, rxFilePath, receiverParStrTemp, receiverNodeInfoTemp);
                        MainWindow.IsReturnMidwayInNewProcess = false;
                        return false;
                    }
                    if (!rxStrRecoverInitReceiverNum(currentNode , initReceiverNum, initialReceiverBlockSite, initialReceiverNodeSite))
                    {
                        if (currentNode.Parent.Text.Equals("点状分布"))
                        {
                         
                                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0].LastNode.Remove();
                        }
                        else
                        {       
                                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1].LastNode.Remove();
                        }
                        

                        MessageBox.Show("程序内部发生错误,更改操作失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        FileOperation.RecoverOldRevision(rxFileStrTemp, rxFilePath, receiverParStrTemp, receiverNodeInfoTemp);
                        //FileOperation.WriteFile(rxFileStrTemp, rxFilePath, false);
                        //FileOperation.WriteFile(receiverParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(receiverNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                        return false;
                    }
                    switch (currentNode.Parent.Text)
                    {
                        case "点状分布":
                            MainWindow.staticTreeView.SelectedNode.Text = MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0].LastNode.Text;
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0].LastNode.Remove();
                            break;
                        case "区域分布":
                            MainWindow.staticTreeView.SelectedNode.Text = MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1].LastNode.Text;
                            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1].LastNode.Remove();
                            break;
                    }
                    return true;
                    //break;
                default:
                    FileOperation.RecoverOldRevision(rxFileStrTemp, rxFilePath, receiverParStrTemp, receiverNodeInfoTemp);
                        //FileOperation.WriteFile(rxFileStrTemp, rxFilePath, false);

                        //FileOperation.WriteFile(receiverParStrTemp, MainWindow.waveinfoFilePath, false);
                        //FileOperation.WriteFile(receiverNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                    return false;
                    //break;
            }
            //b = true;
            //return b;
        }
        //从sourceStr中找出receiverName所对应的编号行，因为接收机有两种，根据第一个参数索引字符串确定
        private static string GetReceiverNumLine(string indexOfPointsOrGrid,string receiverName, string sourceStr)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int receiverNameSite = sourceStr.IndexOf(indexOfPointsOrGrid + " " + receiverName + "\r\n") + (indexOfPointsOrGrid + " " + receiverName + "\r\n").Length ;
                while (sourceStr[receiverNameSite] != '\r')
                {
                    sb = sb.Append(sourceStr[receiverNameSite]);
                    receiverNameSite++;
                }
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return sb.ToString();
        }
        //将单击确定按钮后新插入的接收机的编号替换为原来的编号，并利用原来保存的位置信息写到相应的文件中
        private static bool rxStrRecoverInitReceiverNum(TreeNode currentNode,string initReceiverNum, int initialReceiverBlockSite, int initialReceiverNodeSite)
        {
            bool b = false;
            //将单击新建接收机窗口中确定按钮后rx文件中新插入的接收机块提取出来 放在newInsertReceiverBlock
            try
            {
                string rxFilePath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx";
                string rxFileStr = FileOperation.ReadFile(rxFilePath);

                string beginStr = null;
                string endStr = null;
                switch (currentNode.Parent.Text)
                {
                    case "点状分布":
                        beginStr = SetupContent.pointReceiverOfRxStr0;
                        endStr = SetupContent.transmitterStr24OfTr;
                        break;
                    case "区域分布":
                        beginStr = SetupContent.gridReceiverOfRxStr0;
                        endStr = SetupContent.gridReceiverOfRxStr11;
                        break;
                }
                int start = rxFileStr.LastIndexOf(beginStr );
                int end = rxFileStr.LastIndexOf(endStr ) + endStr.Length + 2;
                //提取出来单击新建接收机确定按钮后的新添加的相应的接收机块
                string newInsertReceiverBlock = rxFileStr.Substring(start, end - start);
                //将新增加的接收机块删除
                rxFileStr = rxFileStr.Remove(start, end - start);

                string receiverName = StringFound.FoundBackStr(beginStr , newInsertReceiverBlock, true);
                //获得新增加接收机的编号
                string newReceiverNum = GetReceiverNumLine(beginStr , receiverName, newInsertReceiverBlock);
                //用原来接收机的编号去代替新加接收机的编号
                newInsertReceiverBlock = newInsertReceiverBlock.Replace(newReceiverNum, initReceiverNum);
                //将修改了接收机编号后的接收机块插入到setupStr中
                rxFileStr = rxFileStr.Insert(initialReceiverBlockSite, newInsertReceiverBlock);
                FileOperation.WriteFile(rxFileStr, rxFilePath, false);

                string receiverNodeInfo = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                
                //将更改后的接收机信息插入到原来的位置
                receiverNodeInfo = receiverNodeInfo.Insert(initialReceiverNodeSite, beginStr  + " " + receiverName + "\r\n");
                //在info文件中删除增加的接收机结点信息
                receiverNodeInfo = receiverNodeInfo.Remove(receiverNodeInfo.LastIndexOf(beginStr ));

                FileOperation.WriteFile(receiverNodeInfo, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
                //return false;
            }
            return b;
        }
    }
}
