using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace WF1
{
    public partial class DBmanageWindow : Form
    {
  
        ServiceReference1.Service1Client client = null;
        ServiceReference1.TerInfo ExistedTerrain = null;
        ServiceReference1.Antenna ExistedAntenna = null;
        ServiceReference1.Transmitter ExistedTransmitter = null;
        ServiceReference1.WaveForm ExistedWaveform = null;
        ServiceReference1.ProjectInfo[] ExistedPro = null;
        List<string> proname = new List<string>();
        List<string> prodir = new List<string>();
        string Cellvalue = null;
        public DBmanageWindow()
        {
            
            client = new ServiceReference1.Service1Client();
            InitializeComponent();
            int count = this.Controls.Count * 2 + 2;
            float[] factor = new float[count];
            int i = 0;
            factor[i++] = Size.Width;
            factor[i++] = Size.Height;
            foreach (Control ctrl in this.Controls)
            {
                factor[i++] = ctrl.Location.X / (float)Size.Width;
                factor[i++] = ctrl.Location.Y / (float)Size.Height;
                ctrl.Tag = ctrl.Size;
            }
            Tag = factor;
        }

        private void dbmanagewaveformcancel_button3_Click(object sender, EventArgs e)
        {
            client.Close();
            this.Close();
        }

        private void dbmanageantennacancel_button4_Click(object sender, EventArgs e)
        {
            client.Close();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            client.Close();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Close();
            this.Close();
        }

       

        private void DBmanageWindow_Load(object sender, EventArgs e)
        {
            Terrainmanager_dataGridView1.ReadOnly = true;
            pro_dataGridView1.ReadOnly = true;
            bwidth = this.Width;
            bheight = this.Height;
        }

        private void Terrainmanager_button3_Click(object sender, EventArgs e)
        {
            if (Terrainmanager_dataGridView1.Rows.Count != 1 && Terrainmanager_dataGridView1.SelectedRows.Count!=0)
            {
                string tername = Terrainmanager_dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int index = Terrainmanager_dataGridView1.SelectedRows[0].Index;

                try
                {
                    client.iDelTerInfo(tername);
                    Terrainmanager_dataGridView1.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                catch (System.TimeoutException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (FaultException<ServiceReference1.WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Detail.message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (CommunicationException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
            }
            else
            {
                MessageBox.Show("请先添加数据或选中数据");
            }
        }

        private void Terrainmanager_comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                string[] a = new string[Terrainmanager_dataGridView1.Rows.Count-1];
                for (int i = 0; i < Terrainmanager_dataGridView1.Rows.Count-1; i++)
                {
                    a[i] = Terrainmanager_dataGridView1.Rows[i].Cells[0].Value.ToString();
                }
                //判断选中的值是否已经存在视图中
                for (int i = 0; i < Terrainmanager_dataGridView1.Rows.Count-1; i++)
                {
                    if (a[i] == (string)Terrainmanager_comboBox2.SelectedItem)
                    {
                        Terrainmanager_dataGridView1.Rows[i].Selected = true;
                        for (int j = 0; j < Terrainmanager_dataGridView1.Rows.Count - 1; j++)
                        {
                            if (j != i) Terrainmanager_dataGridView1.Rows[j].Selected = false;
                        }
                        break;
                    }
                    else
                    {
                        count++;
                        continue;
                    }         
                }
                //如果不存在相同的则增加表中项
                if (count == Terrainmanager_dataGridView1.Rows.Count-1)
                {
                    ExistedTerrain = client.iGetTer((string)Terrainmanager_comboBox2.SelectedItem);
                    //string[] row = { ExistedTerrain.name, ExistedTerrain.originX.ToString(), ExistedTerrain.originY.ToString(), ExistedTerrain.path };
                    Terrainmanager_dataGridView1.Rows.Add(ExistedTerrain.name, ExistedTerrain.originX.ToString(), ExistedTerrain.originY.ToString(), ExistedTerrain.path);
                    Terrainmanager_dataGridView1.Rows[Terrainmanager_dataGridView1.Rows.Count-2].Selected = true;
                    for (int i = 0; i < Terrainmanager_dataGridView1.Rows.Count - 2; i++)
                    {
                        Terrainmanager_dataGridView1.Rows[i].Selected = false;
                    }
                }
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();
            }
        }

        private void Terrainmanager_dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            for (int i = 0; i < Terrainmanager_dataGridView1.Rows.Count - 1; i++)
            {
                if (i != Terrainmanager_dataGridView1.CurrentCell.RowIndex)
                    Terrainmanager_dataGridView1.Rows[i].Selected = false;
                else Terrainmanager_dataGridView1.Rows[i].Selected = true;
            }
        }


        private void Terrainmanager_comboBox2_DropDown(object sender, EventArgs e)
        {
            string[] TerNames = new string[] { };
            TerNames = client.iGetTerNames();
            this.Terrainmanager_comboBox2.Items.Clear();
            foreach (string s in TerNames)
            {
                this.Terrainmanager_comboBox2.Items.Add(s);
            }  
        }

        private void Transmiter_comboBox6_DropDown(object sender, EventArgs e)
        {
            string[] TransmitterNames = new string[] { };
            TransmitterNames = client.iGetAllTransmitter();
            Transmiter_comboBox6.Items.Clear();
            foreach (string s in TransmitterNames)
            {
                this.Transmiter_comboBox6.Items.Add(s);
            }  
        }

        private void Transmiter_comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                string[] a = new string[Transmiter_dataGridView3.Rows.Count - 1];
                for (int i = 0; i < Transmiter_dataGridView3.Rows.Count - 1; i++)
                {
                    a[i] = Transmiter_dataGridView3.Rows[i].Cells[0].Value.ToString();
                }
                //判断选中的值是否已经存在视图中
                for (int i = 0; i < Transmiter_dataGridView3.Rows.Count - 1; i++)
                {
                    if (a[i] == (string)Transmiter_comboBox6.SelectedItem)
                    {
                        Transmiter_dataGridView3.Rows[i].Selected = true;
                        for (int j = 0; j < Transmiter_dataGridView3.Rows.Count - 1; j++)
                        {
                            if (j != i) Transmiter_dataGridView3.Rows[j].Selected = false;
                        }
                        break;
                    }
                    else
                    {
                        count++;
                        continue;
                    }         
                }
                //如果不存在相同的则增加表中项
                if (count == Transmiter_dataGridView3.Rows.Count - 1)
                {
                    ExistedTransmitter = client.iGetTransmitter((string)Transmiter_comboBox6.SelectedItem);
                    //string[] row = { ExistedTerrain.name, ExistedTerrain.originX.ToString(), ExistedTerrain.originY.ToString(), ExistedTerrain.path };
                    //ExistedAntenna=client.iGetAntenna(ExistedTransmitter.AntennaName);
                    Transmiter_dataGridView3.Rows.Add(ExistedTransmitter.Name, ExistedTransmitter.AntennaName,ExistedTransmitter.RotateX.ToString(), ExistedTransmitter.RotateY.ToString(), ExistedTransmitter.RotateZ.ToString(),ExistedTransmitter.Power.ToString());
                    Transmiter_dataGridView3.Rows[Transmiter_dataGridView3.Rows.Count - 2].Selected = true;
                    for (int i = 0; i < Transmiter_dataGridView3.Rows.Count - 2; i++)
                    {
                        Transmiter_dataGridView3.Rows[i].Selected = false;
                    }
                }
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();
            }
        }

        private void Transmiter_button9_Click(object sender, EventArgs e)
        {
            if (Transmiter_dataGridView3.Rows.Count != 1 && Transmiter_dataGridView3.SelectedRows.Count!=0)
            {
                string Transmittername = Transmiter_dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                int index = Transmiter_dataGridView3.SelectedRows[0].Index;

                try
                {
                    client.iDelTransmitter(Transmittername);
                    Transmiter_dataGridView3.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                catch (System.TimeoutException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (FaultException<ServiceReference1.WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Detail.message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (CommunicationException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
            }
            else
            {
                MessageBox.Show("请先添加数据或选中数据");
            }
        }

        private void Transmiter_dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < Transmiter_dataGridView3.Rows.Count - 1; i++)
            {
                if (i != Transmiter_dataGridView3.CurrentCell.RowIndex)
                    Transmiter_dataGridView3.Rows[i].Selected = false;
                else Transmiter_dataGridView3.Rows[i].Selected = true;
            }
        }

        //private void Transmiter_button8_Click(object sender, EventArgs e)
        //{
        //    ServiceReference1.Transmitter tr = new ServiceReference1.Transmitter();
        //    int index = Transmiter_dataGridView3.SelectedRows[0].Index;
        //    tr.Name = Transmiter_dataGridView3.Rows[index].Cells[0].Value.ToString();
        //    tr.AntennaName = Transmiter_dataGridView3.Rows[index].Cells[1].Value.ToString();
        //    tr.RotateX = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[2].Value);
        //    tr.RotateY = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[3].Value);
        //    tr.RotateZ = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[4].Value);
        //    tr.Power = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[5].Value);
        //    try
        //    {
        //        client.iUpdateTransmitter(tr);
        //        MessageBox.Show("更新成功");
        //    }
        //    catch (System.TimeoutException ex)
        //    {
        //        LogFileManager.ObjLog.error(ex.Message, ex);
        //        MessageBox.Show("更新失败");
        //        client.Abort();

        //    }
        //    catch (FaultException<ServiceReference1.WcfException> ex)
        //    {
        //        LogFileManager.ObjLog.error(ex.Detail.message, ex);
        //        MessageBox.Show("更新失败");
        //        client.Abort();

        //    }
        //    catch (CommunicationException ex)
        //    {
        //        LogFileManager.ObjLog.error(ex.Message, ex);
        //        MessageBox.Show("更新失败");
        //        client.Abort();

        //    }
        //    catch (Exception ex)
        //    {
        //        LogFileManager.ObjLog.error(ex.Message, ex);
        //        MessageBox.Show("更新失败");
        //        client.Abort();
        //    }
        //}

        private void Transmiter_dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((Transmiter_dataGridView3.CurrentCell.Value != null && Cellvalue != Transmiter_dataGridView3.CurrentCell.Value.ToString())||(Transmiter_dataGridView3.CurrentCell.Value==null&&Cellvalue!=null))
            {            
                    Transmiter_dataGridView3.Rows[Transmiter_dataGridView3.CurrentCell.RowIndex].Selected = true;
                    string name = Transmiter_dataGridView3.Rows[Transmiter_dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    string item = Transmiter_dataGridView3.Columns[Transmiter_dataGridView3.CurrentCell.ColumnIndex].HeaderText;
                    DBmanage_notice ds = new DBmanage_notice();
                    ds.label1.Text = name + "的" + item + "已经更改,是否同步到数据库？";
                    ds.ShowDialog(this);   
            }
        }
        public void Transmitter_update()
        {
            ServiceReference1.Transmitter tr = new ServiceReference1.Transmitter();
            int index = Transmiter_dataGridView3.SelectedRows[0].Index;
            if (Transmiter_dataGridView3.Rows[index].Cells[0].Value != "" && Transmiter_dataGridView3.Rows[index].Cells[0].Value != null)
                tr.Name = Transmiter_dataGridView3.Rows[index].Cells[0].Value.ToString();
            else tr.Name = null;
            if (Transmiter_dataGridView3.Rows[index].Cells[1].Value != "" && Transmiter_dataGridView3.Rows[index].Cells[1].Value != null)
                tr.AntennaName = Transmiter_dataGridView3.Rows[index].Cells[1].Value.ToString();
            else tr.AntennaName = null;
            tr.RotateX = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[2].Value);
            tr.RotateY = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[3].Value);
            tr.RotateZ = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[4].Value);
            tr.Power = Convert.ToDouble(Transmiter_dataGridView3.Rows[index].Cells[5].Value);
            try
            {
                client.iUpdateTransmitter(tr);
                MessageBox.Show("更新成功");
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();
            }
        }
        public void Transmitter_Recover()
        {
            ServiceReference1.Transmitter tr = new ServiceReference1.Transmitter();
            int index=Transmiter_dataGridView3.SelectedRows[0].Index;
            string name=Transmiter_dataGridView3.Rows[index].Cells[0].Value.ToString();
            tr = client.iGetTransmitter(name);
            Transmiter_dataGridView3.Rows[index].Cells[1].Value = tr.AntennaName;
            Transmiter_dataGridView3.Rows[index].Cells[2].Value = tr.RotateX;
            Transmiter_dataGridView3.Rows[index].Cells[3].Value = tr.RotateY;
            Transmiter_dataGridView3.Rows[index].Cells[4].Value = tr.RotateZ;
            Transmiter_dataGridView3.Rows[index].Cells[5].Value = tr.Power;
        }

        private void dbmanageAntennaname_comboBox3_DropDown(object sender, EventArgs e)
        {
            if (dbmanangeAntennatype_comboBox4.SelectedItem != "")
            {
                string antennatype = Translate.KeyWordsDictionary(dbmanangeAntennatype_comboBox4);
                string[] AntennaNames = new string[] { };
                AntennaNames = client.iGetAllAntenna(antennatype);
                dbmanageAntennaname_comboBox3.Items.Clear();
                foreach (string s in AntennaNames)
                {
                    this.dbmanageAntennaname_comboBox3.Items.Add(s);
                }
            }
            else
            {
                string[] AntennaNames = new string[] { };
                AntennaNames = client.iGetAllAntennaNames();
                dbmanageAntennaname_comboBox3.Items.Clear();
                foreach (string s in AntennaNames)
                {
                    this.dbmanageAntennaname_comboBox3.Items.Add(s);
                }
            }
        }

        private void dbmanageAntennaname_comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                string[] a = new string[Antenna_dataGridView2.Rows.Count - 1];
                for (int i = 0; i < Antenna_dataGridView2.Rows.Count - 1; i++)
                {
                    a[i] = Antenna_dataGridView2.Rows[i].Cells[0].Value.ToString();
                }
                //判断选中的值是否已经存在视图中
                for (int i = 0; i < Antenna_dataGridView2.Rows.Count - 1; i++)
                {
                    if (a[i] == (string)dbmanageAntennaname_comboBox3.SelectedItem)
                    {
                        Antenna_dataGridView2.Rows[i].Selected = true;
                        for (int j = 0; j < Antenna_dataGridView2.Rows.Count - 1; j++)
                        {
                            if (j != i) Antenna_dataGridView2.Rows[j].Selected = false;
                        }
                        break;
                    }
                    else
                    {
                        count++;
                        continue;
                    }
                }
                //如果不存在相同的则增加表中项
                if (count == Antenna_dataGridView2.Rows.Count - 1)
                {
                    //string[] row = { ExistedTerrain.name, ExistedTerrain.originX.ToString(), ExistedTerrain.originY.ToString(), ExistedTerrain.path };
                    ExistedAntenna = client.iGetAntenna((string)dbmanageAntennaname_comboBox3.SelectedItem);
                    Antenna_dataGridView2.Rows.Add(ExistedAntenna.Name, ExistedAntenna.Type, ExistedAntenna.MaxGain.ToString(), ExistedAntenna.Polarization, ExistedAntenna.RecieveThrehold.ToString(), ExistedAntenna.TransmissionLoss.ToString(), ExistedAntenna.VSWR.ToString(), ExistedAntenna.Temperature.ToString(), ExistedAntenna.Radius.ToString(), ExistedAntenna.BlockageRadius.ToString(), ExistedAntenna.ApertureDistribution, ExistedAntenna.EdgeTeper.ToString(), ExistedAntenna.Length.ToString(), ExistedAntenna.Pitch.ToString());
                    Antenna_dataGridView2.Rows[Antenna_dataGridView2.Rows.Count - 2].Selected = true;
                    for (int i = 0; i < Antenna_dataGridView2.Rows.Count - 2; i++)
                    {
                        Antenna_dataGridView2.Rows[i].Selected = false;
                    }
                }
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();
            }
        }

        private void dbmanageAntennadelete_button6_Click(object sender, EventArgs e)
        {
            if (Antenna_dataGridView2.Rows.Count != 1 && Antenna_dataGridView2.SelectedRows.Count!=0)
            {
                string name = Antenna_dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                int index = Antenna_dataGridView2.SelectedRows[0].Index;

                try
                {
                    client.iDelAntenna(name);
                    Antenna_dataGridView2.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                catch (System.TimeoutException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (FaultException<ServiceReference1.WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Detail.message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (CommunicationException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
            }
            else
            {
                MessageBox.Show("请先添加数据或选中数据");
            }
        }

        private void Antenna_dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < Antenna_dataGridView2.Rows.Count - 1; i++)
            {
                if (i != Antenna_dataGridView2.CurrentCell.RowIndex)
                    Antenna_dataGridView2.Rows[i].Selected = false;
                else Antenna_dataGridView2.Rows[i].Selected = true;
            }
        }

        private void Antenna_dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((Antenna_dataGridView2.CurrentCell.Value != null && Cellvalue != Antenna_dataGridView2.CurrentCell.Value.ToString()) || (Antenna_dataGridView2.CurrentCell.Value == null && Cellvalue != null))
            {
                Antenna_dataGridView2.Rows[Antenna_dataGridView2.CurrentCell.RowIndex].Selected = true;
                string name = Antenna_dataGridView2.Rows[Antenna_dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string item = Antenna_dataGridView2.Columns[Antenna_dataGridView2.CurrentCell.ColumnIndex].HeaderText;
                AntennaDBmanage_notice ds = new AntennaDBmanage_notice();
                ds.label1.Text = name + "的" + item + "已经更改,是否同步到数据库？";
                ds.ShowDialog(this);
            }
        }
        public void Antenna_update()
        {
            ServiceReference1.Antenna tr = new ServiceReference1.Antenna();
            int index = Antenna_dataGridView2.SelectedRows[0].Index;
            if (Antenna_dataGridView2.Rows[index].Cells[0].Value != null&&Antenna_dataGridView2.Rows[index].Cells[0].Value != "")
                tr.Name = Antenna_dataGridView2.Rows[index].Cells[0].Value.ToString();
            else tr.Name = null;
            if (Antenna_dataGridView2.Rows[index].Cells[1].Value != null && Antenna_dataGridView2.Rows[index].Cells[1].Value != "")
                tr.Type = Antenna_dataGridView2.Rows[index].Cells[1].Value.ToString();
            else tr.Type = null;
            tr.MaxGain = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[2].Value);
            if (Antenna_dataGridView2.Rows[index].Cells[3].Value != null && Antenna_dataGridView2.Rows[index].Cells[3].Value != "")
                tr.Polarization = Antenna_dataGridView2.Rows[index].Cells[3].Value.ToString();
            else tr.Polarization = null;
            tr.RecieveThrehold = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[4].Value);
            tr.TransmissionLoss = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[5].Value);
            tr.VSWR = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[6].Value);
            tr.Temperature = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[7].Value);
            if (Antenna_dataGridView2.Rows[index].Cells[10].Value != null && Antenna_dataGridView2.Rows[index].Cells[10].Value != "")
                tr.ApertureDistribution = Antenna_dataGridView2.Rows[index].Cells[10].Value.ToString();
            else tr.ApertureDistribution = null;
            if (Antenna_dataGridView2.Rows[index].Cells[8].Value != "" &&Antenna_dataGridView2.Rows[index].Cells[8].Value != null)
                tr.Radius = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[8].Value);
            else tr.Radius = null;
            if (Antenna_dataGridView2.Rows[index].Cells[9].Value != "" && Antenna_dataGridView2.Rows[index].Cells[9].Value != null)
                tr.BlockageRadius = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[9].Value);
            else tr.BlockageRadius = null;
            if (Antenna_dataGridView2.Rows[index].Cells[11].Value != "" && Antenna_dataGridView2.Rows[index].Cells[11].Value != null)
                tr.EdgeTeper = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[11].Value);
            else tr.EdgeTeper = null;
            if (Antenna_dataGridView2.Rows[index].Cells[12].Value != "" && Antenna_dataGridView2.Rows[index].Cells[12].Value != null)
                tr.Length = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[12].Value);
            else tr.Length = null;
            if (Antenna_dataGridView2.Rows[index].Cells[13].Value != "" && Antenna_dataGridView2.Rows[index].Cells[13].Value != null)
                tr.Pitch = Convert.ToDouble(Antenna_dataGridView2.Rows[index].Cells[13].Value);
            else tr.Pitch = null;
            try
            {
                client.iUpdateAntenna(tr);
                MessageBox.Show("更新成功");
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();
            }
        }
        public void Antenna_Recover()
        {
            ServiceReference1.Antenna tr = new ServiceReference1.Antenna();
            int index = Antenna_dataGridView2.SelectedRows[0].Index;
            string name = Antenna_dataGridView2.Rows[index].Cells[0].Value.ToString();
            tr = client.iGetAntenna(name);
            Antenna_dataGridView2.Rows[index].Cells[1].Value =tr.Type;
            Antenna_dataGridView2.Rows[index].Cells[2].Value = tr.MaxGain;
            Antenna_dataGridView2.Rows[index].Cells[3].Value =tr.Polarization;
            Antenna_dataGridView2.Rows[index].Cells[4].Value = tr.RecieveThrehold;
            Antenna_dataGridView2.Rows[index].Cells[5].Value = tr.TransmissionLoss ;
            Antenna_dataGridView2.Rows[index].Cells[6].Value= tr.VSWR ;
            Antenna_dataGridView2.Rows[index].Cells[7].Value = tr.Temperature;
            Antenna_dataGridView2.Rows[index].Cells[8].Value=tr.Radius;
            Antenna_dataGridView2.Rows[index].Cells[9].Value=tr.BlockageRadius;
            Antenna_dataGridView2.Rows[index].Cells[10].Value=tr.ApertureDistribution ;
            Antenna_dataGridView2.Rows[index].Cells[11].Value=tr.EdgeTeper; 
            Antenna_dataGridView2.Rows[index].Cells[12].Value=tr.Length;
            Antenna_dataGridView2.Rows[index].Cells[13].Value= tr.Pitch;
        }

        private void dbmanagewaveformname_comboBox2_DropDown(object sender, EventArgs e)
        {
            if (dbmanagewaveformtype_comboBox1.SelectedItem != "")
            {
                string Waveformtype = Translate.KeyWordsDictionary(dbmanagewaveformtype_comboBox1);
                string[] WaveformNames = new string[] { };
                WaveformNames = client.iGetAllWaveForm(Waveformtype);
                dbmanagewaveformname_comboBox2.Items.Clear();
                foreach (string s in WaveformNames)
                {
                    this.dbmanagewaveformname_comboBox2.Items.Add(s);
                }
            }
            else
            {
                string[] WaveformNames = new string[] { };
                WaveformNames = client.iGetAllWaveFormNames();
                dbmanagewaveformname_comboBox2.Items.Clear();
                foreach (string s in WaveformNames)
                {
                    this.dbmanagewaveformname_comboBox2.Items.Add(s);
                }
            }
        }

        private void dbmanagewaveformdelete_button1_Click(object sender, EventArgs e)
        {
            if (dbmanagewaveform_dataGridView1.Rows.Count != 1 && dbmanagewaveform_dataGridView1.SelectedRows.Count!=0)
            {
                string name = dbmanagewaveform_dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int index = dbmanagewaveform_dataGridView1.SelectedRows[0].Index;

                try
                {
                    client.iDelWaveForm(name);
                    dbmanagewaveform_dataGridView1.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                catch (System.TimeoutException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (FaultException<ServiceReference1.WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Detail.message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
                catch (CommunicationException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
            }
            else
            {
                MessageBox.Show("请先添加数据或选中数据");
            }
        }

        private void dbmanagewaveformname_comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                string[] a = new string[dbmanagewaveform_dataGridView1.Rows.Count - 1];
                for (int i = 0; i < dbmanagewaveform_dataGridView1.Rows.Count - 1; i++)
                {
                    a[i] = dbmanagewaveform_dataGridView1.Rows[i].Cells[0].Value.ToString();
                }
                //判断选中的值是否已经存在视图中
                for (int i = 0; i < dbmanagewaveform_dataGridView1.Rows.Count - 1; i++)
                {
                    if (a[i] == (string)dbmanagewaveformname_comboBox2.SelectedItem)
                    {
                        dbmanagewaveform_dataGridView1.Rows[i].Selected = true;
                        for (int j = 0; j < dbmanagewaveform_dataGridView1.Rows.Count - 1; j++)
                        {
                            if (j != i) dbmanagewaveform_dataGridView1.Rows[j].Selected = false;
                        }
                        break;
                    }
                    else
                    {
                        count++;
                        continue;
                    }
                }
                //如果不存在相同的则增加表中项
                if (count == dbmanagewaveform_dataGridView1.Rows.Count - 1)
                {
                    //string[] row = { ExistedTerrain.name, ExistedTerrain.originX.ToString(), ExistedTerrain.originY.ToString(), ExistedTerrain.path };
                    ExistedWaveform = client.iGetWaveForm((string)dbmanagewaveformname_comboBox2.SelectedItem);
                    dbmanagewaveform_dataGridView1.Rows.Add(ExistedWaveform.Name, ExistedWaveform.Type, ExistedWaveform.Frequency.ToString(), ExistedWaveform.BandWidth.ToString(), ExistedWaveform.Phase.ToString(), ExistedWaveform.StartFrequency.ToString(), ExistedWaveform.EndFrequency.ToString(), ExistedWaveform.RollOffFactor.ToString(), ExistedWaveform.FreChangeRate);
                    dbmanagewaveform_dataGridView1.Rows[dbmanagewaveform_dataGridView1.Rows.Count - 2].Selected = true;
                    for (int i = 0; i < dbmanagewaveform_dataGridView1.Rows.Count - 2; i++)
                    {
                        dbmanagewaveform_dataGridView1.Rows[i].Selected = false;
                    }
                }
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();
            }
        }

        private void dbmanagewaveform_dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((dbmanagewaveform_dataGridView1.CurrentCell.Value != null && Cellvalue != dbmanagewaveform_dataGridView1.CurrentCell.Value.ToString()) || (dbmanagewaveform_dataGridView1.CurrentCell.Value == null && Cellvalue != null))
            {
                dbmanagewaveform_dataGridView1.Rows[dbmanagewaveform_dataGridView1.CurrentCell.RowIndex].Selected = true;
                string name = dbmanagewaveform_dataGridView1.Rows[dbmanagewaveform_dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string item = dbmanagewaveform_dataGridView1.Columns[dbmanagewaveform_dataGridView1.CurrentCell.ColumnIndex].HeaderText;
                WaveformDBmanage_notice ds = new WaveformDBmanage_notice();
                ds.label1.Text = name + "的" + item + "已经更改,是否同步到数据库？";
                ds.ShowDialog(this);
            }
        }
        public void Waveform_update()
        {
            ServiceReference1.WaveForm tr = new ServiceReference1.WaveForm();
            int index = dbmanagewaveform_dataGridView1.SelectedRows[0].Index;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[0].Value != null && dbmanagewaveform_dataGridView1.Rows[index].Cells[0].Value != "")
                tr.Name = dbmanagewaveform_dataGridView1.Rows[index].Cells[0].Value.ToString();
            else tr.Name = null;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[1].Value != null && dbmanagewaveform_dataGridView1.Rows[index].Cells[1].Value != "")
                tr.Type = dbmanagewaveform_dataGridView1.Rows[index].Cells[1].Value.ToString();
            else tr.Type = null;
            tr.Frequency = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[2].Value);
            tr.BandWidth = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[3].Value);
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[4].Value != null && dbmanagewaveform_dataGridView1.Rows[index].Cells[4].Value != "")
                tr.Phase = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[4].Value);
            else tr.Phase = null;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[5].Value != null && dbmanagewaveform_dataGridView1.Rows[index].Cells[5].Value != "")
                tr.StartFrequency = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[5].Value);
            else tr.StartFrequency = null;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[6].Value != "" && dbmanagewaveform_dataGridView1.Rows[index].Cells[6].Value != null)
                tr.EndFrequency = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[6].Value);
            else tr.EndFrequency = null;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[7].Value != "" && dbmanagewaveform_dataGridView1.Rows[index].Cells[7].Value != null)
                tr.RollOffFactor = Convert.ToDouble(dbmanagewaveform_dataGridView1.Rows[index].Cells[7].Value);
            else tr.RollOffFactor = null;
            if (dbmanagewaveform_dataGridView1.Rows[index].Cells[8].Value != "" && dbmanagewaveform_dataGridView1.Rows[index].Cells[8].Value != null)
                tr.FreChangeRate = dbmanagewaveform_dataGridView1.Rows[index].Cells[8].Value.ToString();
            else tr.FreChangeRate = null;
            
            try
            {
                client.iUpdateWaveForm(tr);
                MessageBox.Show("更新成功");
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                MessageBox.Show("更新失败");
                client.Abort();
            }
        }
        public void Waveform_Recover()
        {
            ServiceReference1.WaveForm tr = new ServiceReference1.WaveForm();
            int index = dbmanagewaveform_dataGridView1.SelectedRows[0].Index;
            string name = dbmanagewaveform_dataGridView1.Rows[index].Cells[0].Value.ToString();
            tr = client.iGetWaveForm(name);
            dbmanagewaveform_dataGridView1.Rows[index].Cells[1].Value = tr.Type;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[2].Value = tr.Frequency;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[4].Value = tr.Phase;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[3].Value = tr.BandWidth;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[5].Value = tr.StartFrequency;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[6].Value = tr.EndFrequency;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[7].Value = tr.RollOffFactor;
            dbmanagewaveform_dataGridView1.Rows[index].Cells[8].Value = tr.FreChangeRate;
           
        }

        private void dbmanagewaveform_dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dbmanagewaveform_dataGridView1.Rows.Count - 1; i++)
            {
                if (i != dbmanagewaveform_dataGridView1.CurrentCell.RowIndex)
                    dbmanagewaveform_dataGridView1.Rows[i].Selected = false;
                else dbmanagewaveform_dataGridView1.Rows[i].Selected = true;
            }
        }

        private void pro_button3_Click(object sender, EventArgs e)
        {
            client.Close();
            this.Close();
        }

        private void pro_comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ExistedPro = client.iGetProjectInfo();
                if (pro_comboBox1.Text == "已完成")
                {
                    pro_button2.Enabled = true;
                    prodir.Clear();
                    proname.Clear();
                    while (pro_dataGridView1.Rows.Count!=1)
                    {
                        pro_dataGridView1.Rows.RemoveAt(0);
                    }
                    for (int i = 0; i < ExistedPro.Length; i++)
                    {
                        if (ExistedPro[i].ProState == 2)
                        {
                            proname.Add(ExistedPro[i].Name);
                            prodir.Add(ExistedPro[i].Directory);
                        }
                    }
                    for (int i = 0; i < proname.Count; i++)
                    {
                        pro_dataGridView1.Rows.Add(proname[i], prodir[i]);
                    }
                }

                else if (pro_comboBox1.Text == "未完成")
                {
                    pro_button2.Enabled = false;
                    prodir.Clear();
                    proname.Clear();
                    while (pro_dataGridView1.Rows.Count != 1)
                    {
                        pro_dataGridView1.Rows.RemoveAt(0);
                    }
                    for (int i = 0; i < ExistedPro.Length; i++)
                    {
                        if (ExistedPro[i].ProState == 0 || ExistedPro[i].ProState==1)
                        {
                            proname.Add(ExistedPro[i].Name);
                            prodir.Add(ExistedPro[i].Directory);
                        }
                    }
                    for (int i = 0; i < proname.Count; i++)
                    {
                        pro_dataGridView1.Rows.Add(proname[i], prodir[i]);
                    }
                }
            }
            catch (System.TimeoutException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (FaultException<ServiceReference1.WcfException> ex)
            {
                LogFileManager.ObjLog.error(ex.Detail.message, ex);
                client.Abort();

            }
            catch (CommunicationException ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();

            }
            catch (Exception ex)
            {
                LogFileManager.ObjLog.error(ex.Message, ex);
                client.Abort();
            }

        }

        private void pro_dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < pro_dataGridView1.Rows.Count - 1; i++)
            {
                if (i != pro_dataGridView1.CurrentCell.RowIndex)
                    pro_dataGridView1.Rows[i].Selected = false;
                else pro_dataGridView1.Rows[i].Selected = true;
            }
        }

        private void pro_button2_Click(object sender, EventArgs e)
        {
            if (pro_dataGridView1.Rows.Count != 1&&pro_dataGridView1.SelectedRows.Count!=0)
            {
                string name = pro_dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                int index = pro_dataGridView1.SelectedRows[0].Index;

                try
                {
                    client.iDelProject(name);
                    pro_dataGridView1.Rows.RemoveAt(index);
                    MessageBox.Show("删除成功");
                }
                catch (System.TimeoutException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (FaultException<ServiceReference1.WcfException> ex)
                {
                    LogFileManager.ObjLog.error(ex.Detail.message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
                catch (CommunicationException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();

                }
                catch (Exception ex)
                {
                    LogFileManager.ObjLog.error(ex.Message, ex);
                    MessageBox.Show("删除失败");
                    client.Abort();
                }
            }
            else
            {
                MessageBox.Show("请先添加数据或选中数据");
            }
        }

        private void DBmanageWindow_Resize(object sender, EventArgs e)
        {
            float[] scale = (float[])Tag;
            int i = 2;
            //遍历每一个控件使其同比例放大
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Left = (int)(Size.Width * scale[i++]);
                ctrl.Top = (int)(Size.Height * scale[i++]);
                ctrl.Width = (int)(Size.Width / (float)scale[0] * ((Size)ctrl.Tag).Width);
                ctrl.Height = (int)(Size.Height / (float)scale[1] * ((Size)ctrl.Tag).Height);
                //工程树单独同比例放大  
                if (WindowState == FormWindowState.Maximized)
                {
                    //最大化时调整字体的型号如下
                    ctrl.Font = new System.Drawing.Font("宋体", 12);
                    Terrainmanager_dataGridView1.Height = 500;
                    Terrainmanager_dataGridView1.Width = 1250;
                    Terrainmanager_button3.Location = new Point(375, 600);
                    button1.Location = new Point(875, 600);
                    Transmiter_dataGridView3.Height = 500;
                    Transmiter_dataGridView3.Width = 1250;
                    Transmiter_button9.Location = new Point(375, 600);
                    button7.Location = new Point(875, 600);
                    dbmanagewaveform_dataGridView1.Height = 500;
                    dbmanagewaveform_dataGridView1.Width = 1250;
                    dbmanagewaveformcancel_button3.Location = new Point(875, 600);
                    dbmanagewaveformdelete_button1.Location = new Point(375, 600);
                    Antenna_dataGridView2.Height = 500;
                    Antenna_dataGridView2.Width = 1250;
                    dbmanageantennacancel_button4.Location = new Point(875, 600);
                    dbmanageAntennadelete_button6.Location = new Point(375, 600);
                    pro_dataGridView1.Height = 500;
                    pro_dataGridView1.Width = 1250;
                    pro_button2.Location = new Point(375, 600);
                    pro_button3.Location = new Point(875, 600);

                }
                else if (WindowState == FormWindowState.Normal)
                {
                    //默认大小的窗口时所需的操作
                    ctrl.Font = new System.Drawing.Font("宋体", 9);
                    Terrainmanager_dataGridView1.Height = 150;
                    Terrainmanager_dataGridView1.Width = 551;
                    Terrainmanager_button3.Location = new Point(153, 254);
                    button1.Location = new Point(305, 254);
                    Transmiter_dataGridView3.Height = 150;
                    Transmiter_dataGridView3.Width = 545;
                    Transmiter_button9.Location = new Point(153, 254);
                    button7.Location = new Point(305, 254);
                    dbmanagewaveform_dataGridView1.Height = 150;
                    dbmanagewaveform_dataGridView1.Width = 545;
                    dbmanagewaveformcancel_button3.Location = new Point(305, 254);
                    dbmanagewaveformdelete_button1.Location = new Point(153, 254);
                    Antenna_dataGridView2.Height = 150;
                    Antenna_dataGridView2.Width = 552;
                    dbmanageantennacancel_button4.Location = new Point(305, 254);
                    dbmanageAntennadelete_button6.Location = new Point(153, 254);
                    pro_dataGridView1.Height = 150;
                    pro_dataGridView1.Width = 486;
                    pro_button2.Location = new Point(153, 254);
                    pro_button3.Location = new Point(305, 254);
                    ewidth = this.Width;
                    eheight = this.Height;
                    button1.Left = (int)(button1.Left * (ewidth / bwidth));
                    button1.Top = (int)(button1.Top * (eheight / bheight));
                    Terrainmanager_dataGridView1.Height = (int)(Terrainmanager_dataGridView1.Height * (eheight / bheight));
                    Terrainmanager_dataGridView1.Width = (int)(Terrainmanager_dataGridView1.Width * (ewidth / bwidth));
                    Terrainmanager_button3.Left = (int)(Terrainmanager_button3.Left * (ewidth / bwidth));
                    Terrainmanager_button3.Top = (int)(Terrainmanager_button3.Top * (eheight / bheight));
                    Transmiter_dataGridView3.Height = (int)(Transmiter_dataGridView3.Height * (eheight / bheight));
                    Transmiter_dataGridView3.Width = (int)(Transmiter_dataGridView3.Width * (ewidth / bwidth));
                    Transmiter_button9.Left = (int)(Transmiter_button9.Left * (ewidth / bwidth));
                    Transmiter_button9.Top = (int)(Transmiter_button9.Top * (eheight / bheight));
                    button7.Left = (int)(button7.Left * (ewidth / bwidth));
                    button7.Top = (int)(button7.Top * (eheight / bheight));
                    dbmanagewaveform_dataGridView1.Height = (int)(dbmanagewaveform_dataGridView1.Height * (eheight / bheight));
                    dbmanagewaveform_dataGridView1.Width = (int)(dbmanagewaveform_dataGridView1.Width * (ewidth / bwidth));
                    dbmanagewaveformcancel_button3.Left = (int)(dbmanagewaveformcancel_button3.Left * (ewidth / bwidth));
                    dbmanagewaveformcancel_button3.Top = (int)(dbmanagewaveformcancel_button3.Top * (eheight / bheight));
                    dbmanagewaveformdelete_button1.Left = (int)(dbmanagewaveformdelete_button1.Left * (ewidth / bwidth));
                    dbmanagewaveformdelete_button1.Top = (int)(dbmanagewaveformdelete_button1.Top * (eheight / bheight));
                    Antenna_dataGridView2.Height = (int)(Antenna_dataGridView2.Height * (eheight / bheight));
                    Antenna_dataGridView2.Width = (int)(Antenna_dataGridView2.Width * (ewidth / bwidth));
                    dbmanageantennacancel_button4.Left = (int)(dbmanageantennacancel_button4.Left * (ewidth / bwidth));
                    dbmanageantennacancel_button4.Top = (int)(dbmanageantennacancel_button4.Top * (eheight / bheight));
                    dbmanageAntennadelete_button6.Left = (int)(dbmanageAntennadelete_button6.Left * (ewidth / bwidth));
                    dbmanageAntennadelete_button6.Top = (int)(dbmanageAntennadelete_button6.Top * (eheight / bheight));
                    pro_dataGridView1.Height = (int)(pro_dataGridView1.Height * (eheight / bheight));
                    pro_dataGridView1.Width = (int)(pro_dataGridView1.Width * (ewidth / bwidth));
                    pro_button2.Left = (int)(pro_button2.Left * (ewidth / bwidth));
                    pro_button2.Top = (int)(pro_button2.Top * (eheight / bheight));
                    pro_button3.Left = (int)(pro_button3.Left * (ewidth / bwidth));
                    pro_button3.Top = (int)(pro_button3.Top * (eheight / bheight));
                }
            }
            
            

            

        }

        private void dbmanagewaveform_dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dbmanagewaveform_dataGridView1.CurrentCell.Value != null)
            {
                Cellvalue = dbmanagewaveform_dataGridView1.CurrentCell.Value.ToString();
            }
            else Cellvalue = null;
        }

        private void Antenna_dataGridView2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (Antenna_dataGridView2.CurrentCell.Value != null)
            {
                Cellvalue = Antenna_dataGridView2.CurrentCell.Value.ToString();
            }
            else
                Cellvalue = null;
        }

        private void Transmiter_dataGridView3_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (Transmiter_dataGridView3.CurrentCell.Value != null )
            {
                Cellvalue = Transmiter_dataGridView3.CurrentCell.Value.ToString();
            }
            else Cellvalue = null;
        }
        double bwidth, bheight, ewidth, eheight; 

        //private void DBmanageWindow_ResizeEnd(object sender, EventArgs e)
        //{
        //    ewidth = this.Width;
        //    eheight = this.Height;
        //    button1.Left = (int)(button1.Left * (ewidth / bwidth));
        //   // button1.Width = (int)(button1.Width * (ewidth / bwidth));
        //    button1.Top = (int)(button1.Top * (eheight / bheight));
        //   // button1.Height = (int)(button1.Height * (eheight / bheight));
    
        //}

       
        }

      
    }

