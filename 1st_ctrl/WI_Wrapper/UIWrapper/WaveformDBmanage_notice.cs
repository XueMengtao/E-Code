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
    public partial class WaveformDBmanage_notice : Form
    {
        public WaveformDBmanage_notice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Waveform_update();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (this.Owner as DBmanageWindow).Waveform_Recover();
            this.Close();
        }
    }
}
