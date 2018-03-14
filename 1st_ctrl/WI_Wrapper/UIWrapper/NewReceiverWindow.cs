using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Collections;
namespace WF1
{
    public partial class NewReceiverWindow : Form
    {
        string antennaNamesOfCombox = null;
        public NewReceiverWindow()
        {
            InitializeComponent();
            if ((MainWindow.setupStr == null) || MainWindow.mProjectFullName == null)
            {
                return;
            }
            if (File.Exists(MainWindow.nodeInfoFullPath))
            {
                antennaNamesOfCombox = FileOperation.ReadFile(MainWindow.nodeInfoFullPath);
            }
            if (MainWindow.nodeInfoFullPath != null)
            {
                newReceiverlongitude_textBox1.Text = MainWindow.longitudeStr;
                newReceiverLatitude_textBox3.Text = MainWindow.latitudeStr;
                ArrayList antennaNamesArr = new ArrayList();
                //向工程树中添加波形节点
                antennaNamesArr = ProjectTreeMatchString.MatchStr(antennaNamesOfCombox, SetupContent.antennaIndeStr, SetupContent.antennaIndeStr.Length);
                foreach (string s in antennaNamesArr)
                {
                    newReceiverAntennaName_comboBox3.Items.Add(s);
                }
            }

        }

