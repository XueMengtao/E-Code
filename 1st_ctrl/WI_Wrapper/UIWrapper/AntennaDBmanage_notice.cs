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
    public partial class AntennaDBmanage_notice : Form
    {
        public AntennaDBmanage_notice()
        {
            InitializeComponent();
        }

        private void AntennaDBmanage_notice_button1_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Antenna_update();
            this.Close();
        }

        private void AntennaDBmanage_notice_button2_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Antenna_Recover();
            this.Close();
        }
    }
}
