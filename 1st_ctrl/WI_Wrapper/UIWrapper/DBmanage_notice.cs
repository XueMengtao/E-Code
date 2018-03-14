using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WF1
{
    public partial class DBmanage_notice : Form
    {
        public DBmanage_notice()
        {
            InitializeComponent();
        }

        private void DBmanage_notice_button1_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Transmitter_update();
            this.Close();
        }

        private void DBmanage_notice_button2_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Transmitter_Recover();
            this.Close();
        }
    }
}
