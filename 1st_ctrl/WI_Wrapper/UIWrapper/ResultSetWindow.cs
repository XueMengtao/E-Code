using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
namespace WF1
{
    public partial class ResultSetWindow : Form
    {
        public ResultSetWindow()
        {
            InitializeComponent();
            if (MainWindow.PathsCheck != null)
            {
                //在新建工程的过程中，关闭结果设定窗口后又打开结果设定窗口时，将状态显示在结果设定窗口中
                resultsetpropagationpath_checkBox2.CheckState  = MainWindow.PathsCheck.CheckState ;
                resultsetpathloss_checkBox1.CheckState=MainWindow.PathlossCheck.CheckState ;
                resultsetelectricfieldmagnitudeandphase_checkBox3.CheckState=MainWindow.EFieldCheck.CheckState ;
                resultsettotalelectricfieldmagnitudeandphase_checkBox4.CheckState=MainWindow.EFieldTotalCheck.CheckState ;
                resultsetreceivedpower_checkBox5.CheckState= MainWindow.PowerCheck.CheckState  ;

            }
        }
        private void resultsetcancel_button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resultsetok_button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(MainWindow.mProjectFullName))
            {
                this.Close();
                return;
            }
            ResultSet(resultsetpropagationpath_checkBox2, "\nPaths");
            MainWindow.PathsCheck.CheckState  = resultsetpropagationpath_checkBox2.CheckState ;
            //如果没有写到信息文件则提醒用户有严重错误
            if(!RecordResultSetted(resultsetpropagationpath_checkBox2,"<Paths>"))
            {
                MessageBox.Show("应用程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            ResultSet(resultsetpathloss_checkBox1, "\nPathloss");
            MainWindow.PathlossCheck.CheckState = resultsetpathloss_checkBox1.CheckState;
            if (!RecordResultSetted(resultsetpathloss_checkBox1, "<Pathloss>"))
            {
                MessageBox.Show("应用程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
            ResultSet(resultsetelectricfieldmagnitudeandphase_checkBox3, "\nEField");
            MainWindow.EFieldCheck.CheckState = resultsetelectricfieldmagnitudeandphase_checkBox3.CheckState;
            if (!RecordResultSetted(resultsetelectricfieldmagnitudeandphase_checkBox3, "<EField>"))
            {
                MessageBox.Show("应用程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResultSet(resultsettotalelectricfieldmagnitudeandphase_checkBox4, "\nEFieldTotal");
            MainWindow.EFieldTotalCheck.CheckState = resultsettotalelectricfieldmagnitudeandphase_checkBox4.CheckState;
            if (!RecordResultSetted(resultsettotalelectricfieldmagnitudeandphase_checkBox4, "<EFieldTotal>"))
            {
                MessageBox.Show("应用程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ResultSet(resultsetreceivedpower_checkBox5, "\nPower");
            MainWindow.PowerCheck.CheckState = resultsetreceivedpower_checkBox5.CheckState;
            if (!RecordResultSetted(resultsetreceivedpower_checkBox5, "<Power>"))
            {
                MessageBox.Show("应用程序内部发生错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //当打开一个工程时，若用户单击结果设定，则应先删除结点“仿真结果设定“，再添加结点，以避免重复加载
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Remove (MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5]);
            MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes.Add("仿真结果设定");

            //如果CheckBox选中，则添加到工程树中
            if (resultsetpropagationpath_checkBox2.Checked)
            {
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(resultsetpropagationpath_checkBox2.Text);
            }

            if (resultsetpathloss_checkBox1.Checked)
            {
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(resultsetpathloss_checkBox1.Text);
            }
            if (resultsetelectricfieldmagnitudeandphase_checkBox3.Checked)
            {
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(resultsetelectricfieldmagnitudeandphase_checkBox3.Text);
            }
            if (resultsettotalelectricfieldmagnitudeandphase_checkBox4.Checked)
            {
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(resultsettotalelectricfieldmagnitudeandphase_checkBox4.Text);
            }
            if (resultsetreceivedpower_checkBox5.Checked)
            {
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[5].Nodes.Add(resultsetreceivedpower_checkBox5.Text);
            }
            this.Close();
        }
        //根据索引字符串找出其后的yes或no,再根据CheckBox的状态去修改yes或no
        void ResultSet(CheckBox ch, string indexStr)
        {
            int site = MainWindow.setupStr.IndexOf(indexStr) + indexStr.Length+1;
            //找出yes或no
            string initStr = StringFound.FoundBackStr(indexStr, MainWindow.setupStr,true);
            //当初始是no
            if (initStr.Equals("no"))
            {
                //如果CheckBox被选中
                if (ch.Checked)
                {
                     MainWindow.setupStr = MainWindow.setupStr.Remove(site, 2);
                     MainWindow.setupStr = MainWindow.setupStr.Insert(site,"yes");
                }
            }
                //当初始是yes
            else
            {
                //CheckBox没被选中
                if (!ch.Checked)
                {
                    MainWindow.setupStr = MainWindow.setupStr.Remove(site, 3);
                    MainWindow.setupStr = MainWindow.setupStr.Insert(site, "no");
                }
            }
        }
        //将结果设定的状态写到信息文件中，返回值为true表示正确写进信息文件，false表示没有正确写进文件
        //第二个参数是写进信息文件的字符串
        bool RecordResultSetted(CheckBox cb,string str)
        {
            try
            {
                if (cb.Checked)
                {
                    //若复选框被选中则在索引字符串str后面写1
                    FileOperation.WriteLineFile(str+" 1", MainWindow.nodeInfoFullPath, true);
                }
                else
                {
                   //若复选框没有被选中则在索引字符串后面写0
                    FileOperation.WriteLineFile(str + " 0", MainWindow.nodeInfoFullPath, true);
                }
            }
            catch (Exception ex)
            {
                //没有写进文件则会产生异常，返回false
                LogFileManager.ObjLog.fatal(ex.Message, ex);
                return false;
            }
            return true;
         }
    }
}
