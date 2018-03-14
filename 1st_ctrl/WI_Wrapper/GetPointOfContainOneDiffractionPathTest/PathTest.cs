using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml;
using CreateXml;

 
namespace GetPointOfContainOneDiffractionPathUseXmlTest
{


    /// <summary>
    ///这是 PathTest 的测试类，旨在
    ///包含所有 PathTest 单元测试
    ///</summary>
    [TestClass()]
    public class PathTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]

        public void GetPointsOfContainOneDiffractPathTest1()
        {

            string xmlTrace = "GetPointOfContainOneDiffractionPathUseXmlTest\\测试1.xml";//声明字符串变量用来存储xml路径
            Judge(xmlTrace);
        }

        public void Judge(string xmlTrace)
        {
            string oringinPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //"C:\\Users\\wangnan\\Desktop\\EMC_Project\\FinalCode6\\1st_ctrl\\WI_Wrapper\\GetPointOfContainOneDiffractionPathTest\\bin\\Debug"
            string xmlPath = oringinPath.Substring(0, oringinPath.LastIndexOf("\\"));
            xmlPath = oringinPath.Substring(0, xmlPath.LastIndexOf("\\"));
            xmlPath = oringinPath.Substring(0, xmlPath.LastIndexOf("\\"));
            xmlPath = xmlPath + "\\" + "CreateXml" + "\\" + "CreateXml\\bin\\" + xmlTrace;
            ////初始化xml实例
            //XmlDocument doc = new XmlDocument();
            ////加载xml文件
            //doc.Load(xmlTrace);
            ////获取文件的根节点
            //XmlNode rootNode = doc.SelectSingleNode("TheCollectionOfTestCases");
            ////分别获得该节点的InnerXml和OuterXml信息
            //string innerXmlInfo = rootNode.InnerXml.ToString();
            //string outerXmlInfo = rootNode.OuterXml.ToString();
            ////获得第一层子节点
            //XmlNodeList firstLevelNodelist = rootNode.ChildNodes;
            ////设置node变量
            ////Node node1 = new Node();
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

       







        //public static List<Node> getNodesFromXml(string xmlTrace, ref List<Point> expected, ref int i)
        //{
        //    int k = 0;
        //    i = 0;
        //    //初始化xml实例
        //    XmlDocument doc = new XmlDocument();
        //    //加载xml文件
        //    doc.Load(xmlTrace);//获取路径
        //    //获取文件的根节点
        //    XmlNode rootNode = doc.SelectSingleNode("TheCollectionOfTestCases");
        //    //分别获得该节点的InnerXml和OuterXml信息
        //    string innerXmlInfo = rootNode.InnerXml.ToString();
        //    string outerXmlInfo = rootNode.OuterXml.ToString();
        //    //获得第一层子节点
        //    XmlNodeList firstLevelNodelist = rootNode.ChildNodes;
        //    //设置node变量
        //    Node node1 = new Node();
        //    List<Node> nodes = new List<Node>();
        //    expected = new List<Point>();
        //    node1.NodeStyle = NodeStyle.Tx;
        //    Node node2 = new Node();
        //    node2.NodeStyle = NodeStyle.DiffractionNode;
        //    Node node5 = new Node();
        //    node5.NodeStyle = NodeStyle.Rx;
        //    foreach (XmlNode node in firstLevelNodelist)
        //    {
        //        XmlAttributeCollection xmlAttributeCol = node.Attributes;
        //        //获得节点属性
        //        string path = xmlAttributeCol[0].Value;
        //        string havePath = xmlAttributeCol[1].Value;
        //        XmlNodeList secondLevelNodeList = node.ChildNodes;
        //        foreach (XmlNode secondLevelNode in secondLevelNodeList)
        //        {
        //            if (secondLevelNode.Name == "Transmitter")
        //            {
        //                XmlAttributeCollection xmlAttributeCollectionOfTransmitter = secondLevelNode.Attributes;
        //                double x = Convert.ToDouble(xmlAttributeCollectionOfTransmitter[0].Value);
        //                double y = Convert.ToDouble(xmlAttributeCollectionOfTransmitter[1].Value);
        //                double z = Convert.ToDouble(xmlAttributeCollectionOfTransmitter[2].Value);
        //                node1.NodeStyle = NodeStyle.Tx;
        //                node1.Position = new Point(x, y, z);
        //                nodes.Add(node1);
        //                k++;
        //            }

        //            if (secondLevelNode.Name == "Edge")
        //            {
        //                XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
        //                XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
        //                double x = Convert.ToDouble(xmlAttributeCollectionOfEdge[0].Value);
        //                double y = Convert.ToDouble(xmlAttributeCollectionOfEdge[1].Value);
        //                double z = Convert.ToDouble(xmlAttributeCollectionOfEdge[2].Value);
        //                Point point1 = new Point(x, y, z);
        //                xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
        //                x = Convert.ToDouble(xmlAttributeCollectionOfEdge[0].Value);
        //                y = Convert.ToDouble(xmlAttributeCollectionOfEdge[1].Value);
        //                z = Convert.ToDouble(xmlAttributeCollectionOfEdge[2].Value);
        //                Point point2 = new Point(x, y, z);
        //                node2.DiffractionEdge = new AdjacentEdge(point1, point2);
        //                nodes.Add(node2);
        //                i = k;
        //            }
        //            if (secondLevelNode.Name == "Triangle")
        //            {
        //                XmlNodeList thirdLevelNodeList = secondLevelNode.ChildNodes;
        //                XmlAttributeCollection xmlAttributeCollectionOfEdge = thirdLevelNodeList[0].Attributes;
        //                double x = Convert.ToDouble(xmlAttributeCollectionOfEdge[0].Value);
        //                double y = Convert.ToDouble(xmlAttributeCollectionOfEdge[1].Value);
        //                double z = Convert.ToDouble(xmlAttributeCollectionOfEdge[2].Value);
        //                Point point1 = new Point(x, y, z);
        //                xmlAttributeCollectionOfEdge = thirdLevelNodeList[1].Attributes;
        //                x = Convert.ToDouble(xmlAttributeCollectionOfEdge[0].Value);
        //                y = Convert.ToDouble(xmlAttributeCollectionOfEdge[1].Value);
        //                z = Convert.ToDouble(xmlAttributeCollectionOfEdge[2].Value);
        //                Point point2 = new Point(x, y, z);
        //                xmlAttributeCollectionOfEdge = thirdLevelNodeList[2].Attributes;
        //                x = Convert.ToDouble(xmlAttributeCollectionOfEdge[0].Value);
        //                y = Convert.ToDouble(xmlAttributeCollectionOfEdge[1].Value);
        //                z = Convert.ToDouble(xmlAttributeCollectionOfEdge[2].Value);
        //                Point point3 = new Point(x, y, z);
        //                Node node3 = new Node();
        //                node3.NodeStyle = NodeStyle.ReflectionNode;
        //                Node node4 = new Node();
        //                node3.ReflectionFace = new Triangle(point1, point2, point3);
        //                nodes.Add(node3);
        //                k++;

        //            }
        //            if (secondLevelNode.Name == "Receiver")
        //            {
        //                XmlAttributeCollection xmlAttributeCollectionOfReceiver = secondLevelNode.Attributes;
        //                double x = Convert.ToDouble(xmlAttributeCollectionOfReceiver[0].Value);
        //                double y = Convert.ToDouble(xmlAttributeCollectionOfReceiver[1].Value);
        //                double z = Convert.ToDouble(xmlAttributeCollectionOfReceiver[2].Value);
        //                node5.NodeStyle = NodeStyle.Rx;
        //                node5.Position = new Point(x, y, z);
        //                nodes.Add(node5);
        //            }
        //            if (secondLevelNode.Name == "Expected")
        //            {
        //                XmlAttributeCollection xmlAttributeCollectionOfExpected = secondLevelNode.Attributes;
        //                double x = Convert.ToDouble(xmlAttributeCollectionOfExpected[0].Value);
        //                double y = Convert.ToDouble(xmlAttributeCollectionOfExpected[1].Value);
        //                double z = Convert.ToDouble(xmlAttributeCollectionOfExpected[2].Value);
        //                Point expectedPoint = new Point(x, y, z);
        //                expected.Add(expectedPoint);

        //            }
        //        }
        //    }
        //    return nodes;
        //}
    }
}
