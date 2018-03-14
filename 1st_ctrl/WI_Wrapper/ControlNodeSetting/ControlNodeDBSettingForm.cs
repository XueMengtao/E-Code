using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;


namespace ControlNodeSetting
{
    public partial class ControlNodeDBSettingForm : Form
    {
        public ControlNodeDBSettingForm()
        {
            InitializeComponent();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
           // ChangeWCF_URL(ControlNode_ipAddressBox.Text);
            //string connectStrContent1 = "Data Source=192.111.0.128;Initial Catalog=ParallelTask;User ID=sa;Password=sa";
            if (DB_ipAddressBox.Text == "")
            {
                this.Close();
                return;
            }
            if (DB_textBox.Text.Trim().Equals(""))
                DB_textBox.Text = "ParallelTask";
            if (DBAccount_textBox.Text.Trim().Equals(""))
                DBAccount_textBox.Text = "sa";
            if (DBPassWord_textBox.Text.Trim().Equals(""))
                DBPassWord_textBox.Text = "sa";
            string connectStrContent = "Data Source=" + DB_ipAddressBox.Text + ";"
                + "Initial Catalog=" + DB_textBox.Text + ";"
                + "User ID=" + DBAccount_textBox.Text + ";"
                + "Password=" + DBPassWord_textBox.Text;
            UpdateConnectionStringsConfig(connectStrContent);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region
        /// <summary>
        /// 更改配置文件
        /// </summary>
        /// <param name="serverIp"></param>
        private void ChangeWCF_URL(string serverIp)
        {
            string wcfURL = "http://" + serverIp + ":8732/Design_Time_Addresses/WcfService/Service1/";
            //XDocument doc = XDocument.Load(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
            //doc.Element("configuration").Element("system.serviceModel").Element("services").Element("service").Element("host").Element("baseAddresses").Element("add").SetAttributeValue("baseAddress", wcfURL);
            //doc.Save(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
            foreach (XmlElement item in doc.SelectNodes("/configuration/system.serviceModel/services/service/host/baseAddresses/add"))
            {
                item.SetAttribute("baseAddress", wcfURL);
            }
            doc.Save(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
        }
        ///<summary>
        ///更新连接字符串
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        ///<param name="newProviderName">数据提供程序名称</param>
        private  void UpdateConnectionStringsConfig(string newConStr)
        {
            XDocument doc = XDocument.Load(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
            XElement connectStrEle = doc.Element("configuration").Element("connectionStrings").Element("add");
            connectStrEle.Attribute("connectionString").Value = newConStr;
            doc.Save(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
        }

        #endregion

        private void ControlNodeDBSettingForm_Load(object sender, EventArgs e)
        {
            try
            {
                XDocument doc = XDocument.Load(Application.StartupPath + "\\WindowsServiceHostServer.exe.config");
                XElement connectStrEle = doc.Element("configuration").Element("connectionStrings").Element("add");
                string conStr = connectStrEle.Attribute("connectionString").Value;

                string[] str = new string[3];
                int begin = 0, end;
                for (int i = 0; i < 3; i++)
                {
                    begin = conStr.IndexOf('=');
                    end = conStr.IndexOf(';');
                    str[i] = conStr.Substring(begin + 1, end - begin - 1);
                    conStr = conStr.Substring(end + 1); ;
                }
                DB_ipAddressBox.Text = str[0];
                DB_textBox.Text = str[1];
                DBAccount_textBox.Text = str[2];
                DBPassWord_textBox.Text = conStr.Substring(begin + 2);
            }
            catch (Exception )
            {
                MessageBox.Show("配置文件有错！");
            }
            //conStr = conStr.Substring(end + 1);
            //begin = conStr.IndexOf('=');
            //end = conStr.IndexOf(';');
            //DB_textBox.Text = conStr.Substring(begin + 1, end - begin - 1);
        }
    }
}
