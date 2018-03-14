using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CreateXml
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
            comboBox2.SelectedIndex = 2;
            comboBox3.SelectedIndex = 2;
            comboBox4.SelectedIndex = 2;
            comboBox7.SelectedIndex = 1;
            comboBox8.SelectedIndex = 1;
            comboBox9.SelectedIndex = 1;
            comboBox10.SelectedIndex = 1;
            textBox57.Text = "GetPointOfContainOneDiffractionPathUseXmlTest";
        }
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\Users\wangnan\Desktop\测试1.xml");
            XmlElement root = doc.CreateElement("TheCollectionOfTestCases");//创建根节点
            doc.AppendChild(root);//将根节点加入到xml文件中
            XmlElement firstLevelElement = doc.CreateElement("TestCase");//创建第一层子节点
            firstLevelElement.SetAttribute("path", comboBox5.SelectedItem.ToString());//添加path属性
            firstLevelElement.SetAttribute("havePath", comboBox6.SelectedItem.ToString());//添加havePath属性
            root.AppendChild(firstLevelElement);//将子节点加到根节点上
            XmlElement secondLevelElement1 = doc.CreateElement("Transmitter");//创建第二层的子节点
            secondLevelElement1.SetAttribute("x", textBox39.Text);
            secondLevelElement1.SetAttribute("y", textBox38.Text);
            secondLevelElement1.SetAttribute("z", textBox37.Text);
            firstLevelElement.AppendChild(secondLevelElement1);//将第二层节点加到第一层节点上
            //创建第二层的第二个节点
            //判断是棱还是三角形
            if (comboBox1.SelectedItem.ToString() == "Triangle")
            {
                //为三角形赋值
                XmlElement secondLevelElement2 = doc.CreateElement("Triangle");
                XmlElement thirdLevelElement21 = doc.CreateElement("TheFirstEndPointOfTriangle");
                //为第一个顶点赋值
                thirdLevelElement21.SetAttribute("x", textBox1.Text);
                thirdLevelElement21.SetAttribute("y", textBox2.Text);
                thirdLevelElement21.SetAttribute("z", textBox3.Text);
                XmlElement thirdLevelElement22 = doc.CreateElement("TheSecondEndPointOfTriangle");
                //为第二个顶点赋值
                thirdLevelElement22.SetAttribute("x", textBox4.Text);
                thirdLevelElement22.SetAttribute("y", textBox5.Text);
                thirdLevelElement22.SetAttribute("z", textBox6.Text);
                XmlElement thirdLevelElement23 = doc.CreateElement("TheThirdEndPointOfTriangle");
                //为第三个顶点赋值
                thirdLevelElement23.SetAttribute("x", textBox7.Text);
                thirdLevelElement23.SetAttribute("y", textBox8.Text);
                thirdLevelElement23.SetAttribute("z", textBox9.Text);
                secondLevelElement2.AppendChild(thirdLevelElement21);
                secondLevelElement2.AppendChild(thirdLevelElement22);
                secondLevelElement2.AppendChild(thirdLevelElement23);
                firstLevelElement.AppendChild(secondLevelElement2);
            }
            if (comboBox1.SelectedItem.ToString() == "Edge")
            {
                //为三角形赋值
                XmlElement secondLevelElement2 = doc.CreateElement("Edge");
                XmlElement thirdLevelElement21 = doc.CreateElement("TheFirstEndPointOfEdge");
                //为第一个顶点赋值
                thirdLevelElement21.SetAttribute("x", textBox1.Text);
                thirdLevelElement21.SetAttribute("y", textBox2.Text);
                thirdLevelElement21.SetAttribute("z", textBox3.Text);

                XmlElement thirdLevelElement22 = doc.CreateElement("TheSecondEndPointOfTEdge");
                //为第二个顶点赋值
                thirdLevelElement22.SetAttribute("x", textBox4.Text);
                thirdLevelElement22.SetAttribute("y", textBox5.Text);
                thirdLevelElement22.SetAttribute("z", textBox6.Text);

                secondLevelElement2.AppendChild(thirdLevelElement21);
                secondLevelElement2.AppendChild(thirdLevelElement22);
                firstLevelElement.AppendChild(secondLevelElement2);
            }




            //判断是棱还是三角形
            if (comboBox2.SelectedItem.ToString() == "Triangle")
            {
                //为三角形赋值
                XmlElement secondLevelElement3 = doc.CreateElement("Triangle");
                XmlElement thirdLevelElement31 = doc.CreateElement("TheFirstEndPointOfTriangle");
                //为第一个顶点赋值
                thirdLevelElement31.SetAttribute("x", textBox18.Text);
                thirdLevelElement31.SetAttribute("y", textBox17.Text);
                thirdLevelElement31.SetAttribute("z", textBox16.Text);

                XmlElement thirdLevelElement32 = doc.CreateElement("TheSecondEndPointOfTriangle");
                //为第二个顶点赋值
                thirdLevelElement32.SetAttribute("x", textBox15.Text);
                thirdLevelElement32.SetAttribute("y", textBox14.Text);
                thirdLevelElement32.SetAttribute("z", textBox13.Text);
                XmlElement thirdLevelElement33 = doc.CreateElement("TheThirdEndPointOfTriangle");
                //为第三个顶点赋值
                thirdLevelElement33.SetAttribute("x", textBox12.Text);
                thirdLevelElement33.SetAttribute("y", textBox11.Text);
                thirdLevelElement33.SetAttribute("z", textBox10.Text);
                secondLevelElement3.AppendChild(thirdLevelElement31);
                secondLevelElement3.AppendChild(thirdLevelElement32);
                secondLevelElement3.AppendChild(thirdLevelElement33);
                firstLevelElement.AppendChild(secondLevelElement3);
            }
            if (comboBox2.SelectedItem.ToString() == "Edge")
            {
                //为三角形赋值
                XmlElement secondLevelElement3 = doc.CreateElement("Edge");
                XmlElement thirdLevelElement31 = doc.CreateElement("EheFirstEndPointOfEdge");
                //为第一个顶点赋值
                thirdLevelElement31.SetAttribute("x", textBox18.Text);
                thirdLevelElement31.SetAttribute("y", textBox17.Text);
                thirdLevelElement31.SetAttribute("z", textBox16.Text);
                XmlElement thirdLevelElement32 = doc.CreateElement("TheSecondEndPointOfTEdge");
                //为第二个顶点赋值
                thirdLevelElement32.SetAttribute("x", textBox15.Text);
                thirdLevelElement32.SetAttribute("y", textBox14.Text);
                thirdLevelElement32.SetAttribute("z", textBox13.Text);
                secondLevelElement3.AppendChild(thirdLevelElement31);
                secondLevelElement3.AppendChild(thirdLevelElement32);
                firstLevelElement.AppendChild(secondLevelElement3);
            }




            //判断是棱还是三角形
            if (comboBox3.SelectedItem.ToString() == "Triangle")
            {
                //为三角形赋值
                XmlElement secondLevelElement4 = doc.CreateElement("Triangle");
                XmlElement thirdLevelElement41 = doc.CreateElement("TheFirstEndPointOfTriangle");
                //为第一个顶点赋值
                thirdLevelElement41.SetAttribute("x", textBox27.Text);
                thirdLevelElement41.SetAttribute("y", textBox26.Text);
                thirdLevelElement41.SetAttribute("z", textBox25.Text);
                XmlElement thirdLevelElement42 = doc.CreateElement("TheSecondEndPointOfTriangle");
                //为第二个顶点赋值
                thirdLevelElement42.SetAttribute("x", textBox24.Text);
                thirdLevelElement42.SetAttribute("y", textBox23.Text);
                thirdLevelElement42.SetAttribute("z", textBox22.Text);
                XmlElement thirdLevelElement43 = doc.CreateElement("TheThirdEndPointOfTriangle");
                //为第三个顶点赋值
                thirdLevelElement43.SetAttribute("x", textBox21.Text);
                thirdLevelElement43.SetAttribute("y", textBox20.Text);
                thirdLevelElement43.SetAttribute("z", textBox19.Text);
                secondLevelElement4.AppendChild(thirdLevelElement41);
                secondLevelElement4.AppendChild(thirdLevelElement42);
                secondLevelElement4.AppendChild(thirdLevelElement43);
                firstLevelElement.AppendChild(secondLevelElement4);
            }
            if (comboBox3.SelectedItem.ToString() == "Edge")
            {
                //为三角形赋值
                XmlElement secondLevelElement4 = doc.CreateElement("Edge");
                XmlElement thirdLevelElement41 = doc.CreateElement("TheFirstEndPointOfEdge");
                //为第一个顶点赋值
                thirdLevelElement41.SetAttribute("x", textBox27.Text);
                thirdLevelElement41.SetAttribute("y", textBox26.Text);
                thirdLevelElement41.SetAttribute("z", textBox25.Text);
                XmlElement thirdLevelElement42 = doc.CreateElement("TheSecondEndPointOfTEdge");
                //为第二个顶点赋值
                thirdLevelElement42.SetAttribute("x", textBox24.Text);
                thirdLevelElement42.SetAttribute("y", textBox23.Text);
                thirdLevelElement42.SetAttribute("z", textBox22.Text);

                secondLevelElement4.AppendChild(thirdLevelElement41);
                secondLevelElement4.AppendChild(thirdLevelElement42);
                firstLevelElement.AppendChild(secondLevelElement4);
            }




            //判断是棱还是三角形
            if (comboBox4.SelectedItem.ToString() == "Triangle")
            {
                //为三角形赋值
                XmlElement secondLevelElement5 = doc.CreateElement("Triangle");
                XmlElement thirdLevelElement51 = doc.CreateElement("TheFirstEndPointOfTriangle");
                //为第一个顶点赋值
                thirdLevelElement51.SetAttribute("x", textBox36.Text);
                thirdLevelElement51.SetAttribute("y", textBox35.Text);
                thirdLevelElement51.SetAttribute("z", textBox34.Text);
                XmlElement thirdLevelElement52 = doc.CreateElement("TheSecondEndPointOfTriangle");
                //为第二个顶点赋值
                thirdLevelElement52.SetAttribute("x", textBox33.Text);
                thirdLevelElement52.SetAttribute("y", textBox32.Text);
                thirdLevelElement52.SetAttribute("z", textBox31.Text);
                XmlElement thirdLevelElement53 = doc.CreateElement("TheThirdEndPointOfTriangle");
                //为第三个顶点赋值
                thirdLevelElement53.SetAttribute("x", textBox30.Text);
                thirdLevelElement53.SetAttribute("y", textBox29.Text);
                thirdLevelElement53.SetAttribute("z", textBox28.Text);
                secondLevelElement5.AppendChild(thirdLevelElement51);
                secondLevelElement5.AppendChild(thirdLevelElement52);
                secondLevelElement5.AppendChild(thirdLevelElement53);
                firstLevelElement.AppendChild(secondLevelElement5);
            }
            if (comboBox4.SelectedItem.ToString() == "Edge")
            {
                //为三角形赋值
                XmlElement secondLevelElement5 = doc.CreateElement("Edge");
                XmlElement thirdLevelElement51 = doc.CreateElement("TheFirstEndPointOfEdge");
                //为第一个顶点赋值
                thirdLevelElement51.SetAttribute("x", textBox36.Text);
                thirdLevelElement51.SetAttribute("y", textBox35.Text);
                thirdLevelElement51.SetAttribute("z", textBox34.Text);
                XmlElement thirdLevelElement52 = doc.CreateElement("TheSecondEndPointOfTEdge");
                //为第二个顶点赋值
                thirdLevelElement52.SetAttribute("x", textBox33.Text);
                thirdLevelElement52.SetAttribute("y", textBox32.Text);
                thirdLevelElement52.SetAttribute("z", textBox31.Text);
                secondLevelElement5.AppendChild(thirdLevelElement51);
                secondLevelElement5.AppendChild(thirdLevelElement52);
                firstLevelElement.AppendChild(secondLevelElement5);
            }
            //为Rx赋值
            XmlElement secondLevelElement6 = doc.CreateElement("Receiver");//创建第二层的子节点
            secondLevelElement6.SetAttribute("x", textBox42.Text);
            secondLevelElement6.SetAttribute("y", textBox41.Text);
            secondLevelElement6.SetAttribute("z", textBox40.Text);
            firstLevelElement.AppendChild(secondLevelElement6);//将第二层节点加到第一层节点上
            //为expected0赋值
            if (comboBox7.SelectedItem.ToString() != "null")
            {
                XmlElement secondLevelElement7 = doc.CreateElement("Expected");//创建第二层的子节点
                secondLevelElement7.SetAttribute("x", textBox45.Text);
                secondLevelElement7.SetAttribute("y", textBox44.Text);
                secondLevelElement7.SetAttribute("z", textBox43.Text);
                firstLevelElement.AppendChild(secondLevelElement7);//将第二层节点加到第一层节点上
            }
            if (comboBox8.SelectedItem.ToString() != "null")
            {

                //为expected1赋值
                XmlElement secondLevelElement8 = doc.CreateElement("Expected");//创建第二层的子节点
                secondLevelElement8.SetAttribute("x", textBox48.Text);
                secondLevelElement8.SetAttribute("y", textBox47.Text);
                secondLevelElement8.SetAttribute("z", textBox46.Text);
                firstLevelElement.AppendChild(secondLevelElement8);//将第二层节点加到第一层节点上
            }
            if (comboBox9.SelectedItem.ToString() != "null")
            {
                //为expected2赋值
                XmlElement secondLevelElement9 = doc.CreateElement("Expected");//创建第二层的子节点
                secondLevelElement9.SetAttribute("x", textBox51.Text);
                secondLevelElement9.SetAttribute("y", textBox50.Text);
                secondLevelElement9.SetAttribute("z", textBox49.Text);
                firstLevelElement.AppendChild(secondLevelElement9);//将第二层节点加到第一层节点上
            }
            //为expected3赋值
            if (comboBox10.SelectedItem.ToString() != "null")
            {
                XmlElement secondLevelElement10 = doc.CreateElement("Expected");//创建第二层的子节点
                secondLevelElement10.SetAttribute("x", textBox54.Text);
                secondLevelElement10.SetAttribute("y", textBox53.Text);
                secondLevelElement10.SetAttribute("z", textBox52.Text);
                firstLevelElement.AppendChild(secondLevelElement10);//将第二层节点加到第一层节点上
            }
            string xmlTrace = textBox55.Text.ToString();
            //xmlTrace = xmlTrace.Replace("\\ \"","\"");
            //xmlTrace = xmlTrace.Replace("\\\\","\\");

            string num = textBox56.Text;
            //textBox56.Text = number.ToString()
            
            string relativeXmlPath2 = ("测试" + num + ".xml");
            string fullName = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\"));
            if (!Directory.Exists(fullName + "\\" + textBox57.Text))
            {
                Directory.CreateDirectory(fullName + "\\" + textBox57.Text);
            }
            fullName = fullName + "\\" + textBox57.Text + "\\" + relativeXmlPath2;
            doc.Save(fullName);
            int number = int.Parse(num);
            MessageBox.Show("恭喜您,第"+number+"个测试用例添加成功");
            number++;
            textBox56.Text = number.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // foreach(
            foreach (Control c in this.panel1.Controls)
            {
               c.Text= "";
                
            }
            foreach (Control c in this.panel2.Controls)
            {
                c.Text = "";

            }
            foreach (Control c in this.panel3.Controls)
            {
                c.Text = "";

            }
            foreach (Control c in this.panel4.Controls)
            {
                c.Text = "";

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox55_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            foreach (Control c in this.panel1.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                } 
            }
            foreach (Control c in this.panel2.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                } 
            }
            foreach (Control c in this.panel3.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                } 
            }
            foreach (Control c in this.panel4.Controls)
            {
                //c.Text = "";
                if (c is TextBox)
                {
                    c.Text = "";
                } 
            }
            foreach (Control c in Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                } 
                
            }
        }

       
        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            comboBox1.SelectedIndex = 2;
            comboBox2.SelectedIndex = 2;
            comboBox3.SelectedIndex = 2;
            comboBox4.SelectedIndex = 2;
            comboBox7.SelectedIndex = 1;
            comboBox8.SelectedIndex = 1;
            comboBox9.SelectedIndex = 1;
            comboBox10.SelectedIndex = 1;
            //初始化xml实例
            XmlDocument doc = new XmlDocument();
            //加载xml文件
            //string xmlTrace = textBox55.Text.ToString();

            //
            //弹出敞口、、若果为空，不读取，不为空，读取
            //
            //
            //
            //if (true)
          // {
                //doc.Load(@"C:\Users\ZFF\Desktop\测试1.xml");
                //string relativeXmlPath = @"测试1.xml";
                string num = textBox55.Text;  
                string relativeXmlPath = (@"测试" + num + ".xml");
                string fullName = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\"));
                if (!Directory.Exists(fullName+"\\"+textBox57.Text))
                {
                    Directory.CreateDirectory(fullName+"\\"+textBox57.Text);
                }
                fullName= fullName+ "\\"+textBox57.Text+"\\"+ relativeXmlPath;
            
                doc.Load(fullName);

                //获取文件的根节点
                XmlNode rootNode = doc.SelectSingleNode("TheCollectionOfTestCases");
                //分别获得该节点的InnerXml和OuterXml信息
                string innerXmlInfo = rootNode.InnerXml.ToString();
                string outerXmlInfo = rootNode.OuterXml.ToString();
                //获得第一层子节点
                XmlNodeList firstLevelNodelist = rootNode.ChildNodes;
                //设置node变量

                foreach (XmlNode node in firstLevelNodelist)
                {
                    int i = 0;//用来判断读取的棱或者边是第几组
                    int k = 0;//用来判断读取的预测点是第几个点
                    XmlAttributeCollection xmlAttributeCol = node.Attributes;
                    //获得节点属性
                    string path = xmlAttributeCol[0].Value;
                    string havePath = xmlAttributeCol[1].Value;
                    comboBox5.SelectedItem = path;
                    comboBox6.SelectedItem = havePath;
                    XmlNodeList secondLevelNodeList = node.ChildNodes;
                    foreach (XmlNode secondLevelNode in secondLevelNodeList)
                    {
                        if (secondLevelNode.Name == "Transmitter")
                        {
                            //读取发射机的点

                            XmlAttributeCollection xmlAttributeCollectionOfTransmitter = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfTransmitter[0].Value;
                            string y = xmlAttributeCollectionOfTransmitter[1].Value;
                            string z = xmlAttributeCollectionOfTransmitter[2].Value;
                            textBox39.Text = x;
                            textBox38.Text = y;
                            textBox37.Text = z;
                        }

                        if (secondLevelNode.Name == "Edge" && (i == 0))
                        {
                            //读取棱的位置
                            comboBox1.SelectedItem = "Edge";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox1.Text = x;
                            textBox2.Text = y;
                            textBox3.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox4.Text = x;
                            textBox5.Text = y;
                            textBox6.Text = z;


                            i++;//i加一

                            continue;
                        }
                        if (secondLevelNode.Name == "Triangle" && (i == 0))
                        {
                            //读取三角面的位置
                            comboBox1.SelectedItem = "Triangle";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox1.Text = x;
                            textBox2.Text = y;
                            textBox3.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox4.Text = x;
                            textBox5.Text = y;
                            textBox6.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[2].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox7.Text = x;
                            textBox8.Text = y;
                            textBox9.Text = z;

                            i++;
                            continue;
                        }

                        if (secondLevelNode.Name == "Edge" && (i == 1))
                        {
                            //读取棱的位置
                            comboBox2.SelectedItem = "Edge";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox18.Text = x;
                            textBox17.Text = y;
                            textBox16.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox15.Text = x;
                            textBox14.Text = y;
                            textBox13.Text = z;


                            i++;//i加一

                            continue;
                        }
                        if (secondLevelNode.Name == "Triangle" && (i == 1))
                        {
                            //读取三角面的位置
                            comboBox2.SelectedItem = "Triangle";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox18.Text = x;
                            textBox17.Text = y;
                            textBox16.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox15.Text = x;
                            textBox14.Text = y;
                            textBox13.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[2].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox12.Text = x;
                            textBox11.Text = y;
                            textBox10.Text = z;

                            i++;
                            continue;
                        }


                        if (secondLevelNode.Name == "Edge" && (i == 2))
                        {
                            //读取棱的位置
                            comboBox3.SelectedItem = "Edge";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox27.Text = x;
                            textBox26.Text = y;
                            textBox25.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox24.Text = x;
                            textBox23.Text = y;
                            textBox22.Text = z;


                            i++;//i加一
                            continue;

                        }
                        if (secondLevelNode.Name == "Triangle" && (i == 2))
                        {
                            //读取三角面的位置
                            comboBox3.SelectedItem = "Triangle";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox27.Text = x;
                            textBox26.Text = y;
                            textBox25.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox24.Text = x;
                            textBox23.Text = y;
                            textBox22.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[2].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox21.Text = x;
                            textBox20.Text = y;
                            textBox19.Text = z;

                            i++;
                            continue;
                        }


                        if (secondLevelNode.Name == "Edge" && (i == 3))
                        {
                            //读取棱的位置
                            comboBox4.SelectedItem = "Edge";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox36.Text = x;
                            textBox35.Text = y;
                            textBox34.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox33.Text = x;
                            textBox32.Text = y;
                            textBox31.Text = z;


                            i++;//i加一
                            continue;

                        }
                        if (secondLevelNode.Name == "Triangle" && (i == 3))
                        {
                            //读取三角面的位置
                            comboBox4.SelectedItem = "Triangle";
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
                            string x = xmlAttributeCollectionOfEdge[0].Value;
                            string y = xmlAttributeCollectionOfEdge[1].Value;
                            string z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox36.Text = x;
                            textBox35.Text = y;
                            textBox34.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox33.Text = x;
                            textBox32.Text = y;
                            textBox31.Text = z;

                            xmlAttributeCollectionOfEdge = thirdLevelNodeList[2].Attributes;
                            x = xmlAttributeCollectionOfEdge[0].Value;
                            y = xmlAttributeCollectionOfEdge[1].Value;
                            z = xmlAttributeCollectionOfEdge[2].Value;
                            textBox30.Text = x;
                            textBox29.Text = y;
                            textBox28.Text = z;

                            i++;
                            continue;
                        }
                        if (secondLevelNode.Name == "Receiver")
                        {
                            XmlAttributeCollection xmlAttributeCollectionOfReceiver = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfReceiver[0].Value;
                            string y = xmlAttributeCollectionOfReceiver[1].Value;
                            string z = xmlAttributeCollectionOfReceiver[2].Value;
                            textBox42.Text = x;
                            textBox41.Text = y;
                            textBox40.Text = z;
                        }
                        if (secondLevelNode.Name == "Expected" && (k == 0))
                        {
                            comboBox7.SelectedItem = "Expected0";
                            XmlAttributeCollection xmlAttributeCollectionOfExpected = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfExpected[0].Value;
                            string y = xmlAttributeCollectionOfExpected[1].Value;
                            string z = xmlAttributeCollectionOfExpected[2].Value;
                            textBox45.Text = x;
                            textBox44.Text = y;
                            textBox43.Text = z;
                            k++;
                            continue;
                        }
                        if (secondLevelNode.Name == "Expected" && (k == 1))
                        {
                            comboBox8.SelectedItem = "Expected1";
                            XmlAttributeCollection xmlAttributeCollectionOfExpected = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfExpected[0].Value;
                            string y = xmlAttributeCollectionOfExpected[1].Value;
                            string z = xmlAttributeCollectionOfExpected[2].Value;
                            textBox48.Text = x;
                            textBox47.Text = y;
                            textBox46.Text = z;
                            k++;
                            continue;
                        }
                        if (secondLevelNode.Name == "Expected" && (k == 2))
                        {
                            comboBox9.SelectedItem = "Expected2";
                            XmlAttributeCollection xmlAttributeCollectionOfExpected = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfExpected[0].Value;
                            string y = xmlAttributeCollectionOfExpected[1].Value;
                            string z = xmlAttributeCollectionOfExpected[2].Value;
                            textBox51.Text = x;
                            textBox50.Text = y;
                            textBox49.Text = z;
                            k++;
                            continue;
                        }
                        if (secondLevelNode.Name == "Expected" && (k == 3))
                        {
                            comboBox10.SelectedItem = "Expected3";
                            XmlAttributeCollection xmlAttributeCollectionOfExpected = secondLevelNode.Attributes;
                            string x = xmlAttributeCollectionOfExpected[0].Value;
                            string y = xmlAttributeCollectionOfExpected[1].Value;
                            string z = xmlAttributeCollectionOfExpected[2].Value;
                            textBox54.Text = x;
                            textBox53.Text = y;
                            textBox52.Text = z;
                            k++;
                            continue;
                        }
                    }

                }
            }

        private void label23_Click_1(object sender, EventArgs e)
        {

        }

        

        }

    }

    

