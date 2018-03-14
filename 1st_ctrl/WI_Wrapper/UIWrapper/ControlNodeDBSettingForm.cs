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

namespace WF1
{
    public partial class ControlNodeDBSettingForm : Form
    {
        public event ChildClose closeFather;
        public ControlNodeDBSettingForm()
        {
            InitializeComponent();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeWCF_URL(ControlNode_ipAddressBox.Text);
                /*string connectionStrName = "WF1.Properties.Settings.ParallelTaskConnectionString";
                string connectStrContent = "Data Source=" + DB_ipAddressBox.Text + ";"
                    + "Initial Catalog=" + DB_textBox.Text + ";"
                    + "User ID=" + DBAccount_textBox.Text + ";"
                    + "Password=" + DBPassWord_textBox.Text;
                UpdateConnectionStringsConfig(connectionStrName, connectStrContent, "System.Data.SqlClient");*/
                MessageBox.Show("配置在关闭应用程序后才生效。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("IP格式不正确，请重新输入");
            }
        
            this.Close();
            closeFather();
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
        public void ChangeWCF_URL(string serverIp)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConfigurationSectionGroup sct = config.SectionGroups["system.serviceModel"];
            ServiceModelSectionGroup serviceModelSectionGroup = sct as ServiceModelSectionGroup;
            ClientSection clientSection = serviceModelSectionGroup.Client;

            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                //string[] str = item.Address.ToString().Split('/');
                //string pattern = "";
                //for (int i = 0; i < str.Length - 2; i++)
                //    pattern += str[i] + '/';
                string address = item.Address.ToString();
                //string replacement = string.Format("{0}", serverIp);
                //address = Regex.Replace(address, pattern, replacement);
                address = "http://" + serverIp + ":8732/Design_Time_Addresses/WcfService/Service1/";
                try
                {
                    item.Address = new Uri(address);
                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.debug(ex.Message, ex);
                    return;
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("system.serviceModel"); 
        }
        public void ChangeWCF_IP(string serverIp)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConfigurationSectionGroup sct = config.SectionGroups["system.serviceModel"];
            ServiceModelSectionGroup serviceModelSectionGroup = sct as ServiceModelSectionGroup;
            ClientSection clientSection = serviceModelSectionGroup.Client;

            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                string[] str = item.Address.ToString().Split('/');
                string pattern = "";
                for (int i = 0; i < str.Length - 2; i++)
                    pattern += str[i] + '/';
                string address = item.Address.ToString();
                string replacement = string.Format("{0}", serverIp);
                address = Regex.Replace(address, pattern, replacement);
                item.Address = new Uri(address);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("system.serviceModel");
        }
        ///<summary>
        ///更新连接字符串
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        ///<param name="newProviderName">数据提供程序名称</param>
        private  void UpdateConnectionStringsConfig(string newName,
            string newConString,
            string newProviderName)
        {
            bool isModified = false;    //记录该连接串是否已经存在
            //如果要更改的连接串已经存在
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            //新建一个连接字符串实例
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            // 打开可执行的配置文件*.exe.config
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        #endregion

        private void ControlNodeDBSettingForm_Load(object sender, EventArgs e)
        {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConfigurationSectionGroup sct = config.SectionGroups["system.serviceModel"];
            ServiceModelSectionGroup serviceModelSectionGroup = sct as ServiceModelSectionGroup;
            ClientSection clientSection = serviceModelSectionGroup.Client;
            string address = "";
            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                //string[] str = item.Address.ToString().Split('/');
                //string pattern = "";
                //for (int i = 0; i < str.Length - 2; i++)
                //    pattern += str[i] + '/';
                 address = item.Address.ToString();
                //string replacement = string.Format("{0}", serverIp);
                //address = Regex.Replace(address, pattern, replacement);
            }
            address = address.Substring(address.IndexOf(':')+3);
            ControlNode_ipAddressBox.Text = address.Substring(0,address.IndexOf(':'));
        }
    }
}
