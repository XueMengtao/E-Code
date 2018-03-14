using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF1
{
    class WaveformNodeOfConMenu
    {

        public static bool cancelOfUpdate = false;
        public static bool WaveformDelOfSetupAndInfo(TreeNode currentNode)
        {
            bool b = false;
            string indStr = SetupContent.waveFormStr1 + " " + currentNode.Text+"\r\n";
           
            int start = MainWindow.setupStr.IndexOf(indStr );
            //只能用LastIndexOf(nextNodeStr) 因为当删除的波形结点是树中同一级的最后一个时，只能查找end_<Waveform>
       
            int end;
            //删除的结点后面有没有结点，索引的天线结束字符串不一样
            //当删除结点后面没有结点时，索引end_<antenna>
            if (currentNode.NextNode != null)
            {
                end = MainWindow.setupStr.LastIndexOf(SetupContent.waveFormStr1 + " " + currentNode.NextNode.Text + "\r\n");
            }
            //当删除结点后面还有结点时，索引begin_<antenna> currentNode.Text字符串
            else
            {
                end = MainWindow.setupStr.LastIndexOf(SetupContent.waveFormStr2 + "\r\n") + (SetupContent.waveFormStr2 + "\r\n").Length;
            }
            try
            {
                MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);
                //删除后保存到setup文件中
                FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
                //还要从.info文件中将天线的信息删除
                string infoStr = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
                infoStr = infoStr.Remove(infoStr.IndexOf(SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n"), (SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n").Length);

                FileOperation.WriteFile(infoStr, MainWindow.nodeInfoFullPath, false);
                b = true;
            }
            catch (Exception e)
            {
                LogFileManager.ObjLog.fatal(e.Message, e);
            }
            return b;
        }
       private static bool WaveformDelOfWaveInfo(TreeNode currentNode)
       {
           bool b = false;
           try
           {
               string waveformAllInfo = FileOperation.ReadFile(MainWindow.waveinfoFilePath);
               //只能用IndexOf(waveName)函数，因为"END" + antennaName也含有antennaName
               int delStart = waveformAllInfo.IndexOf(SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n");
               int delEnd = waveformAllInfo.LastIndexOf("END" + SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n") + ("END" + SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n").Length;
               waveformAllInfo = waveformAllInfo.Remove(delStart, delEnd - delStart);
               FileOperation.WriteFile(waveformAllInfo, MainWindow.waveinfoFilePath, false);
               
               b = true;
           }
           catch (Exception ex)
           {
               LogFileManager.ObjLog.fatal(ex.Message, ex);
           }
           return b;
       }
       public static bool WaveformDelMenu(TreeNode currentNode)
       {
           bool b = false;
           if (!StringFound.WaveOrAntennaInUse(currentNode.Text, "wave"))
           {
               DialogResult result = MessageBox.Show("您确定要删除\"" + currentNode.Text + "\"波形吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
               if (result == DialogResult.OK)
               {
                   //AntennaDelOfSetupAndInfo(currentNode);
                   //AntennaDelOfWaveInfo(currentNode);
                   if (WaveformDelOfSetupAndInfo(currentNode) && WaveformDelOfWaveInfo(currentNode))
                   {
                       b = true;
                   }
               }
           }
           else
           {
               MessageBox.Show("您所删除的波形正在被使用，请修改天线后再删除此波形！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               //b = true;
           }
           return b;
       }
       public static bool WaveformUpdateMenu(TreeNode currentNode)
       {
           //先将setupStr保存在一个字符串setupStrTemp中
           string setupStrTemp =MainWindow.setupStr;
           //将waveinfo文件中的内容保存在字符串waveAllInfoTemp中
           string waveformParStrTemp = FileOperation.ReadFile(MainWindow.waveinfoFilePath);

           //将info文件的内容保存到字符串allNodesInfoTemp中
           string waveformNodeInfoTemp = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
           int initialWaveformNodeSite = waveformNodeInfoTemp.IndexOf(SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n");
           int initialWaveformBlockSite = MainWindow.setupStr.IndexOf(SetupContent.waveFormStr1 + " " + currentNode.Text + "\r\n");

           NewWaveformWindow wfw = new NewWaveformWindow();
           //向窗体中的控件填写以前的内容
           WaveformDataRecoverOfProjectTree.AlterationWavePar(currentNode, waveformParStrTemp, wfw);
           //int siteOfUpdateWave = setupStr.IndexOf(SetupContent.waveFormStr1 + " " + currentNode.Text);

           //保存波形编号的那一行
           string initWaveformNum = GetWaveformNumLine(currentNode.Text, MainWindow.setupStr);

           //先删除需要更改的波形，否则同名的波形不能写到setup文件中
           if (!(WaveformDelOfSetupAndInfo(currentNode) && WaveformDelOfWaveInfo(currentNode)))
           {
               MessageBox.Show("文件被破坏，导致错误，原工程不可再用！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return false;
           }

           MainWindow.creatSuccMesDisp = false;
           MainWindow.newFuncSign = false;
           wfw.Text = "更改波形";

           //新建波形窗口可见，只能用showDialog()
           wfw.ShowDialog();
           //判断在新建波形窗口中单击的是哪个按钮
           switch (wfw.DialogResult)
           {
               case DialogResult.OK:
                   //删除单击确定按钮后新添加的波形结点
                   //在新建波形窗口中单击确定按钮后，会在工程树波形结点的最后添加更改的波形，将其的名字赋给选中的结点，
                   //再将确定按钮增加的波形删除掉
                   if (MainWindow.IsReturnMidwayInNewProcess)
                   {
                       //myEventArgs e = new myEventArgs( setupStrTemp, waveformParStrTemp, waveformNodeInfoTemp);
                       //wfw.newsinusoidcancel_button2.Click -= new System.EventHandler(wfw.newSinusoidCancel_button2_Click);
                       //wfw.newsinusoidcancel_button2.Click +=e.A ;
                       //wfw.ShowDialog();

                       FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                       MainWindow.setupStr = setupStrTemp;
                       FileOperation.WriteFile(waveformParStrTemp, MainWindow.waveinfoFilePath, false);
                       FileOperation.WriteFile(waveformNodeInfoTemp, MainWindow.nodeInfoFullPath, false);



                       MainWindow.IsReturnMidwayInNewProcess = false;
                       return false;
                   }
                   //if (cancelOfUpdate)
                   //{
                   //    return true;
                   //}
                   if (!setupStrRecoverInitWaveformNum(currentNode, initWaveformNum, initialWaveformBlockSite, initialWaveformNodeSite))
                   {
                       MessageBox.Show("程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                       FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                       MainWindow.setupStr = setupStrTemp;
                       FileOperation.WriteFile(waveformParStrTemp, MainWindow.waveinfoFilePath, false);
                       FileOperation.WriteFile(waveformNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                       return false;
                   }
                   MainWindow.staticTreeView.SelectedNode.Text = MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].LastNode.Text;
                   MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[1].LastNode.Remove();
                   return true;
               default:
                   FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
                   MainWindow.setupStr = setupStrTemp;
                   FileOperation.WriteFile(waveformParStrTemp, MainWindow.waveinfoFilePath, false);
                   FileOperation.WriteFile(waveformNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
                   //b= false;
                   return false;
           }
           //return b;
       }
      public  static string GetWaveformNumLine(string waveformName, string sourceStr)
       {
           string sb = "";
           try
           {
               int waveformNameSite = sourceStr.IndexOf(SetupContent.waveFormStr1 + " " + waveformName + "\r\n") + (SetupContent.waveFormStr1 + " " + waveformName + "\r\n").Length;
               for (int i = 0; i < 2; i++)
               {
                   sb = "";
                   while (sourceStr[waveformNameSite] != '\r')
                   {
                       sb = sb+sourceStr[waveformNameSite];
                       waveformNameSite++;
                   }
                   waveformNameSite = waveformNameSite+2;
                   
               }
           }
           catch (Exception e)
           {
               LogFileManager.ObjLog.fatal(e.Message, e);
               throw e;
           }
           return sb;
       }
       private static bool setupStrRecoverInitWaveformNum(TreeNode currentNode, string initWaveformNum, int initialWaveformBlockSite, int initialWaveformNodeSite)
       {
           bool b = false;
           //将单击新建波形窗口中确定按钮后插入的新波形块提取出来 放在newInsertAntennaBlock
           try
           {
               int start = MainWindow.setupStr.LastIndexOf(SetupContent.waveFormStr1);
               int end = MainWindow.setupStr.LastIndexOf(SetupContent.waveFormStr2 + "\r\n") + (SetupContent.waveFormStr2 + "\r\n").Length;
               //提取出来单击新建天线确定按钮后的心添加的相应的天线块
               string newInsertWaveformBlock = MainWindow.setupStr.Substring(start, end - start);
               //将新增加的天线块删除
               MainWindow.setupStr = MainWindow.setupStr.Remove(start, end - start);

               string waveformName = StringFound.FoundBackStr(SetupContent.waveFormStr1, newInsertWaveformBlock, true);
               //获得新增加天线的编号
               string newAntennaNum = GetWaveformNumLine(waveformName, newInsertWaveformBlock);
               //用原来天线的编号去代替新加天线的编号
               newInsertWaveformBlock = newInsertWaveformBlock.Replace(newAntennaNum, initWaveformNum);
               //将修改了天线编号后的天线块插入到setupStr中
               MainWindow.setupStr = MainWindow.setupStr.Insert(initialWaveformBlockSite, newInsertWaveformBlock);
               FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
               if(System.IO.File.Exists(MainWindow.relationOfAntAndWavePath))
               {
                    string updatedWaveformAndAntennaMatch = FileOperation.ReadFile(MainWindow.relationOfAntAndWavePath);
                    updatedWaveformAndAntennaMatch = updatedWaveformAndAntennaMatch.Replace("#" + currentNode.Text + "\r\n", "#" + waveformName + "\r\n");
                    FileOperation.WriteFile(updatedWaveformAndAntennaMatch, MainWindow.relationOfAntAndWavePath, false);
               }
               string waveformNodeInfo = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
               //在info文件中先删除增加的天线结点信息
               waveformNodeInfo = waveformNodeInfo.Remove(waveformNodeInfo.LastIndexOf(SetupContent.waveFormStr1));
               //将更改后的天线信息插入到原来的位置
               waveformNodeInfo = waveformNodeInfo.Insert(initialWaveformNodeSite, SetupContent.waveFormStr1 + " " + waveformName + "\r\n");
               FileOperation.WriteFile(waveformNodeInfo, MainWindow.nodeInfoFullPath, false);
               b = true;
           }
           catch (Exception e)
           {
               LogFileManager.ObjLog.fatal(e.Message, e);
           }
           return b;
       }
       //private static void A(string setupStrTemp,string waveformParStrTemp,string waveformNodeInfoTemp)
       //{
       // FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
       // MainWindow.setupStr = setupStrTemp;
       // FileOperation.WriteFile(waveformParStrTemp, MainWindow.waveinfoFilePath, false);
       // FileOperation.WriteFile(waveformNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
       //}
    }
    //class myEventArgs : EventArgs
    //{
    //    string setupStrTemp;
    //    string waveformParStrTemp;
    //    string waveformNodeInfoTemp;
    //    public  myEventArgs(string setupStrTemp, string waveformParStrTemp, string waveformNodeInfoTemp)
    //        //: base()
    //    {
    //        this.setupStrTemp = setupStrTemp;
    //        this.waveformParStrTemp = waveformParStrTemp;
    //        this.waveformNodeInfoTemp = waveformNodeInfoTemp;
    //    }
    //    public   void A(Object sender,EventArgs e)
    //    {
    //        FileOperation.WriteFile(setupStrTemp, MainWindow.mProjectFullName, false);
    //        MainWindow.setupStr = setupStrTemp;
    //        FileOperation.WriteFile(waveformParStrTemp, MainWindow.waveinfoFilePath, false);
    //        FileOperation.WriteFile(waveformNodeInfoTemp, MainWindow.nodeInfoFullPath, false);
    //        WaveformNodeOfConMenu.cancelOfUpdate = true;
    //    }
    //}
}