/* //初始化xml实例
            XmlDocument doc = new XmlDocument();
            //加载xml文件
            doc.Load(@"C:\Users\wangnan\Desktop\测试1.xml");
            //获取文件的根节点
            XmlNode rootNode = doc.SelectSingleNode("theCollectionOfTestCases");
            //分别获得该节点的InnerXml和OuterXml信息
            string innerXmlInfo = rootNode.InnerXml.ToString();
            string outerXmlInfo = rootNode.OuterXml.ToString();
            //获得第一层子节点
            XmlNodeList firstLevelNodelist = rootNode.ChildNodes;
            //设置node变量
            Node node1 = new Node();
            List<Node> nodes = new List<Node>();
            List<Point> expected = new List<Point>();
            //node1.NodeStyle = NodeStyle.Tx;
            //Node node2 = new Node();
            //node2.NodeStyle = NodeStyle.DiffractionNode;
            //Node node3 = new Node();
            //node3.NodeStyle = NodeStyle.ReflectionNode;
            //Node node4=new Node();
            //node4.NodeStyle = NodeStyle.ReflectionNode;
            //Node node5 = new Node();
            //node5.NodeStyle = NodeStyle.ReflectionNode;
            //Node node6 = new Node();
            //node6.NodeStyle = NodeStyle.Rx;
            foreach (XmlNode node in firstLevelNodelist)
            {
                XmlAttributeCollection xmlAttributeCol = node.Attributes;
                //获得节点属性
                string path = xmlAttributeCol[0].Value;
                string havePath = xmlAttributeCol[1].Value;
                XmlNodeList secondLevelNodeList = node.ChildNodes;
                foreach (XmlNode secondLevelNode in secondLevelNodeList)
                {
                    if (secondLevelNode.Name == "Tx")
                    {
                        XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                        double x = Convert.ToDouble(thirdLevelNodeList[0].ToString());
                        double y = Convert.ToDouble(thirdLevelNodeList[1].ToString());
                        double z = Convert.ToDouble(thirdLevelNodeList[2].ToString());
                        node1.NodeStyle = NodeStyle.Tx;
                        node1.Position = new Point(x, y, z);
                    }
                    else
                    {
                        if (secondLevelNode.Name == "edge")
                        {
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlNodeList forthLevelNodeList = thirdLevelNodeList[0].ChildNodes;
                            double x = Convert.ToDouble(forthLevelNodeList[0].ToString());
                            double y = Convert.ToDouble(forthLevelNodeList[1].ToString());
                            double z = Convert.ToDouble(forthLevelNodeList[2].ToString());
                            Point point1 = new Point(x,y,z);
                            forthLevelNodeList = thirdLevelNodeList[1].ChildNodes;
                            x = Convert.ToDouble(forthLevelNodeList[0].ToString());
                            y = Convert.ToDouble(forthLevelNodeList[1].ToString());
                            z = Convert.ToDouble(forthLevelNodeList[2].ToString());
                            Point point2 = new Point(x, y, z);
                            node1.NodeStyle = NodeStyle.DiffractionNode;
                            node1.DiffractionEdge = new AdjacentEdge(point1,point2);
                        }
                        if (secondLevelNode.Name=="triangle")
                        {
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            XmlNodeList forthLevelNodeList = thirdLevelNodeList[0].ChildNodes;
                            double x = Convert.ToDouble(forthLevelNodeList[0].ToString());
                            double y = Convert.ToDouble(forthLevelNodeList[1].ToString());
                            double z = Convert.ToDouble(forthLevelNodeList[2].ToString());
                            Point point1 = new Point(x, y, z);
                            forthLevelNodeList = thirdLevelNodeList[1].ChildNodes;
                            x = Convert.ToDouble(forthLevelNodeList[0].ToString());
                            y = Convert.ToDouble(forthLevelNodeList[1].ToString());
                            z = Convert.ToDouble(forthLevelNodeList[2].ToString());
                            Point point2 = new Point(x, y, z);
                            forthLevelNodeList = thirdLevelNodeList[1].ChildNodes;
                            x = Convert.ToDouble(forthLevelNodeList[0].ToString());
                            y = Convert.ToDouble(forthLevelNodeList[1].ToString());
                            z = Convert.ToDouble(forthLevelNodeList[2].ToString());
                            Point point3 = new Point(x, y, z);
                            node1.NodeStyle = NodeStyle.ReflectionNode;
                            node1.ReflectionFace = new Triangle(point1,point2,point3);
                        }
                        if (secondLevelNode.Name=="expected")
                        {
                            XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
                            double x = Convert.ToDouble(thirdLevelNodeList[0].ToString());
                            double y = Convert.ToDouble(thirdLevelNodeList[1].ToString());
                            double z = Convert.ToDouble(thirdLevelNodeList[2].ToString());
                            Point expectedPoint = new Point(x, y, z);
                            expected.Add(expectedPoint);
                        }
                    }
                    nodes.Add(node1);
                }
                if ((path=="Tx-D-Rx")&&(havePath=="true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(expected[0].equal(actual[0]));
                }
                if ((path == "Tx-D-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(actual.Count==0);
                }
                if ((path=="Tx-R-D-Rx")&&(havePath=="true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(expected[0].equal(actual[0])&&expected[1].equal(actual[1]));
                }
                if ((path == "Tx-R-D-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-D-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]));
                }
                if ((path == "Tx-D-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-R-R-D-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(3);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1])&expected[2].equal(actual[2]));
                }
                if ((path == "Tx-R-R-D-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(3);
                    Assert.IsTrue(actual.Count==0);
                }
                if ((path == "Tx-R-D-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2]));
                }
                if ((path == "Tx-R-D-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-D-R-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2]));
                }
                if ((path == "Tx-D-R-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-R-R-R-D-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(4);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2])&expected[3].equal(actual[3]));
                }
                if ((path == "Tx-R-R-R-D-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(4);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-D-R-R-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2]) & expected[3].equal(actual[3]));
                }
                if ((path == "Tx-D-R-R-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(1);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-R-D-R-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2]) & expected[3].equal(actual[3]));
                }
                if ((path == "Tx-R-D-R-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(2);
                    Assert.IsTrue(actual.Count == 0);
                }
                if ((path == "Tx-R-R-D-R-Rx") && (havePath == "true"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(3);
                    Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) & expected[2].equal(actual[2]) & expected[3].equal(actual[3]));
                }
                if ((path == "Tx-R-R-D-R-Rx") && (havePath == "false"))
                {
                    Path_Accessor target = new Path_Accessor(nodes);
                    List<Point> actual = new List<Point>();
                    actual = target.GetPointsOfContainOneDiffractPath(3);
                    Assert.IsTrue(actual.Count == 0);
                }*/