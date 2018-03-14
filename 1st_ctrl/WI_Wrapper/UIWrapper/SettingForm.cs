using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.ServiceModel.Configuration;
using System.Text.RegularExpressions;

namespace WF1
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingBrowerButton_Click(object sender, EventArgs e)
        {
            setting_folderBrowserDialog.ShowNewFolderButton = false;
            setting_folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            setting_folderBrowserDialog.Description = "请选择Wireless InSite的安装路径(Insite.exe所在的文件夹)";
            if (setting_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                MainWindow.exePath = setting_folderBrowserDialog.SelectedPath+"\\Insite.exe";
                if (!File.Exists(MainWindow.exePath))
                {
                    MessageBox.Show("您所选择的Wireless Insite软件的安装路径不对，请重新选择！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings["WIPath"].Value = MainWindow.exePath;
                setting_textBox.Text = MainWindow.exePath;
                cfa.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");  
            }
        }

        internal void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            setting_textBox.Text = ConfigurationManager.AppSettings["WIPath"];
        }  
    }
}