        private void newReceiverClose_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newRceiverType_comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((string)newRceiverType_comboBox1.SelectedItem).Equals("点状<Points>"))
            {
                newReceiverSpace_textBox1.Enabled = false;
                newReceiverAntennaHeight_textBox1.Enabled = false;
            }
            else
            {
                newReceiverSpace_textBox1.Enabled = true;
                newReceiverAntennaHeight_textBox1.Enabled = true;
            }
        }

        private void newReceiverOk_button1_Click(object sender, EventArgs e)
        {
            if(!File.Exists(MainWindow.mProjectFullName))
            {
                this.Close();
                return;
            }
            if (!NewTransmitterWindow.ConditionIsAll(MainWindow.nodeInfoFullPath))
            {
                MessageBox.Show("请先创建天线和添加波形之后再新建接收机!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                this.Close();
                return;
            }
            if (newRceiverType_comboBox1.SelectedItem == null)
            {
                MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainWindow.IsReturnMidwayInNewProcess = true;
                return;
            }
            else
            {
                string newRceiverType = null;
                newRceiverType = (string)(newRceiverType_comboBox1.SelectedItem);
                switch(newRceiverType)
                {
                    case"点状<Points>":
                        {
                            if ( newReceiverName_textBox2.Text == "" || newReceiverlongitude_textBox1.Text == "" || newReceiverLatitude_textBox3.Text == "" || newReceiverReferencePlane_comboBox2.SelectedItem == null || newReceiverAntennaName_comboBox3.SelectedItem == null || newReceiverAntennaHeight_textBox1.Text == ""|| newReceiverAntennaRotationX_textBox4.Text == "" || newReceiverAntennaRotationY_textBox6.Text == "" || newReceiverAntennaRotationXZ_textBox5.Text == "")
                                {
                                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    MainWindow.IsReturnMidwayInNewProcess = true;
                                    return;
                                }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationX_textBox4.Text))
                            {
                                MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationY_textBox6.Text))
                            {
                                MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationXZ_textBox5.Text))
                            {
                                MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationX_textBox4))
                            {
                                MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationY_textBox6))
                            {
                                MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationXZ_textBox5))
                            {
                                MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                        }
                        break;
                    case"区域状<XYgrid>":
                        {
                            if ( newReceiverName_textBox2.Text == "" || newReceiverlongitude_textBox1.Text == "" || newReceiverLatitude_textBox3.Text == "" || newReceiverReferencePlane_comboBox2.SelectedItem == null || newReceiverAntennaName_comboBox3.SelectedItem == null || newReceiverAntennaHeight_textBox1.Text == "" || newReceiverSpace_textBox1.Text == "" || newReceiverAntennaRotationX_textBox4.Text == "" || newReceiverAntennaRotationY_textBox6.Text == "" || newReceiverAntennaRotationXZ_textBox5.Text == "")
                                {
                                    MessageBox.Show("窗口中有未设置的信息，请您设置完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    MainWindow.IsReturnMidwayInNewProcess = true;
                                    return;
                                }

                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaHeight_textBox1.Text))
                            {
                                MessageBox.Show("天线位置关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaHeight_textBox1))
                            {
                                MessageBox.Show("天线位置关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverSpace_textBox1.Text))
                            {
                                MessageBox.Show("接收机间隔值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (double.Parse(newReceiverSpace_textBox1.Text) < 0.000)
                            {
                                MessageBox.Show("接收机间隔值需大于0.000", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationX_textBox4.Text))
                            {
                                MessageBox.Show("天线关于X轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationY_textBox6.Text))
                            {
                                MessageBox.Show("天线关于Y轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (!BoudingLimition.IsScienceFigure(newReceiverAntennaRotationXZ_textBox5.Text))
                            {
                                MessageBox.Show("天线关于Z轴旋转角度值输入必须是实数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationX_textBox4))
                            {
                                MessageBox.Show("天线关于X轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationY_textBox6))
                            {
                                MessageBox.Show("天线关于Y轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                            if (BoudingLimition.RotationLimition(newReceiverAntennaRotationXZ_textBox5))
                            {
                                MessageBox.Show("天线关于Z轴旋转角度值需在0度至360度之间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                MainWindow.IsReturnMidwayInNewProcess = true;
                                return;
                            }
                        }
                        break;
                        default:
                    break;
               }
            }
            //先判断是否存在.rx文件
            if (File.Exists(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx"))
            {
                WaveformWriting receiverm = new WaveformWriting(FileOperation.ReadFile(MainWindow.nodeInfoFullPath));
                //注意 新建的天线个数不能超过1000
                string[] pointReceiverNames = new string[2000];
                string[] gridReceiverNames = new string[2000];
                //判断是否存在重名的接收机
                pointReceiverNames = receiverm.waveformNames(SetupContent.transmitterStr1OfTr);
                gridReceiverNames = receiverm.waveformNames(SetupContent.gridReceiverOfRxStr0);
                if (receiverm.judge(newReceiverName_textBox2.Text, pointReceiverNames) || receiverm.judge(newReceiverName_textBox2.Text, gridReceiverNames))
                {
                    MessageBox.Show("此接收机已存在， 请您换个接收机名称！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainWindow.IsReturnMidwayInNewProcess = true;
                    return;
                }
            }
            string receiverNum = NewTransmitterWindow.GetTransmitterNum(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx", SetupContent.gridReceiverOfRxStr1);

            string rxFileStr = null;
            string hasSpace = "";
            string startStr = null;
            if (((string)newRceiverType_comboBox1.SelectedItem).Equals("区域状<XYgrid>"))
             {
                  rxFileStr=Translate.KeyWordsDictionary(newRceiverType_comboBox1)+newReceiverName_textBox2.Text +"\r\n"
                                        +SetupContent.gridReceiverOfRxStr1+receiverNum+"\r\n"
                                        +SetupContent.gridReceiverOfRxStr2+"\r\n"
                                        +SetupContent.gridReceiverOfRxStr3+MainWindow.longitudeStr+"\r\n"
                                        +SetupContent.gridReceiverOfRxStr4+MainWindow.latitudeStr +"\r\n"
                                        +SetupContent.gridReceiverOfRxStr5+"\r\n"
                                        +Translate.KeyWordsDictionary(newReceiverReferencePlane_comboBox2)+"\r\n"
                                        +SetupContent.gridReceiverOfRxStr6+"\r\n"
                                        + SetupContent.gridReceiverOfRxStr7 + "\r\n"
                                        +SetupContent.gridReceiverOfRxStr8+newReceiverSpace_textBox1.Text +"\r\n"
                                        +SetupContent.gridReceiverOfRxStr9+"\r\n"
                                        +SetupContent.gridpointReceiverOfRxStr0+newReceiverAntennaHeight_textBox1.Text+"\r\n"
                                        + NewTransmitterWindow.GetAntennaStr(MainWindow.transInfoFullPath, newReceiverAntennaName_comboBox3)
                                        +SetupContent.gridpointReceiverOfRxStr1+newReceiverAntennaRotationX_textBox4.Text+"\r\n"
                                        +SetupContent.gridpointReceiverOfRxStr2+newReceiverAntennaRotationY_textBox6.Text+"\r\n"
                                        +SetupContent.gridpointReceiverOfRxStr3+newReceiverAntennaRotationXZ_textBox5.Text+"\r\n"
                                        +SetupContent.gridReceiverOfRxStr10+"\r\n";
                  MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[1].Nodes.Add(newReceiverName_textBox2.Text);
                  FileOperation.WriteLineFile(SetupContent.gridReceiverIndeStr  + " " + newReceiverName_textBox2.Text, MainWindow.nodeInfoFullPath, true);

                  hasSpace = newReceiverSpace_textBox1.Text ;
                  startStr = SetupContent.gridReceiverOfRxStr0 + " " + newReceiverName_textBox2.Text + "\r\n";
                  FileOperation.WriteLineFile(SetupContent.gridReceiverOfRxStr0 + " " + newReceiverName_textBox2.Text + "#" + newReceiverAntennaName_comboBox3.SelectedItem, MainWindow.relationOfAntAndWavePath, true);
            }
             else
             {
                  rxFileStr = Translate.KeyWordsDictionary(newRceiverType_comboBox1) + newReceiverName_textBox2.Text + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr1 + receiverNum + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr2 + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr3 + "\r\n"
                                     + SetupContent.gridReceiverOfRxStr3 + MainWindow.longitudeStr + "\r\n"
                                     + SetupContent.gridReceiverOfRxStr4 + MainWindow.latitudeStr + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr4 + "\r\n"
                                     + Translate.KeyWordsDictionary(newReceiverReferencePlane_comboBox2) + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr5 + "\r\n"
                                     + SetupContent.gridpointReceiverOfRxStr0 + newReceiverAntennaHeight_textBox1.Text + "\r\n"
                                     + NewTransmitterWindow.GetAntennaStr(MainWindow.transInfoFullPath, newReceiverAntennaName_comboBox3)
                                     + SetupContent.gridpointReceiverOfRxStr1 + newReceiverAntennaRotationX_textBox4.Text + "\r\n"
                                     + SetupContent.gridpointReceiverOfRxStr2 + newReceiverAntennaRotationY_textBox6.Text + "\r\n"
                                     + SetupContent.gridpointReceiverOfRxStr3 + newReceiverAntennaRotationXZ_textBox5.Text + "\r\n"
                                     + SetupContent.pointReceiverOfRxStr6 + "\r\n";
                  MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[4].Nodes[0].Nodes.Add(newReceiverName_textBox2.Text);
                  FileOperation.WriteLineFile(SetupContent.pointReceiverIndeStr + " " + newReceiverName_textBox2.Text, MainWindow.nodeInfoFullPath, true);
                 
                 FileOperation.WriteLineFile(SetupContent.transmitterStr1OfTr + " " + newReceiverName_textBox2.Text + "#" + newReceiverAntennaName_comboBox3.SelectedItem , MainWindow.relationOfAntAndWavePath, true);

                  startStr = SetupContent.pointReceiverOfRxStr0 + " " + newReceiverName_textBox2.Text + "\r\n";

             }

            string rxInfoStr = startStr 
                       + newRceiverType_comboBox1.Text  + "\r\n"
                       + newReceiverReferencePlane_comboBox2.Text + "\r\n"
                       + newReceiverAntennaName_comboBox3.Text + "\r\n"
                       + newReceiverAntennaHeight_textBox1.Text + "\r\n"
                       //+ newReceiverSpace_textBox1.Text + "\r\n"
                        + hasSpace +"\r\n"
                       + newReceiverAntennaRotationX_textBox4.Text + "\r\n"
                       + newReceiverAntennaRotationY_textBox6.Text + "\r\n"
                       + newReceiverAntennaRotationXZ_textBox5.Text + "\r\n"                      
                       + "END" + startStr;
            FileOperation.WriteFile(rxInfoStr, MainWindow.waveinfoFilePath , true);

            //FileOperation.WriteFile(SetupContent.receiverOfSetupStr0 + " " + newReceiverName_textBox2.Text + "#" + newReceiverAntennaName_comboBox3.SelectedItem+"\r\n" , MainWindow.relationOfAntAndWavePath, true);

             FileOperation.WriteFile(rxFileStr, MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx", true);
             //写到setup文件中
             string sourceStrOfRx = FileOperation.ReadFile(MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx");
             //找出天线的个数
             int receiverCount = WaveformWriting.GetCountOfMatchStr("begin_<grid>", sourceStrOfRx) + WaveformWriting.GetCountOfMatchStr("begin_<points>", sourceStrOfRx) - 1;
             //如果已经存在了receiverr块，则先删除
             if (MainWindow.setupStr.LastIndexOf("end_<receiver>") != -1)
             {
                 MainWindow.setupStr = MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<receiver>"), MainWindow.setupStr.IndexOf("end_<receiver>") - MainWindow.setupStr.IndexOf("begin_<receiver>") + 16);
             }
            
            int insertSiteOfRec = 0;
             if (MainWindow.setupStr.LastIndexOf("end_<transmitter>") != -1)
             {
                 insertSiteOfRec = MainWindow.setupStr.LastIndexOf("end_<transmitter>") + 2 + "end_<transmitter>".Length;
             }
             else
             {
                 insertSiteOfRec = MainWindow.setupStr.LastIndexOf("end_<feature>") + "end_<feature>".Length +2;
             }
             string insertStr = SetupContent.receiverOfSetupStr0 + "\r\n"
                                    + SetupContent.receiverOfSetupStr1 + MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx"+"\r\n"
                                    + SetupContent.receiverOfSetupStr2 + receiverCount.ToString()+"\r\n"
                                    +SetupContent.receiverOfSetupStr3 + "\r\n";
             MainWindow.setupStr = MainWindow.setupStr.Insert(insertSiteOfRec, insertStr);
             FileOperation.WriteFile(MainWindow.setupStr, MainWindow.mProjectFullName, false);
             if (MainWindow.creatSuccMesDisp)
             {
                 MessageBox.Show("\"" + newReceiverName_textBox2.Text + "\"接收机创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             //MessageBox.Show("对接收机" + newReceiverName_textBox2.Text + "操作成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newReceiverSave_button3_Click(object sender, EventArgs e)
        {

        }
    }
}
