using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace WF1
{
    public partial class InterfenceWindow : Form
    {
        Regex r = null;
        MatchCollection mc = null;
        string Rxpath = null;
        string data = null;
        string[] pointname = new string[]{};
        string[] gridname = new string[]{};
        int[] gridindex = null;
        int[] pointindex = null;
        string Selectgird = "begin_<grid>";
        string Selectpoint = "begin_<points>";
        int[] gridRxset = null;
        int[] pointRxset = null;
        List<string> XFilecontent = new List<string>();
        List<string> YFilecontent = new List<string>();
        List<string> ZFilecontent = new List<string>();
        string[] XYZFilename = null;
        string Totalresultpath =null;   
        public InterfenceWindow()
        {
            InitializeComponent();           
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            if (((string)comboBox1.SelectedItem) == "区域（grid）")
            {
                comboBox2.Text = "";
                comboBox2.Items.Clear();
                if (gridname.Length!=0)
                {
                 
                    for (int i = 0; i < gridname.Length; i++)
                    {
                        string s = gridname[i] + " RxSet " + gridRxset[i].ToString();
                        comboBox2.Items.Add(s);
                    }
                }
                else MessageBox.Show("不存在区域接收机");
            }
            else if (((string)comboBox1.SelectedItem) == "点（point）")
            {
                comboBox2.Text = "";
                comboBox2.Items.Clear();
                if (pointname.Length != 0)
                {
                    for (int i = 0; i < pointname.Length; i++)
                    {
                        string s = pointname[i] + " RxSet " + pointRxset[i].ToString();
                        comboBox2.Items.Add(s);
                    }
                }
                else MessageBox.Show("不存在点状接收机");
            }
            else
            {
                MessageBox.Show("请先选择接收机类型");
            }
        }

        private void InterfenceWindow_Load(object sender, EventArgs e)
        {
            Rxpath = MainWindow.projectRealPath + "\\" + MainWindow.onlyProjectName + ".rx";
            //Rxpath = @"C:\Users\Tracy~lee\Desktop\23\test2.rx";
            if (!File.Exists(Rxpath))
            {
                this.Close();
                MessageBox.Show("还未添加接收机,请先添加接收机并得到计算结果！");               
            }
            else
            {
                StreamReader sr = new StreamReader(Rxpath);
                data = sr.ReadToEnd();
                r = new Regex(Selectgird);
                mc = r.Matches(data);
                if (mc.Count != 0)
                {
                    gridindex = new int[mc.Count];
                    for (int i = 0; i < mc.Count; i++)
                    {
                        gridindex[i] = mc[i].Index;
                    }
                    gridRxset = new int[gridindex.Length];
                    gridname = new string[gridindex.Length];
                    for (int i = 0; i < gridindex.Length; i++)
                    {
                        int m = gridindex[i] + Selectgird.Length + 1;
                        int count = 0;
                        while (data[m + count] != '\n') count++;
                        gridname[i] = data.Substring(m, count-1);
                        int b=0;
                        while(data[m+count+7+b]!='\n')b++;
                        gridRxset[i] = Convert.ToInt32(data.Substring(m + count + 7, b-1));
                    }
                }
                r = new Regex(Selectpoint);
                mc = r.Matches(data);
                if (mc.Count != 0)
                {
                    pointindex = new int[mc.Count];
                    for (int i = 0; i < mc.Count; i++)
                    {
                        pointindex[i] = mc[i].Index;
                    }
                    pointRxset = new int[pointindex.Length];
                    pointname = new string[pointindex.Length];
                    for (int i = 0; i < pointindex.Length; i++)
                    {
                        int m = pointindex[i] + Selectpoint.Length + 1;
                        int count = 0;
                        while (data[m + count] != '\n') count++;
                        pointname[i] = data.Substring(m, count-1);
                        int b = 0;
                        while (data[m + count + 7 + b] != '\n') b++;
                        pointRxset[i] = Convert.ToInt32(data.Substring(m + count + 7, b-1));
                    }
                }              
                sr.Close();
            }


        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            XFilecontent.Clear();
            YFilecontent.Clear();
            ZFilecontent.Clear();
            Totalresultpath =MainWindow.projectRealPath+"\\studyarea\\"+comboBox4.Text;
            //Totalresultpath = @"C:\Users\Tracy~lee\Desktop\Test\" + comboBox4.Text;                     
            int a=0;
            if (comboBox2.Text != "" && comboBox2.Text != null)
            {
                a = Convert.ToInt32(comboBox2.Text.Substring(comboBox2.Text.LastIndexOf(" ") + 1));
                DirectoryInfo resultfiles = new DirectoryInfo(Totalresultpath);
                FileInfo[] files = resultfiles.GetFiles();
                string[] filenames = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    filenames[i] = files[i].Name;
                }
                XYZFilename = GetMag(filenames, a);
                StreamReader sr = new StreamReader(Totalresultpath + "\\" + XYZFilename[0]);
                while (!sr.EndOfStream)
                {
                    XFilecontent.Add(sr.ReadLine());
                }
                sr.Close();
                for (int i = 1; i <= XFilecontent.Count - 2; i++)
                {
                    comboBox3.Items.Add(i.ToString());
                }
            }
            else MessageBox.Show("请先选择接收机名称");
            

        }
        private string [] GetMag(string [] a,int b)
        {
            List<string> Magfiles = new List<string>();
            List<string> correspondfile = new List<string>();
            for (int i = 0; i < a.Length; i++)
            {
                string s=a[i].Substring(a[i].IndexOf('.')+1,6);
                if (s == "etxmag" || s == "etymag" || s == "etzmag")
                {
                    Magfiles.Add(a[i]);
                }
            }
            for (int i = 0; i < Magfiles.Count; i++)
            {
                int m =Convert.ToInt32( Magfiles[i].Substring(Magfiles[i].LastIndexOf('.')-3,3));
                if (m == b) correspondfile.Add(Magfiles[i]);
            }
            return correspondfile.ToArray();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text != "" && comboBox3.Text != null)
            {
                
                int a=Convert.ToInt32( comboBox3.Text);
                double x = 0;
                double y = 0;
                double z = 0;
                double total=0;
                if (XYZFilename.Length == 3)
                {
                    StreamReader sr = new StreamReader(Totalresultpath + "\\" + XYZFilename[1]);
                    while (!sr.EndOfStream)
                    {
                        YFilecontent.Add(sr.ReadLine());
                    }
                    sr.Close();
                    sr = new StreamReader(Totalresultpath + "\\" + XYZFilename[2]);
                    while (!sr.EndOfStream)
                    {
                        ZFilecontent.Add(sr.ReadLine());
                    }
                    sr.Close();
                    x=Convert.ToDouble( XFilecontent[a + 1].Substring(61, 11));
                    y = Convert.ToDouble(YFilecontent[a + 1].Substring(61, 11));
                    z = Convert.ToDouble(ZFilecontent[a + 1].Substring(61, 11));
                    total = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
                    textBox3.Text = total.ToString();
                    if (total >= 2) textBox4.Text = "强";
                    else if (total < 2 && total > 1) textBox4.Text = "中";
                    else textBox4.Text = "弱";
                }
                else MessageBox.Show("缺少计算结果");
            }
            else MessageBox.Show("请先选择接收机编号");
        }

        private void comboBox4_DropDown(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            string path=MainWindow.projectRealPath+"\\studyarea\\";
            //string path = @"C:\Users\Tracy~lee\Desktop\Test";
            if (!Directory.Exists(path))
            {
                MessageBox.Show("还未得到计算结果！");
            }
            else
            {
                string[] frequencepath = Directory.GetDirectories(path);
                string[] frequence = new string[frequencepath.Length];
                for (int i = 0; i < frequencepath.Length; i++)
                {
                    int y = frequencepath[i].LastIndexOf("\\");
                    frequence[i] = frequencepath[i].Substring(y + 1);
                }
                for (int i = 0; i < frequence.Length; i++)
                {
                    comboBox4.Items.Add(frequence[i]);
                }
            }
        }
    }
}
