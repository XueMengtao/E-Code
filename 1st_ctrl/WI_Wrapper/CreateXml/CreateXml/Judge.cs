using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CreateXml
{
    public class Judge
    {
        public void IsPass(string xmlTrace)
        {
            string oringinPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //"C:\\Users\\wangnan\\Desktop\\EMC_Project\\FinalCode6\\1st_ctrl\\WI_Wrapper\\GetPointOfContainOneDiffractionPathTest\\bin\\Debug"
            string xmlPath = oringinPath.Substring(0, oringinPath.LastIndexOf("\\"));
            xmlPath = oringinPath.Substring(0, xmlPath.LastIndexOf("\\"));
            xmlPath = oringinPath.Substring(0, xmlPath.LastIndexOf("\\"));
            xmlPath = xmlPath + "\\" + "CreateXml" + "\\" + "CreateXml\\bin\\" + xmlTrace;
            //初始化xml实例
            XmlDocument doc = new XmlDocument();
            //加载xml文件
            doc.Load(xmlTrace);
            //获取文件的根节点
            XmlNode rootNode = doc.SelectSingleNode("TheCollectionOfTestCases");
            //分别获得该节点的InnerXml和OuterXml信息
            string innerXmlInfo = rootNode.InnerXml.ToString();
            string outerXmlInfo = rootNode.OuterXml.ToString();
            //获得第一层子节点
            XmlNodeList firstLevelNodelist = rootNode.ChildNodes;
            //设置node变量
            //Node node1 = new Node();
            List<Node> nodes = new List<Node>();//声明nodes
            List<Point> expected = new List<Point>();//声明接收点集合
            //XmlAttributeCollection xmlAttributeCol = firstLevelNodelist[0].Attributes;
            //   //获得节点属性
            //   string path = xmlAttributeCol[0].Value;
            //   string havePath = xmlAttributeCol[1].Value;
            int i = 0;
            bool flag = true;
            nodes = ReadXml.getNodesFromXml(xmlTrace, ref expected, ref i);//调用方法得到nodes，运用ref得到expected点集
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(i);//生成实际预测点集
            if (actual.Count == 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                for (int j = 0; j < actual.Count; j++)
                {
                    if (!expected[j].equal(actual[j]))
                    { flag = false; }
                }
                Assert.IsTrue(flag);
            }
        }
    }
}
