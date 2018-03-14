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

namespace NodeApplication
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void setting_OkBtn_Click(object sender, EventArgs e)
        {
            ChangeWCF_URL(ControlNode_IpAddressBox.Text);
            this.Close();
        }
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
                    LogFileManager.ObjLog.info(address);
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

        private void SettingForm_Load(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConfigurationSectionGroup sct = config.SectionGroups["system.serviceModel"];
            ServiceModelSectionGroup serviceModelSectionGroup = sct as ServiceModelSectionGroup;
            ClientSection clientSection = serviceModelSectionGroup.Client;
            string address="";
            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                //string[] str = item.Address.ToString().Split('/');
                //string pattern = "";
                //for (int i = 0; i < str.Length - 2; i++)
                //    pattern += str[i] + '/';
                 address = item.Address.ToString();
            }
            int begin = address.IndexOf(':');
            address = address.Substring(begin + 3);
            ControlNode_IpAddressBox.Text=address.Substring(0, address.IndexOf(':'));
        }
    }
}
